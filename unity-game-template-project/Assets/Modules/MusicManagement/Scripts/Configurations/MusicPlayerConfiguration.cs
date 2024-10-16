using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.MusicManagement.Configurations
{
    [CreateAssetMenu(fileName = "new MusicPlayerConfiguration", 
        menuName = "Modules/MusicManagement/MusicPlayerConfiguration")]
    public sealed class MusicPlayerConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReferenceGameObject MusicPlayerReference { get; private set; }
    }
}