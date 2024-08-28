using Cysharp.Threading.Tasks;

namespace ExternalLibs.CoreStateMachine.States
{
    public interface IExitableState
    {
        public UniTask Exit();
    }
}