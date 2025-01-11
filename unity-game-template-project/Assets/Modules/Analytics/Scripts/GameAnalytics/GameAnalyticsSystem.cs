using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using Modules.Advertisements.Types;
using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;
using Modules.Wallet.Types;
using UnityEngine;

namespace Modules.Analytics.GA
{
    public sealed class GameAnalyticsSystem : AnalyticsSystem, IGameAnalyticsATTListener
    {
        public GameAnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService) 
            : base(logSystem, staticDataService)
        {
        }

        public override async UniTask InitializeAsync()
        {
            await base.InitializeAsync();
            
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                InitializeSDK();
            }
            
            LogSystem.SetPrefix("[GameAnalytics] ");
        }

        public void EnableAppLovinIntegration()
        {
            GameAnalyticsILRD.SubscribeMaxImpressions();
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode)
        {
            if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
                return;

            if (Application.isEditor)
            {
                LogEvent(eventCode);

                return;
            }
            
            GameAnalytics.NewDesignEvent(eventName);
            LogEvent(eventCode);
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode, Dictionary<string, object> data)
        {
            if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
                return;
            
            if (Application.isEditor)
            {
                LogEvent(eventCode);

                return;
            }
            
            GameAnalytics.NewDesignEvent(eventName, data);
            LogEvent(eventCode);
        }

        public override void SendCustomEvent(AnalyticsEventCode eventCode, float value)
        {
            if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
                return;
            
            if (Application.isEditor)
            {
                LogEvent(eventCode);

                return;
            }
            
            GameAnalytics.NewDesignEvent(eventName, value);
            LogEvent(eventCode);
        }

        public override void SendInterstitialEvent(AdvertisementAction advertisementAction,
            AdvertisementPlacement placement, AdvertisementsPlatform platform)
        {
            if (Application.isEditor)
            {
                LogEvent(advertisementAction, placement, platform);

                return;
            }
            
            SendAdvertisementEvent(advertisementAction, placement, platform, GAAdType.Interstitial);
            LogEvent(advertisementAction, placement, platform);
        }
        
        public override void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement, 
            AdvertisementsPlatform platform)
        {
            if (Application.isEditor)
            {
                LogEvent(advertisementAction, placement, platform);

                return;
            }
            
            SendAdvertisementEvent(advertisementAction, placement, platform, GAAdType.RewardedVideo);
            LogEvent(advertisementAction, placement, platform);
        }

        public override void SendErrorEvent(LogLevel logLevel, string message)
        {
            if (Application.isEditor)
            {
                LogEvent(logLevel, message);

                return;
            }
            
            GameAnalytics.NewErrorEvent(logLevel.ToGameAnalytics(), message);
            LogEvent(logLevel, message);
        }

        public override void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent) =>
            SendProgressEvent(progressStatus, string.Empty, levelName, progressPercent);
        
        public override void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName, 
            int progressPercent)
        {
            if (Application.isEditor)
            {
                LogEvent(progressStatus, levelType, levelName, progressPercent);

                return;
            }
            
            GAProgressionStatus gameAnalyticsProgressStatus = progressStatus.ToGameAnalytics();
            
            if (string.IsNullOrEmpty(levelType))
                GameAnalytics.NewProgressionEvent(gameAnalyticsProgressStatus, levelName, progressPercent);
            else
                GameAnalytics.NewProgressionEvent(gameAnalyticsProgressStatus, levelType, 
                    levelName, progressPercent);
            
            LogEvent(progressStatus, levelType, levelName, progressPercent);
        }

        private void SendResourceEvent(ResourceFlowType flowType, CurrencyType currencyType, float amount, 
            string itemType, string itemId)
        {
            if (Application.isEditor)
            {
                LogEvent(flowType, currencyType, amount, itemType, itemId);

                return;
            }

            GameAnalytics.NewResourceEvent(flowType.ToGameAnalytics(), currencyType.ToString(), amount, itemType, 
                itemId);
            
            LogEvent(flowType, currencyType, amount, itemType, itemId);
        }

        private void SendAdvertisementEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform advertisementsPlatform, GAAdType advertisementType)
        {
            if (Application.isEditor)
            {
                LogEvent(advertisementAction, placement, advertisementsPlatform);

                return;
            }

            GameAnalytics.NewAdEvent(advertisementAction.ToGameAnalytics(), advertisementType, 
                advertisementsPlatform.ToString(), placement.ToString());
            
            LogEvent(advertisementAction, placement, advertisementsPlatform);
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            InitializeSDK();
        }

        public void GameAnalyticsATTListenerRestricted()
        {
            InitializeSDK();
        }

        public void GameAnalyticsATTListenerDenied()
        {
            InitializeSDK();
        }

        public void GameAnalyticsATTListenerAuthorized()
        {
            InitializeSDK();
        }

        private void InitializeSDK()
        {
            GameAnalytics.Initialize();
            GameAnalytics.SetBuildAllPlatforms(Application.version);
        } 
    }
}