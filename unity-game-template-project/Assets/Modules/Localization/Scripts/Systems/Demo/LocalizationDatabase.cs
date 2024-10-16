using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Modules.Localization.Types;
using UnityEngine;

namespace Modules.Localization.Systems.Demo
{
    [CreateAssetMenu(fileName = "new LocalizationDatabase", menuName = "GameTemplate/Localization/LocalizationDatabase")]
    public sealed class LocalizationDatabase : ScriptableObject
    {
        [SerializeField, IsNotNoneLanguage] private Language _defaultLanguage;

        [ValidateInput(nameof(IsUniqueTermsTranslations))]
        [SerializeField] private List<TermTranslations> _termTranslations;

        public bool IsExistTranslation(string term, Language language, out string translation)
        {
            translation = string.Empty;

            TermTranslations termTranslations = _termTranslations
                .FirstOrDefault(x => x.Term.ToLower() == term.ToLower());

            if (termTranslations == null)
                return false;

            if (termTranslations.IsExistTranslation(language, out translation))
                return true;

            return language != _defaultLanguage &&
                   termTranslations.IsExistTranslation(_defaultLanguage, out translation);
        }

        private bool IsUniqueTermsTranslations(List<TermTranslations> termTranslations, ref string errorMessage)
        {
            if (termTranslations.GroupBy(x => x.Term).Count() >= termTranslations.Count) 
                return true;
            
            errorMessage = "Duplicates with the same localization terms found";

            return false;
        }
    }
}
