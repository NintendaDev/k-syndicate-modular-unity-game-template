using UnityEngine;

namespace Modules.Device.Detecting
{
    public sealed class UnityDeviceDetector : IDeviceDetector
    {
        public bool IsMobile()
        {
            if (Application.isEditor)
                return false;

            return Application.platform == RuntimePlatform.Android 
                   || Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
}
