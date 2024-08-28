using GameTemplate.UI.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameTemplate.UI.GameHub.SettinsMenu.Views
{
    public class SettingsMenuView : ViewWithBackButton
    {
        [SerializeField, Required] private SliderBehaviour _musicSlider;
        [SerializeField, Required] private SliderBehaviour _effectsSlider;

        public event Action<float> MusicValueChanged;

        public event Action<float> EffectsValueChanged;

        protected override void OnEnable()
        {
            base.OnEnable();

            _musicSlider.ValueChanged += OnMusicSliderValueChange;
            _effectsSlider.ValueChanged += OnEffectsSliderValueChange;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _musicSlider.ValueChanged -= OnMusicSliderValueChange;
            _effectsSlider.ValueChanged -= OnEffectsSliderValueChange;
        }

        public void Initialize(float musicVolume, float effectsVolume)
        {
            _musicSlider.SetValueWithoutEvent(musicVolume);
            _effectsSlider.SetValueWithoutEvent(effectsVolume);
        }

        private void OnMusicSliderValueChange(float value) =>
            MusicValueChanged?.Invoke(value);

        private void OnEffectsSliderValueChange(float value) =>
            EffectsValueChanged?.Invoke(value);
    }
}
