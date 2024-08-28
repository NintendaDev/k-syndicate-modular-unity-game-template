using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Services.StaticData;
using GameTemplate.UI.Services.Popups.Simple;
using Zenject;

namespace GameTemplate.UI.Services.Popups.Factories
{
    public class ErrorPopupFabric : PrefabFactoryAsync<SimplePopup>
    {
        private readonly IStaticDataService _staticDataService;
        private PopupsAssetsConfiguration _configuration;

        public ErrorPopupFabric(IInstantiator instantiator, IComponentAssetProvider componentAssetProvider,
            IStaticDataService staticDataService)
            : base(instantiator, componentAssetProvider)
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
