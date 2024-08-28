using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Services.StaticData;
using GameTemplate.Services.Wallet;
using System.Collections.Generic;
using System.Threading;

namespace GameTemplate.UI.Wallets
{
    public class WalletsPanelPresenter
    {
        private readonly IWalletPanelView _walletsPanelView;
        private readonly IStaticDataService _staticDataService;
        private readonly IWalletService _walletService;
        private readonly WalletViewFactory _walletViewFactory;
        private GameHubConfiguration _gameHubConfiguration;
        private CancellationTokenSource _cancellationTokenSource;

        public WalletsPanelPresenter(IWalletPanelView walletsPanelView, IStaticDataService staticDataService, 
            IWalletService walletService, WalletViewFactory walletViewFactory)
        {
            _walletsPanelView = walletsPanelView;
            _staticDataService = staticDataService;
            _walletService = walletService;
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

            if (_gameHubConfiguration == null)
                _gameHubConfiguration = _staticDataService.GetConfiguration<GameHubConfiguration>();

            foreach (CurrencyType currencyType in _walletService.AvailableTypes)
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
