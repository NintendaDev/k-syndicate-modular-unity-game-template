using UnityEngine;

namespace Modules.AssetsManagement.StaticData
{
    [CreateAssetMenu(fileName = "new StaticDataServiceConfiguration", 
        menuName = "Modules/Services/StaticDataServiceConfiguration")]
    public class StaticDataServiceConfiguration : ScriptableObject
    {
        [field: SerializeField] public string ConfigurationsAssetLabel { get; private set; } = "configurations";
    }
}