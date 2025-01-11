using Modules.Analytics;
using Modules.Analytics.Types;
using Modules.EventBus;
using Modules.Logging;

namespace Game.Infrastructure.StateMachineComponents.States
{
    public class AnalyticsGameState : GameState
    {
        private readonly IAnalyticsSystem _analyticsSystem;

        public AnalyticsGameState(GameStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem,
            IAnalyticsSystem analyticsSystem)
            : base(stateMachine, signalBus, logSystem)
        {
            _analyticsSystem = analyticsSystem;
        }

        protected void SendAnalyticsEvent(AnalyticsEventCode eventCode) =>
            _analyticsSystem.SendCustomEvent(eventCode);
    }
}
