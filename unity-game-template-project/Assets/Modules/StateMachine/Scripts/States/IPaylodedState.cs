using Cysharp.Threading.Tasks;

namespace Modules.StateMachines.States
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        public UniTask Enter(TPayload payload);
    }
}