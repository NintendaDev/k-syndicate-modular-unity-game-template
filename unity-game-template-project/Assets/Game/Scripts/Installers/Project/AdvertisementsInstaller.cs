﻿using Modules.Advertisements.AnalyticsAddon;
using Modules.Advertisements.Dummy;
using Zenject;

namespace Game.Installers.Project
{
    public class AdvertisementsInstaller : Installer<AdvertisementsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DummyAdvertisementsSystem>()
                .AsSingle()
                .WhenInjectedInto<AdvertisementsFacade>();
            
            Container.BindInterfacesAndSelfTo<AdvertisementsFacade>().AsSingle();
        }
    }
}