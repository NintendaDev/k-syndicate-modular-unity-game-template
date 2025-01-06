using Modules.StateMachines;
using System;
using Game.Infrastructure.StateMachineComponents;
using Modules.AudioManagement.Player;
using Modules.Core.Systems;
using Zenject;

namespace Game.Infrastructure.ContextInstallers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResetObjects();
            BindAudioAssetPlayer();
            BindLocalizationProcessors();
            BindStateMachine();
            BindSceneObjectsDisposable();
        }

        private void BindResetObjects() =>
            Container.Bind<IReset>().FromComponentsInHierarchy().AsSingle();

        private void BindAudioAssetPlayer() =>
            Container.BindInterfacesTo<AudioAssetPlayer>().AsSingle();
        
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
