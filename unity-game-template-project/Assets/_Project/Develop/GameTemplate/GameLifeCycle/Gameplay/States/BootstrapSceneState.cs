using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.Infrastructure.Music;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.MusicPlay;
using GameTemplate.UI.LoadingCurtain;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class BootstrapSceneState : SceneState
    {
        private readonly IMusicPlayService _musicPlayService;
        private readonly LevelConfiguration _currentLevelConfiguration;
        private readonly AddressableAudioClipFactory _addressableAudioClipFactory;
        private readonly ILoadingCurtain _loadingCurtain;

        public BootstrapSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem, 
            ILoadingCurtain loadingCurtain, IMusicPlayService musicPlayService, 
            ICurrentLevelConfiguration levelConfigurator, AddressableAudioClipFactory addressableAudioClipFactory)
            : base(stateMachine, eventBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _musicPlayService = musicPlayService;
            _currentLevelConfiguration = levelConfigurator.CurrentLevelConfiguration;
            _addressableAudioClipFactory = addressableAudioClipFactory;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.Show();
            await Initialize();

            await StateMachine.SwitchState<StartGameplaySceneState>();
        }

        private async UniTask Initialize()
        {
            await _musicPlayService.InitializeAsync();

            AddressableAudioClip addressableAudioClip = _addressableAudioClipFactory.Create();

            if (await addressableAudioClip.TryInitializeAsync(_currentLevelConfiguration))
                _musicPlayService.Set(addressableAudioClip);
        }
    }
}
