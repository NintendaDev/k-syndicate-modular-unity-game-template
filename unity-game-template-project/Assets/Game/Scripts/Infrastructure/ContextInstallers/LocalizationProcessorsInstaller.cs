using Modules.Localization.Processors;
using Modules.Localization.Processors.Factories;
using Zenject;

namespace GameTemplate.Infrastructure.ContextInstallers 
{
    public class LocalizationProcessorsInstaller : Installer<LocalizationProcessorsInstaller>
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
