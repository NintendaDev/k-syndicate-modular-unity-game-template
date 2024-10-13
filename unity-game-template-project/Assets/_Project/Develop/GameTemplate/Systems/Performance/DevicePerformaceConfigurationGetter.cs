using GameTemplate.Infrastructure.DevicesDetecting;
using Modules.AssetsManagement.StaticData;

namespace GameTemplate.Systems.Performance
{
    public class DevicePerformaceConfigurationGetter : IDevicePerformaceConfigurator
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IDeviceDetector _deviceDetector;
        private DevicesPerformanceConfigurations _devicesPerformanceConfigurations;

        public DevicePerformaceConfigurationGetter(IStaticDataService staticDataService, IDeviceDetector deviceDetector)
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
