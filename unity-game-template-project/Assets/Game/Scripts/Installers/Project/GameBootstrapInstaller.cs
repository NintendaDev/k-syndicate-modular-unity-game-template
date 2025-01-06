using Game.Infrastructure.Bootstrap;
using Zenject;

namespace Game.Installers.Project
{
    public class GameBootstrapInstaller : Installer<GameBootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrapperFactory>().AsSingle();
        }
    }
}