using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Services.Localization;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.UI.LoadingCurtain;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Systems;
using GameTemplate.Systems.Performance;
using GameTemplate.Services.SaveLoad;
using GameTemplate.Infrastructure.Signals;
using Modules.AssetManagement.StaticData;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Bootstrap
{
    public class BootstrapGameState : GameState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly LoadingCurtainProxy _loadingCurtainProxy;
        private readonly ILevelLoaderService _gameLevelLoaderService;
        private readonly IAudioMixerService _audioMixerService;
        private readonly ILocalizationService _localizationService;
        private readonly IAnalyticsService _analyticsService;
        private readonly SystemPerformanceSetter _performanceSetter;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IDevicePerformaceConfigurator _devicePerformaceConfigurator;

        public BootstrapGameState(GameStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem, 
            IAnalyticsService analyticsService, IStaticDataService staticDataService, 
            LoadingCurtainProxy loadingCurtainProxy, ILevelLoaderService gameLevelLoaderService, 
            IAudioMixerService audioMixerService, IDevicePerformaceConfigurator devicePerformaceConfigurator,
            ILocalizationService localizationService, ISaveLoadService saveLoadService, 
            SystemPerformanceSetter performanceSetter)
            
            : base(stateMachine, eventBus, logSystem)
        {
            _staticDataService = staticDataService;
            _loadingCurtainProxy = loadingCurtainProxy;
            _gameLevelLoaderService = gameLevelLoaderService;
            _audioMixerService = audioMixerService;
            _localizationService = localizationService;
            _analyticsService = analyticsService;
            _performanceSetter = performanceSetter;
            _saveLoadService = saveLoadService;
            _devicePerformaceConfigurator = devicePerformaceConfigurator;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            await InitializeServices();
            await StateMachine.SwitchState<GameLoadingState>();
        }

        private async UniTask InitializeServices()
        {
            await _staticDataService.InitializeAsync();
            _devicePerformaceConfigurator.Initialize();
            _performanceSetter.Initialize();
            await _loadingCurtainProxy.InitializeAsync();
            await _saveLoadService.InitializeAsync();
            _gameLevelLoaderService.Initialize();
            _audioMixerService.Initialize();
            _localizationService.Initialize();
            _analyticsService.Initialize();
        }
    }
}