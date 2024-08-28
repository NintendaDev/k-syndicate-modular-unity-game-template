using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Log;
using GameTemplate.Services.StaticData;

namespace GameTemplate.Services.Analytics
{
    public class DummyAnalyticsService : AnalyticsService
    {
        public DummyAnalyticsService(ILogService logService, IStaticDataService staticDataService) 
            : base(logService, staticDataService)
        {
        }

        public override void SendDesignEvent(DesignEventData eventData)
        {
            MakeLogMessage($"Event sended: {eventData.GetEventName()}");
        }

        public override void SendInterstitialAdvertisementAnalytics(AdvertisementAction advertisementAction, AdvertisementPlacement placement)
        {
            MakeLogMessage($"Interstitial advertisement event sended: {advertisementAction}");
        }
    }
}
