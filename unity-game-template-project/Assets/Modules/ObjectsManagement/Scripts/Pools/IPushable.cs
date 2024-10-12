using UnityEngine;

namespace Modules.ObjectsManagement.Pools
{
    public interface IPushable<T> where T : Component
    {
        public void Push(T poolableObject);
    }
}