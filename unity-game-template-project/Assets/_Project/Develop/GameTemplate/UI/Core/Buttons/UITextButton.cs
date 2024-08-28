using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.UI.Core.Buttons
{
    public class UITextButton : UIButton
    {
        [SerializeField, Required] private UIText _buttonTitle;

        public string Title => _buttonTitle.Text;

        public void SetTitle(string title) =>
            _buttonTitle.SetText(title);
    }
}
