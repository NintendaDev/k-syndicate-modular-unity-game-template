using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Configurations;
using Modules.AssetsManagement.AddressablesServices;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace GameTemplate.UI.LoadingCurtain
{
    public class LoadingCurtainFabric : PrefabFactoryAsync<LoadingCurtain>
    {
        private readonly GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;

        public LoadingCurtainFabric(IInstantiator instantiator, IComponentAssetService componentAssetService,
            GameLoadingAssetsConfiguration gameLoadingAssetsConfiguration) 
            : base(instantiator, componentAssetService)
        {
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public async UniTask<LoadingCurtain> CreateAsync() =>
            await CreateAsync(_gameLoadingAssetsConfiguration.Curtain.AssetGUID);
    }
}
