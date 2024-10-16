using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Modules.Localization.Types;
using UnityEngine;

namespace Modules.Localization.Systems.Demo
{
    [System.Serializable]
    public sealed class TermTranslations
    {
        [SerializeField, Required] private string _term;

        [ValidateInput(nameof(IsUniqueLocalizedTextlanguage))]
        [SerializeField, Required] private List<LocalizedText> _localizedText;

        public string Term => _term;

        public IEnumerable<LocalizedText> LocalizedText => _localizedText;

        public bool IsExistTranslation(Language language, out string translation)
        {
            translation = _localizedText.Where(x => x.Language == language).Select(x => x.Text).FirstOrDefault();

            return translation != null;
        }

        private bool IsUniqueLocalizedTextlanguage(List<LocalizedText> localizedTextList, ref string errorMessage)
        {
            if (localizedTextList.GroupBy(x => x.Language).Count() < _localizedText.Count)
            {
                errorMessage = "Duplicate translations with the same language found";
                
                return false;
            }
                
            return true;
        }
    }
}