using GameTemplate.Services.Analytics;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class AnalyticsSceneState : SceneState
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem,
            IAnalyticsService analyticsService, IStaticDataService staticDataService)
            : base(stateMachine, eventBus, logSystem)
        {
            _analyticsService = analyticsService;
            AnalyticsConfiguration = staticDataService.GetConfiguration<AnalyticsConfiguration>();
        }

        protected AnalyticsConfiguration AnalyticsConfiguration { get; }

        protected void SendAnalyticsEvent(DesignEventData eventData) =>
            _analyticsService.SendDesignEvent(eventData);
    }
}
