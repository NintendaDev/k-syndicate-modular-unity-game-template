using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameTemplate.Infrastructure.AssetManagement
{
    public abstract class PrefabFactoryAsync<TComponent> : IDisposable 
        where TComponent : MonoBehaviour
    {
        private readonly IInstantiator _instantiator;
        private readonly IComponentAssetProvider _componentAssetProvider;
        private List<string> _loadedAssetAddresses = new();

        public PrefabFactoryAsync(IInstantiator instantiator, IComponentAssetProvider componentAssetProvider)
        {
            _instantiator = instantiator;
            _componentAssetProvider = componentAssetProvider;
        }

        public virtual void Dispose()
        {
            _loadedAssetAddresses.ForEach(x => _componentAssetProvider.Release(x));
        }

        protected async virtual UniTask<TComponent> CreateAsync(string assetAddress)
        {
            TComponent prefab = await _componentAssetProvider.LoadByAddressAsync<TComponent>(assetAddress);

            if (_loadedAssetAddresses.Contains(assetAddress) == false)
                _loadedAssetAddresses.Add(assetAddress);

            GameObject newObject = _instantiator.InstantiatePrefab(prefab);

            return newObject.GetComponent<TComponent>();
        }
    }
}