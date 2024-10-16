using Sirenix.OdinInspector;
using System;
using Modules.Core.UI;
using UnityEngine;

namespace Modules.AudioManagement.UI.Views
{
    public sealed class AudioSettingsView : MonoBehaviour
    {
        [SerializeField, Required] private SliderBehaviour _musicSlider;
        [SerializeField, Required] private SliderBehaviour _effectsSlider;

        public event Action<float> MusicValueChanged;

        public event Action<float> EffectsValueChanged;

        private void OnEnable()
        {

            _musicSlider.ValueChanged += OnMusicSliderValueChange;
            _effectsSlider.ValueChanged += OnEffectsSliderValueChange;
        }

        private void OnDisable()
        {
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
