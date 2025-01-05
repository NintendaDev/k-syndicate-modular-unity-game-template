using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Modules.LoadingTree
{
    public sealed class SequenceOperation : ILoadingOperation
    {
        private readonly IReadOnlyList<ILoadingOperation> _operations;

        private readonly float _maxChildWeight;
        private float _accumulativeChildWeight;
        private ILoadingOperation _currentOperation;

        private readonly float _weight;

        public SequenceOperation(int weight = 1, params ILoadingOperation[] operations)
        {
            _weight = weight;
            _operations = operations;
            _maxChildWeight = operations.Sum(operation => operation.GetWeight());
        }

        public async UniTask<LoadingResult> Run(LoadingBundle bundle)
        {
            _currentOperation = null;
            _accumulativeChildWeight = 0;

            foreach (ILoadingOperation operation in _operations)
            {
                _currentOperation = operation;

                LoadingResult result = await _currentOperation.Run(bundle);
                
                if (result.IsSuccess == false)
                    return LoadingResult.Error(result.Message);

                _accumulativeChildWeight += _currentOperation.GetWeight();
            }

            _currentOperation = null;
            
            return LoadingResult.Success();
        }

        public float GetWeight() => _weight;

        public float GetProgress()
        {
            float childWeight = _accumulativeChildWeight;

            if (_currentOperation != null) 
                childWeight += _currentOperation.GetWeight() * _currentOperation.GetProgress();

            return childWeight / _maxChildWeight;
        }
    }
}