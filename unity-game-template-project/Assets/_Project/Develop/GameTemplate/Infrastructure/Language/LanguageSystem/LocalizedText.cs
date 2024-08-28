using UnityEngine;

namespace GameTemplate.Infrastructure.LanguageSystem
{
    [System.Serializable]
    public class LocalizedText
    {
        [field: SerializeField, IsNotNoneLanguage] public Language Language { get; private set; }

        [field: SerializeField] public string Text { get; private set; }
    }
}
