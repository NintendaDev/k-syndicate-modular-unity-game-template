using ExternalLibraries.Extensions;
using System;
using UnityEngine;

namespace GameTemplate.Services.Advertisiments
{
    public abstract class AbstractAdvertisimentsService : IAdvertisimentsService
    {
        private float _originalAudioVolume;
        private float _originalTimeScale;
        private bool _isDisabledInterstitialAdvertisiments;

        public AbstractAdvertisimentsService()
        {
            SaveSystemParameters();
        }

        public event Action AdvertisimentStarted;

        public event Action<bool> AdvertisimentClosed;

        public virtual bool CanShowInterstitial => _isDisabledInterstitialAdvertisiments == false;

        public void DisableInterstitial() =>
            _isDisabledInterstitialAdvertisiments = true;

        public bool TryShowInterstitial(float probability)
        {
            if (probability.HasChance())
                return TryShowInterstitial();

            return false;
        }

        public bool TryShowInterstitial()
        {
            if (CanShowInterstitial == false)
                return false;

            DisableSoundAndGameTime();
            StartInterstitialBehaviour();

            return true;
        }

        protected void DisableSoundAndGameTime()
        {
            SaveSystemParameters();

            AudioListener.volume = 0;
            Time.timeScale = 0;
        }

        protected abstract void StartInterstitialBehaviour();

        protected void EnableSoundAndGameTime()
        {
            AudioListener.volume = _originalAudioVolume;
            Time.timeScale = _originalTimeScale;
        }

        protected void SendAdvertisimentStartedEvent() =>
            AdvertisimentStarted?.Invoke();

        protected void SendAdvertisimentClosedEvent(bool isSuccess) =>
            AdvertisimentClosed?.Invoke(isSuccess);

        private void SaveSystemParameters()
        {
            _originalAudioVolume = AudioListener.volume;
            _originalTimeScale = Time.timeScale;
        }
    }
}
