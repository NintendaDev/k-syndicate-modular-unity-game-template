using Modules.StateMachines;
using Zenject;

namespace Game.Infrastructure.StateMachineComponents.Installers
{
    public sealed class GameStateMachineInstaller : Installer<GameStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StatesFactory>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
        }
    }
}