using System.Collections.Generic;
using UnityEngine;

namespace Modules.ObjectsManagement.Pools
{
    public class Pool<T> where T :Component
    {
        private readonly T _prefab;
        private readonly Queue<T> _objects = new();

        public Pool(T prefab)
        {
            _prefab = prefab;
        }

        public T Rent()
        {
            T newObject;
            
            if (_objects.TryDequeue(out newObject) == false)
                newObject = Object.Instantiate(_prefab);

            OnSpawned(newObject);

            return newObject;
        }

        public void Return(T poolableObject)
        {
            OnDespawned(poolableObject);
            _objects.Enqueue(poolableObject);
        }

        protected virtual void OnDespawned(T poolableObject)
        {
            poolableObject.gameObject.SetActive(false);
        }

        protected virtual void OnSpawned(T poolableObject)
        {
            poolableObject.gameObject.SetActive(true);
        }
    }
}