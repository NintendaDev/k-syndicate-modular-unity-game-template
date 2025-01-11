using System;
using Modules.Advertisements.Types;
using Modules.Analytics.Types;

namespace Modules.Analytics.Utils
{
    public static class AnalyticsEventCodeConvertors
    {
        public static AnalyticsEventCode ToAnalyticsEventCode(this AdvertisementAction advertisementAction)
        {
            switch (advertisementAction)
            {
                case AdvertisementAction.Clicked:
                    return AnalyticsEventCode.AdvertisementClick;
                
                case AdvertisementAction.Request:
                    return AnalyticsEventCode.AdvertisementRequest;
                
                case AdvertisementAction.Show:
                    return AnalyticsEventCode.AdvertisementShow;
                
                case AdvertisementAction.FailedShow:
                    return AnalyticsEventCode.AdvertisementFailedShow;
                
                case AdvertisementAction.RewardReceived:
                    return AnalyticsEventCode.AdvertisementRewardReceived;
                
                default:
                    throw new Exception($"Unknown advertisement action {advertisementAction}");
            }
        }

        public static AnalyticsEventCode ToAnalyticsEventCode(this ProgressStatus progressStatus)
        {
            switch (progressStatus)
            {
                case ProgressStatus.Complete:
                    return AnalyticsEventCode.LevelComplete;
                
                case ProgressStatus.Fail:
                    return AnalyticsEventCode.LevelFail;
                
                case ProgressStatus.Start:
                    return AnalyticsEventCode.LevelStart;
                
                default:
                    throw new Exception($"Unknown progress status: {progressStatus}");
            }
        }
    }
}