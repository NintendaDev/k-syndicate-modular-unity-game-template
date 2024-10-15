using Modules.Analytics.Types;

namespace Modules.Analytics
{
    public interface IAnalyticsSystem
    {
        public void Initialize();

        public void SendDesignEvent(DesignEventData eventData);
    }
}