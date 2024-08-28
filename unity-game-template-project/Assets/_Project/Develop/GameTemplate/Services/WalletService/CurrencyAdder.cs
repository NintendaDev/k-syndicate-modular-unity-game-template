using GameTemplate.Services.SaveLoad;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.Services.Wallet
{
    public class CurrencyAdder : MonoBehaviour
    {
        private IWalletService _walletService;
        private ISaveSignal _saveSignaller;

        [Inject]
        private void Construct(IWalletService walletService, ISaveSignal saveSignaller)
        {
            _walletService = walletService;
            _saveSignaller = saveSignaller;
        }

        [Button, DisableInEditorMode]
        public void AddCoins(long count) =>
            Add(CurrencyType.Coin, count);

        private void Add(CurrencyType currencyType, long count)
        {
            _walletService.AddAmount(currencyType, count);
            _saveSignaller.SendSaveSignal();
        }
    }
}
