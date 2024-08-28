using GameTemplate.Infrastructure.Signals;
using GameTemplate.Services.Log;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class GameState : DefaultState
    {
        public GameState(GameStateMachine stateMachine, IEventBus eventBus, ILogService logService) 
            : base(stateMachine, eventBus, logService)
        {
        }
    }
}