using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class SceneState : DefaultState
    {
        public SceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem) 
            : base(stateMachine, eventBus, logSystem)
        {
        }
    }
}
