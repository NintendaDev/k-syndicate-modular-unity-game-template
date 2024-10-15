using Modules.Analytics;
using Modules.Analytics.Configurations;
using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class AnalyticsSceneState : SceneState
    {
        private readonly IAnalyticsSystem _analyticsSystem;

        public AnalyticsSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem,
            IAnalyticsSystem analyticsSystem, IStaticDataService staticDataService)
            : base(stateMachine, eventBus, logSystem)
        {
            _analyticsSystem = analyticsSystem;
            AnalyticsConfiguration = staticDataService.GetConfiguration<AnalyticsConfiguration>();
        }

        protected AnalyticsConfiguration AnalyticsConfiguration { get; }

        protected void SendAnalyticsEvent(DesignEventData eventData) =>
            _analyticsSystem.SendDesignEvent(eventData);
    }
}
