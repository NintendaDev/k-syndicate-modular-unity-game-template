using Modules.StateMachines;
using GameTemplate.GameLifeCycle.Loading;
using GameTemplate.Infrastructure.StateMachineComponents;
using Zenject;

namespace GameTemplate.Infrastructure.ContextInstallers.Loading
{
    public class GameLoadingSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameLoadingSceneBootstrapper>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StatesFactory>().AsSingle();
            Container.Bind<SceneStateMachine>().AsSingle();
        }
    }
}
