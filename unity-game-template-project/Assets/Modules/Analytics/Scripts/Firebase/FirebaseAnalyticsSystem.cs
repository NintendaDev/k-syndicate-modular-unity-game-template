// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Firebase;
// using Firebase.Analytics;
// using Firebase.Crashlytics;
// using Modules.Analytics.Types;
// using Modules.AssetsManagement.StaticData;
// using Modules.Logging;
//
// namespace Modules.Analytics.FirebaseSystem
// {
//     public sealed class FirebaseAnalyticsSystem : AnalyticsSystem
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
//         }
//
//         public override void SendCustomEvent(EventCode eventCode)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName))
//                 return;
//             
//             FirebaseAnalytics.LogEvent(eventName);
//         }
//
//         public override void SendCustomEvent(EventCode eventCode, Dictionary<string, object> data)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName))
//                 return;
//             
//             FirebaseAnalytics.LogEvent(eventName, new Parameter(DefaultParameterName, data));
//         }
//
//         public override void SendCustomEvent(EventCode eventCode, float value)
//         {
//             if (IsExistEventName(eventCode, AnalyticsSystemCode.GameAnalytics, out string eventName))
//                 return;
//             
//             FirebaseAnalytics.LogEvent(eventName, DefaultParameterName, value);
//         }
//     }
// }