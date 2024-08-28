using System;

namespace GameTemplate.Infrastructure.LanguageSystem
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
