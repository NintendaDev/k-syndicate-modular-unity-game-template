using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.Core.UI
{
    public class UITextButton : UIButton
    {
        [SerializeField, Required] private UIText _buttonTitle;

        public string Title => _buttonTitle.Text;

        public void SetTitle(string title) =>
            _buttonTitle.SetText(title);
    }
}
