using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.Authorization;
using GameTemplate.Services.MusicPlay;
using GameTemplate.UI.LoadingCurtain;
using Modules.EventBus;
using Modules.Localization.Types;
using Modules.Logging;
using Modules.PopupsSystem;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public class AuthorizationSceneState : SceneState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IMusicPlayService _musicPlayService;
        private readonly IPopups _popups;

        public AuthorizationSceneState(SceneStateMachine stateMachine, IEventBus eventBus, 
            GameStateMachine gameStateMachine, ILogSystem logSystem,
            IAuthorizationService authorizationService, ILoadingCurtain loadingCurtain, 
            IMusicPlayService musicPlayService, IPopups popups) 
            : base(stateMachine, eventBus, logSystem)
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
            _loadingCurtain.Show();

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
