using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Modules.AssetsManagement.AddressablesOperations
{
    public interface IAddressablesService
    {
        public UniTask<TAsset[]> LoadAsync<TAsset>(IEnumerable<AssetReference> assetReferences) 
            where TAsset : UnityEngine.Object;

        public UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference) where TAsset : UnityEngine.Object;

        public UniTask<TAsset> LoadByAddressAsync<TAsset>(string assetAddress) where TAsset : UnityEngine.Object;

        public UniTask<TAsset[]> LoadByAddressAsync<TAsset>(IEnumerable<string> assetAddresses) 
            where TAsset : UnityEngine.Object;
        
        public UniTask<TAsset[]> LoadByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object;
        
        public void Release(IEnumerable<AssetReference> assetReferences);
        
        public void Release(AssetReference assetReference);

        public UniTask ReleaseByLabel<TAsset>(string label) where TAsset : UnityEngine.Object;

        public void ReleaseByAddress(string address);
    }
}