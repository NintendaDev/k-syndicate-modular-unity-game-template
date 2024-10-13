using Sirenix.OdinInspector;
using System;
using Modules.Core.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Core.UI
{
    public abstract class ViewWithBackButton : EnableDisableBehaviour
    {
        [SerializeField, Required] private Button _backButton;

        public event Action BackButtonClicked;

        protected virtual void OnEnable() =>
            _backButton.onClick.AddListener(OnBackButtonClick);

        protected virtual void OnDisable() =>
            _backButton.onClick.RemoveListener(OnBackButtonClick);

        protected virtual void OnBackButtonClick() =>
            BackButtonClicked?.Invoke();
    }
}
