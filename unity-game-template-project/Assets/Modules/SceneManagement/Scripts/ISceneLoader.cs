using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Modules.SceneManagement
{
    public interface ISceneLoader
    {
        public UniTask Load(string nextScene, float minLoadMilliseconds);

        public UniTask Load(string nextScene);

        public UniTask Load(AssetReference nextScene);
    }
}