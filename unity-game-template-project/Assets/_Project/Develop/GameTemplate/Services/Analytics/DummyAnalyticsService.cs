using GameTemplate.Services.Advertisiments;
using Modules.AssetManagement.StaticData;
using Modules.Logging;

namespace GameTemplate.Services.Analytics
{
    public class DummyAnalyticsService : AnalyticsService
    {
        public DummyAnalyticsService(ILogSystem logSystem, IStaticDataService staticDataService) 
            : base(logSystem, staticDataService)
        {
        }

        public override void SendDesignEvent(DesignEventData eventData)
        {
            MakeLogMessage($"Event sended: {eventData.GetEventName()}");
        }

        public override void SendInterstitialAdvertisementAnalytics(AdvertisementAction advertisementAction, 
            AdvertisementPlacement placement)
        {
            MakeLogMessage($"Interstitial advertisement event sended: {advertisementAction}");
        }
    }
}
