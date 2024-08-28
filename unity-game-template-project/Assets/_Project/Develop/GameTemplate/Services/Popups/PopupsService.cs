using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameTemplate.Services.Localization;
using GameTemplate.UI.Services.Popups.Factories;
using GameTemplate.UI.Services.Popups.Simple;

namespace GameTemplate.Services.Popups
{
    public class PopupsService : IPopupsService, IDisposable
    {
        private readonly IPopupFactory _popupFactory;
        private readonly ILocalizationService _localizationService;
        private CancellationTokenSource _cancellationTokenSource;

        public PopupsService(IPopupFactory popupFactory, ILocalizationService localizationService)
        {
            _popupFactory = popupFactory;
            _localizationService = localizationService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask ShowInfoAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm) =>
           await ShowPopupAsync(headerTerm, messageTerm, buttonTerm, _popupFactory.CreateInfoPopup);

        public async UniTask ShowInfoAsync(string messageHeader, string messageBody, string buttonText = "Ok") =>
            await ShowPopupAsync(messageHeader, messageBody, buttonText, _popupFactory.CreateInfoPopup);

        public async UniTask ShowErrorAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm) =>
           await ShowPopupAsync(headerTerm, messageTerm, buttonTerm, _popupFactory.CreateErrorPopup);

        public async UniTask ShowErrorAsync(string messageHeader, string messageBody, string buttonText = "Ok") =>
            await ShowPopupAsync(messageHeader, messageBody, buttonText, _popupFactory.CreateErrorPopup);

        public void Dispose() => 
            _cancellationTokenSource.Cancel();

        private async UniTask ShowPopupAsync(LocalizationTerm headerTerm, LocalizationTerm messageTerm, LocalizationTerm buttonTerm,
            Func<SimplePopupConfig, UniTask<SimplePopup>> popupCreateFunc)
        {
            await ShowPopupAsync(_localizationService.MakeTranslatedText(headerTerm), _localizationService.MakeTranslatedText(messageTerm),
               _localizationService.MakeTranslatedText(buttonTerm), popupCreateFunc);
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