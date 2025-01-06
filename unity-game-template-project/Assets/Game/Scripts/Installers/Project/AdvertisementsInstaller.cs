using Modules.Advertisements.Systems;
using Zenject;

namespace Game.Installers.Project
{
    public class AdvertisementsInstaller : Installer<AdvertisementsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DummyAdvertisementsSystem>().AsSingle();
        }
    }
}