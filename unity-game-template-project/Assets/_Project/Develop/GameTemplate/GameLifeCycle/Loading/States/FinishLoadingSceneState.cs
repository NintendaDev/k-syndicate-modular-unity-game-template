using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.Analytics;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class FinishLoadingSceneState : AnalyticsGameState
    {
        public FinishLoadingSceneState(GameStateMachine stateMachine, ILogSystem logSystem, IEventBus eventBus,
            IAnalyticsService analyticsService, IStaticDataService staticDataService) 
            : base(stateMachine, eventBus, logSystem, analyticsService, staticDataService)
        {
        }

        public override async UniTask Enter()
        {
            await base.Enter();
            await StateMachine.SwitchState<GameHubGameState>();
            SendAnalyticsEvent(AnalyticsConfiguration.MainMenuStageEvent);
        }
    }
}
