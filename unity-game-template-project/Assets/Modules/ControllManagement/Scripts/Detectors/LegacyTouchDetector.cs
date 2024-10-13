using UnityEngine;

namespace Modules.ControllManagement.Detectors
{
    public class LegacyTouchDetector : ITouchDetector
    {
        public bool IsHold() =>
            Input.GetMouseButton(0);
    }
}
