using Cysharp.Threading.Tasks;
using Game.Infrastructure.StateMachineComponents;
using Modules.StateMachines;
using Zenject;

namespace Game.GameLifeCycle.GameHub
{
    public sealed class GameHubBootstrapper : IInitializable
    {
        private readonly SceneStateMachine _sceneStateMachine;
        private readonly StatesFactory _statesFactory;

        public GameHubBootstrapper(SceneStateMachine sceneStateMachine, StatesFactory statesFactory)
        {
            _sceneStateMachine = sceneStateMachine;
            _statesFactory = statesFactory;
        }

        public void Initialize()
        {
            _sceneStateMachine.RegisterState(_statesFactory.Create<BootstrapSceneState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<MainSceneState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<AuthorizationSceneState>());
            _sceneStateMachine.SwitchState<BootstrapSceneState>().Forget();
        }
    }
}