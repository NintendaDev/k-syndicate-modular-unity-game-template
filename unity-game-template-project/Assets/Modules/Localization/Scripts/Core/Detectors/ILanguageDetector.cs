using Modules.Localization.Core.Types;

namespace Modules.Localization.Core.Detectors
{
    public interface ILanguageDetector
    {
        public Language GetCurrentLanguage();

        public string GetCurrentLanguageName();
    }
}