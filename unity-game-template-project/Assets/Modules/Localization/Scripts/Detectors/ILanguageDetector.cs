using Modules.Localization.Types;

namespace Modules.Localization.Detectors
{
    public interface ILanguageDetector
    {
        public Language GetCurrentLanguage();

        public string GetCurrentLanguageName();
    }
}