using UnityEngine;

namespace GameTemplate.Infrastructure.Language.Detectors
{
    public class UnitytLanguageDetector : LanguageDetector
    {
        public override Language GetCurrentLanguage()
        {
            SystemLanguage deviceLanguage = Application.systemLanguage;

            switch (deviceLanguage)
            {
                case SystemLanguage.English:
                    return Language.English;

                case SystemLanguage.Russian:
                    return Language.Russian;

                case SystemLanguage.Turkish:
                    return Language.Turkish;

                default:
                    return Language.English;
            }
        }
    }
}
