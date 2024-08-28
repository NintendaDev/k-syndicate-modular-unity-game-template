using Cysharp.Threading.Tasks;

namespace ExternalLibs.CoreStateMachine.States
{
    public interface IState : IExitableState
    {
        public UniTask Enter();
    }
}