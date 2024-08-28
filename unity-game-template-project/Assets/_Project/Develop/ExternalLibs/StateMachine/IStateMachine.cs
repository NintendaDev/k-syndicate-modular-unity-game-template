using Cysharp.Threading.Tasks;
using ExternalLibs.CoreStateMachine.States;

namespace ExternalLibs.CoreStateMachine
{
    public interface IStateMachine
    {
        public UniTask SwitchState<TState>() where TState : class, IState;

        public UniTask SwitchState<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>;

        public void RegisterState<TState>(TState state) where TState : IExitableState;
    }
}