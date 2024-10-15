namespace Modules.Device.Performance.Configurations
{
    public interface IPerformaceConfiguration
    {
        public PerformanceConfiguration GetConfiguration();

        public void Initialize();
    }
}