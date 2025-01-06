using Modules.EventBus;
using Modules.Logging;

namespace Game.Infrastructure.StateMachineComponents.States
{
    public abstract class GameState : DefaultState
    {
        public GameState(GameStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem) 
            : base(stateMachine, signalBus, logSystem)
        {
        }
    }
}