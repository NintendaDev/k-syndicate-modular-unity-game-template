using Cysharp.Threading.Tasks;
using System;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Simple;

namespace Modules.PopupsSystem.UI.Factories
{
    public sealed class PopupFactory : IPopupFactory
    {
        private readonly InfoPopupFactory _infoPopupFactory;
        private readonly ErrorPopupFactory _errorPopupFactory;

        public PopupFactory(InfoPopupFactory infoPopupFactory, ErrorPopupFactory errorPopupFactory)
        {
            _infoPopupFactory = infoPopupFactory;
            _errorPopupFactory = errorPopupFactory;
        }

        public async UniTask<SimplePopup> CreateInfoPopup(SimplePopupConfig config) =>
            await CreatePopup(config, _infoPopupFactory.CreateAsync);

        public async UniTask<SimplePopup> CreateErrorPopup(SimplePopupConfig config) =>
            await CreatePopup(config, _errorPopupFactory.CreateAsync);

        private async UniTask<SimplePopup> CreatePopup(SimplePopupConfig config, Func<UniTask<SimplePopup>> popupCreateFunc)
        {
            SimplePopup popup = await popupCreateFunc();
            popup.Initialize(config);

            return popup;
        }
    }
}