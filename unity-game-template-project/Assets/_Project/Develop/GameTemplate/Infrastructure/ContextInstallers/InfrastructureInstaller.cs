using ExternalLibs.CoreStateMachine;
using GameTemplate.Core;
using GameTemplate.Infrastructure.Music;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.MusicPlay;
using System;
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
            Container.BindInterfacesAndSelfTo<MusicPlayerFactory>().AsSingle().WhenInjectedInto<MusicPlayService>();
            Container.BindInterfacesTo<MusicPlayService>().AsSingle();
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
