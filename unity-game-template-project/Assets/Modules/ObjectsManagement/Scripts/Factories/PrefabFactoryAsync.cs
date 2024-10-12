using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.AddressablesServices;
using UnityEngine;
using Zenject;

namespace Modules.ObjectsManagement.Factories
{
    public abstract class PrefabFactoryAsync<TComponent> : IDisposable 
        where TComponent : MonoBehaviour
    {
        private readonly IInstantiator _instantiator;
        private readonly IComponentAssetService _componentAssetService;
        private List<string> _loadedAssetAddresses = new();

        public PrefabFactoryAsync(IInstantiator instantiator, IComponentAssetService componentAssetService)
        {
            _instantiator = instantiator;
            _componentAssetService = componentAssetService;
        }

        public virtual void Dispose()
        {
            _loadedAssetAddresses.ForEach(x => _componentAssetService.Release(x));
        }

        protected async virtual UniTask<TComponent> CreateAsync(string assetAddress)
        {
            TComponent prefab = await _componentAssetService.LoadByAddressAsync<TComponent>(assetAddress);

            if (_loadedAssetAddresses.Contains(assetAddress) == false)
                _loadedAssetAddresses.Add(assetAddress);

            GameObject newObject = _instantiator.InstantiatePrefab(prefab);

            return newObject.GetComponent<TComponent>();
        }
    }
}