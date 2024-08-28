using GameTemplate.Systems.Performance;
using UnityEngine;

namespace GameTemplate.Systems
{
    public class SystemPerformanceSetter
    {
        private readonly IDevicePerformaceConfigurator _performanceConfigurator;

        public SystemPerformanceSetter(IDevicePerformaceConfigurator performanceConfigurator)
        {
            _performanceConfigurator = performanceConfigurator;
        }

        public void Initialize()
        {
            InitializePerformanceParameters(_performanceConfigurator.GetConfiguration());
        }

        private void InitializePerformanceParameters(PerformanceConfiguration configuration)
        {
            Application.targetFrameRate = configuration.TargetFrameRate;
            Time.fixedDeltaTime = configuration.FixedDeltaTime;
        }
    }
}
