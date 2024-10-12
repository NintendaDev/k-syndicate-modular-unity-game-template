using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Modules.Core.UI
{
    public class UIText : MonoBehaviour
    {
        [SerializeField, Required] private TMP_Text _label;

        public string Text => _label.text;

        public void SetText(string text) =>
            _label.text = text;
    }
}
