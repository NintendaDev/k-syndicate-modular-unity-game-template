using System;

namespace Modules.Core.Systems
{
    public interface IDestroyEvent
    {
        public event Action Destroyed;
    }
}
