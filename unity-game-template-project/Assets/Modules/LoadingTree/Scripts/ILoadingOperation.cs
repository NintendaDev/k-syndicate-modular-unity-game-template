using Cysharp.Threading.Tasks;

namespace Modules.LoadingTree
{
    public interface ILoadingOperation
    {
        public UniTask<LoadingResult> Run(LoadingBundle bundle);

        public float GetWeight() => 1;
        
        public float GetProgress() => 1;
    }
}