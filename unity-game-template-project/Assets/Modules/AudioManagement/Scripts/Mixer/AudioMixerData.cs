using UnityEngine;

namespace Modules.AudioManagement.Mixer
{
    [System.Serializable]
    public sealed class AudioMixerData
    {
        [SerializeField] private float _musicPercentVolume;
        [SerializeField] private float _effectsPercentVolume;

        public AudioMixerData(float musicPercentVolume, float effectsPercentVolume)
        {
            _musicPercentVolume = musicPercentVolume;
            _effectsPercentVolume = effectsPercentVolume;
        }

        public float MusicPercentVolume => _musicPercentVolume;

        public float EffectsPercentVolume => _effectsPercentVolume;
    }
}