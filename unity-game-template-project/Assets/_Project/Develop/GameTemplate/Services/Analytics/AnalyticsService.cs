using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Log;
using GameTemplate.Services.StaticData;

namespace GameTemplate.Services.Analytics
{
    public abstract class AnalyticsService : IAnalyticsService
    {
        private readonly string _logTemplate = "ANALYTICS INFO: {0}";
        private readonly IStaticDataService _staticDataService;

        public AnalyticsService(ILogService logService, IStaticDataService staticDataService)
        {
            LogService = logService;
            _staticDataService = staticDataService;
        }

        protected ILogService LogService { get; }

        protected AnalyticsConfiguration Configuration { get; private set; }

        public virtual void Initialize()
        {
            Configuration = _staticDataService.GetConfiguration<AnalyticsConfiguration>();
        }

        public abstract void SendInterstitialAdvertisementAnalytics(AdvertisementAction advertisementAction, AdvertisementPlacement placement);

        public abstract void SendDesignEvent(DesignEventData eventData);

        protected string MakeLogMessage(string message) => string.Format(_logTemplate, message);
    }
}
