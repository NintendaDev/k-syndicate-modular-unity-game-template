using Modules.Localization.Core.Processors;
using Modules.Localization.Core.Processors.Factories;
using Zenject;

namespace Game.Infrastructure.ContextInstallers 
{
    public sealed class LocalizationProcessorsInstaller : Installer<LocalizationProcessorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalizedTermProcessorFactory>()
                            .AsSingle()
                            .WhenInjectedInto<LocalizedTermProcessorLinker>();
            
            Container.BindInterfacesAndSelfTo<LocalizedTermProcessorLinker>().AsSingle();
        }
    }
}
