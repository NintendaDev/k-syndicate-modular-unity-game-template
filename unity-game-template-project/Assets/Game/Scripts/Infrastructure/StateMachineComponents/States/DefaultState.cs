using Cysharp.Threading.Tasks;
using Modules.StateMachines;
using Modules.StateMachines.States;
using System;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class DefaultState : IState
    {
        private readonly Type _stateType;

        public DefaultState(IStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem)
        {
            StateMachine = stateMachine;
            LogSystem = logSystem;
            StateEventBus = eventBus;
            _stateType = GetType();
        }

        protected IStateMachine StateMachine { get; }

        protected IEventBus StateEventBus { get; }

        protected ILogSystem LogSystem { get; }

        public virtual UniTask Enter()
        {
            LogSystem.Log($"Enter {_stateType} state");

            return default;
        }

        public virtual UniTask Exit()
        {
            LogSystem.Log($"Exit {_stateType} state");

            return default;
        }
    }
}
