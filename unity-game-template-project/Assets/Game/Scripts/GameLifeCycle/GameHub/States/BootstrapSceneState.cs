using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;
using Modules.AudioManagement.Clip;
using Modules.AudioManagement.Systems;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public sealed class BootstrapSceneState : SceneState
    {
        private readonly IMusicPlaySystem _musicPlayService;
        private readonly IStaticDataService _staticDataService;
        private readonly AddressableAudioClipFactory _addressableAudioClipFactory;

        public BootstrapSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem, 
            IMusicPlaySystem musicPlayService, IStaticDataService staticDataService, 
            AddressableAudioClipFactory addressableAudioClipFactory)
            : base(stateMachine, signalBus, logSystem)
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