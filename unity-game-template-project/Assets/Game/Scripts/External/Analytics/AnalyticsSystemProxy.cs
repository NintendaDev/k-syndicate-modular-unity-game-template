using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.Advertisements.Types;
using Modules.Analytics;
using Modules.Analytics.GA;
using Modules.Analytics.Types;
using Modules.Logging;

namespace Game.External.Analytics
{
    public sealed class AnalyticsSystemProxy : IAnalyticsSystem
    {
        private readonly GameAnalyticsSystem _gameAnalyticsSystem;
        private readonly StubAnalyticsSystem _stubAnalyticsSystem;

        public AnalyticsSystemProxy(GameAnalyticsSystem gameAnalyticsSystem, StubAnalyticsSystem stubAnalyticsSystem)
        {
            _gameAnalyticsSystem = gameAnalyticsSystem;
            _stubAnalyticsSystem = stubAnalyticsSystem;
        }

        public async UniTask InitializeAsync()
        {
            await UniTask.WhenAll(_gameAnalyticsSystem.InitializeAsync(), _stubAnalyticsSystem.InitializeAsync());
        }

        public void SendCustomEvent(EventCode eventCode)
        {
            _gameAnalyticsSystem.SendCustomEvent(eventCode);
            _stubAnalyticsSystem.SendCustomEvent(eventCode);
        }

        public void SendCustomEvent(EventCode eventCode, Dictionary<string, object> data)
        {
            _gameAnalyticsSystem.SendCustomEvent(eventCode, data);
            _stubAnalyticsSystem.SendCustomEvent(eventCode, data);
        }

        public void SendCustomEvent(EventCode eventCode, float value)
        {
            _gameAnalyticsSystem.SendCustomEvent(eventCode, value);
            _stubAnalyticsSystem.SendCustomEvent(eventCode, value);
        }

        public void SendInterstitialEvent(AdvertisementAction advertisementAction, 
            AdvertisementPlacement placement, AdvertisementsPlatform platform)
        {
            _gameAnalyticsSystem.SendInterstitialEvent(advertisementAction, placement, platform);
            _stubAnalyticsSystem.SendInterstitialEvent(advertisementAction, placement, platform);
        }

        public void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            _gameAnalyticsSystem.SendRewardEvent(advertisementAction, placement, platform);
            _stubAnalyticsSystem.SendRewardEvent(advertisementAction, placement, platform);
        }

        public void SendErrorEvent(LogLevel logLevel, string message)
        {
            _gameAnalyticsSystem.SendErrorEvent(logLevel, message);
            _stubAnalyticsSystem.SendErrorEvent(logLevel, message);
        }

        public void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent)
        {
            _gameAnalyticsSystem.SendProgressEvent(progressStatus, levelName, progressPercent);
            _stubAnalyticsSystem.SendProgressEvent(progressStatus, levelName, progressPercent);
        }

        public void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName, 
            int progressPercent)
        {
            _gameAnalyticsSystem.SendProgressEvent(progressStatus, levelType, levelName, progressPercent);
            _stubAnalyticsSystem.SendProgressEvent(progressStatus, levelType, levelName, progressPercent);
        }
    }
}