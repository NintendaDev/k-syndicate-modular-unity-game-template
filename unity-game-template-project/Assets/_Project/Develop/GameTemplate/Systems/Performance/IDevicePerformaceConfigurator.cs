namespace GameTemplate.Systems.Performance
{
    public interface IDevicePerformaceConfigurator
    {
        public PerformanceConfiguration GetConfiguration();

        public void Initialize();
    }
}