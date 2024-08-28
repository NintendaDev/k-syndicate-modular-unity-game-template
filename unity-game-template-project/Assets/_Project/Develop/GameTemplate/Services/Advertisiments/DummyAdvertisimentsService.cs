namespace GameTemplate.Services.Advertisiments
{
    public class DummyAdvertisimentsService : AbstractAdvertisimentsService
    {
        protected override void StartInterstitialBehaviour()
        {
            OnAdvertisimentClose(isSuccess: true);
        }

        private void OnAdvertisimentClose(bool isSuccess)
        {
            EnableSoundAndGameTime();
            SendAdvertisimentClosedEvent(isSuccess);
        }
    }
}
