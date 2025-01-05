using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class SceneState : DefaultState
    {
        public SceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem) 
            : base(stateMachine, signalBus, logSystem)
        {
        }
    }
}
