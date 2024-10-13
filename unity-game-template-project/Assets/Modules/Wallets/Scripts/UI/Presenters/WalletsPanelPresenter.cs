using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Wallet.Types;
using Modules.Wallets.Systems;
using Modules.Wallets.UI.Factories;
using Modules.Wallets.UI.Views;

namespace Modules.Wallets.UI.Presenters
{
    public class WalletsPanelPresenter
    {
        private readonly IWalletPanelView _walletsPanelView;
        private readonly IWallet _wallet;
        private readonly WalletViewFactory _walletViewFactory;
        private CancellationTokenSource _cancellationTokenSource;

        public WalletsPanelPresenter(IWalletPanelView walletsPanelView, IWallet wallet, 
            WalletViewFactory walletViewFactory)
        {
            _walletsPanelView = walletsPanelView;
            _wallet = wallet;
            _walletViewFactory = walletViewFactory;

            UpdateWalletViewsAsync().Forget();
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

            foreach (CurrencyType currencyType in _wallet.AvailableTypes)
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
