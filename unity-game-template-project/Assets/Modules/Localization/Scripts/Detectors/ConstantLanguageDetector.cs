using Modules.Localization.Types;

namespace Modules.Localization.Detectors
{
    public class ConstantLanguageDetector : LanguageDetector
    {
        private readonly Language _language;

        public ConstantLanguageDetector(Language language) =>
            _language = language;

        public override Language GetCurrentLanguage() => _language;
    }
}
