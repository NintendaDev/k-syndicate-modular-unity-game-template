using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ExternalLibraries.SceneManagement
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
