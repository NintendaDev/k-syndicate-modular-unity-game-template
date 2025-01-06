using Cysharp.Threading.Tasks;
using Game.Infrastructure.StateMachineComponents;
using Game.Infrastructure.StateMachineComponents.States;
using Modules.AudioManagement.Player;
using Modules.AudioManagement.Types;
using Modules.EventBus;
using Modules.Logging;

namespace Game.GameLifeCycle.GameHub.States
{
    public sealed class BootstrapSceneState : SceneState
    {
        private readonly IAudioAssetPlayer _audioAssetPlayer;

        public BootstrapSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem, 
            IAudioAssetPlayer audioAssetPlayer)
            : base(stateMachine, signalBus, logSystem)
        {
            _audioAssetPlayer = audioAssetPlayer;
        }

        public async override UniTask Enter()
        {
            await base.Enter();

            await InitializeAudioAssetPlayer();
            await StateMachine.SwitchState<MainSceneState>();
        }

        private async UniTask InitializeAudioAssetPlayer()
        {
            _audioAssetPlayer.Initialize();
            await _audioAssetPlayer.WarmupAsync(AudioCode.GameHubMusic);
        }
    }
}