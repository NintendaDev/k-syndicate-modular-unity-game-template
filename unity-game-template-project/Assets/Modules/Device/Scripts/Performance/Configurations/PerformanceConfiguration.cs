using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Modules.Device.Performance.Configurations
{
    [Serializable]
    public class PerformanceConfiguration
    {
        [field: SerializeField, Required, MinValue(0)] public int TargetFrameRate { get; private set; } = 60;

        [field: SerializeField, Required, MinValue(0)] public float FixedDeltaTime { get; private set; } = 0.02f;
    }
}
