using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.UI.Services.Popups.Simple;
using Modules.AssetManagement.StaticData;
using Zenject;

namespace GameTemplate.UI.Services.Popups.Factories
{
    public class InfoPopupFabric : PrefabFactoryAsync<SimplePopup>
    {
        private readonly IStaticDataService _staticDataService;
        private PopupsAssetsConfiguration _configuration;

        public InfoPopupFabric(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IStaticDataService staticDataService)
            : base(instantiator, componentAssetService)
        {
            _staticDataService = staticDataService;
        }

        public async UniTask<SimplePopup> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<PopupsAssetsConfiguration>();

            return await CreateAsync(_configuration.InfoPopup.AssetGUID);
        }
    }
}
