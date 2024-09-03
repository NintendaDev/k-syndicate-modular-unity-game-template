using GameTemplate.Infrastructure.LanguageSystem.Processors;
using GameTemplate.UI.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.Infrastructure.LanguageSystem.Localization
{
    public class UITextLocalizator : MonoBehaviour
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
