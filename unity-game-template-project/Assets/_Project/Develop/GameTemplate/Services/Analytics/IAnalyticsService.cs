using GameTemplate.Services.Advertisiments;

namespace GameTemplate.Services.Analytics
{
    public interface IAnalyticsService
    {
        public void Initialize();

        public void SendInterstitialAdvertisementAnalytics(AdvertisementAction advertisementAction, AdvertisementPlacement placement);

        public void SendDesignEvent(DesignEventData eventData);
    }
}