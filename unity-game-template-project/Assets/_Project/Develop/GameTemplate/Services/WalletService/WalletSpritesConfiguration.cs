using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTemplate.Services.Wallet
{
    [CreateAssetMenu(fileName = "new WalletSpritesConfiguration", menuName = "GameTemplate/Wallet/WalletSpritesConfiguration")]
    public class WalletSpritesConfiguration : ScriptableObject
    {
        [ValidateInput(nameof(IsUnique))]
        [SerializeField, RequiredListLength(1, null)] private List<CurrencySprite> _currencySprites;

        public bool IsExistCurrencySprite(CurrencyType currencyType, out Sprite sprite)
        {
            sprite = _currencySprites.Where(x => x.CurrencyType == currencyType)
                .Select(x => x.Sprite)
                .FirstOrDefault();

            return sprite != null;
        }

        private bool IsUnique(List<CurrencySprite> rewardsSprites, ref string errorMessage)
        {
            errorMessage = "Currency sprites is not unique";

            return rewardsSprites.GroupBy(x => x.CurrencyType).Count() == rewardsSprites.Count();
        }

        [System.Serializable]
        public class CurrencySprite
        {
            [field: SerializeField, Required] public CurrencyType CurrencyType;

            [field: SerializeField, Required] public Sprite Sprite;
        }
    }
}
