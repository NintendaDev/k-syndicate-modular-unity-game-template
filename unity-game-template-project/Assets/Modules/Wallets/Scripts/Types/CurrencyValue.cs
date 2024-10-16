using UnityEngine;

namespace Modules.Wallet.Types
{
    [System.Serializable]
    public class CurrencyValue
    {
        public CurrencyValue(CurrencyType currencyType, long amount) 
        {
            CurrencyType = currencyType;
            Amount = amount;
        }

        [field: SerializeField] public CurrencyType CurrencyType { get; private set; }

        [field: SerializeField] public long Amount { get; private set; }
    }
}
