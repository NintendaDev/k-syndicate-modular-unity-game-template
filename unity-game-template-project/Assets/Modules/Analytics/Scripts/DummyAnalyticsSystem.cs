using Modules.Analytics.Types;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;

namespace Modules.Analytics
{
    public sealed class DummyAnalyticsSystem : AnalyticsSystem
    {
        public DummyAnalyticsSystem(ILogSystem logSystem, IStaticDataService staticDataService) 
            : base(logSystem, staticDataService)
        {
        }

        public override void SendDesignEvent(DesignEventData eventData)
        {
            MakeLogMessage($"Event sended: {eventData.GetEventName()}");
        }
    }
}
