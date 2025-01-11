using Modules.Extensions;
using System;
using Cysharp.Threading.Tasks;
using Modules.Advertisements.Types;
using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public abstract class AdvertisementsSystem : IAdvertisementsSystem
    {
        private readonly TimeAndAudioState _timeAndAudioState = new();
        private bool _isEnabledInterstitial = true;

        public abstract event Action<AdvertisementRevenue> RevenueReceived;
        
        public abstract bool IsShowInterstitialOrReward { get; }

        public abstract bool CanShowReward { get; }
        
        public virtual bool CanShowInterstitial => _isEnabledInterstitial;
        
        public abstract AdvertisementsPlatform Platform { get; }
        
        public abstract UniTask InitializeAsync();
        
        public void DisableInterstitial() =>
            _isEnabledInterstitial = false;

        public void EnableInterstitial() =>
            _isEnabledInterstitial = true;

        public abstract bool TryShowBanner();
        
        public abstract void HideBanner();

        public bool TryShowInterstitial(float probability, Action onCloseCallback, Action onShowCallback = null, 
            Action onClickCallback = null)
        {
            if (probability.HasChance())
                return TryShowInterstitial(onCloseCallback: onCloseCallback, onShowCallback: onShowCallback, 
                    onClickCallback: onClickCallback);

            return false;
        }

        public bool TryShowInterstitial(Action onCloseCallback, Action onShowCallback = null, 
            Action onClickCallback = null)
        {
            if (CanShowInterstitial == false)
                return false;
            
            StartInterstitialBehaviour(onCloseCallback: onCloseCallback, onShowCallback: onShowCallback, 
                onClickCallback: onClickCallback);

            return true;
        }

        public bool TryShowReward(Action onSuccessCallback = null, Action onCloseCallback = null,
            Action onShowCallback = null, Action onClickCallback = null)
        {
            if (CanShowReward == false)
                return false;
            
            StartRewardBehaviour(onSuccessCallback: onSuccessCallback, onCloseCallback: onCloseCallback, 
                onShowCallback: onShowCallback, onClickCallback: onClickCallback);

            return true;
        }

        protected abstract void StartInterstitialBehaviour(Action onCloseCallback = null, Action onShowCallback = null, 
            Action onClickCallback = null);

        protected abstract void StartRewardBehaviour(Action onSuccessCallback = null, Action onCloseCallback = null,
            Action onShowCallback = null, Action onClickCallback = null);

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
