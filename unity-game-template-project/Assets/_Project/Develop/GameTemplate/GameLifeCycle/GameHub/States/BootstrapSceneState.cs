using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Music;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.MusicPlay;
using Modules.AssetManagement.StaticData;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public class BootstrapSceneState : SceneState
    {
        private readonly IMusicPlayService _musicPlayService;
        private readonly IStaticDataService _staticDataService;
        private readonly AddressableAudioClipFactory _addressableAudioClipFactory;

        public BootstrapSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem, 
            IMusicPlayService musicPlayService, IStaticDataService staticDataService, 
            AddressableAudioClipFactory addressableAudioClipFactory)
            : base(stateMachine, eventBus, logSystem)
        {
            _musicPlayService = musicPlayService;
            _staticDataService = staticDataService;
            _addressableAudioClipFactory = addressableAudioClipFactory;
        }

        public async override UniTask Enter()
        {
            await base.Enter();

            GameHubConfiguration gameHubConfiguration = _staticDataService.GetConfiguration<GameHubConfiguration>();

            await _musicPlayService.InitializeAsync();

            AddressableAudioClip addressableAudioClip = _addressableAudioClipFactory.Create();

            if (await addressableAudioClip.TryInitializeAsync(gameHubConfiguration))
                _musicPlayService.Set(addressableAudioClip);

            await StateMachine.SwitchState<MainSceneState>();
        }
    }
}