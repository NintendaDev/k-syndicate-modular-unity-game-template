using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Localization.Core.Systems;
using Modules.Localization.Core.Types;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Factories;
using Modules.PopupsSystem.UI.Simple;

namespace Modules.PopupsSystem
{
    public sealed class Popups : IPopups, IDisposable
    {
        private readonly IPopupFactory _popupFactory;
        private readonly ILocalizationSystem _localizationSystem;
        private CancellationTokenSource _cancellationTokenSource;

        public Popups(IPopupFactory popupFactory, ILocalizationSystem localizationSystem)
        {
            _popupFactory = popupFactory;
            _localizationSystem = localizationSystem;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask ShowInfoAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm,
            LocalizationTerm buttonTerm)
        {
            await ShowPopupAsync(headerTerm, messageTerm, buttonTerm, _popupFactory.CreateInfoPopup);
        }
        
        public async UniTask ShowInfoAsync(string messageHeader, string messageBody, string buttonText = "Ok") =>
            await ShowPopupAsync(messageHeader, messageBody, buttonText, _popupFactory.CreateInfoPopup);

        public async UniTask ShowErrorAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm,
            LocalizationTerm buttonTerm)
        {
            await ShowPopupAsync(headerTerm, messageTerm, buttonTerm, _popupFactory.CreateErrorPopup);
        }

        public async UniTask ShowErrorAsync(string messageHeader, string messageBody, string buttonText = "Ok") =>
            await ShowPopupAsync(messageHeader, messageBody, buttonText, _popupFactory.CreateErrorPopup);

        public void Dispose() => 
            _cancellationTokenSource.Cancel();

        private async UniTask ShowPopupAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm,
            LocalizationTerm buttonTerm, Func<SimplePopupConfig, UniTask<SimplePopup>> popupCreateFunc)
        {
            await ShowPopupAsync(_localizationSystem.MakeTranslatedText(headerTerm), 
                _localizationSystem.MakeTranslatedText(messageTerm),
               _localizationSystem.MakeTranslatedText(buttonTerm), popupCreateFunc);
        }

        private async UniTask ShowPopupAsync(string messageHeader, string messageBody, string buttonText, 
            Func<SimplePopupConfig, UniTask<SimplePopup>> popupCreateFunc)
        {
            SimplePopupConfig popupConfig = new(messageHeader, messageBody, buttonText);

            SimplePopup popup = await popupCreateFunc(popupConfig);
            await popup.Show().AttachExternalCancellation(_cancellationTokenSource.Token);

            popup.Destroy();
        }
    }
}