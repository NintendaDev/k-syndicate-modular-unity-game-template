using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AudioManagement.Mixer
{
    [CreateAssetMenu(fileName = "new AudioMixerConfiguration", menuName = "GameTemplate/Audio/AudioMixerConfiguration")]
    public class AudioMixerConfiguration : ScriptableObject
    {
        [field: SerializeField, Required] public UnityEngine.Audio.AudioMixer AudioMixer { get; private set; }

        [field: SerializeField, Required] public string MasterMixerParameter { get; private set; }

        [field: SerializeField, Required] public string MusicMixerParameter { get; private set; }

        [field: SerializeField, Required] public string EffectsMixerParameter { get; private set; }

        [field: SerializeField, Range(0, 1)] public float DefaultMusicVolumePercent { get; private set; } = 1f;

        [field: SerializeField, Range(0, 1)] public float DefaultEffectsVolumePercent { get; private set; } = 1f;
    }
}