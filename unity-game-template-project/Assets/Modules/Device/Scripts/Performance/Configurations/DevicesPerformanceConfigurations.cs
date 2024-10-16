using UnityEngine;

namespace Modules.Device.Performance.Configurations
{
    [CreateAssetMenu(fileName = "new DevicesPerformanceConfigurations", menuName = "GameTemplate/System/DevicesPerformanceConfigurations")]
    public sealed class DevicesPerformanceConfigurations : ScriptableObject
    {
        [field: SerializeField] public PerformanceConfiguration MobileConfiguration { get; private set; }

        [field: SerializeField] public PerformanceConfiguration DesktopConfiguration { get; private set; }
    }
}
