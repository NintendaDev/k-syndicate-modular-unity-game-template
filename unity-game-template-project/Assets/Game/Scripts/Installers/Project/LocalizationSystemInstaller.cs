using Modules.Localization.Core.Detectors;
using Modules.Localization.I2System;
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