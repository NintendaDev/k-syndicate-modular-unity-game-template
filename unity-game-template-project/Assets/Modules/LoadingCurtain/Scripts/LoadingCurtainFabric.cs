using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesServices;
using Modules.LoadingCurtain.Configurations;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace Modules.LoadingCurtain
{
    public sealed class LoadingCurtainFabric : PrefabFactoryAsync<LoadingCurtain>
    {
        private readonly LoadingCurtainConfiguration _gameLoadingAssetsConfiguration;

        public LoadingCurtainFabric(IInstantiator instantiator, IComponentAssetService componentAssetService,
            LoadingCurtainConfiguration gameLoadingAssetsConfiguration) 
            : base(instantiator, componentAssetService)
        {
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public async UniTask<LoadingCurtain> CreateAsync() =>
            await CreateAsync(_gameLoadingAssetsConfiguration.CurtainPrefabReference.AssetGUID);
    }
}
