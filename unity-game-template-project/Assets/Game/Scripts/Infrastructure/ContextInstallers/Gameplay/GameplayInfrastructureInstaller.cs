using GameTemplate.GameLifeCycle.Gameplay;
using GameTemplate.UI.Gameplay.PauseMenu.Presenters;
using GameTemplate.UI.Gameplay.PauseMenu.Views;
using GameTemplate.UI.Gameplay.PlayMenu.Views;
using GameTemplate.UI.Gameplay.Presenters;
using Sirenix.OdinInspector;

using UnityEngine;

namespace GameTemplate.Infrastructure.ContextInstallers.Gameplay
{
    public class GameplayInfrastructureInstaller : InfrastructureInstaller
    {
        [SerializeField, Required] private PlayMenuView _playMenuView;
        [SerializeField, Required] private PauseMenuView _pauseMenuView;

        public override void InstallBindings()
        {
            base.InstallBindings();

            BindPlayMenuPresenter();
            BindPauseMenuPresenter();
            BindGameplayBootstrapper();
        }

        private void BindPlayMenuPresenter() =>
            Container.BindInterfacesTo<PlayMenuPresenter>()
            .AsSingle()
            .WithArguments(_playMenuView)
            .NonLazy();

        private void BindPauseMenuPresenter() =>
            Container.BindInterfacesTo<PauseMenuPresenter>()
            .AsSingle()
            .WithArguments(_pauseMenuView)
            .NonLazy();

        private void BindGameplayBootstrapper() =>
            Container.BindInterfacesTo<GameplayBootstrapper>().AsSingle().NonLazy();
    }
}
