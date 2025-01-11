using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.Advertisements.Types;
using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;

namespace Modules.Analytics.Stub
{
    public sealed class StubAnalyticsSystem : AnalyticsSystem, IAdRevenueAnalytics
    {
        public StubAnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService) 
            : base(logSystem, staticDataService)
        {
        }

        public async override UniTask InitializeAsync()
        {
            await base.InitializeAsync();
            LogSystem.SetPrefix("[Stub Analytics] ");
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode)
        {
            LogEvent(eventCode);
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode, Dictionary<string, object> data)
        {
            LogEvent(eventCode);
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode, float value)
        {
            LogEvent(eventCode);
        }

        public override void SendInterstitialEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            LogEvent(advertisementAction, placement, platform);
        }

        public override void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            LogEvent(advertisementAction, placement, platform);
        }

        public override void SendErrorEvent(LogLevel logLevel, string message)
        {
            LogEvent(logLevel, message);
        }

        public override void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent)
        {
            LogEvent(progressStatus, string.Empty, levelName, progressPercent);
        }

        public override void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName, 
            int progressPercent)
        {
            LogEvent(progressStatus, levelType, levelName, progressPercent);
        }

        public void SendAdvertisementRevenueEvent(AdvertisementRevenue revenue)
        {
            LogEvent(revenue);
        }
    }
}
