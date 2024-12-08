using System;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.AssetsManagement.StaticData;
using Modules.ObjectsManagement.Factories;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Simple;
using Zenject;

namespace Modules.PopupsSystem.UI.Factories
{
    public sealed class ErrorPopupFactory : IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly PrefabFactoryAsync<SimplePopup> _prefabFactory;
        private PopupsAssetsConfiguration _configuration;

        public ErrorPopupFactory(IInstantiator instantiator, IAddressablesService addressablesService,
            IStaticDataService staticDataService)
        {
            _prefabFactory = new PrefabFactoryAsync<SimplePopup>(instantiator, addressablesService);
            _staticDataService = staticDataService;
        }

        public void Dispose()
        {
            _prefabFactory.Dispose();
        }

        public async UniTask<SimplePopup> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<PopupsAssetsConfiguration>();

            return await _prefabFactory.CreateAsync(_configuration.ErrorPopup.AssetGUID);
        }
    }
}
