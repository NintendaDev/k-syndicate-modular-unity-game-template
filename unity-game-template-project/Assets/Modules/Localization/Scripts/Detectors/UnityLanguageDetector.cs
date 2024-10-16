using Modules.Localization.Types;
using UnityEngine;

namespace Modules.Localization.Detectors
{
    public sealed class UnityLanguageDetector : LanguageDetector
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
