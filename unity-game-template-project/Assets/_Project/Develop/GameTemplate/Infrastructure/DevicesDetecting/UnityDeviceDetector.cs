using UnityEngine;

namespace GameTemplate.Infrastructure.DevicesDetecting
{
    public class UnityDeviceDetector : IDeviceDetector
    {
        public bool IsMobile()
        {
            if (Application.isEditor)
                return false;

            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
}
