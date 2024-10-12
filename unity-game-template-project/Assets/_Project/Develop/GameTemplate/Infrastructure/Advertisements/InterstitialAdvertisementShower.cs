using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using System;
using Modules.AssetManagement.StaticData;
using Modules.Extensions;

namespace GameTemplate.Infrastructure.Advertisements
{
    public class InterstitialAdvertisementShower : AdvertisementShower, IInterstitialAdvertisimentShower
    {
        private AdvertisementPlacement _advertisementPlacement;
        private bool _isInitialized;

        public InterstitialAdvertisementShower(IAdvertisimentsService advertisementService, 
            IAnalyticsService analyticsService, IStaticDataService staticDataService)
            : base(advertisementService, analyticsService, staticDataService)
        {
        }

        public event Action<bool> Finished;

        public void Initialize(AdvertisementPlacement advertisementPlacement)
        {
            _advertisementPlacement = advertisementPlacement;
            _isInitialized = true;
        }

        public void Reset()
        {
            _isInitialized = false;
        }

        protected override bool CanShowAdvertisement() => AdvertisimentsService.CanShowInterstitial;

        protected override bool TryShowAdvertisement() =>
            AdvertisimentsService.TryShowInterstitial(Configuration.InterstitialOnStartLevelProbability);

        protected override void SendAdvertisementAnalytics(AdvertisementAction action) =>
            Analytics.SendInterstitialAdvertisementAnalytics(action, _advertisementPlacement);

        protected override bool HasShowChance()
        {
            switch(_advertisementPlacement)
            {
                case AdvertisementPlacement.LevelStart:
                    return Configuration.InterstitialOnStartLevelProbability.HasChance();

                case AdvertisementPlacement.LevelEnd:
                    return Configuration.InterstitialOnExitLevelProbability.HasChance();

                default:
                    return true;
            }
        }

        protected override void OnFinish(bool isSuccess) => Finished?.Invoke(isSuccess);

        protected override bool IsInitialized() => _isInitialized;
    }
}