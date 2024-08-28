using GameTemplate.Infrastructure.Language;
using GameTemplate.Infrastructure.Language.Detectors;
using GameTemplate.Infrastructure.Language.Localization;
using GameTemplate.Services.StaticData;
using System;

namespace GameTemplate.Services.Localization
{
    public class SimpleLocalizationService : ILocalizationService
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly IStaticDataService _staticDataService;
        private Language _currentLanguage;
        private LocalizationDatabase _localizationDatabase;

        public SimpleLocalizationService(ILanguageDetector languageDetector, IStaticDataService staticDataService)
        {
            _languageDetector = languageDetector;
            _staticDataService = staticDataService;
        }

        public event Action LocalizationChanged;

        public void Initialize()
        {
            _currentLanguage = _languageDetector.GetCurrentLanguage();
            _localizationDatabase = _staticDataService.GetConfiguration<LocalizationDatabase>();
        }

        public string MakeTranslatedTextByTerm(string term)
        {
            if (_localizationDatabase.IsExistTranslation(term, _currentLanguage, out string translation))
                return translation;

            return term;
        }

        public string MakeTranslatedText(LocalizationTerm term) =>
            MakeTranslatedTextByTerm(term.ToString());
    }
}
