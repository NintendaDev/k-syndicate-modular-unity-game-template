using Modules.StateMachines;
using GameTemplate.Infrastructure.StateMachineComponents;
using System;
using Modules.Core.Systems;
using Modules.AudioManagement.Clip;
using Modules.AudioManagement.Player;
using Modules.AudioManagement.Systems;
using Zenject;

namespace GameTemplate.Infrastructure.ContextInstallers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResetObjects();
            BindMusicPlayService();
            BindAddressableAudioClipFactory();
            BindLocalizationProcessors();
            BindStateMachine();
            BindSceneObjectsDisposable();
        }

        private void BindResetObjects() =>
            Container.Bind<IReset>().FromComponentsInHierarchy().AsSingle();

        private void BindMusicPlayService()
        {
            Container.BindInterfacesAndSelfTo<MusicPlayerFactory>().AsSingle().WhenInjectedInto<MusicPlaySystem>();
            Container.BindInterfacesTo<MusicPlaySystem>().AsSingle();
        }
        private void BindAddressableAudioClipFactory()
        {
            Container.Bind<AddressableAudioClip>().AsTransient();
            Container.BindInterfacesAndSelfTo<AddressableAudioClipFactory>().AsSingle();
        }
        
        private void BindLocalizationProcessors() =>
            LocalizationProcessorsInstaller.Install(Container);

        private void BindStateMachine()
        {
            Container.Bind<SceneStateMachine>().AsSingle();
            Container.Bind<StatesFactory>().AsSingle();
        }

        private void BindSceneObjectsDisposable() =>
            Container.Bind<IDisposable>().To<SceneObjectDisposable>()
            .FromComponentInHierarchy().AsSingle();
    }
}
