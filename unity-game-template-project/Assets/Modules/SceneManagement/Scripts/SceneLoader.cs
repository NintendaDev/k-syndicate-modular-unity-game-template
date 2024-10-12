using Cysharp.Threading.Tasks;
using System.Diagnostics;
using Modules.Specifications;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Modules.SceneManagement
{
    public class SceneLoader : ISceneLoader
    {
        private FloatValidator _floatValidator;
        private AsyncOperationHandle<SceneInstance> _previousSceneOperationHandle;

        public SceneLoader()
        {
            _floatValidator = new FloatValidator();
        }

        public async UniTask Load(string nextScene) =>
            await Load(nextScene, 0);

        public async UniTask Load(string nextScene, float minLoadMilliseconds)
        {
            _floatValidator.GreatOrEqualZero(minLoadMilliseconds);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            AsyncOperationHandle<SceneInstance> handler = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single, false);

            await handler.ToUniTask();

            stopwatch.Stop();
            int additionalDelay = (int)Mathf.Max(minLoadMilliseconds - stopwatch.ElapsedMilliseconds, 0);

            await UniTask.Delay(additionalDelay);
            await handler.Result.ActivateAsync().ToUniTask();

            if (_previousSceneOperationHandle.IsValid())
                await UnloadSceneAsync(_previousSceneOperationHandle);

            _previousSceneOperationHandle = handler;
        }

        public async UniTask Load(AssetReference nextScene) =>
            await Load(nextScene.AssetGUID, 0);

        private async UniTask UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Result.Scene.isLoaded == false)
                return;

            await Addressables.UnloadSceneAsync(handle, autoReleaseHandle: true);
        }
    }
}