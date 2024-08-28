using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameTemplate.Services.PlayerAccountInfo
{
    public interface IPlayerAccountInfoService
    {
        public Texture2D Avatar { get; }

        public string Name { get; }

        public UniTask InitializeAsync();
    }
}