using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.UI.Services.Popups
{
    [CreateAssetMenu(fileName = "new PopupsAssetsConfiguration", menuName = "ObbyGame/Infrastructure/PopupsAssetsConfiguration")]
    public class PopupsAssetsConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public AssetReference InfoPopup { get; private set; }

        [field: SerializeField, Required] public AssetReference ErrorPopup { get; private set; }
    }
}
