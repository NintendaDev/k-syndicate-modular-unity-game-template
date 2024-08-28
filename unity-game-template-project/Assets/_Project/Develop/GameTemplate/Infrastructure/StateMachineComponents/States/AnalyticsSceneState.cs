using GameTemplate.Infrastructure.Signals;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.Log;
using GameTemplate.Services.StaticData;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class AnalyticsSceneState : SceneState
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogService logService,
            IAnalyticsService analyticsService, IStaticDataService staticDataService)
            : base(stateMachine, eventBus, logService)
        {
            _analyticsService = analyticsService;
            AnalyticsConfiguration = staticDataService.GetConfiguration<AnalyticsConfiguration>();
        }

        protected AnalyticsConfiguration AnalyticsConfiguration { get; }

        protected void SendAnalyticsEvent(DesignEventData eventData) =>
            _analyticsService.SendDesignEvent(eventData);
    }
}
