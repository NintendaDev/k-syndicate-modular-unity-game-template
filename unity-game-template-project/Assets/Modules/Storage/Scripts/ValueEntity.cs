using System;

namespace Modules.Storage
{
    public class IntValueEntity
    {
        private int _value;
        
        public IntValueEntity()
        {
            _value = default;
        }

        public IntValueEntity(int initialValue)
        {
            _value = initialValue;
        }

        public event Action<int> Changed;

        public int Value => _value;

        public void Set(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _value = value;
            
            Changed?.Invoke(_value);
        }

        public void Increase(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            
            _value += amount;
            
            Changed?.Invoke(_value);
        }
        
        public bool TryDecrease(int amount)
        {
            if (CanDecrease(amount) == false)
                return false;
            
            _value -= amount;
            
            Changed?.Invoke(_value);
            
            return true;
        }
        
        public bool CanDecrease(int amount) => _value >= amount;
    }
}