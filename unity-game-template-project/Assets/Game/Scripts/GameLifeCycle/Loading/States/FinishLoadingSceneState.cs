using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using Modules.Analytics;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading
{
    public sealed class FinishLoadingSceneState : AnalyticsGameState
    {
        public FinishLoadingSceneState(GameStateMachine stateMachine, ILogSystem logSystem, ISignalBus signalBus,
            IAnalyticsSystem analyticsSystem, IStaticDataService staticDataService) 
            : base(stateMachine, signalBus, logSystem, analyticsSystem, staticDataService)
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
