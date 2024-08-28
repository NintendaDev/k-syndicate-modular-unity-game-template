using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameTemplate.Infrastructure.Language.Localization
{
    [CreateAssetMenu(fileName = "new LocalizationDatabase", menuName = "GameTemplate/Localization/LocalizationDatabase")]
    public class LocalizationDatabase : ScriptableObject
    {
        [SerializeField, IsNotNoneLanguage] private Language _defaultLanguage;

        [ValidateInput(nameof(IsUniqueTermsTranslations))]
        [SerializeField] private List<TermTranslations> _termTranslations;

        public bool IsExistTranslation(string term, Language language, out string translation)
        {
            translation = string.Empty;

            TermTranslations termTranslations = _termTranslations.Where(x => x.Term.ToLower() == term.ToLower()).FirstOrDefault();

            if (termTranslations == null)
                return false;

            if (termTranslations.IsExistTranslation(language, out translation))
            {
                return true;
            }
            else
            {
                if (language != _defaultLanguage && termTranslations.IsExistTranslation(_defaultLanguage, out translation))
                    return true;
            }

            return false;
        }

        private bool IsUniqueTermsTranslations(List<TermTranslations> termTranslations, ref string errorMessage)
        {
            if (termTranslations.GroupBy(x => x.Term).Count() < termTranslations.Count)
            {
                errorMessage = "Duplicates with the same localization terms found";

                return false;
            }

            return true;
        }
    }
}
