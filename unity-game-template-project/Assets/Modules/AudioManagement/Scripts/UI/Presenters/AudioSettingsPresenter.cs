using System;
using Modules.AudioManagement.Mixer;
using Modules.AudioManagement.UI.Views;
using Modules.ControllManagement.Detectors;
using Modules.EventBus;
using Modules.SaveSystem.Signals;
using Zenject;

namespace Modules.AudioManagement.UI.Presenters
{
    public sealed class AudioSettingsPresenter : IDisposable, ILateTickable
    {
        private readonly AudioSettingsView _view;
        private readonly IAudioMixerSystem _audioMixerSystem;
        private readonly ITouchDetector _touchDetector;
        private readonly ISignalBus _signalBus;
        private bool _isRequiredSaving;

        public AudioSettingsPresenter(AudioSettingsView view, IAudioMixerSystem audioMixerSystem, 
            ITouchDetector touchDetector, ISignalBus signalBus)
        {
            _view = view;
            _audioMixerSystem = audioMixerSystem;
            _touchDetector = touchDetector;
            _signalBus = signalBus;

            _view.Initialize(_audioMixerSystem.MusicPercentVolume, _audioMixerSystem.EffectsPercentVolume);
            _view.MusicValueChanged += OnMusicValueChange;
            _view.EffectsValueChanged += OnEffectsValueChange;
        }

        public void Dispose()
        {
            _view.MusicValueChanged -= OnMusicValueChange;
            _view.EffectsValueChanged -= OnEffectsValueChange;
        }

        public void LateTick()
        {
            StartSaveBehaviour();
        }

        private void OnMusicValueChange(float value) =>
            SetMusicVolume(value);

        private void SetMusicVolume(float volume)
        {
            _audioMixerSystem.SetMusicPercentVolume(volume);
            _isRequiredSaving = true;
        }

        private void OnEffectsValueChange(float value) =>
            SetEffectsVolume(value);

        private void SetEffectsVolume(float volume)
        {
            _audioMixerSystem.SetEffectsPercentVolume(volume);
            _isRequiredSaving = true;
        }

        private void StartSaveBehaviour()
        {
            if (_isRequiredSaving == false)
                return;

            if (_touchDetector.IsHold() == false)
            {
                _signalBus.Invoke<SaveSignal>();
                _isRequiredSaving = false;
            }
        }
    }
}
