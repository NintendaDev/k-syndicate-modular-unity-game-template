using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Services.Wallet;
using System;
using System.Collections.Generic;
using Modules.AssetManagement.StaticData;
using UnityEngine;
using Zenject;

namespace GameTemplate.UI.Wallets
{
    public class WalletViewFactory : PrefabFactoryAsync<WalletView>
    {
        private readonly IWalletService _walletService;
        private readonly IStaticDataService _staticDataService;
        private GameHubConfiguration _gameHubConfiguration;
        private CurrencyType _currencyType;
        private List<IDisposable> _disposablesObjects = new();
        private WalletSpritesConfiguration _walletSpritesConfiguration;

        public WalletViewFactory(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IWalletService walletService, IStaticDataService staticDataService) 
            : base(instantiator, componentAssetService)
        {
            _walletService = walletService;
            _staticDataService = staticDataService;
        }

        public void Initialize(CurrencyType currencyType) =>
            _currencyType = currencyType;

        public override void Dispose()
        {
            base.Dispose();

            _disposablesObjects.ForEach(x => x.Dispose());
        }

        public async UniTask<WalletView> CreateAsync(Transform parent)
        {
            WalletView view = await CreateAsync();
            view.transform.SetParent(parent);

            return view;
        }

        public async UniTask<WalletView> CreateAsync()
        {
            if (_gameHubConfiguration == null)
                _gameHubConfiguration = _staticDataService.GetConfiguration<GameHubConfiguration>();

            if (_walletSpritesConfiguration == null)
                _walletSpritesConfiguration = _staticDataService.GetConfiguration<WalletSpritesConfiguration>();

            WalletView view = await base.CreateAsync(_gameHubConfiguration.WalletViewPrebafReference.AssetGUID);
            
            if (_walletSpritesConfiguration.IsExistCurrencySprite(_currencyType, out Sprite currencySprite))
                view.SetIcon(currencySprite);

            WalletsObserver observer = new(view, _walletService, _currencyType);
            _disposablesObjects.Add(observer);

            return view;
        }
    }
}
