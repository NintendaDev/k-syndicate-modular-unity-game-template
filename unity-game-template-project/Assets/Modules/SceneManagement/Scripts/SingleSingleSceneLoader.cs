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
        private AsyncOperationHandle<SceneInstance> _currentSceneHandler;

        public void Dispose()
        {
            UnloadSceneAsync(_currentSceneHandler).Forget();
        }

        public float GetProgress() => _currentSceneHandler.PercentComplete;

        public async UniTask Load(string nextScene)
        {
            List<UniTask> tasks = new();
            tasks.Add(UnloadSceneAsync(_currentSceneHandler));
            _currentSceneHandler = Addressables.LoadSceneAsync(nextScene);
            tasks.Add(_currentSceneHandler.ToUniTask());
            
            await UniTask.WhenAll(tasks);
        }

        public async UniTask Load(AssetReference nextScene) => await Load(nextScene.AssetGUID);

        private async UniTask UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handler)
        {
            if (handler.Result.Scene.isLoaded == false)
                return;

            await Addressables.UnloadSceneAsync(handler, autoReleaseHandle: true);
        }
    }
}