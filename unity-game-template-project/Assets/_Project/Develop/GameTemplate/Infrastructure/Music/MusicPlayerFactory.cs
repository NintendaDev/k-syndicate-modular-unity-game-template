using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Configurations;
using Modules.AssetManagement.StaticData;
using Zenject;

namespace GameTemplate.Infrastructure.Music
{
    public class MusicPlayerFactory : PrefabFactoryAsync<MusicPlayer>
    {
        private readonly IStaticDataService _staticDataService;
        private InfrastructureAssetsConfiguration _configuration;

        public MusicPlayerFactory(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IStaticDataService staticDataService) 
            : base(instantiator, componentAssetService)
        {
            _staticDataService = staticDataService;
        }

        public async UniTask<MusicPlayer> CreateAsync()
        {
            if (_configuration == null)
                _configuration = _staticDataService.GetConfiguration<InfrastructureAssetsConfiguration>();

            return await CreateAsync(_configuration.MusicPlayerReference.AssetGUID);
        }
    }
}
