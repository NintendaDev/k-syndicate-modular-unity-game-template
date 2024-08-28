using GameTemplate.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameTemplate.UI.Core
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
