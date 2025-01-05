using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using Modules.Authorization.Interfaces;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Localization.Types;
using Modules.Logging;
using Modules.AudioManagement.Systems;
using Modules.PopupsSystem;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public sealed class AuthorizationSceneState : SceneState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IMusicPlaySystem _musicPlayService;
        private readonly IPopups _popups;

        public AuthorizationSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, 
            GameStateMachine gameStateMachine, ILogSystem logSystem,
            IAuthorizationService authorizationService, ILoadingCurtain loadingCurtain, 
            IMusicPlaySystem musicPlayService, IPopups popups) 
            : base(stateMachine, signalBus, logSystem)
        {
            _gameStateMachine = gameStateMachine;
            _authorizationService = authorizationService;
            _loadingCurtain = loadingCurtain;
            _musicPlayService = musicPlayService;
            _popups = popups;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _musicPlayService.Pause();
            _loadingCurtain.ShowWithoutProgressBar();

            _authorizationService.LoginCompleted += OnLoginCompleted;
            _authorizationService.LoginError += OnLoginError;

            _authorizationService.StartAuthorizationBehaviour();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _authorizationService.LoginCompleted -= OnLoginCompleted;
            _authorizationService.LoginError -= OnLoginError;
        }

        private async void OnLoginCompleted()
        {
            await _popups.ShowInfoAsync(LocalizationTerm.Info, LocalizationTerm.SuccessAuthorizationMessage, 
                LocalizationTerm.Ok);
            
            await _gameStateMachine.SwitchState<GameLoadingState>();
        }

        private async void OnLoginError()
        {
            await _popups.ShowErrorAsync(LocalizationTerm.Info, LocalizationTerm.SuccessAuthorizationMessage, 
                LocalizationTerm.Ok);
            
            await StateMachine.SwitchState<MainSceneState>();
        }    
    }
}
