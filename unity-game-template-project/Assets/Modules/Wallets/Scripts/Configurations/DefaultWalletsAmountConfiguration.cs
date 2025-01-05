using System.Collections.Generic;
using System.Linq;
using Modules.Wallet.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Wallets.Configurations
{
    [CreateAssetMenu(fileName = "new DefaultWalletsAmountConfiguration", 
        menuName = "Modules/Wallets/DefaultWalletsAmountConfiguration")]
    public sealed class DefaultWalletsAmountConfiguration : ScriptableObject
    {
        [ValidateInput(nameof(IsUniqueCurrency))]
        [SerializeField] private List<CurrencyValue> _currencyValues;

        public IEnumerable<CurrencyValue> WalletsData => _currencyValues;

        private bool IsUniqueCurrency(List<CurrencyValue> _currencyValues, ref string errorMessage)
        {
            errorMessage = "Currencies is not unique";

            return _currencyValues.GroupBy(x => x.CurrencyType).Count() == _currencyValues.Count();
        }
    }
}