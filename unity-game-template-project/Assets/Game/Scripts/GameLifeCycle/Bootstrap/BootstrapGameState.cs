using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using Modules.LoadingCurtain;
using GameTemplate.Infrastructure.StateMachineComponents;
using Modules.Analytics;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.Device.Performance;
using Modules.Device.Performance.Configurations;
using Modules.EventBus;
using Modules.Localization.Systems;
using Modules.Logging;
using Modules.SaveManagement.Interfaces;

namespace GameTemplate.GameLifeCycle.Bootstrap
{
    public class BootstrapGameState : GameState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly LoadingCurtainProxy _loadingCurtainProxy;
        private readonly ILevelLoaderService _gameLevelLoaderService;
        private readonly IAudioMixerSystem _audioMixerSystem;
        private readonly ILocalizationSystem _localizationSystem;
        private readonly IAnalyticsSystem _analyticsSystem;
        private readonly SystemPerformanceSetter _performanceSetter;
        private readonly ISaveLoadSystem _saveLoadSystem;
        private readonly IPerformaceConfiguration _devicePerformaceConfigurator;

        public BootstrapGameState(GameStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem, 
            IAnalyticsSystem analyticsSystem, IStaticDataService staticDataService, 
            LoadingCurtainProxy loadingCurtainProxy, ILevelLoaderService gameLevelLoaderService, 
            IAudioMixerSystem audioMixerSystem, IPerformaceConfiguration devicePerformaceConfigurator,
            ILocalizationSystem localizationSystem, ISaveLoadSystem saveLoadSystem, 
            SystemPerformanceSetter performanceSetter)
            
            : base(stateMachine, eventBus, logSystem)
        {
            _staticDataService = staticDataService;
            _loadingCurtainProxy = loadingCurtainProxy;
            _gameLevelLoaderService = gameLevelLoaderService;
            _audioMixerSystem = audioMixerSystem;
            _localizationSystem = localizationSystem;
            _analyticsSystem = analyticsSystem;
            _performanceSetter = performanceSetter;
            _saveLoadSystem = saveLoadSystem;
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
            await _saveLoadSystem.InitializeAsync();
            _gameLevelLoaderService.Initialize();
            _audioMixerSystem.Initialize();
            _localizationSystem.Initialize();
            _analyticsSystem.Initialize();
        }
    }
}