using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameTemplate.Services.PlayerAccountInfo
{
    public class DummyPlayerAccountInfoService : IPlayerAccountInfoService
    {
        public Texture2D Avatar => null;

        public string Name => string.Empty;

        public UniTask InitializeAsync() => UniTask.CompletedTask;
    }
}
