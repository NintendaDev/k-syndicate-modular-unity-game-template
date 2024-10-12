using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Bootstrappers;
using Modules.AssetsManagement.AddressablesServices;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace GameTemplate.Infrastructure.Bootstrap
{
    public class GameBootstrapperFactory : PrefabFactoryAsync<GameBootstrapper>
    {
        private readonly BootstrapAssetAddresser _bootstrapAssetAddresser;

        public GameBootstrapperFactory(IInstantiator instantiator, IComponentAssetService componentAssetService) 
            : base(instantiator, componentAssetService)
        {
             _bootstrapAssetAddresser = new BootstrapAssetAddresser();
        }

        public async UniTask<GameBootstrapper> CreateAsync() =>
            await CreateAsync(_bootstrapAssetAddresser.BootstrapPrefabAddress);
    }
}
