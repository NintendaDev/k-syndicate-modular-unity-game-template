using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Modules.SceneManagement
{
    public interface ISingleSceneLoader
    {
        public float GetProgress();
        
        public UniTask Load(string nextScene);

        public UniTask Load(AssetReference nextScene);
    }
}