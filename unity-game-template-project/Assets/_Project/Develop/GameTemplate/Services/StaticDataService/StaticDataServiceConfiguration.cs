using UnityEngine;

namespace GameTemplate.Services.StaticData
{
    [CreateAssetMenu(fileName = "new StaticDataServiceConfiguration", menuName = "GameTemplate/Services/StaticData")]
    public class StaticDataServiceConfiguration : ScriptableObject
    {
        [field: SerializeField] public string ConfigurationsAssetLabel { get; private set; } = "Configurations";
    }
}