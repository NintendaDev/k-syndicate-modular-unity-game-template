// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Firebase;
// using Firebase.Analytics;
// using Firebase.Crashlytics;
// using Modules.Advertisements.Types;
// using Modules.Analytics.Types;
// using Modules.Analytics.Utils;
// using Modules.AssetsManagement.StaticData;
// using Modules.Logging;
// using UnityEngine;
// using LogLevel = Modules.Logging.LogLevel;
//
// namespace Modules.Analytics.FirebaseSystem
// {
//     public sealed class FirebaseAnalyticsSystem : AnalyticsSystem, IAdRevenueAnalytics
//     {
//         public FirebaseAnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService) 
//             : base(logSystem, staticDataService)
//         {
//         }
//
//         public override async UniTask InitializeAsync()
//         {
//             await base.InitializeAsync();
//
//             DependencyStatus dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
//             
//             if (dependencyStatus == DependencyStatus.Available)
//             {
//                 Crashlytics.ReportUncaughtExceptionsAsFatal = true;
//             }
//             else
//             {
//                 UnityEngine.Debug.LogError(System.String.Format(
//                     "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
//             }
//             
//             LogSystem.SetPrefix("[Firebase] ");
//         }
//
//         public override void SendCustomEvent(AnalyticsEventCode eventCode)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
//                 return;
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(eventCode);
//
//                 return;
//             }
//             
//             FirebaseAnalytics.LogEvent(eventName);
//             LogEvent(eventCode);
//         }
//
//         public override void SendCustomEvent(AnalyticsEventCode eventCode, Dictionary<string, object> data)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
//                 return;
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(eventCode);
//
//                 return;
//             }
//             
//             FirebaseAnalytics.LogEvent(eventName, new Parameter(DefaultParamsNames.Value, data));
//             LogEvent(eventCode);
//         }
//
//         public override void SendCustomEvent(AnalyticsEventCode eventCode, float value)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName) == false)
//                 return;
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(eventCode);
//
//                 return;
//             }
//             
//             FirebaseAnalytics.LogEvent(eventName, DefaultParamsNames.Value, value);
//             LogEvent(eventCode);
//         }
//
//         public override void SendErrorEvent(LogLevel logLevel, string message)
//         {
//             if (Application.isEditor)
//             {
//                 LogEvent(logLevel, message);
//
//                 return;
//             }
//             
//             Crashlytics.Log($"[{logLevel}]: {message}");
//             LogEvent(logLevel, message);
//         }
//
//         public override void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent)
//         {
//             if (IsExistEventName(progressStatus.ToAnalyticsEventCode(), AnalyticsSystemCode.GameAnalytics, 
//                     out string eventName))
//             {
//                 return;
//             }
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(progressStatus, string.Empty, levelName, progressPercent);
//
//                 return;
//             }
//             
//             Parameter[] parameters = new[]
//             {
//                 new Parameter(DefaultParamsNames.ProgressStatus, progressStatus.ToString()),
//                 new Parameter(DefaultParamsNames.LevelName, levelName),
//                 new Parameter(DefaultParamsNames.ProgressPercent, progressPercent),
//             };
//                 
//             FirebaseAnalytics.LogEvent(eventName, parameters);
//             LogEvent(progressStatus, string.Empty, levelName, progressPercent);
//         }
//
//         public override void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName, 
//             int progressPercent)
//         {
//             if (IsExistEventName(progressStatus.ToAnalyticsEventCode(), AnalyticsSystemCode.GameAnalytics,
//                     out string eventName))
//             {
//                 return;
//             }
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(progressStatus, levelType, levelName, progressPercent);
//
//                 return;
//             }
//             
//             Parameter[] parameters = new[]
//             {
//                 new Parameter(DefaultParamsNames.ProgressStatus, progressStatus.ToString()),
//                 new Parameter(DefaultParamsNames.LevelType, levelType),
//                 new Parameter(DefaultParamsNames.LevelName, levelName),
//                 new Parameter(DefaultParamsNames.ProgressPercent, progressPercent),
//             };
//                 
//             FirebaseAnalytics.LogEvent(eventName, parameters);
//             LogEvent(progressStatus, levelType, levelName, progressPercent);
//         }
//
//         public override void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
//             AdvertisementsPlatform platform)
//         {
//             if (IsExistEventName(advertisementAction.ToAnalyticsEventCode(), AnalyticsSystemCode.GameAnalytics,
//                     out string eventName))
//             {
//                 return;
//             }
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(advertisementAction, placement, platform);
//
//                 return;
//             }
//
//             Parameter[] parameters = new[]
//             {
//                 new Parameter(DefaultParamsNames.AdvertisementType, DefaultParamsNames.Reward),
//                 new Parameter(DefaultParamsNames.AdvertisementPlacement, placement.ToString()),
//                 new Parameter(DefaultParamsNames.AdvertisementPlatform, platform.ToString()),
//             };
//                 
//             FirebaseAnalytics.LogEvent(eventName, parameters);
//             LogEvent(advertisementAction, placement, platform);
//         }
//
//         public override void SendInterstitialEvent(AdvertisementAction advertisementAction, 
//             AdvertisementPlacement placement, AdvertisementsPlatform platform)
//         {
//             if (IsExistEventName(advertisementAction.ToAnalyticsEventCode(), AnalyticsSystemCode.GameAnalytics,
//                     out string eventName))
//             {
//                 return;
//             }
//             
//             if (Application.isEditor)
//             {
//                 LogEvent(advertisementAction, placement, platform);
//
//                 return;
//             }
//
//             Parameter[] parameters = new[]
//             {
//                 new Parameter(DefaultParamsNames.AdvertisementType, DefaultParamsNames.Interstitial),
//                 new Parameter(DefaultParamsNames.AdvertisementPlacement, placement.ToString()),
//                 new Parameter(DefaultParamsNames.AdvertisementPlatform, platform.ToString()),
//             };
//                 
//             FirebaseAnalytics.LogEvent(eventName, parameters);
//             LogEvent(advertisementAction, placement, platform);
//         }
//
//         public void SendAdvertisementRevenueEvent(AdvertisementRevenue revenue)
//         {
//             if (Application.isEditor)
//             {
//                 LogEvent(revenue);
//
//                 return;
//             }
//             
//             List<Parameter> impressionParameters = new();
//             
//             impressionParameters.Add(new Parameter(DefaultParamsNames.Value, revenue.Revenue));
//             impressionParameters.Add(new Parameter(DefaultParamsNames.Currency, revenue.Currency));
//             
//             if (string.IsNullOrEmpty(revenue.Platform) == false)
//                 impressionParameters.Add(new Parameter(DefaultParamsNames.AdvertisementPlatform, revenue.Platform));
//             
//             if (string.IsNullOrEmpty(revenue.Source) == false)
//                 impressionParameters.Add(new Parameter(DefaultParamsNames.AdvertisementSource, revenue.Source));
//             
//             if (string.IsNullOrEmpty(revenue.AdvertisementUnitName) == false)
//                 impressionParameters.Add(new Parameter(DefaultParamsNames.AdvertisementUnitName, 
//                     revenue.AdvertisementUnitName));
//             
//             if (string.IsNullOrEmpty(revenue.Format) == false)
//                 impressionParameters.Add(new Parameter(DefaultParamsNames.AdvertisementFormat, revenue.Format));
//
//             FirebaseAnalytics.LogEvent(DefaultParamsNames.AdvertisementImpression, impressionParameters.ToArray());
//             LogEvent(revenue);
//         }
//     }
// }