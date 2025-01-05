using Modules.Specifications;
using Modules.AssetsManagement.StaticData;
using Modules.Types.MemorizedValues.Core;
using UnityEngine;

namespace Modules.AudioManagement.Mixer
{
    public sealed class AudioMixerSystem : IAudioMixerSystem
    {
        private const float MinPercent = 0;
        private const float MaxPercent = 1;
        private const float AttenuationLevelMultiplier = 20f;

        private readonly IStaticDataService _staticDataService;
        private readonly FloatMemorizedValue _lastMusicVolumePercent = new();
        private readonly FloatMemorizedValue _lastEffectVolumePercent = new();
        private readonly FloatValidator _floatValidator = new();
        private AudioMixerConfiguration _mixerConfiguration;
        private UnityEngine.Audio.AudioMixer _audioMixer;

        public AudioMixerSystem(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public float EffectsPercentVolume => _lastEffectVolumePercent.CurrentValue;

        public float MusicPercentVolume => _lastMusicVolumePercent.CurrentValue;

        public bool IsChanged => _lastEffectVolumePercent.IsChanged || _lastMusicVolumePercent.IsChanged;

        public void Initialize()
        {
            _mixerConfiguration = _staticDataService.GetConfiguration<AudioMixerConfiguration>();
            _audioMixer = _mixerConfiguration.AudioMixer;

            SetMusicPercentVolume(_mixerConfiguration.DefaultMusicVolumePercent);
            SetEffectsPercentVolume(_mixerConfiguration.DefaultEffectsVolumePercent);
        }

        public void SetMusicPercentVolume(float percent)
        {
            _lastMusicVolumePercent.Set(percent);
            SetVolume(_mixerConfiguration.MusicMixerParameter, percent);
        }
            
        public void SetEffectsPercentVolume(float percent)
        {
            _lastEffectVolumePercent.Set(percent);
            SetVolume(_mixerConfiguration.EffectsMixerParameter, percent);
        }

        public void Mute() =>
            SetVolume(_mixerConfiguration.MasterMixerParameter, MinPercent);

        public void Unmute() =>
            SetVolume(_mixerConfiguration.MasterMixerParameter, MaxPercent);

        public void Reset()
        {
            _lastMusicVolumePercent.ResetChangeHistory();
            _lastMusicVolumePercent.ResetChangeHistory();
        }

        private void SetVolume(string mixerParameter, float percent)
        {
            _floatValidator.BetweenZeroAndOne(percent);

            _audioMixer.SetFloat(mixerParameter, Mathf.Log10(percent) * AttenuationLevelMultiplier);
        }
    }
}