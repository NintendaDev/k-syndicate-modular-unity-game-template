using Modules.SceneManagement;
using GameTemplate.Infrastructure.Bootstrap;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.SaveManagement.Defaults;
using GameTemplate.Infrastructure.StateMachineComponents.Installers;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.PlayerStatistics;
using Modules.Advertisements.Systems;
using Modules.Analytics;
using Modules.LoadingCurtain;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.Authorization.Interfaces;
using Modules.ControllManagement.Detectors;
using Modules.Device.Detecting;
using Modules.Device.Performance;
using Modules.EventBus;
using Modules.LoadingCurtain.Configurations;
using Modules.Localization.Detectors;
using Modules.Localization.Systems.Demo;
using Modules.Logging;
using Modules.NetworkAccount;
using Modules.PopupsSystem;
using Modules.PopupsSystem.Configurations;
using Modules.PopupsSystem.UI.Factories;
using Modules.SaveManagement.Persistent;
using Modules.SaveManagement.Systems;
using Modules.Wallets.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.CompositionRoot
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField, Required] private GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;
        [SerializeField, Required] private StaticDataServiceConfiguration _staticDataServiceConfiguration;
        [SerializeField, Required] private PopupsAssetsConfiguration _popupsAssetsConfiguration;
        [SerializeField, Required] private LoadingCurtainConfiguration _curtainConfiguration;
        
        public override void InstallBindings()
        {
            BindInfrastructureAssetsConfiguration();
            BindLogService();
            BindAssetsServices();
            BindStaticDataService();
            BindDeviceDetector();
            BindTouchDetector();
            BindDevicePerformanceConfigurationGetter();
            BindSystemPerformanceSetter();
            BindPopupsService();
            BindPlayerAccountInfoService();
            BindLanguageDetector();
            BindLocalizationService();
            BindSceneLoader();
            BindInfrastructureUI();
            BindPersistentProgressService();
            BindWallet();
            BindPlayerStatisticsService();
            BindAdvertisimentsService();
            BindGameLevelLoaderService();
            BindAuthorizationService();
            BindAnalyticsService();
            BindAudioMixerService();
            BindDefaultPlayerProgressProvider();
            BindSaveLoadService();
            BindGameBootstrapperFactory();
            BindEventBus();
            BindGameStatemachine();
        }

        private void BindInfrastructureAssetsConfiguration() =>
            Container.Bind<GameLoadingAssetsConfiguration>().FromInstance(_gameLoadingAssetsConfiguration);

        private void BindLogService() =>
            Container.BindInterfacesTo<LogSystem>().AsSingle();

        private void BindAssetsServices()
        {
            Container.BindInterfacesTo<AddressablesService>().AsSingle();
        }
            
        private void BindStaticDataService() =>
            Container.BindInterfacesTo<StaticDataService>()
                .AsSingle()
                .WithArguments(_staticDataServiceConfiguration);

        private void BindDeviceDetector() =>
            Container.BindInterfacesTo<UnityDeviceDetector>().AsSingle();

        private void BindTouchDetector() =>
            Container.BindInterfacesTo<LegacyTouchDetector>().AsSingle();

        private void BindDevicePerformanceConfigurationGetter() =>
            Container.BindInterfacesTo<PerformaceConfigurationProvider>().AsSingle();

        private void BindSystemPerformanceSetter() =>
            Container.Bind<SystemPerformanceSetter>().AsSingle();

        private void BindPopupsService()
        {
            Container.BindInterfacesAndSelfTo<InfoPopupFactory>().AsSingle().WhenInjectedInto<PopupFactory>();
            Container.BindInterfacesAndSelfTo<ErrorPopupFactory>().AsSingle().WhenInjectedInto<PopupFactory>();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();

            Container.BindInterfacesTo<Popups>().AsSingle();
        }

        private void BindPlayerAccountInfoService() =>
            Container.BindInterfacesTo<DummyNetworkAccount>().AsSingle();

        private void BindLanguageDetector() =>
            Container.BindInterfacesTo<UnityLanguageDetector>().AsSingle();

        private void BindLocalizationService() =>
            Container.BindInterfacesTo<SimpleLocalizationSystem>().AsSingle();

        private void BindSceneLoader() =>
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
            
        private void BindInfrastructureUI()
        {
            Container.BindInterfacesAndSelfTo<LoadingCurtainFabric>()
                .AsSingle()
                .WithArguments(_curtainConfiguration);
            
            Container.BindInterfacesAndSelfTo<LoadingCurtainProxy>().AsSingle();
        }

        private void BindPersistentProgressService() =>
            Container.BindInterfacesTo<PersistentProgressProvider>().AsSingle();

        private void BindWallet() =>
            Container.BindInterfacesTo<Wallet>().AsSingle();

        private void BindPlayerStatisticsService() =>
            Container.BindInterfacesAndSelfTo<PlayerStatisticsService>().AsSingle();

        private void BindAdvertisimentsService() =>
            Container.BindInterfacesTo<DummyAdvertisementsSystem>().AsSingle();
            
        private void BindGameLevelLoaderService() =>
            Container.BindInterfacesTo<LevelLoaderService>().AsSingle();

        private void BindAuthorizationService() =>
            Container.BindInterfacesTo<DummyAuthorizationService>().AsSingle();

        private void BindAnalyticsService() =>
            Container.BindInterfacesTo<DummyAnalyticsSystem>().AsSingle();

        private void BindAudioMixerService() =>
            Container.BindInterfacesTo<AudioMixerSystem>().AsSingle();

        private void BindSaveLoadService() =>
            Container.BindInterfacesTo<PlayerPrefsSaveLoadSystem>().AsSingle();
            
        private void BindDefaultPlayerProgressProvider() =>
            Container.BindInterfacesTo<DefaultPlayerProgressProvider>().AsSingle();

        private void BindGameBootstrapperFactory() =>
            Container.BindInterfacesAndSelfTo<GameBootstrapperFactory>().AsSingle();

        private void BindEventBus() =>
            Container.BindInterfacesTo<EventBus>().AsSingle();

        private void BindGameStatemachine() =>
            GameStateMachineInstaller.Install(Container);
    }
}
