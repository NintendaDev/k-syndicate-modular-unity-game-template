namespace GameTemplate.Infrastructure.LanguageSystem
{
    public interface ILanguageDetector
    {
        public Language GetCurrentLanguage();

        public string GetCurrentLanguageName();
    }
}