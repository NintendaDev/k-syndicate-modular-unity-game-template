using GameTemplate.Infrastructure.Inputs;
using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.SaveLoad;
using GameTemplate.UI.GameHub.SettinsMenu.Views;
using System;
using Zenject;

namespace GameTemplate.UI.GameHub.SettinsMenu.Presenters
{
    public class SettingsPresenter : IDisposable, ILateTickable
    {
        private readonly SettingsMenuView _view;
        private readonly IAudioMixerService _audioMixerService;
        private readonly ITouchDetector _touchDetector;
        private readonly ISaveSignal _saveSignaller;
        private bool _isRequiredSaving;

        public SettingsPresenter(SettingsMenuView view, IAudioMixerService audioMixerService, 
            ITouchDetector touchDetector, ISaveSignal saveSignaller)
        {
            _view = view;
            _audioMixerService = audioMixerService;
            _touchDetector = touchDetector;
            _saveSignaller = saveSignaller;

            _view.Initialize(_audioMixerService.MusicPercentVolume, _audioMixerService.EffectsPercentVolume);
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
            _audioMixerService.SetMusicVolume(volume);
            _isRequiredSaving = true;
        }

        private void OnEffectsValueChange(float value) =>
            SetEffectsVolume(value);

        private void SetEffectsVolume(float volume)
        {
            _audioMixerService.SetEffectsVolume(volume);
            _isRequiredSaving = true;
        }

        private void StartSaveBehaviour()
        {
            if (_isRequiredSaving == false)
                return;

            if (_touchDetector.IsHold() == false)
            {
                _saveSignaller.SendSaveSignal();
                _isRequiredSaving = false;
            }
        }
    }
}
