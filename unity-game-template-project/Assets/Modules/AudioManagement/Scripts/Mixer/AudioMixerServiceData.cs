using UnityEngine;

namespace Modules.AudioManagement.Mixer
{
    [System.Serializable]
    public class AudioMixerServiceData
    {
        [SerializeField] private float _musicPercentVolume;
        [SerializeField] private float _effectsPercentVolume;

        public AudioMixerServiceData(float musicPercentVolume, float effectsPercentVolume)
        {
            _musicPercentVolume = musicPercentVolume;
            _effectsPercentVolume = effectsPercentVolume;
        }

        public AudioMixerServiceData(IAudioMixerSystem audioMixerSystem) 
            : this(audioMixerSystem.MusicPercentVolume, audioMixerSystem.EffectsPercentVolume)
        {
        }

        public float MusicPercentVolume => _musicPercentVolume;

        public float EffectsPercentVolume => _effectsPercentVolume;
    }
}