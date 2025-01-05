using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Wallet.Types;
using Modules.Wallets.UI.Factories;
using Modules.Wallets.UI.Views;

namespace Modules.Wallets.UI.Presenters
{
    public sealed class WalletsPanelPresenter : IDisposable
    {
        private readonly IWalletPanelView _walletsPanelView;
        private readonly WalletViewFactory _walletViewFactory;
        private CancellationTokenSource _cancellationTokenSource;

        public WalletsPanelPresenter(IWalletPanelView walletsPanelView, WalletViewFactory walletViewFactory)
        {
            _walletsPanelView = walletsPanelView;
            _walletViewFactory = walletViewFactory;

            UpdateWalletViewsAsync().Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask UpdateWalletViewsAsync()
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
            IEnumerable<WalletView> walletViews = await CreateWalletViewsAsync(_cancellationTokenSource.Token);
            _walletsPanelView.Link(walletViews);
        }

        public async UniTask<IEnumerable<WalletView>> CreateWalletViewsAsync(CancellationToken cancellationToken)
        {
            List<WalletView> levelViews = new();

            foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
            {
                if (cancellationToken.IsCancellationRequested)
                    return levelViews;

                _walletViewFactory.Initialize(currencyType);
                levelViews.Add(await _walletViewFactory.CreateAsync());
            }
                
            return levelViews;
        }
    }
}
