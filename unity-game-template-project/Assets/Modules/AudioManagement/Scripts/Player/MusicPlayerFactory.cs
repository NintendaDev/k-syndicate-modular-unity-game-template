using System;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Configurations;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace Modules.AudioManagement.Player
{
    public sealed class MusicPlayerFactory : IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        private MusicPlayerConfiguration _configuration;
        private readonly PrefabFactoryAsync<MusicPlayer> _prefabFactory;

        public MusicPlayerFactory(IInstantiator instantiator, IAddressablesService addressablesService,
            IStaticDataService staticDataService)
        {
            _prefabFactory = new PrefabFactoryAsync<MusicPlayer>(instantiator, addressablesService);
            _staticDataService = staticDataService;
        }

        public void Dispose()
        {
            _prefabFactory.Dispose();
        }

        public async UniTask<MusicPlayer> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<MusicPlayerConfiguration>();

            return await _prefabFactory.CreateAsync(_configuration.MusicPlayerReference);
        }
    }
}
