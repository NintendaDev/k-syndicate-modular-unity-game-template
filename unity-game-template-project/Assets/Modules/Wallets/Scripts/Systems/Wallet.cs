using System;
using Modules.Storage;
using Modules.Wallet.Types;

namespace Modules.Wallets.Systems
{
    public sealed class Wallet : IWallet
    {
        private readonly MultiValueAccounter<CurrencyType> _multiValueAccounter = new();

        public event Action<CurrencyType, int> Updated;

        public bool TrySpend(CurrencyType currencyType, int price)
        {
            if (_multiValueAccounter.TryDecrease(currencyType, price) == false)
                return false;
            
            Updated?.Invoke(currencyType, Get(currencyType));

            return true;
        }

        public void Add(CurrencyType currencyType, int amount)
        {
            _multiValueAccounter.Increase(currencyType, amount);
            
            Updated?.Invoke(currencyType, Get(currencyType));
        }

        public int Get(CurrencyType currencyType) => _multiValueAccounter.Get(currencyType);

        public void Set(CurrencyType currencyType, int amount)
        {
            _multiValueAccounter.Set(currencyType, amount);
            
            Updated?.Invoke(currencyType, Get(currencyType));
        }
    }
}