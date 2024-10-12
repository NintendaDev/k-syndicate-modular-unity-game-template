using System;
using UnityEngine;

namespace Modules.Core.Systems
{
    public abstract class SceneObjectDisposable : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }
}
