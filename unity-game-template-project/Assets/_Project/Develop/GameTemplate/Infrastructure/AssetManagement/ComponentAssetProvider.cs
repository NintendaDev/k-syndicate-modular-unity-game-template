using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.AssetManagement
{
    public class ComponentAssetProvider : IComponentAssetProvider
    {
        private IAssetProvider _assetProvider;

        public ComponentAssetProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask<TAsset> LoadAsync<TAsset>(AssetReferenceGameObject assetReference) where TAsset : MonoBehaviour =>
            await LoadByAddressAsync<TAsset>(assetReference.AssetGUID);

        public async UniTask<TAsset> LoadByAddressAsync<TAsset>(string address) where TAsset : MonoBehaviour
        {
            GameObject assetObject = await _assetProvider.LoadByAddressAsync<GameObject>(address);
            TAsset asset = assetObject.GetComponent<TAsset>();

            if (asset == null)
            {
                _assetProvider.Release(address);
                throw new Exception($"Choosed component was not found in the uploaded object");
            }

            return asset;
        }

        public void Release(AssetReferenceGameObject assetReference) =>
            _assetProvider.Release(assetReference);

        public void Release(string assetAddress) =>
            _assetProvider.Release(assetAddress);
    }
}
