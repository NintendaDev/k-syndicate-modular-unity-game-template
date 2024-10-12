using Cysharp.Threading.Tasks;

namespace Modules.StateMachines.States
{
    public interface IExitableState
    {
        public UniTask Exit();
    }
}