using System;

namespace Modules.Advertisements.Systems
{
    public interface IAdvertisementsSystem
    {
        public bool CanShowInterstitial { get; }

        public bool CanShowReward { get; }

        public void DisableInterstitial();

        public bool TryShowInterstitial(float probability, Action onCloseCallback);

        public bool TryShowInterstitial(Action onCloseCallback);

        public bool TryShowReward(Action onSuccessCallback, Action onCloseCallback);
    }
}