using GameTemplate.Infrastructure.Bootstrap;
using Zenject;

namespace GameTemplate.Installers.Project
{
    public class GameBootstrapInstaller : Installer<GameBootstrapInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameBootstrapperFactory>().AsSingle();
        }
    }
}