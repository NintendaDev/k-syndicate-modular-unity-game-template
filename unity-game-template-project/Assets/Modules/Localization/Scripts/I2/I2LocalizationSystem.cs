using System;
using System.Text;
using I2.Loc;
using Modules.Localization.Core.Detectors;
using Modules.Localization.Core.Systems;
using Modules.Localization.Core.Types;

namespace Modules.Localization.I2System
{
    public sealed class I2LocalizationSystem : ILocalizationSystem
    {
        private const string TermPreffix = nameof(LocalizationTerm);
        private readonly ILanguageDetector _languageDetector;
        private readonly StringBuilder _stringBuilder = new();

        public I2LocalizationSystem(ILanguageDetector languageDetector)
        {
            _languageDetector = languageDetector;

            LocalizationManager.OnLocalizeEvent += InLocalizationChange;
        }

        public event Action LocalizationChanged;

        public void Dispose()
        {
            LocalizationManager.OnLocalizeEvent -= InLocalizationChange;
        }

        public void Initialize()
        {
            string currentLanguageName = _languageDetector.GetCurrentLanguageName();

            if (LocalizationManager.HasLanguage(currentLanguageName))
                LocalizationManager.CurrentLanguage = currentLanguageName;
        }

        public string MakeTranslatedTextByTerm(string term)
        {
            if (LocalizationManager.TryGetTranslation(term, out string translation))
                return translation;

            return term;
        }

        public string MakeTranslatedText(LocalizationTerm term)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(TermPreffix);
            _stringBuilder.Append("/");
            _stringBuilder.Append(term.ToString());
            
            return MakeTranslatedTextByTerm(_stringBuilder.ToString());
        }

        private void InLocalizationChange() =>
            LocalizationChanged?.Invoke();
    }
}