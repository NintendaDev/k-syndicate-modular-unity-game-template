using GameTemplate.Infrastructure.StateMachineComponents;
using Modules.StateMachines;
using Zenject;

namespace GameTemplate.Installers
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