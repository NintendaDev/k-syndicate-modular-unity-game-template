using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.NetworkAccount
{
    public sealed class DummyNetworkAccount : INetworkAccount
    {
        public Texture2D Avatar => null;

        public string Name => string.Empty;

        public UniTask InitializeAsync() => UniTask.CompletedTask;
    }
}
