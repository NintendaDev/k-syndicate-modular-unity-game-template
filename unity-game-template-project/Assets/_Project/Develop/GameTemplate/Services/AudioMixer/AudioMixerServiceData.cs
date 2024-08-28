using UnityEngine;

namespace GameTemplate.Services.AudioMixer
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

        public AudioMixerServiceData(IAudioMixerService audioMixerService) 
            : this(audioMixerService.MusicPercentVolume, audioMixerService.EffectsPercentVolume)
        {
        }

        public float MusicPercentVolume => _musicPercentVolume;

        public float EffectsPercentVolume => _effectsPercentVolume;
    }
}