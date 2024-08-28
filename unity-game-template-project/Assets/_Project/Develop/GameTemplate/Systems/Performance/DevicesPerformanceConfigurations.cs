using UnityEngine;

namespace GameTemplate.Systems.Performance
{
    [CreateAssetMenu(fileName = "new DevicesPerformanceConfigurations", menuName = "GameTemplate/System/DevicesPerformanceConfigurations")]
    public class DevicesPerformanceConfigurations : ScriptableObject
    {
        [field: SerializeField] public PerformanceConfiguration MobileConfiguration { get; private set; }

        [field: SerializeField] public PerformanceConfiguration DesktopConfiguration { get; private set; }
    }
}
