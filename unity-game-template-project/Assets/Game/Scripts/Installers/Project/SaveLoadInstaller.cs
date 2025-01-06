using Game.Application.SaveLoad;
using Modules.AudioManagement.Mixer;
using Modules.SaveSystem.SaveLoad;
using Modules.Wallets.Systems;
using Zenject;

namespace Game.Installers.Project
{
    public class SaveLoadInstaller : Installer<SaveLoadInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DefaultSaveLoader>().AsSingle();
            Container.BindInterfacesTo<WalletSerializer>().AsCached().WhenInjectedInto<GameSaveLoader>();
            Container.BindInterfacesTo<AudioMixerSystemsSerializer>().AsCached().WhenInjectedInto<GameSaveLoader>();
            Container.BindInterfacesTo<GameSaveLoader>().AsSingle();
            Container.BindInterfacesTo<SaveLoadController>().AsSingle();
        }
    }
}