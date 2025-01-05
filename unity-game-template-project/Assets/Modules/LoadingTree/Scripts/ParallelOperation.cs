using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Modules.LoadingTree
{
    public sealed class ParallelOperation : ILoadingOperation
    {
        private readonly IReadOnlyList<ILoadingOperation> _operations;

        private readonly float _weight;
        private readonly float _maxChildWeight;
        
        public ParallelOperation(int weight = 1, params ILoadingOperation[] operations)
        {
            _weight = weight;
            _operations = operations;
            _maxChildWeight = operations.Sum(operation => operation.GetWeight());
        }

        public async UniTask<LoadingResult> Run(LoadingBundle bundle)
        {
            int count = _operations.Count;
            var pool = System.Buffers.ArrayPool<UniTask<LoadingResult>>.Shared;
            UniTask<LoadingResult>[] tasks = pool.Rent(count);

            try
            {
                for (int i = 0; i < count; i++)
                {
                    ILoadingOperation operation = _operations[i];
                    UniTask<LoadingResult> task = operation.Run(bundle);
                    tasks[i] = task;
                }

                LoadingResult[] results = await UniTask.WhenAll(tasks);
              
                for (int i = 0; i < count; i++)
                {
                    LoadingResult result = results[i];
                    
                    if (result.IsSuccess == false)
                        return LoadingResult.Error(result.Message);
                }

                return LoadingResult.Success();
            }
            finally
            {
                pool.Return(tasks);
            }
        }
        
        public float GetWeight()
        {
            return _weight;
        }

        public float GetProgress()
        {
            float currentWeight = 0;

            foreach (ILoadingOperation operation in _operations)
                currentWeight += operation.GetWeight() * operation.GetProgress();

            return currentWeight / _maxChildWeight;
        }
    }
}