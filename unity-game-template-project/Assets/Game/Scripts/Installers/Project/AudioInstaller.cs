using Modules.AudioManagement.Mixer;
using Zenject;

namespace Game.Installers.Project
{
    public class AudioInstaller : Installer<AudioInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioMixerSystem>().AsSingle();
        }
    }
}