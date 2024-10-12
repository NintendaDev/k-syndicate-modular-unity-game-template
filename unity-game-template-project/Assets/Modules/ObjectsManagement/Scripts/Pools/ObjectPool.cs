using System.Collections.Generic;
using UnityEngine;

namespace Modules.ObjectsManagement.Pools
{
    public class ObjectPool<T> : IPushable<T> where T : Component
    {
        private readonly T _prefab;
        private Queue<T> _pool = new();

        public ObjectPool(T prefab)
        {
            _prefab = prefab;
        }

        public T Pool()
        {
            T newObject;
            
            if (_pool.Count > 0)
                newObject = _pool.Dequeue();
            else
                newObject = Object.Instantiate(_prefab);
            
            newObject.gameObject.SetActive(true);

            return newObject;
        }

        public void Push(T poolableObject)
        {
            poolableObject.gameObject.SetActive(false);
            _pool.Enqueue(poolableObject);
        }
    }
}