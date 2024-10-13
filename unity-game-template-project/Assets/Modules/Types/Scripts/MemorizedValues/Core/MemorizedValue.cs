using System;
using UnityEngine;

namespace Modules.Types.MemorizedValues.Core
{
    [Serializable]
    public class MemorizedValue<T> where T : IComparable
    {
        [SerializeField] private T _currentValue;
        [SerializeField] private T _previousValue;

        public MemorizedValue()
        {
        }

        public MemorizedValue(T value)
        {
            _currentValue = value;
            _previousValue = value;
        }

        public T CurrentValue => _currentValue;

        public T PreviousValue => _previousValue;

        public bool IsChanged => PreviousValue.Equals(CurrentValue) == false;

        public void ResetChangeHistory() =>
            _previousValue = _currentValue;

        public void Set(T value)
        {
            _previousValue = _currentValue;
            _currentValue = value;
        }
    }
}
