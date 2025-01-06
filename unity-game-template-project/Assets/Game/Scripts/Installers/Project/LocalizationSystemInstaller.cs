using Game.Application.Localization;
using Modules.Localization.Detectors;
using Zenject;

namespace Game.Installers.Project
{
    public class LocalizationSystemInstaller : Installer<LocalizationSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UnityLanguageDetector>().AsSingle();
            Container.BindInterfacesTo<I2LocalizationSystem>().AsSingle();
        }
    }
}