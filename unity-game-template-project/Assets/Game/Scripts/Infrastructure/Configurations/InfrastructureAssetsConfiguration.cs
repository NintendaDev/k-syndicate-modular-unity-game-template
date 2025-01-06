using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Infrastructure.Configurations
{
    [CreateAssetMenu(fileName = "new InfrastructureAssetsConfiguration", menuName = "GameTemplate/Infrastructure/InfrastructureAssetsConfiguration")]
    public sealed class InfrastructureAssetsConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReferenceGameObject GameBootstrapper { get; private set; }
    }
}
