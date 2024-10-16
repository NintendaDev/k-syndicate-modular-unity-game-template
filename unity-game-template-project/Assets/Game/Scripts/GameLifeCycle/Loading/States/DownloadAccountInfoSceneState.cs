using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using Modules.EventBus;
using Modules.Logging;
using Modules.NetworkAccount;

namespace GameTemplate.GameLifeCycle.Loading
{
    public sealed class DownloadAccountInfoSceneState : SceneState
    {
        private readonly INetworkAccount _networkAccount;

        public DownloadAccountInfoSceneState(SceneStateMachine stateMachine, IEventBus eventBus,
            ILogSystem logSystem, INetworkAccount networkAccount) 
            : base(stateMachine, eventBus, logSystem)
        {
            _networkAccount = networkAccount;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            await _networkAccount.InitializeAsync();
            await StateMachine.SwitchState<LoadPlayerProgressSceneState>();
        }
    }
}
