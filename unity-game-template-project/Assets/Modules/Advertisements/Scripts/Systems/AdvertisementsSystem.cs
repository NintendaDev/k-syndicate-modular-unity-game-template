using Modules.Extensions;
using System;
using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public abstract class AdvertisementsSystem : IAdvertisementsSystem
    {
        private readonly TimeAndAudioState _timeAndAudioState = new();
        private bool _isEnabledInterstitial = true;

        public virtual bool CanShowInterstitial => _isEnabledInterstitial;

        public virtual bool CanShowReward => true;

        public void DisableInterstitial() =>
            _isEnabledInterstitial = false;
        
        public void EnableInterstitial() =>
            _isEnabledInterstitial = true;

        public bool TryShowInterstitial(float probability, Action onCloseCallback)
        {
            if (probability.HasChance())
                return TryShowInterstitial(onCloseCallback);

            return false;
        }

        public bool TryShowInterstitial(Action onCloseCallback)
        {
            if (CanShowInterstitial == false)
                return false;
            
            StartInterstitialBehaviour(onCloseCallback);

            return true;
        }

        public bool TryShowReward(Action onSuccessCallback, Action onCloseCallback)
        {
            if (CanShowReward == false)
                return false;
            
            StartRewardBehaviour(onSuccessCallback, onCloseCallback);

            return true;
        }

        protected abstract void StartInterstitialBehaviour(Action onCloseCallback);

        protected abstract void StartRewardBehaviour(Action onSuccessCallback, Action onCloseCallback);

        protected void EnableSoundAndGameTime() => _timeAndAudioState.On();

        protected void DisableSoundAndGameTime() => _timeAndAudioState.Off();

        private sealed class TimeAndAudioState
        {
            private float _originalAudioVolume;
            private float _originalTimeScale;
            private bool _isOffState;

            public void On()
            {
                if (_isOffState == false)
                    return;
                
                AudioListener.volume = _originalAudioVolume;
                Time.timeScale = _originalTimeScale;
                _isOffState = false;
            }

            public void Off()
            {
                if (_isOffState)
                    return;
                
                Save();
                
                AudioListener.volume = 0;
                Time.timeScale = 0;

                _isOffState = true;
            }

            private void Save()
            {
                _originalAudioVolume = AudioListener.volume;
                _originalTimeScale = Time.timeScale;
            }
        }
    }
}
