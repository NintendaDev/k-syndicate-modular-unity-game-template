using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.GameLevelLoader;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Gameplay
{
    public sealed class GameplayGameState : GameState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IFastLoadLevel _levelLoader;

        public GameplayGameState(GameStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem,
            ILoadingCurtain loadingCurtain, IFastLoadLevel levelLoader) 
            : base(stateMachine, signalBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _levelLoader = levelLoader;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.ShowWithoutProgressBar();
            await _levelLoader.FastLoadLevelAsync();
            _loadingCurtain.Hide();
        }
    }
}
