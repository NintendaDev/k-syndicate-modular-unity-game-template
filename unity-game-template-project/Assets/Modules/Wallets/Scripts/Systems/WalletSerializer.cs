using System;
using System.Collections.Generic;
using Modules.SaveSystem.SaveLoad;
using Modules.Wallet.Types;

namespace Modules.Wallets.Systems
{
    public sealed class WalletSerializer : GameSerializer<IWallet, WalletData>
    {
        public WalletSerializer(IWallet service) : base(service)
        {
        }

        protected override WalletData Serialize(IWallet service)
        {
            Dictionary<CurrencyType, int> data = new();
            
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType))) 
                data.Add(currencyType, service.Get(currencyType));
            
            return new WalletData(data);
        }

        protected override void Deserialize(IWallet service, WalletData data)
        {
            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                if (data.Data.ContainsKey(currencyType) == false)
                    continue;
                
                service.Set(currencyType, data.Data[currencyType]);
            }
        }
    }
}