using GameTemplate.Infrastructure.Language.Processors;
using GameTemplate.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.Infrastructure.Localization
{
    public class UITextLocalizator : MonoBehaviour
    {
        [SerializeField, Required] private UIText _text;

        [Inject]
        private void Construct(LocalizedTermProcessorLinker localizedTermProcessorLinker)
        {
            localizedTermProcessorLinker.Link(_text);
        }
    }
}
