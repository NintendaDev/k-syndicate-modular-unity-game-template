using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public sealed class DummyAdvertisementsSystem : AbstractAdvertisementsSystem
    {
        protected override void StartInterstitialBehaviour()
        {
            Debug.Log("Interstitial Ad show started");
            ProcessCloseInterstitialCallbacks();
            EnableSoundAndGameTime();
        }

        protected override void StartRewardBehaviour()
        {
            Debug.Log("Redard Ad show started");
            ProcessRewardSuccessCallbacks();
            ProcessRewardCloseCallbacks();
            EnableSoundAndGameTime();
        }
    }
}
