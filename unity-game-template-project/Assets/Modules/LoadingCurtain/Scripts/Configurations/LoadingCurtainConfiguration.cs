using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.LoadingCurtain.Configurations
{
    [CreateAssetMenu(fileName = "new LoadingCurtainConfiguration",
        menuName = "Modules/LoadingCurtain/LoadingCurtainConfiguration")]
    public sealed class LoadingCurtainConfiguration : ScriptableObject
    {
        [SerializeField, Required] private AssetReferenceGameObject _curtainPrefabReference;
        
        public AssetReference CurtainPrefabReference => _curtainPrefabReference;
    }
}