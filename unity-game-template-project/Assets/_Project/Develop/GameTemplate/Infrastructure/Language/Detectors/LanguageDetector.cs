namespace GameTemplate.Infrastructure.Language.Detectors
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
