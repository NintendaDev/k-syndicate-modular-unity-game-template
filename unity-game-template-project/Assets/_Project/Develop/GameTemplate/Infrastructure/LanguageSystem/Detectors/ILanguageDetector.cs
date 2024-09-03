namespace GameTemplate.Infrastructure.LanguageSystem.Detectors
{
    public interface ILanguageDetector
    {
        public Language GetCurrentLanguage();

        public string GetCurrentLanguageName();
    }
}