using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Core.UI
{
    public class SliderBehaviour : MonoBehaviour
    {
        [SerializeField, Required] private Slider _slider;
        [SerializeField, Required] private Image _enabledIcon;
        [SerializeField, Required] private Image _disabledIcon;

        private bool _isSubscribed;

        public event Action<float> ValueChanged;

        public float Value => _slider.normalizedValue;

        private void Start()
        {
            UpdateIcon();
        }

        private void OnEnable() =>
            Subscribe();

        private void OnDisable() =>
            Unsubscribe();

        public void SetValueWithoutEvent(float value)
        {
            Unsubscribe();

            SetValue(value);
            UpdateIcon();

            Subscribe();
        }

        public void SetValue(float value) =>
            _slider.normalizedValue = value;

        private void UpdateIcon()
        {
            if (Value > 0)
                ShowEnabledIcon();
            else
                ShowDisabledIcon();
        }

        private void ShowEnabledIcon()
        {
            _enabledIcon.gameObject.SetActive(true);
            _disabledIcon.gameObject.SetActive(false);
        }

        private void ShowDisabledIcon()
        {
            _enabledIcon.gameObject.SetActive(false);
            _disabledIcon.gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            if (_isSubscribed)
                return;

            _slider.onValueChanged.AddListener(OnValueChange);
            _isSubscribed = true;
        }

        private void OnValueChange(float value)
        {
            UpdateIcon();
            ValueChanged?.Invoke(value);
        }

        private void Unsubscribe()
        {
            if (_isSubscribed == false)
                return;

            _slider.onValueChanged.RemoveListener(OnValueChange);
            _isSubscribed = false;
        }
    }
}
