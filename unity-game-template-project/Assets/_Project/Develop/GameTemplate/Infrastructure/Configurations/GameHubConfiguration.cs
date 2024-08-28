using GameTemplate.Infrastructure.Music;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.Configurations
{
    [CreateAssetMenu(fileName = "new GameHubConfiguration", menuName = "GameTemplate/Infrastructure/GameHubConfiguration")]
    public class GameHubConfiguration : ScriptableObject, IReferenceAudio
    {
        [field: SerializeField, Required] public AssetReferenceT<AudioClip> AudioReference { get; private set; }

        [field: SerializeField, Required] public AssetReferenceGameObject LevelViewPrebafReference { get; private set; }

        [field: SerializeField, Required] public AssetReferenceGameObject WalletViewPrebafReference { get; private set; }
    }
}
