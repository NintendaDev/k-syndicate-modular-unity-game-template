using Cysharp.Threading.Tasks;
using System;
using Modules.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.AssetManagement
{
    public class ComponentAssetService : IComponentAssetService
    {
        private IAddressablesService _addressablesService;

        public ComponentAssetService(IAddressablesService addressablesService)
        {
            _addressablesService = addressablesService;
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReferenceGameObject assetReference) where TAsset : MonoBehaviour =>
            await LoadByAddressAsync<TAsset>(assetReference.AssetGUID);

        public async UniTask<TAsset> LoadByAddressAsync<TAsset>(string address) where TAsset : MonoBehaviour
        {
            GameObject assetObject = await _addressablesService.LoadByAddressAsync<GameObject>(address);
            TAsset asset = assetObject.GetComponent<TAsset>();

            if (asset == null)
            {
                _addressablesService.Release(address);
                throw new Exception($"Choosed component was not found in the uploaded object");
            }

            return asset;
        }

        public void Release(AssetReferenceGameObject assetReference) =>
            _addressablesService.Release(assetReference);

        public void Release(string assetAddress) =>
            _addressablesService.Release(assetAddress);
    }
}
