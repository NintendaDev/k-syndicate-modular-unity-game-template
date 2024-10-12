using Cysharp.Threading.Tasks;
using GameTemplate.UI.Services.Popups.Simple;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace GameTemplate.UI.Services.Popups.Factories
{
    public class ErrorPopupFabric : PrefabFactoryAsync<SimplePopup>
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
