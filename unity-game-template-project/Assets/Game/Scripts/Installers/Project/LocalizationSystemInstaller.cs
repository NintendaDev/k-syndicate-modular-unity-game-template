using Modules.Localization.Detectors;
using Modules.Localization.Systems.Demo;
using Zenject;

namespace Game.Installers.Project
{
    public class LocalizationSystemInstaller : Installer<LocalizationSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UnityLanguageDetector>().AsSingle();
            Container.BindInterfacesTo<SimpleLocalizationSystem>().AsSingle();
        }
    }
}