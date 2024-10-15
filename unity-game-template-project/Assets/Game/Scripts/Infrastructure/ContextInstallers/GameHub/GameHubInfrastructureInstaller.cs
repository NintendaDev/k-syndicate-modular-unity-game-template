using GameTemplate.GameLifeCycle.GameHub;
using GameTemplate.UI.GameHub.LevelsMenu.Factories;
using GameTemplate.UI.GameHub.LevelsMenu.Presenters;
using GameTemplate.UI.GameHub.LevelsMenu.Views;
using GameTemplate.UI.GameHub.MainMenu.Presenters;
using GameTemplate.UI.GameHub.MainMenu.Views;
using Modules.AudioManagement.UI.Presenters;
using Modules.AudioManagement.UI.Views;
using Modules.Wallets.UI.Factories;
using Modules.Wallets.UI.Presenters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.Infrastructure.ContextInstallers.GameHub
{
    public sealed class GameHubInfrastructureInstaller : InfrastructureInstaller
    {
        [SerializeField, Required] private MainMenuView _mainMenuView;
        [SerializeField, Required] private LevelsMenuView _levelsMenuView;
        [SerializeField, Required] private AudioSettingsView _audioSettingsView;

        public override void InstallBindings()
        {
            base.InstallBindings();

            BindMainMenuPresenter();
            BindLevelsMenuPresenter();
            BindWalletsPanelPresenter();
            BindSettingsPresenter();
            BindGameHubBootstrapper();
        }

        private void BindMainMenuPresenter() 
        {
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>()
                .AsSingle()
                .WithArguments(_mainMenuView)
                .NonLazy();
        }

        private void BindLevelsMenuPresenter()
        {
            Container.BindInterfacesAndSelfTo<LevelViewFactory>()
                .AsSingle()
                .WhenInjectedInto<LevelsMenuPresenter>();

            Container.Bind<LevelsMenuPresenter>()
                .AsSingle()
                .WithArguments(_levelsMenuView)
                .NonLazy();
        }
            
        private void BindWalletsPanelPresenter()
        {
            Container.BindInterfacesAndSelfTo<WalletViewFactory>()
                .AsSingle()
                .WhenInjectedInto<WalletsPanelPresenter>();

            Container.Bind<WalletsPanelPresenter>()
                .AsSingle()
                .WithArguments(_mainMenuView)
                .NonLazy();
        }
            
        private void BindSettingsPresenter()
        {
            Container.BindInterfacesTo<AudioSettingsPresenter>()
                .AsSingle()
                .WithArguments(_audioSettingsView)
                .NonLazy();
        }
            
        private void BindGameHubBootstrapper() =>
             Container.BindInterfacesTo<GameHubBootstrapper>().AsSingle().NonLazy();
    }
}
