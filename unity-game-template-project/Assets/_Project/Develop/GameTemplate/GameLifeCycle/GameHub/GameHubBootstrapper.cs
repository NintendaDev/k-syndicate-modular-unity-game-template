using Cysharp.Threading.Tasks;
using ExternalLibs.CoreStateMachine;
using GameTemplate.Infrastructure.StateMachineComponents;
using Zenject;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public class GameHubBootstrapper : IInitializable
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