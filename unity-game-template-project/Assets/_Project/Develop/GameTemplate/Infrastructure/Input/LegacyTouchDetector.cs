using UnityEngine;

namespace GameTemplate.Infrastructure.Inputs
{
    public class LegacyTouchDetector : ITouchDetector
    {
        public bool IsHold() =>
            Input.GetMouseButton(0);
    }
}
