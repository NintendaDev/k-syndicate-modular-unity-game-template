using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.ObjectsManagement.Factories;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Simple;
using Zenject;

namespace Modules.PopupsSystem.UI.Factories
{
    public sealed class ErrorPopupFabric : PrefabFactoryAsync<SimplePopup>
    {
        private readonly IStaticDataService _staticDataService;
        private PopupsAssetsConfiguration _configuration;

        public ErrorPopupFabric(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IStaticDataService staticDataService)
            : base(instantiator, componentAssetService)
        {
            _staticDataService = staticDataService;
        }

        public async UniTask<SimplePopup> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<PopupsAssetsConfiguration>();

            return await CreateAsync(_configuration.ErrorPopup.AssetGUID);
        }
    }
}
