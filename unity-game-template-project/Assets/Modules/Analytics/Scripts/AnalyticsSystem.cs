using Modules.Analytics.Configurations;
using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;

namespace Modules.Analytics
{
    public abstract class AnalyticsSystem : IAnalyticsSystem
    {
        private readonly string _logTemplate = "ANALYTICS INFO: {0}";
        private readonly IStaticDataService _staticDataService;

        public AnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService)
        {
            LogSystem = logSystem;
            _staticDataService = staticDataService;
        }

        protected ILogSystem LogSystem { get; }

        protected AnalyticsConfiguration Configuration { get; private set; }

        public virtual void Initialize()
        {
            Configuration = _staticDataService.GetConfiguration<AnalyticsConfiguration>();
        }

        public abstract void SendDesignEvent(DesignEventData eventData);

        protected string MakeLogMessage(string message) => string.Format(_logTemplate, message);
    }
}
