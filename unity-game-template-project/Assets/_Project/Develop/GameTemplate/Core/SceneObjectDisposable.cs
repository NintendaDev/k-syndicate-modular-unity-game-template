using System;
using UnityEngine;

namespace GameTemplate.Core
{
    public abstract class SceneObjectDisposable : MonoBehaviour, IDisposable
    {
        public abstract void Dispose();
    }
}
