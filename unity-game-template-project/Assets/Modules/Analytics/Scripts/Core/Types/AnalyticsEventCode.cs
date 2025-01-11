namespace Modules.Analytics.Types
{
    public enum AnalyticsEventCode
    {
        None = 0,
        AdvertisementRequest = 1,
        AdvertisementClick = 2,
        AdvertisementRewardReceived = 3,
        AdvertisementFailedShow = 4,
        AdvertisementShow = 5,
        LevelStart = 6,
        LevelComplete= 7,
        LevelFail = 8,
        GameBootAuth = 100,
        GameBootGameHub = 101,
        GameBootLoadProgress = 102,
    }
}