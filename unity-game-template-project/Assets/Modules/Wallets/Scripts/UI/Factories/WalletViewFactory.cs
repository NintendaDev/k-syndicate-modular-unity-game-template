using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.ObjectsManagement.Factories;
using Modules.Wallet.Types;
using Modules.Wallets.Configurations;
using Modules.Wallets.Systems;
using Modules.Wallets.UI.Observers;
using Modules.Wallets.UI.Views;
using UnityEngine;
using Zenject;

namespace Modules.Wallets.UI.Factories
{
    public class WalletViewFactory : PrefabFactoryAsync<WalletView>
    {
        private readonly IWallet _wallet;
        private readonly IStaticDataService _staticDataService;
        private CurrencyType _currencyType;
        private List<IDisposable> _disposablesObjects = new();
        private WalletAssetsConfiguration _walletAssetsConfiguration;

        public WalletViewFactory(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IWallet wallet, IStaticDataService staticDataService) 
            : base(instantiator, componentAssetService)
        {
            _wallet = wallet;
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
            if (_walletAssetsConfiguration == null)
                _walletAssetsConfiguration = _staticDataService.GetConfiguration<WalletAssetsConfiguration>();

            WalletView view = await base.CreateAsync(_walletAssetsConfiguration.WalletViewPrefabReference.AssetGUID);
            
            if (_walletAssetsConfiguration.IsExistCurrencySprite(_currencyType, out Sprite currencySprite))
                view.SetIcon(currencySprite);

            WalletsObserver observer = new(view, _wallet, _currencyType);
            _disposablesObjects.Add(observer);

            return view;
        }
    }
}
