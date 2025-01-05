using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Modules.LoadingTree
{
    public sealed class LoadingBundle
    {
        private readonly ConcurrentDictionary<string, object> _values;

        public LoadingBundle(params KeyValuePair<string, object>[] values)
        {
            _values = new ConcurrentDictionary<string, object>(values);
        }

        public bool TryAdd<T>(in string key, in T value) => _values.TryAdd(key, value);

        public bool TryRemove(in string key) => _values.TryRemove(key, out _);

        public T Get<T>(in string key) => (T)_values[key];

        public bool TryGet<T>(in string key, out T value)
        {
            if (_values.TryGetValue(key, out object result))
            {
                value = (T) result;
                
                return true;
            }

            value = default;
            
            return false;
        }
    }
}