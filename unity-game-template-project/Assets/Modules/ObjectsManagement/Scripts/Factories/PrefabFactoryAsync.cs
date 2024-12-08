using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.AddressablesOperations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace Modules.ObjectsManagement.Factories
{
    public sealed class PrefabFactoryAsync<TComponent> : IDisposable 
        where TComponent : MonoBehaviour
    {
        private readonly IInstantiator _instantiator;
        private readonly IAddressablesService _addressablesService;
        private List<string> _loadedAssetAddresses = new();

        public PrefabFactoryAsync(IInstantiator instantiator, IAddressablesService addressablesService)
        {
            _instantiator = instantiator;
            _addressablesService = addressablesService;
        }

        public void Dispose()
        {
            _loadedAssetAddresses.ForEach(x => _addressablesService.ReleaseByAddress(x));
        }

        public async UniTask<TComponent> CreateAsync(AssetReference assetreference) =>
            await CreateAsync(assetreference.AssetGUID);
        
        public async UniTask<TComponent> CreateAsync(string assetAddress)
        {
            Object prefab = await _addressablesService.LoadByAddressAsync<Object>(assetAddress);

            if (_loadedAssetAddresses.Contains(assetAddress) == false)
                _loadedAssetAddresses.Add(assetAddress);

            GameObject newObject = _instantiator.InstantiatePrefab(prefab);

            return newObject.GetComponent<TComponent>();
        }
    }
}