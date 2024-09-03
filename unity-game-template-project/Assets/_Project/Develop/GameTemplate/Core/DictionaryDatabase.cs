using System;
using System.Collections.Generic;

namespace GameTemplate.Core
{
    public class DictionaryDatabase<TKey, TValue>
    {
        private Dictionary<TKey,TValue> _database = new();

        public void Add(TKey key, TValue value)
        {
            if (_database.TryAdd(key, value) == false)
                throw new Exception("The key already exists in the database");
        }
        
        public bool TryPopValue(TKey key, out TValue value)
        {
            if (TryGetValue(key, out value) == false)
                return false;
            
            Remove(key);

            return true;
        }
        
        public bool TryGetValue(TKey key, out TValue value) =>
            _database.TryGetValue(key, out value);

        public void Remove(TKey key)
        {
            if (_database.ContainsKey(key))
                _database.Remove(key);
        }
    }
}
