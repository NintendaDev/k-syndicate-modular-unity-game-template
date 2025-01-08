using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Modules.Analytics.Configurations;
using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;
using Modules.Advertisements.Types;
using Modules.Wallet.Types;

namespace Modules.Analytics
{
    public abstract class AnalyticsSystem : IAnalyticsSystem
    {
        private readonly StringBuilder _builder = new();
        private readonly IStaticDataService _staticDataService;
        private CustomEventsConfiguration _configuration;

        public AnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService)
        {
            LogSystem = new TemplateLogger(logSystem);
            _staticDataService = staticDataService;
        }

        protected TemplateLogger LogSystem { get; }

        protected string DefaultParameterName => _configuration.DefaultParameterName;
        
        public virtual UniTask InitializeAsync()
        {
            _configuration = _staticDataService.GetConfiguration<CustomEventsConfiguration>();

            return UniTask.CompletedTask;
        }
        
        public abstract void SendCustomEvent(EventCode eventCode);

        public abstract void SendCustomEvent(EventCode eventCode, Dictionary<string, object> data);
        
        public abstract void SendCustomEvent(EventCode eventCode, float value);

        public abstract void SendInterstitialEvent(AdvertisementAction advertisementAction,
            AdvertisementPlacement placement, AdvertisementsPlatform platform);

        public abstract void SendRewardEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform);
        
        public abstract void SendErrorEvent(LogLevel logLevel, string message);

        public abstract void SendProgressEvent(ProgressStatus progressStatus, string levelName, int progressPercent);

        public abstract void SendProgressEvent(ProgressStatus progressStatus, string levelType, string levelName,
            int progressPercent);

        protected bool IsExistEventName(EventCode eventCode, AnalyticsSystemCode analyticsSystemCode,
            out string eventName)
        {
            bool isExist = _configuration.IsExistEventName(eventCode, analyticsSystemCode, out eventName);
            
            if (isExist == false)
                LogSystem.LogWarning($"The metric name for analyticsSystemCode={analyticsSystemCode} " +
                                     $"and eventCode={eventCode} was not found.");

            return isExist;
        }

        protected void LogEvent(EventCode eventCode)
        {
            _builder.Clear();
            
            _builder.Append($"Custom event have been sent: ");
            _builder.Append($"eventCode={eventCode}");
            LogSystem.Log(_builder.ToString());
        }
        
        protected void LogEvent(LogLevel logLevel, string message)
        {
            _builder.Clear();
            
            _builder.Append($"Error event have been sent: ");
            _builder.Append($"logLevel={logLevel}, ");
            _builder.Append($"message={message}");
            LogSystem.Log(_builder.ToString());
        }
        
        protected void LogEvent(AdvertisementAction advertisementAction, AdvertisementPlacement placement,
            AdvertisementsPlatform platform)
        {
            _builder.Clear();

            _builder.Append("Advertisement statistics have been sent: ");
            _builder.Append($"{nameof(advertisementAction)} = {advertisementAction}, ");
            _builder.Append($"{nameof(platform)}={platform}, ");
            _builder.Append($"{nameof(placement)}={placement}");
            LogSystem.Log(_builder.ToString());
        }
        
        protected void LogEvent(ProgressStatus progressStatus, string levelType, string levelName, 
            int progressPercent)
        {
            _builder.Clear();

            _builder.Append("Progress statistics have been sent: ");
            _builder.Append($"{nameof(progressStatus)} = {progressStatus}, ");
            _builder.Append($"{nameof(levelType)}={levelType}, ");
            _builder.Append($"{nameof(levelName)}={levelName}, ");
            _builder.Append($"{nameof(progressPercent)}={progressPercent}");
            LogSystem.Log(_builder.ToString());
        }
        
        protected void LogEvent(ResourceFlowType flowType, CurrencyType currencyType, float amount, string itemType, 
            string itemId)
        {
            _builder.Clear();

            _builder.Append("Resource statistics have been sent: ");
            _builder.Append($"{nameof(flowType)} = {flowType}, ");
            _builder.Append($"{nameof(currencyType)}={currencyType}, ");
            _builder.Append($"{nameof(amount)}={amount}, ");
            _builder.Append($"{nameof(itemType)}={itemType}, ");
            _builder.Append($"{nameof(itemId)}={itemId}");
            LogSystem.Log(_builder.ToString());
        }
    }
}
