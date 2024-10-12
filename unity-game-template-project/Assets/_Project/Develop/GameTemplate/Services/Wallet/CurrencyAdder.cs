using Modules.SaveManagement.Interfaces;
using Modules.Wallet.Types;
using Modules.Wallets.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.Services.Wallets
{
    public class CurrencyAdder : MonoBehaviour
    {
        private IWallet _wallet;
        private ISaveSignal _saveSignaller;

        [Inject]
        private void Construct(IWallet wallet, ISaveSignal saveSignaller)
        {
            _wallet = wallet;
            _saveSignaller = saveSignaller;
        }

        [Button, DisableInEditorMode]
        public void AddCoins(long count) =>
            Add(CurrencyType.Coin, count);

        private void Add(CurrencyType currencyType, long count)
        {
            _wallet.AddAmount(currencyType, count);
            _saveSignaller.SendSaveSignal();
        }
    }
}
