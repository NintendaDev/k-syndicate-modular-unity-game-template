using Game.GameLifeCycle.Loading;
using Game.Infrastructure.StateMachineComponents;
using Modules.StateMachines;
using Zenject;

namespace Game.Infrastructure.ContextInstallers.Loading
{
    public sealed class GameLoadingSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameLoadingSceneBootstrapper>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StatesFactory>().AsSingle();
            Container.Bind<SceneStateMachine>().AsSingle();
        }
    }
}
