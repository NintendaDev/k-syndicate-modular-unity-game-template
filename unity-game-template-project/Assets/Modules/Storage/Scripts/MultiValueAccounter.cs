using System;
using System.Collections.Generic;

namespace Modules.Storage
{
    public class MultiValueAccounter<T> where T : Enum
    {
        private Dictionary<T, IntValueEntity> _valuesStorage = new();
        
        public MultiValueAccounter()
        {
            foreach (T value in Enum.GetValues(typeof(T))) 
                _valuesStorage.Add(value, new IntValueEntity());
        }
        
        public event Action<T, int> Changed;
        
        public void Set(T type, int value)
        {
            ValidateValueType(type);
            _valuesStorage[type].Set(value);
            
            Changed?.Invoke(type, value);
        }

        public int Get(T type)
        {
            ValidateValueType(type);
            
            return _valuesStorage[type].Value;
        }

        public void Increase(T type, int amount)
        {
            ValidateValueType(type);
            _valuesStorage[type].Increase(amount);
            
            Changed?.Invoke(type, Get(type));
        }
        
        public bool TryDecrease(T type, int amount)
        {
            ValidateValueType(type);
            
            if (_valuesStorage[type].TryDecrease(amount) == false)
                return false;
            
            Changed?.Invoke(type, Get(type));

            return true;
        }

        private void ValidateValueType(T type)
        {
            if (_valuesStorage.ContainsKey(type) == false)
                throw new KeyNotFoundException();
        }
    }
}