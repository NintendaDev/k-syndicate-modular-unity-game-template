using Modules.Analytics;
using Modules.Analytics.GA;
using Modules.Analytics.Stub;
using Zenject;

namespace Game.Installers.Project
{
    public sealed class AnalyticsInstaller : Installer<AnalyticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ParallelAnalyticsSystem>()
                .FromMethod(context =>
                {
                    return new ParallelAnalyticsSystem(context.Container.Instantiate<GameAnalyticsSystem>(),
                        context.Container.Instantiate<StubAnalyticsSystem>());
                })
                .AsSingle();
        }
    }
}