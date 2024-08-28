namespace GameTemplate.Infrastructure.Language.Detectors
{
    public interface ILanguageDetector
    {
        public Language GetCurrentLanguage();

        public string GetCurrentLanguageName();
    }
}