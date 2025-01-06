using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Modules.SceneManagement
{
    public sealed class SingleSingleSceneLoader : ISingleSceneLoader, IDisposable
    {
        private readonly Queue<AsyncOperationHandle<SceneInstance>> _unloadScenesQueue = new();
        private AsyncOperationHandle<SceneInstance> _currentSceneHandler;

        public void Dispose()
        {
            UnloadScenesAsync().Forget();
        }

        public float GetProgress() => _currentSceneHandler.PercentComplete;

        public async UniTask Load(string nextScene)
        {
            AsyncOperationHandle<SceneInstance> sceneHandler = Addressables.LoadSceneAsync(nextScene);
            _currentSceneHandler = sceneHandler;
            
            await sceneHandler.ToUniTask();
            await UnloadScenesAsync();
            _unloadScenesQueue.Enqueue(sceneHandler);
        }

        public async UniTask Load(AssetReference nextScene) => await Load(nextScene.AssetGUID);

        private async UniTask UnloadScenesAsync()
        {
            while (_unloadScenesQueue.Count > 0)
            {
                AsyncOperationHandle<SceneInstance> handler = _unloadScenesQueue.Dequeue();
                
                if (handler.IsValid() == false || handler.Result.Scene.isLoaded == false)
                    continue;

                await Addressables.UnloadSceneAsync(handler, autoReleaseHandle: true);
            }
        }
    }
}