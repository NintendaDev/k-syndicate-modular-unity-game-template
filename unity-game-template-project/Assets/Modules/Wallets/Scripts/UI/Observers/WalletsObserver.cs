using System;
using Modules.Wallet.Types;
using Modules.Wallets.Systems;
using Modules.Wallets.UI.Views;

namespace Modules.Wallets.UI.Observers
{
    public sealed class WalletsObserver : IDisposable
    {
        private readonly WalletView _view;
        private readonly IWallet _wallet;

        public WalletsObserver(WalletView view, IWallet wallet, CurrencyType currencyType)
        {
            _view = view;
            _wallet = wallet;

            _wallet.Updated += OnWalletUpdate;
            UpdateView(_wallet.Get(currencyType));
        }

        public void Dispose() =>
            _wallet.Updated -= OnWalletUpdate;

        private void OnWalletUpdate(CurrencyType type, int currentValue) =>
            UpdateView(currentValue);

        private void UpdateView(int currentValue) =>
            _view.SetAmount(currentValue.ToString());
    }
}
