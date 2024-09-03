using GameTemplate.Services.Localization;
using System;

namespace GameTemplate.Infrastructure.LanguageSystem.Processors
{
    public class LocalizedTermProcessor : IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private string _term;
        private Action<string> _onChangeLocalizationCallback;
        private bool _isInitialized;

        public LocalizedTermProcessor(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _localizationService.LocalizationChanged += OnLocalizationChange;
        }

        public void Dispose() =>
            _localizationService.LocalizationChanged -= OnLocalizationChange;

        public void Initialize(string term, Action<string> onChangeLocalizationCallback)
        {
            _term = term;
            _onChangeLocalizationCallback = onChangeLocalizationCallback;
            _isInitialized = true;

            CallLocalizationCallback();
        }

        private void CallLocalizationCallback()
        {
            if (_isInitialized == false)
                return;

            _onChangeLocalizationCallback.Invoke(_localizationService.MakeTranslatedTextByTerm(_term));
        }

        private void OnLocalizationChange() =>
            CallLocalizationCallback();
    }
}
