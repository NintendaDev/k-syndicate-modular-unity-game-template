using Cysharp.Threading.Tasks;
using Game.Infrastructure.StateMachineComponents;
using Game.Infrastructure.StateMachineComponents.States;
using Game.Services.GameLevelLoader;
using Game.UI.GameHub.Signals;
using Modules.AudioManagement.Mixer;
using Modules.AudioManagement.Player;
using Modules.AudioManagement.Types;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Logging;

namespace Game.GameLifeCycle.GameHub.States
{
    public sealed class MainSceneState : SceneState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IAudioMixerSystem _audioMixerSystem;
        private readonly IAudioAssetPlayer _audioAssetPlayer;
        private readonly IFastLoadInitialize _levelLoaderInitializer;

        public MainSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem, 
            ILoadingCurtain loadingCurtain, IAudioMixerSystem audioMixerSystem, 
            IAudioAssetPlayer audioAssetPlayer, IFastLoadInitialize levelLoaderInitializer)
            : base(stateMachine, signalBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _audioMixerSystem = audioMixerSystem;
            _audioAssetPlayer = audioAssetPlayer;
            _levelLoaderInitializer = levelLoaderInitializer;
        }

        public async override UniTask Enter()
        {
            await base.Enter();

            StateSignalBus.Subscribe<LoginSignal>(OnLoginSignal);
            StateSignalBus.Subscribe<LevelLoadSignal>(OnLevelLoadSignal);
            
            _audioMixerSystem.Unmute();

            if (_audioAssetPlayer.IsPlaying(AudioCode.GameHubMusic) == false)
                await _audioAssetPlayer.TryPlayAsync(AudioCode.GameHubMusic);

            _loadingCurtain.Hide();
        }

        public async override UniTask Exit()
        {
            await base.Exit();

            StateSignalBus.Unsubscribe<LevelLoadSignal>(OnLevelLoadSignal);
            StateSignalBus.Unsubscribe<LoginSignal>(OnLoginSignal);
        }

        private async void OnLevelLoadSignal(LevelLoadSignal signal)
        {
            _levelLoaderInitializer.InitializeFastLoad(signal.LevelCode);
            await StateMachine.SwitchState<FinishSceneState>();
        }

        private async void OnLoginSignal() =>
            await StateMachine.SwitchState<AuthorizationSceneState>();
    }
}
