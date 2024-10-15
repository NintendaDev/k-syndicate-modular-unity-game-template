using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.Configurations
{
    [CreateAssetMenu(fileName = "new InfrastructureAssetsConfiguration", menuName = "GameTemplate/Infrastructure/InfrastructureAssetsConfiguration")]
    public class InfrastructureAssetsConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReferenceGameObject GameBootstrapper { get; private set; }
    }
}
