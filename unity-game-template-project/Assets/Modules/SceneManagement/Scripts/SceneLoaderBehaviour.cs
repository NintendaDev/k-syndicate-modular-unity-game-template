using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.SceneManagement
{
    public class SceneLoaderBehaviour : MonoBehaviour
    {
        [SerializeField] private AssetReference _scene;

        private async void Awake()
        {
            await Addressables.LoadSceneAsync(_scene).Task;
        }
    }
}
