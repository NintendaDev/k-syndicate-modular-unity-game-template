using System;
using Modules.Advertisements.Types;

namespace Modules.Advertisements.Core.Modules.Advertisements.Scripts.Core.Utils
{
    public sealed class AdvertisementRevenueBuilder
    {
        private AdvertisementsPlatform _platform;
        private string _source;
        private string _unitName;
        private string _format;
        private double _revenue;
        private RevenueCurrency _currency;

        public AdvertisementRevenueBuilder()
        {
            Clear();
        }

        public AdvertisementRevenueBuilder SetPlatform(AdvertisementsPlatform platform)
        {
            _platform = platform;

            return this;
        }
        
        public AdvertisementRevenueBuilder SetSource(string source)
        {
            _source = source;

            return this;
        }
        
        public AdvertisementRevenueBuilder SetAdvertisementUnitName(string advertisementUnitName)
        {
            _unitName = advertisementUnitName;

            return this;
        } 
        
        public AdvertisementRevenueBuilder SetFormat(string format)
        {
            _format = format;

            return this;
        } 
        
        public AdvertisementRevenueBuilder SetRevenue(double revenue)
        {
            _revenue = revenue;

            return this;
        } 
        
        public AdvertisementRevenueBuilder SetCurrency(RevenueCurrency currency)
        {
            _currency = currency;

            return this;
        }

        public AdvertisementRevenue Build()
        {
            if (_currency == RevenueCurrency.None)
                throw new Exception("Currency cannot be None");
            
            if (_revenue <= 0)
                throw new Exception("Revenue cannot be less or equal to zero");

            string platformName = (_platform != AdvertisementsPlatform.None) ? _platform.ToString() : string.Empty;
                
            var revenue = new AdvertisementRevenue(platformName, _source, _unitName, _format, _revenue, 
                _currency.ToString());
            
            Clear();

            return revenue;
        }

        public void Clear()
        {
            _platform = AdvertisementsPlatform.None;
            _source = string.Empty;
            _unitName = string.Empty;
            _format = string.Empty;
            _revenue = 0;
            _currency = RevenueCurrency.None;
        }
    }
}