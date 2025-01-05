using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.UI.GameHub.Signals;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Logging;
using Modules.AudioManagement.Systems;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public sealed class MainSceneState : SceneState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IMusicPlaySystem _musicPlayService;

        public MainSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem, 
            ILoadingCurtain loadingCurtain, IMusicPlaySystem musicPlayService)
            : base(stateMachine, signalBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _musicPlayService = musicPlayService;
        }

        public async override UniTask Enter()
        {
            await base.Enter();

            StateSignalBus.Subscribe<LoginSignal>(OnLoginSignal);

            if (_musicPlayService.IsPlaying == false)
                _musicPlayService.PlayOrUnpause();

            _loadingCurtain.Hide();
        }

        public async override UniTask Exit()
        {
            await base.Exit();

            StateSignalBus.Unsubscribe<LoginSignal>(OnLoginSignal);
        }

        private async void OnLoginSignal() =>
            await StateMachine.SwitchState<AuthorizationSceneState>();
    }
}
