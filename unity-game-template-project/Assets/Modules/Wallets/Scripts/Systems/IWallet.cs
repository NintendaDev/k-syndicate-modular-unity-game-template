using System;
using Modules.Wallet.Types;

namespace Modules.Wallets.Systems
{
    public interface IWallet
    {
        public event Action<CurrencyType, int> Updated;
        
        public bool TrySpend(CurrencyType currency, int price);
        
        public void Add(CurrencyType currencyType, int amount);

        public int Get(CurrencyType currencyType);

        public void Set(CurrencyType currencyType, int amount);
    }
}