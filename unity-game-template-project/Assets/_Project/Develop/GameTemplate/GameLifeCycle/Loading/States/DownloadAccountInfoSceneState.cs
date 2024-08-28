using Cysharp.Threading.Tasks;
using GameTemplate.Services.PlayerAccountInfo;
using GameTemplate.Services.Log;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.Signals;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class DownloadAccountInfoSceneState : SceneState
    {
        private readonly IPlayerAccountInfoService _playerAccountInfoService;

        public DownloadAccountInfoSceneState(SceneStateMachine stateMachine, IEventBus eventBus,
            ILogService logService, IPlayerAccountInfoService playerAccountInfoService) 
            : base(stateMachine, eventBus, logService)
        {
            _playerAccountInfoService = playerAccountInfoService;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            await _playerAccountInfoService.InitializeAsync();
            await StateMachine.SwitchState<LoadPlayerProgressSceneState>();
        }
    }
}
