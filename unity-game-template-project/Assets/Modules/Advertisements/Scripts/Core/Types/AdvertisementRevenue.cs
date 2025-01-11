namespace Modules.Advertisements.Types
{
    public struct AdvertisementRevenue
    {
        public AdvertisementRevenue(string platform, string source, string advertisementUnitName, string format, double revenue, 
            string currency)
        {
            Platform = platform;
            Source = source;
            AdvertisementUnitName = advertisementUnitName;
            Format = format;
            Revenue = revenue;
            Currency = currency;
        }

        public string Platform { get;}
        
        public string Source { get; }
        
        public string AdvertisementUnitName { get; }
        
        public string Format { get; }
        
        public double Revenue { get; }

        public string Currency { get; }
    }
}