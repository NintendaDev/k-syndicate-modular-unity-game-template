using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure.Configurations
{
    [CreateAssetMenu(fileName = "new GameHubConfiguration", menuName = "Game/Infrastructure/GameHubConfiguration")]
    public sealed class GameHubConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReferenceGameObject LevelViewPrebafReference { get; private set; }

        [field: SerializeField, Required] public AssetReferenceGameObject WalletViewPrebafReference { get; private set; }
    }
}
