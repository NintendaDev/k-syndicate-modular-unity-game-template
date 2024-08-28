using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.Log;
using GameTemplate.UI.LoadingCurtain;

namespace GameTemplate.GameLifeCycle.Gameplay
{
    public class GameplayGameState : GameState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IFastLoadLevel _levelLoader;

        public GameplayGameState(GameStateMachine stateMachine, IEventBus eventBus, ILogService logService,
            ILoadingCurtain loadingCurtain, IFastLoadLevel levelLoader) 
            : base(stateMachine, eventBus, logService)
        {
            _loadingCurtain = loadingCurtain;
            _levelLoader = levelLoader;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.Show();
            await _levelLoader.FastLoadLevelAsync();
            _loadingCurtain.Hide();
        }
    }
}
