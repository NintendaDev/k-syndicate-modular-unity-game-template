using GameTemplate.Infrastructure.Signals;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class GameState : DefaultState
    {
        public GameState(GameStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem) 
            : base(stateMachine, eventBus, logSystem)
        {
        }
    }
}