using Modules.SceneManagement;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Bootstrap;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Data;
using GameTemplate.Infrastructure.DevicesDetecting;
using GameTemplate.Infrastructure.Inputs;
using GameTemplate.Infrastructure.LanguageSystem.Detectors;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents.Installers;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.Authorization;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.Localization;
using GameTemplate.Services.PlayerAccountInfo;
using GameTemplate.Services.PlayerStatistics;
using GameTemplate.Services.Popups;
using GameTemplate.Services.Progress;
using GameTemplate.Services.SaveLoad;
using GameTemplate.Services.StaticData;
using GameTemplate.Services.Wallet;
using GameTemplate.Systems;
using GameTemplate.Systems.Performance;
using GameTemplate.UI.LoadingCurtain;
using GameTemplate.UI.Serices.Popups.Factories;
using GameTemplate.UI.Services.Popups;
using GameTemplate.UI.Services.Popups.Factories;
using Modules.AssetManagement;
using Modules.AssetManagement.StaticData;
using Modules.Logging;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameTemplate.CompositionRoot
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField, Required] private GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;
        [SerializeField, Required] private StaticDataServiceConfiguration _staticDataServiceConfiguration;
        [SerializeField, Required] private PopupsAssetsConfiguration _popupsAssetsConfiguration;

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
            BindWalletsService();
            BindPlayerStatisticsService();
            BindAdvertisimentsService();
            BindGameLevelLoaderService();
            BindAuthorizationService();
            BindAnalyticsService();
            BindAdvertisimentsShowers();
            BindAudioMixerService();
            BindDefaultPlayerProgress();
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
            Container.BindInterfacesTo<ComponentAssetService>().AsSingle();
        }
            
        private void BindStaticDataService() =>
            Container.BindInterfacesTo<GameTemplateStaticDataService>()
                .AsSingle()
                .WithArguments(_staticDataServiceConfiguration);

        private void BindDeviceDetector() =>
            Container.BindInterfacesTo<UnityDeviceDetector>().AsSingle();

        private void BindTouchDetector() =>
            Container.BindInterfacesTo<LegacyTouchDetector>().AsSingle();

        private void BindDevicePerformanceConfigurationGetter() =>
            Container.BindInterfacesTo<DevicePerformaceConfigurationGetter>().AsSingle();

        private void BindSystemPerformanceSetter() =>
            Container.Bind<SystemPerformanceSetter>().AsSingle();

        private void BindPopupsService()
        {
            Container.BindInterfacesAndSelfTo<InfoPopupFabric>().AsSingle().WhenInjectedInto<PopupFactory>();
            Container.BindInterfacesAndSelfTo<ErrorPopupFabric>().AsSingle().WhenInjectedInto<PopupFactory>();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();

            Container.BindInterfacesTo<PopupsService>().AsSingle();
        }

        private void BindPlayerAccountInfoService() =>
            Container.BindInterfacesTo<DummyPlayerAccountInfoService>().AsSingle();

        private void BindLanguageDetector() =>
            Container.BindInterfacesTo<UnityLanguageDetector>().AsSingle();

        private void BindLocalizationService() =>
            Container.BindInterfacesTo<SimpleLocalizationService>().AsSingle();

        private void BindSceneLoader() =>
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
            
        private void BindInfrastructureUI()
        {
            Container.BindInterfacesAndSelfTo<LoadingCurtainFabric>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingCurtainProxy>().AsSingle();
        }

        private void BindPersistentProgressService() =>
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();

        private void BindWalletsService() =>
            Container.BindInterfacesTo<WalletService>().AsSingle();

        private void BindPlayerStatisticsService() =>
            Container.BindInterfacesAndSelfTo<PlayerStatisticsService>().AsSingle();

        private void BindAdvertisimentsService() =>
            Container.BindInterfacesTo<DummyAdvertisimentsService>().AsSingle();
            
        private void BindGameLevelLoaderService() =>
            Container.BindInterfacesTo<LevelLoaderService>().AsSingle();

        private void BindAuthorizationService() =>
            Container.BindInterfacesTo<DummyAuthorizationService>().AsSingle();

        private void BindAnalyticsService() =>
            Container.BindInterfacesTo<DummyAnalyticsService>().AsSingle();

        private void BindAdvertisimentsShowers()
        {
            Container.BindInterfacesTo<InterstitialAdvertisementShower>().AsTransient();
        }

        private void BindAudioMixerService() =>
            Container.BindInterfacesTo<AudioMixerService>().AsSingle();

        private void BindSaveLoadService() =>
            Container.BindInterfacesTo<PlayerPrefsSaveLoadService>().AsSingle();
            
        private void BindDefaultPlayerProgress() =>
            Container.BindInterfacesTo<DefaultPlayerProgressMaker>().AsSingle();

        private void BindGameBootstrapperFactory() =>
            Container.BindInterfacesAndSelfTo<GameBootstrapperFactory>().AsSingle();

        private void BindEventBus() =>
            Container.BindInterfacesTo<EventBus>().AsSingle();

        private void BindGameStatemachine() =>
            GameStateMachineInstaller.Install(Container);
    }
}
