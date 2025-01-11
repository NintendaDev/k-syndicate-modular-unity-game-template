using Modules.Advertisements.Types;

namespace Modules.Analytics
{
    public interface IAdRevenueAnalytics
    {
        public void SendAdvertisementRevenueEvent(AdvertisementRevenue revenue);
    }
}