using Cysharp.Threading.Tasks;

namespace ExternalLibs.CoreStateMachine.States
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        public UniTask Enter(TPayload payload);
    }
}