using Cysharp.Threading.Tasks;
using Game.GameLifeCycle.Gameplay;
using Game.Infrastructure.StateMachineComponents;
using Game.Infrastructure.StateMachineComponents.States;
using Modules.EventBus;
using Modules.LoadingCurtain;
using Modules.Logging;

namespace Game.GameLifeCycle.GameHub.States
{
    public sealed class FinishSceneState : SceneState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly GameStateMachine _gameStateMachine;

        public FinishSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem,
            ILoadingCurtain loadingCurtain, GameStateMachine gameStateMachine) 
            : base(stateMachine, signalBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
        }

        public override async UniTask Enter()
        {
            _loadingCurtain.ShowWithoutProgressBar();
            await _gameStateMachine.SwitchState<GameplayGameState>();
        }
    }
}