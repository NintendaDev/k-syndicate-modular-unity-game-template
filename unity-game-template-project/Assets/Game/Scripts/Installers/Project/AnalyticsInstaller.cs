using Modules.Analytics;
using Zenject;

namespace GameTemplate.Installers.Project
{
    public sealed class AnalyticsInstaller : Installer<AnalyticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DummyAnalyticsSystem>().AsSingle();
        }
    }
}