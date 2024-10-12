using Cysharp.Threading.Tasks;

namespace Modules.StateMachines.States
{
    public interface IState : IExitableState
    {
        public UniTask Enter();
    }
}