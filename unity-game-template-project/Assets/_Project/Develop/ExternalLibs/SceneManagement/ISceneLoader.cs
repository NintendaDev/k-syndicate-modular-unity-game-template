using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace ExternalLibraries.SceneManagement
{
    public interface ISceneLoader
    {
        public UniTask Load(string nextScene, float minLoadMilliseconds);

        public UniTask Load(string nextScene);

        public UniTask Load(AssetReference nextScene);
    }
}