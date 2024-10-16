using UnityEngine;

namespace Modules.ControllManagement.Detectors
{
    public sealed class LegacyTouchDetector : ITouchDetector
    {
        public bool IsHold() =>
            Input.GetMouseButton(0);
    }
}
