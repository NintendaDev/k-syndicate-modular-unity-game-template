using System;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.LoadingCurtain.Configurations;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace Modules.LoadingCurtain
{
    public sealed class LoadingCurtainFabric : IDisposable
    {
        private readonly LoadingCurtainConfiguration _gameLoadingAssetsConfiguration;
        private readonly PrefabFactoryAsync<LoadingCurtain> _prefabFactory;

        public LoadingCurtainFabric(IInstantiator instantiator, IAddressablesService addressablesService,
            LoadingCurtainConfiguration gameLoadingAssetsConfiguration) 
        {
            _prefabFactory = new PrefabFactoryAsync<LoadingCurtain>(instantiator, addressablesService);
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public void Dispose()
        {
            _prefabFactory.Dispose();
        }

        public async UniTask<LoadingCurtain> CreateAsync() =>
            await _prefabFactory.CreateAsync(_gameLoadingAssetsConfiguration.CurtainPrefabReference);
    }
}
