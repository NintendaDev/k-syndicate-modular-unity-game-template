using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.Services.Advertisiments
{
    [CreateAssetMenu(fileName = "new AdvertisimentsConfiguration", menuName = "GameTemplate/Ads/AdvertisimentsConfiguration")]
    public class AdvertisimentsConfiguration : ScriptableObject
    {
        [LabelWidth(250)]
        [field: SerializeField, Range(0, 1)] public float InterstitialOnStartLevelProbability = 1f;

        [LabelWidth(250)]
        [field: SerializeField, Range(0, 1)] public float InterstitialOnRestartLevelProbability = 1f;

        [LabelWidth(250)]
        [field: SerializeField, Range(0, 1)] public float InterstitialOnExitLevelProbability = 1f;

        [LabelWidth(250)]
        [field: SerializeField, MinValue(0)] public int MaxLevelRestartsWithAdvertisiment = 2;
    }
}
