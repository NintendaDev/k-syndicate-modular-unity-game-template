using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.LoadingCurtain.Configurations
{
    [CreateAssetMenu(fileName = "new LoadingCurtainConfiguration",
        menuName = "Modules/LoadingCurtain/LoadingCurtainConfiguration")]
    public class LoadingCurtainConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReference CurtainPrefabReference { get; private set; }
    }
}