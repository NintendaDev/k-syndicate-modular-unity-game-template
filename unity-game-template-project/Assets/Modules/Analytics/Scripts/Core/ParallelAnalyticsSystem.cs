using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.Advertisements.Types;
using Modules.Analytics.Types;
using Modules.Logging;
using Sirenix.Utilities;

namespace Modules.Analytics
{
    public sealed class ParallelAnalyticsSystem : IAnalyticsSystem, IAdRevenueAnalytics
    {
        private readonly IAnalyticsSystem[] _analyticsSystem;

        public ParallelAnalyticsSystem(params IAnalyticsSystem[] analyticsSystem)
        {
            _analyticsSystem = analyticsSystem;
        }
        
        public async UniTask InitializeAsync()
        {
            UniTask[] tasks = _analyticsSystem.Select(x => x.InitializeAsync()).ToArray();
            await UniTask.WhenAll(tasks);
        }

        public void SendCustomEvent(AnalyticsEventCode eventCode)
        {
            _analyticsSystem.ForEach(x => x.SendCustomEvent(eventCode));
        }

        public void SendCustomEvent(AnalyticsEventCode eventCode, Dictionary<string, object> data)
        {
            _analyticsSystem.ForEach(x => x.SendCustomEvent(eventCode, data));
        }

        public void SendCustomEvent(AnalyticsEventCode eventCode, float value)
        {
            _analyticsSystem.ForEach(x => x.SendCustomEvent(eventCode, value));
        }

        public void SendInterstitialEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            _analyticsSystem.ForEach(x => 
                x.SendInterstitialEvent(advertisementAction, placement, platform));
        }

        public void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            _analyticsSystem.ForEach(x => 
                x.SendRewardEvent(advertisementAction, placement, platform));
        }

        public void SendErrorEvent(LogLevel logLevel, string message)
        {
            _analyticsSystem.ForEach(x => x.SendErrorEvent(logLevel, message));
        }

        public void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent)
        {
            _analyticsSystem.ForEach(x =>
                x.SendProgressEvent(progressStatus, levelName, progressPercent));
        }

        public void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName, 
            int progressPercent)
        {
            _analyticsSystem.ForEach(x =>
                x.SendProgressEvent(progressStatus, levelType, levelName, progressPercent));
        }

        public void SendAdvertisementRevenueEvent(AdvertisementRevenue revenue)
        {
            foreach (IAnalyticsSystem analyticsSystem in _analyticsSystem)
            {
                if (analyticsSystem is IAdRevenueAnalytics adRevenueAnalytics)
                    adRevenueAnalytics.SendAdvertisementRevenueEvent(revenue);
            }
        }
    }
}