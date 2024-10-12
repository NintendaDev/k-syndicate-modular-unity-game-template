using System;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesServices;
using Modules.ObjectsManagement.Pools;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.ObjectsManagement.Factories
{
    public abstract class PooledFactoryAsync<TComponent> : IDisposable
        where TComponent : Component
    {
        private readonly AssetReference _prefabReference;
        private readonly IAddressablesService _addressablesService;
        private bool _isInitialize;

        public PooledFactoryAsync(AssetReference prefabReference, IAddressablesService addressablesService)
        {
            _prefabReference = prefabReference;
            _addressablesService = addressablesService;
        }

        protected ObjectPool<TComponent> Pool { get; private set; }

        public virtual void Dispose()
        {
            if (_isInitialize == false)
                return;
            
            _addressablesService.Release(_prefabReference);
        }
        
        public async UniTask InitializeAsync()
        {
            if (_isInitialize)
                return;
            
            TComponent prefab = await _addressablesService
                .LoadByAddressAsync<TComponent>(_prefabReference.AssetGUID);
            
            Pool = new ObjectPool<TComponent>(prefab);
            _isInitialize = true;
        }
    }
}