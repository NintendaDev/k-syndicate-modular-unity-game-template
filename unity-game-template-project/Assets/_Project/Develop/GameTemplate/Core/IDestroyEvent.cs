using System;

namespace GameTemplate.Core
{
    public interface IDestroyEvent
    {
        public event Action Destroyed;
    }
}
