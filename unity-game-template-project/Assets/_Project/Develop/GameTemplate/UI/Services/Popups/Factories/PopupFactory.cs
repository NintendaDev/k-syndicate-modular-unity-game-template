using Cysharp.Threading.Tasks;
using GameTemplate.UI.Services.Popups.Factories;
using GameTemplate.UI.Services.Popups.Simple;
using System;

namespace GameTemplate.UI.Serices.Popups.Factories
{
    public class PopupFactory : IPopupFactory
    {
        private readonly InfoPopupFabric _infoPopupFactory;
        private readonly ErrorPopupFabric _errorPopupFabric;

        public PopupFactory(InfoPopupFabric infoPopupFactory, ErrorPopupFabric errorPopupFabric)
        {
            _infoPopupFactory = infoPopupFactory;
            _errorPopupFabric = errorPopupFabric;
        }

        public async UniTask<SimplePopup> CreateInfoPopup(SimplePopupConfig config) =>
            await CreatePopup(config, _infoPopupFactory.CreateAsync);

        public async UniTask<SimplePopup> CreateErrorPopup(SimplePopupConfig config) =>
            await CreatePopup(config, _errorPopupFabric.CreateAsync);

        private async UniTask<SimplePopup> CreatePopup(SimplePopupConfig config, Func<UniTask<SimplePopup>> popupCreateFunc)
        {
            SimplePopup popup = await popupCreateFunc();
            popup.Initialize(config);

            return popup;
        }
    }
}