using Cysharp.Threading.Tasks;
using GameTemplate.Services.PlayerAccountInfo;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.Signals;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class DownloadAccountInfoSceneState : SceneState
    {
        private readonly IPlayerAccountInfoService _playerAccountInfoService;

        public DownloadAccountInfoSceneState(SceneStateMachine stateMachine, IEventBus eventBus,
            ILogSystem logSystem, IPlayerAccountInfoService playerAccountInfoService) 
            : base(stateMachine, eventBus, logSystem)
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
