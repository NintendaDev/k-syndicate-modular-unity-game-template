using Modules.Device.Performance.Configurations;
using UnityEngine;

namespace Modules.Device.Performance
{
    public class SystemPerformanceSetter
    {
        private readonly IPerformaceConfiguration _performanceConfiguration;

        public SystemPerformanceSetter(IPerformaceConfiguration performanceConfiguration)
        {
            _performanceConfiguration = performanceConfiguration;
        }

        public void Initialize()
        {
            InitializePerformanceParameters(_performanceConfiguration.GetConfiguration());
        }

        private void InitializePerformanceParameters(PerformanceConfiguration configuration)
        {
            Application.targetFrameRate = configuration.TargetFrameRate;
            Time.fixedDeltaTime = configuration.FixedDeltaTime;
        }
    }
}
