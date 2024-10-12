using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using System;
using Modules.AssetManagement.StaticData;

namespace GameTemplate.Infrastructure.Advertisements
{
    public abstract class AdvertisementShower
    {
        public AdvertisementShower(IAdvertisimentsService advertisementService, IAnalyticsService analyticsService,
            IStaticDataService staticDataService)
        {
            AdvertisimentsService = advertisementService;
            Analytics = analyticsService;
            Configuration = staticDataService.GetConfiguration<AdvertisimentsConfiguration>();
        }

        protected IAdvertisimentsService AdvertisimentsService { get; }

        protected IAnalyticsService Analytics { get; }

        protected AdvertisimentsConfiguration Configuration { get; }

        public bool TryStartAdvertisementBehaviour()
        {
            if (IsInitialized() == false)
                throw new Exception($"{nameof(AdvertisementShower)} is not initialized");

            if (HasShowChance() == false)
                return false;

            SendAdvertisementAnalytics(AdvertisementAction.Request);

            if (CanShowAdvertisement() == false)
            {
                SendAdvertisementAnalytics(AdvertisementAction.FailedShow);

                return false;
            }

            // We cannot guarantee the occurrence of ad events where unsubscription happens.
            // Therefore, you should unsubscribe from everything before subscribing again.
            UnsubscribeAdvertisements();
            SubscribeAdvertisements();

            if (TryShowAdvertisement() == false)
            {
                SendAdvertisementAnalytics(AdvertisementAction.FailedShow);

                return false;
            }

            return true;
        }

        protected abstract bool IsInitialized();

        protected abstract bool CanShowAdvertisement();

        protected abstract bool TryShowAdvertisement();

        protected abstract void SendAdvertisementAnalytics(AdvertisementAction action);

        protected abstract bool HasShowChance();

        protected abstract void OnFinish(bool isSuccess);

        protected void OnAdvertisementClose(bool isSuccess)
        {
            AdvertisimentsService.AdvertisimentClosed -= OnAdvertisementClose;

            if (isSuccess)
                SendAdvertisementAnalytics(AdvertisementAction.Clicked);

            OnFinish(isSuccess);
        }

        protected void OnAdvertisementStart()
        {
            AdvertisimentsService.AdvertisimentStarted -= OnAdvertisementStart;
            SendAdvertisementAnalytics(AdvertisementAction.Show);
        }

        protected virtual void SubscribeAdvertisements()
        {
            AdvertisimentsService.AdvertisimentStarted += OnAdvertisementStart;
            AdvertisimentsService.AdvertisimentClosed += OnAdvertisementClose;
        }

        protected virtual void UnsubscribeAdvertisements()
        {
            AdvertisimentsService.AdvertisimentStarted -= OnAdvertisementStart;
            AdvertisimentsService.AdvertisimentClosed -= OnAdvertisementClose;
        }
    }
}
