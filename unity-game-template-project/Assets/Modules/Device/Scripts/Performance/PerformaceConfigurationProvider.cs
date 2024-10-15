using Modules.AssetsManagement.StaticData;
using Modules.Device.Detecting;
using Modules.Device.Performance.Configurations;

namespace Modules.Device.Performance
{
    public class PerformaceConfigurationProvider : IPerformaceConfiguration
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IDeviceDetector _deviceDetector;
        private DevicesPerformanceConfigurations _devicesPerformanceConfigurations;

        public PerformaceConfigurationProvider(IStaticDataService staticDataService, IDeviceDetector deviceDetector)
        {
            _staticDataService = staticDataService;
            _deviceDetector = deviceDetector;
        }

        public void Initialize()
        {
            _devicesPerformanceConfigurations = _staticDataService.GetConfiguration<DevicesPerformanceConfigurations>();
        }

        public PerformanceConfiguration GetConfiguration() =>
            _deviceDetector.IsMobile() ? _devicesPerformanceConfigurations.MobileConfiguration
            : _devicesPerformanceConfigurations.DesktopConfiguration;
    }
}
