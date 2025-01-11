using Modules.Localization.Core.Types;

namespace Modules.Localization.Core.Detectors
{
    public sealed class ConstantLanguageDetector : LanguageDetector
    {
        private readonly Language _language;

        public ConstantLanguageDetector(Language language) =>
            _language = language;

        public override Language GetCurrentLanguage() => _language;
    }
}
