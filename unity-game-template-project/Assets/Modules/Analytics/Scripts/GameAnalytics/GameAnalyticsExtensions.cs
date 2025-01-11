using GameAnalyticsSDK;
using Modules.Advertisements.Types;
using Modules.Analytics.Types;
using Modules.Logging;

namespace Modules.Analytics.GA
{
    public static class GameAnalyticsExtensions
    {
        public static GAProgressionStatus ToGameAnalytics(this ProgressStatus status)
        {
            switch (status)
            {
                case ProgressStatus.Start:
                    return GAProgressionStatus.Start;

                case ProgressStatus.Complete:
                    return GAProgressionStatus.Complete;

                case ProgressStatus.Fail:
                    return GAProgressionStatus.Fail;

                default:
                    throw new System.Exception($"Unsupported progress status: {status}");
            }
        }

        public static GAAdAction ToGameAnalytics(this AdvertisementAction advertisimentAction)
        {
            switch (advertisimentAction)
            {
                case AdvertisementAction.Clicked:
                    return GAAdAction.Clicked;

                case AdvertisementAction.Request:
                    return GAAdAction.Request;

                case AdvertisementAction.RewardReceived:
                    return GAAdAction.RewardReceived;

                case AdvertisementAction.FailedShow:
                    return GAAdAction.FailedShow;

                case AdvertisementAction.Show:
                    return GAAdAction.Show;

                default:
                    throw new System.Exception($"Unsupported advertisiment action: {advertisimentAction}");
            }
        }
        
        public static GAErrorSeverity ToGameAnalytics(this LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    return GAErrorSeverity.Info;

                case LogLevel.Warning:
                    return GAErrorSeverity.Warning;

                case LogLevel.Critical:
                    return GAErrorSeverity.Critical;
                
                case LogLevel.Debug:
                    return GAErrorSeverity.Debug;
                
                case LogLevel.Error:
                    return GAErrorSeverity.Error;

                default:
                    throw new System.Exception($"Unsupported event type: {logLevel}");
            }
        }
        
        public static GAResourceFlowType ToGameAnalytics(this ResourceFlowType resourceFlowType)
        {
            switch (resourceFlowType)
            {
                case ResourceFlowType.Sink:
                    return GAResourceFlowType.Sink;

                case ResourceFlowType.Source:
                    return GAResourceFlowType.Source;

                case ResourceFlowType.Undefined:
                    return GAResourceFlowType.Undefined;

                default:
                    throw new System.Exception($"Unsupported event type: {resourceFlowType}");
            }
        }
    }
}