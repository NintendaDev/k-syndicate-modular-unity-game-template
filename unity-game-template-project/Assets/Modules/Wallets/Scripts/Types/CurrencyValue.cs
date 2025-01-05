using UnityEngine;

namespace Modules.Wallet.Types
{
    [System.Serializable]
    public class CurrencyValue
    {
        public CurrencyValue(CurrencyType currencyType, int amount) 
        {
            CurrencyType = currencyType;
            Amount = amount;
        }

        [field: SerializeField] public CurrencyType CurrencyType { get; private set; }

        [field: SerializeField] public int Amount { get; private set; }
    }
}
