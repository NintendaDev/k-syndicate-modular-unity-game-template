using Cysharp.Threading.Tasks;
using ExternalLibs.CoreStateMachine;
using ExternalLibs.CoreStateMachine.States;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Services.Log;
using System;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class DefaultState : IState
    {
        private readonly Type _stateType;

        public DefaultState(IStateMachine stateMachine, IEventBus eventBus, ILogService logService)
        {
            StateMachine = stateMachine;
            LogService = logService;
            StateEventBus = eventBus;
            _stateType = GetType();
        }

        protected IStateMachine StateMachine { get; }

        protected IEventBus StateEventBus { get; }

        protected ILogService LogService { get; }

        public virtual UniTask Enter()
        {
            LogService.Log($"Enter {_stateType} state");

            return default;
        }

        public virtual UniTask Exit()
        {
            LogService.Log($"Exit {_stateType} state");

            return default;
        }
    }
}
