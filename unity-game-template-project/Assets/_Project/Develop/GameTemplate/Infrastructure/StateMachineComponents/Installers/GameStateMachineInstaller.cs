using ExternalLibs.CoreStateMachine;
using Zenject;

namespace GameTemplate.Infrastructure.StateMachineComponents.Installers
{
    public class GameStateMachineInstaller : Installer<GameStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
        }
    }
}