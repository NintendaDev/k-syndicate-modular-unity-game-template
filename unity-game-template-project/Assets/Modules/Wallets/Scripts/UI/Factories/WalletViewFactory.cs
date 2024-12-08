using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.AddressablesOperations;
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
    public sealed class WalletViewFactory : IDisposable
    {
        private readonly IWallet _wallet;
        private readonly IStaticDataService _staticDataService;
        private CurrencyType _currencyType;
        private List<IDisposable> _disposablesObjects = new();
        private readonly PrefabFactoryAsync<WalletView> _prefabFactory;
        private WalletAssetsConfiguration _walletAssetsConfiguration;

        public WalletViewFactory(IInstantiator instantiator, IAddressablesService addressablesService,
            IWallet wallet, IStaticDataService staticDataService)
        {
            _wallet = wallet;
            _prefabFactory = new PrefabFactoryAsync<WalletView>(instantiator, addressablesService);
            _staticDataService = staticDataService;
        }

        public void Dispose()
        {
            _disposablesObjects.ForEach(x => x.Dispose());
            _prefabFactory.Dispose();
        }

        public void Initialize(CurrencyType currencyType) =>
            _currencyType = currencyType;

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

            WalletView view = await _prefabFactory.CreateAsync(_walletAssetsConfiguration.WalletViewPrefabReference);
            
            if (_walletAssetsConfiguration.IsExistCurrencySprite(_currencyType, out Sprite currencySprite))
                view.SetIcon(currencySprite);

            WalletsObserver observer = new(view, _wallet, _currencyType);
            _disposablesObjects.Add(observer);

            return view;
        }
    }
}
