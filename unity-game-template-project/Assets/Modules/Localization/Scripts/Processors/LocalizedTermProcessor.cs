using System;
using Modules.Localization.Systems;

namespace Modules.Localization.Processors
{
    public class LocalizedTermProcessor : IDisposable
    {
        private readonly ILocalizationSystem _localizationSystem;
        private string _term;
        private Action<string> _onChangeLocalizationCallback;
        private bool _isInitialized;

        public LocalizedTermProcessor(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
            _localizationSystem.LocalizationChanged += OnLocalizationChange;
        }

        public void Dispose() =>
            _localizationSystem.LocalizationChanged -= OnLocalizationChange;

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

            _onChangeLocalizationCallback.Invoke(_localizationSystem.MakeTranslatedTextByTerm(_term));
        }

        private void OnLocalizationChange() =>
            CallLocalizationCallback();
    }
}
