using UnityEngine;

namespace GameTemplate.Infrastructure.Language.Localization
{
    [System.Serializable]
    public class LocalizedText
    {
        [field: SerializeField, IsNotNoneLanguage] public Language Language { get; private set; }

        [field: SerializeField] public string Text { get; private set; }
    }
}
