using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Modules.AssetsManagement.AddressablesServices
{
    public interface IAddressablesService
    {
        public UniTask InitializeAsync();

        public UniTask<TAsset> LoadAsync<TAsset>(AssetReferenceT<TAsset> assetReference) 
            where TAsset : UnityEngine.Object;

        public UniTask<TAsset> LoadAsync<TAsset>(AssetReference assetReference) where TAsset : UnityEngine.Object;

        public UniTask<TAsset> LoadByAddressAsync<TAsset>(string key) where TAsset : UnityEngine.Object;

        public UniTask<TAsset> LoadOneByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object;

        public UniTask<TAsset[]> LoadByLabelAsync<TAsset>(string label) where TAsset : UnityEngine.Object;

        public UniTask<List<string>> GetAssetsAddressesByLabelAsync<TAsset>(string label);

        public UniTask<List<string>> GetAssetsAddressesByLabelAsync(string label, Type type = null);

        public UniTask<TAsset[]> LoadByAddressAsync<TAsset>(IEnumerable<string> keys) where TAsset : UnityEngine.Object;

        public UniTask WarmupAssetsByLabelAsync(string label);

        public UniTask ReleaseAssetsByLabelAsync(string label);

        public void Release(AssetReference assetReference);

        public void Release(string address);

        public void Release<TAsset>(AssetReferenceT<TAsset> assetReference) where TAsset : UnityEngine.Object;

        public void Cleanup();
    }
}