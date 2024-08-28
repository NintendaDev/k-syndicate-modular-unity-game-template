using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Services.StaticData;
using Zenject;

namespace GameTemplate.Infrastructure.Music
{
    public class MusicPlayerFactory : PrefabFactoryAsync<MusicPlayer>
    {
        private readonly IStaticDataService _staticDataService;
        private InfrastructureAssetsConfiguration _configuration;

        public MusicPlayerFactory(IInstantiator instantiator, IComponentAssetProvider componentAssetProvider,
            IStaticDataService staticDataService) 
            : base(instantiator, componentAssetProvider)
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
