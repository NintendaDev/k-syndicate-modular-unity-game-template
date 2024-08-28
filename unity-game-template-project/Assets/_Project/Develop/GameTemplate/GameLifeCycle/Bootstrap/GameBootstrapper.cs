using Cysharp.Threading.Tasks;
using ExternalLibs.CoreStateMachine;
using GameTemplate.GameLifeCycle.Bootstrap;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.GameLifeCycle.Gameplay;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using UnityEngine;
using Zenject;

namespace GameTemplate.Infrastructure.Bootstrappers
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        void Construct(GameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _gameStateMachine.RegisterState(_statesFactory.Create<BootstrapGameState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoadingState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameHubGameState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameplayGameState>());

            _gameStateMachine.SwitchState<BootstrapGameState>().Forget();
        }
    }
}
