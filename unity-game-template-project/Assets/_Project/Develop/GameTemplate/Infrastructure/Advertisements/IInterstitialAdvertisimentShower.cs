using GameTemplate.Services.Advertisiments;
using System;

namespace GameTemplate.Infrastructure.Advertisements
{
    public interface IInterstitialAdvertisimentShower
    {
        public event Action<bool> Finished;

        public void Initialize(AdvertisementPlacement advertisementPlacement);

        public void Reset();

        public bool TryStartAdvertisementBehaviour();
    }
}