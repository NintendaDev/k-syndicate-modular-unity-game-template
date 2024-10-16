using Modules.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public abstract class AdvertisementsSystem : IAdvertisementsSystem
    {
        private readonly TimeAndAudioState _timeAndAudioState = new();
        private readonly Queue<Action> _interstitialCloseCallbacks = new();
        private readonly Queue<Action> _rewardCloseCallbacks = new();
        private readonly Queue<Action> _rewardSuccessCallbacks = new();
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
                _interstitialCloseCallbacks.Enqueue(onCloseCallback);
            
            StartInterstitialBehaviour();

            return true;
        }

        public bool TryShowReward(Action onSuccessCallback, Action onCloseCallback)
        {
            if (CanShowReward == false)
                return false;
            
            _rewardCloseCallbacks.Enqueue(onCloseCallback);
            _rewardSuccessCallbacks.Enqueue(onSuccessCallback);
            
            StartRewardBehaviour();

            return true;
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
            _timeAndAudioState.On();
        }

        private void DisableSoundAndGameTime()
        {
            _timeAndAudioState.Off();
        }

        private void ProcessCallbacks(Queue<Action> callbacks)
        {
            while (callbacks.Count > 0)
                callbacks.Dequeue().Invoke();
        }

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
