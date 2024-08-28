using GameTemplate.Services.Wallet;
using System;

namespace GameTemplate.UI.Wallets
{
    public class WalletsObserver : IDisposable
    {
        private readonly WalletView _view;
        private readonly IWalletService _walletService;
        private readonly CurrencyType _currencyType;

        public WalletsObserver(WalletView view, IWalletService walletService, CurrencyType currencyType)
        {
            _view = view;
            _walletService = walletService;
            _currencyType = currencyType;

            _walletService.Updated += OnWalletUpdate;
            UpdateView(_walletService.GetAmount(_currencyType));
        }

        public void Dispose() =>
            _walletService.Updated -= OnWalletUpdate;

        private void OnWalletUpdate(CurrencyType type, long previousValue, long currentValue) =>
            UpdateView(currentValue);

        private void UpdateView(long currentValue) =>
            _view.SetAmount(currentValue.ToString());
    }
}
