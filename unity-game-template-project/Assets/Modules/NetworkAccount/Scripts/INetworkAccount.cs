using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.NetworkAccount
{
    public interface INetworkAccount
    {
        public Texture2D Avatar { get; }

        public string Name { get; }

        public UniTask InitializeAsync();
    }
}