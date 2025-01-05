using Modules.AudioManagement.Mixer;
using Zenject;

namespace GameTemplate.Installers.Project
{
    public class AudioInstaller : Installer<AudioInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioMixerSystem>().AsSingle();
        }
    }
}