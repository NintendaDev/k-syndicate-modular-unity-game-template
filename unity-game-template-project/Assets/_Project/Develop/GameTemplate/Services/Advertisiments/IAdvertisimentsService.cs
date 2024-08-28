using System;

namespace GameTemplate.Services.Advertisiments
{
    public interface IAdvertisimentsService
    {
        public event Action<bool> AdvertisimentClosed;

        public event Action AdvertisimentStarted;

        public bool CanShowInterstitial { get; }

        public void DisableInterstitial();

        public bool TryShowInterstitial(float probability);

        public bool TryShowInterstitial();
    }
}