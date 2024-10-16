using Modules.Localization.Types;
using UnityEngine;

namespace Modules.Localization.Systems.Demo
{
    [System.Serializable]
    public sealed class LocalizedText
    {
        [field: SerializeField, IsNotNoneLanguage] public Language Language { get; private set; }

        [field: SerializeField] public string Text { get; private set; }
    }
}
