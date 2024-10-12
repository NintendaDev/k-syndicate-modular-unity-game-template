using UnityEngine;

namespace Modules.AssetManagement.StaticData
{
    [CreateAssetMenu(fileName = "new StaticDataServiceConfiguration", 
        menuName = "Services / StaticDataServiceConfiguration")]
    public class StaticDataServiceConfiguration : ScriptableObject
    {
        [field: SerializeField] public string ConfigurationsAssetLabel { get; private set; } = "configurations";
    }
}