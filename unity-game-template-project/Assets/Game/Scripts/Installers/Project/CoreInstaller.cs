using Modules.EventBus;
using Modules.Logging;
using Zenject;

namespace Game.Installers.Project
{
    public class CoreInstaller : Installer<CoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LogSystem>().AsSingle();
            Container.BindInterfacesTo<SignalBus>().AsSingle();
        }
    }
}