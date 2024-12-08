using System;
using UnityEngine;

namespace Modules.Advertisements.Systems
{
    public sealed class DummyAdvertisementsSystem : AdvertisementsSystem
    {
        protected override void StartInterstitialBehaviour(Action onCloseCallback)
        {
            Debug.Log("Interstitial Ad show started");
            DisableSoundAndGameTime();
            onCloseCallback?.Invoke();
            EnableSoundAndGameTime();
        }

        protected override void StartRewardBehaviour(Action onSuccessCallback, Action onCloseCallback)
        {
            Debug.Log("Redard Ad show started");
            DisableSoundAndGameTime();
            onSuccessCallback?.Invoke();
            onCloseCallback?.Invoke();
            EnableSoundAndGameTime();
        }
    }
}
