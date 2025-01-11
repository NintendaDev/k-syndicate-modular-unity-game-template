using Modules.Localization.Core.Types;

namespace Modules.Localization.Core.Detectors
{
    public abstract class LanguageDetector : ILanguageDetector
    {
        public abstract Language GetCurrentLanguage();

        public string GetCurrentLanguageName()
        {
            Language currentLanguage = GetCurrentLanguage();

            return currentLanguage.ToString();
        }
    }
}
