using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.Loading.States;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.Authorization;
using GameTemplate.Services.Localization;
using GameTemplate.Services.Log;
using GameTemplate.Services.MusicPlay;
using GameTemplate.Services.Popups;
using GameTemplate.UI.LoadingCurtain;

namespace GameTemplate.GameLifeCycle.GameHub
{
    public class AuthorizationSceneState : SceneState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IMusicPlayService _musicPlayService;
        private readonly IPopupsService _popupsService;

        public AuthorizationSceneState(SceneStateMachine stateMachine, IEventBus eventBus, GameStateMachine gameStateMachine, ILogService logService,
            IAuthorizationService authorizationService, ILoadingCurtain loadingCurtain, IMusicPlayService musicPlayService,
            IPopupsService popupsService) 
            : base(stateMachine, eventBus, logService)
        {
            _gameStateMachine = gameStateMachine;
            _authorizationService = authorizationService;
            _loadingCurtain = loadingCurtain;
            _musicPlayService = musicPlayService;
            _popupsService = popupsService;
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
            await _popupsService.ShowInfoAsync(LocalizationTerm.Info, LocalizationTerm.SuccessAuthorizationMessage, LocalizationTerm.Ok);
            await _gameStateMachine.SwitchState<GameLoadingState>();
        }

        private async void OnLoginError()
        {
            await _popupsService.ShowErrorAsync(LocalizationTerm.Info, LocalizationTerm.SuccessAuthorizationMessage, LocalizationTerm.Ok);
            await StateMachine.SwitchState<MainSceneState>();
        }    
    }
}
