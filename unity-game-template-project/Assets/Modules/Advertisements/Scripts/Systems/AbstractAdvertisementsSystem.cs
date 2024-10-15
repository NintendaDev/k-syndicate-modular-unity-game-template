using Modules.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public abstract class AbstractAdvertisementsSystem : IAdvertisementsSystem
    {
        private readonly TimeAndAudioState _timeAndAudioState = new();
        private readonly List<Action> _interstitialCloseCallbacks = new();
        private readonly List<Action> _rewardCloseCallbacks = new();
        private readonly List<Action> _rewardSuccessCallbacks = new();
        private bool _isDisabledInterstitialAdvertisements;

        public virtual bool CanShowInterstitial => _isDisabledInterstitialAdvertisements == false;

        public virtual bool CanShowReward => true;

        public void DisableInterstitial() =>
            _isDisabledInterstitialAdvertisements = true;

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

            DisableSoundAndGameTime();
            
            if (onCloseCallback != null)
                _interstitialCloseCallbacks.Add(onCloseCallback);
            
            StartInterstitialBehaviour();

            return true;
        }

        public bool TryShowReward(Action onSuccessCallback, Action onCloseCallback)
        {
            if (CanShowReward == false)
                return false;
            
            _rewardCloseCallbacks.Add(onCloseCallback);
            _rewardSuccessCallbacks.Add(onSuccessCallback);
            
            StartRewardBehaviour();

            return true;
        }
        
        protected void DisableSoundAndGameTime()
        {
            _timeAndAudioState.Save();
            _timeAndAudioState.Off();
        }

        protected abstract void StartInterstitialBehaviour();
        
        protected abstract void StartRewardBehaviour();

        protected void ProcessCloseInterstitialCallbacks() =>
            ProcessCallbacks(_interstitialCloseCallbacks);

        protected void ProcessRewardCloseCallbacks() =>
            ProcessCallbacks(_rewardCloseCallbacks);
        
        protected void ProcessRewardSuccessCallbacks() =>
            ProcessCallbacks(_rewardSuccessCallbacks);

        protected void EnableSoundAndGameTime()
        {
            _timeAndAudioState.Restore();
        }

        private void ProcessCallbacks(List<Action> callbacks)
        {
            callbacks.ForEach(callback => callback.Invoke());
            callbacks.Clear();
        }

        private class TimeAndAudioState
        {
            private float _originalAudioVolume;
            private float _originalTimeScale;

            public void Save()
            {
                _originalAudioVolume = AudioListener.volume;
                _originalTimeScale = Time.timeScale;
            }

            public void Restore()
            {
                AudioListener.volume = _originalAudioVolume;
                Time.timeScale = _originalTimeScale;
            }

            public void Off()
            {
                AudioListener.volume = 0;
                Time.timeScale = 0;
            }
        }
    }
}
