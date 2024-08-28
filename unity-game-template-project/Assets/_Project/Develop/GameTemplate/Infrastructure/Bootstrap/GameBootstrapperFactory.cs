using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Bootstrappers;
using Zenject;

namespace GameTemplate.Infrastructure.Bootstrap
{
    public class GameBootstrapperFactory : PrefabFactoryAsync<GameBootstrapper>
    {
        private readonly BootstrapAssetAddresser _bootstrapAssetAddresser;

        public GameBootstrapperFactory(IInstantiator instantiator, IComponentAssetProvider componentAssetProvider) 
            : base(instantiator, componentAssetProvider)
        {
             _bootstrapAssetAddresser = new BootstrapAssetAddresser();
        }

        public async UniTask<GameBootstrapper> CreateAsync() =>
            await CreateAsync(_bootstrapAssetAddresser.BootstrapPrefabAddress);
    }
}
