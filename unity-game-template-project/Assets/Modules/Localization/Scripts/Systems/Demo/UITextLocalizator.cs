using Modules.Core.UI;
using Modules.Localization.Processors;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Modules.Localization.Systems.Demo
{
    public sealed class UITextLocalizator : MonoBehaviour
    {
        [SerializeField, Required] private UIText _text;
        private LocalizedTermProcessorLinker _localizedTermProcessorLinker;

        [Inject]
        private void Construct(LocalizedTermProcessorLinker localizedTermProcessorLinker)
        {
            _localizedTermProcessorLinker = localizedTermProcessorLinker;
            _localizedTermProcessorLinker.Link(_text);
        }

        private void OnDestroy()
        {
            _localizedTermProcessorLinker.Unlink(_text);
        }
    }
}
