using GameTemplate.Infrastructure.Signals;
using GameTemplate.Services.Log;

namespace GameTemplate.Infrastructure.StateMachineComponents.States
{
    public abstract class SceneState : DefaultState
    {
        public SceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogService logService) 
            : base(stateMachine, eventBus, logService)
        {
        }
    }
}
