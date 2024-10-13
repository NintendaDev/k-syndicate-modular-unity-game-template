using Modules.SceneManagement;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.Bootstrap;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.DevicesDetecting;
using GameTemplate.Infrastructure.SaveManagement.Defaults;
using GameTemplate.Infrastructure.StateMachineComponents.Installers;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.Authorization;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.PlayerAccountInfo;
using GameTemplate.Services.PlayerStatistics;
using GameTemplate.Services.StaticData;
using GameTemplate.Systems;
using GameTemplate.Systems.Performance;
using GameTemplate.UI.LoadingCurtain;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.ControllManagement.Detectors;
using Modules.EventBus;
using Modules.Localization.Detectors;
using Modules.Localization.Systems.Demo;
using Modules.Logging;
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
            BindWallet();
            BindPlayerStatisticsService();
            BindAdvertisimentsService();
            BindGameLevelLoaderService();
            BindAuthorizationService();
            BindAnalyticsService();
            BindAdvertisimentsShowers();
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

            Container.BindInterfacesTo<Popups>().AsSingle();
        }

        private void BindPlayerAccountInfoService() =>
            Container.BindInterfacesTo<DummyPlayerAccountInfoService>().AsSingle();

        private void BindLanguageDetector() =>
            Container.BindInterfacesTo<UnityLanguageDetector>().AsSingle();

        private void BindLocalizationService() =>
            Container.BindInterfacesTo<SimpleLocalizationSystem>().AsSingle();

        private void BindSceneLoader() =>
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
            
        private void BindInfrastructureUI()
        {
            Container.BindInterfacesAndSelfTo<LoadingCurtainFabric>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingCurtainProxy>().AsSingle();
        }

        private void BindPersistentProgressService() =>
            Container.BindInterfacesTo<PersistentProgressProvider>().AsSingle();

        private void BindWallet() =>
            Container.BindInterfacesTo<Wallet>().AsSingle();

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
