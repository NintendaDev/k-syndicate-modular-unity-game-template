using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.MusicManagement.Configurations;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace Modules.MusicManagement.Player
{
    public class MusicPlayerFactory : PrefabFactoryAsync<MusicPlayer>
    {
        private readonly IStaticDataService _staticDataService;
        private MusicPlayerConfiguration _configuration;

        public MusicPlayerFactory(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IStaticDataService staticDataService) 
            : base(instantiator, componentAssetService)
        {
            _staticDataService = staticDataService;
        }

        public async UniTask<MusicPlayer> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<MusicPlayerConfiguration>();

            return await CreateAsync(_configuration.MusicPlayerReference.AssetGUID);
        }
    }
}
