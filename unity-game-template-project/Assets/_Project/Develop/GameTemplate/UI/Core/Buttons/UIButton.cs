using GameTemplate.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameTemplate.UI.Core.Buttons
{
    public class UIButton : EnableDisableBehaviour
    {
        [SerializeField, Required] private Button _button;

        public event Action Clicked;

        protected virtual void OnEnable() =>
            _button.onClick.AddListener(OnButtonClick);

        protected virtual void OnDisable() =>
            _button.onClick.RemoveListener(OnButtonClick);

        protected virtual void OnButtonClick() =>
            Clicked?.Invoke();
    }
}
