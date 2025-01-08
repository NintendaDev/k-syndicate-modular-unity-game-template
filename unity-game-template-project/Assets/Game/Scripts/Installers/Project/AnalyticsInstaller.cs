using Game.External.Analytics;
using Modules.Analytics;
using Modules.Analytics.GA;
using Zenject;

namespace Game.Installers.Project
{
    public sealed class AnalyticsInstaller : Installer<AnalyticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameAnalyticsSystem>().
                AsSingle()
                .WhenInjectedInto<AnalyticsSystemProxy>();
            
            Container.BindInterfacesAndSelfTo<StubAnalyticsSystem>().
                AsSingle()
                .WhenInjectedInto<AnalyticsSystemProxy>();
            
            Container.BindInterfacesTo<AnalyticsSystemProxy>().AsSingle();
        }
    }
}