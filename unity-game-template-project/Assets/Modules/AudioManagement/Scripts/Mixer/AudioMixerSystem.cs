using Cysharp.Threading.Tasks;
using Modules.Specifications;
using Modules.AssetsManagement.StaticData;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;
using Modules.Types.MemorizedValues.Core;
using UnityEngine;

namespace Modules.AudioManagement.Mixer
{
    public sealed class AudioMixerSystem : IAudioMixerSystem, IProgressLoader, IProgressSaver
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

        public UniTask LoadProgress(PlayerProgress progress)
        {
            AudioMixerServiceData savedData = progress.GetProgressData<AudioMixerServiceData>();
            
            if (savedData == null)
                return UniTask.CompletedTask;

            SetMusicVolume(savedData.MusicPercentVolume);
            SetEffectsVolume(savedData.EffectsPercentVolume);

            _lastMusicVolumePercent.ResetChangeHistory();
            _lastEffectVolumePercent.ResetChangeHistory();

            return UniTask.CompletedTask;
        }

        public UniTask SaveProgress(PlayerProgress progress)
        {
            progress.SetProgressData(new AudioMixerServiceData(this));
            _lastMusicVolumePercent.ResetChangeHistory();
            _lastMusicVolumePercent.ResetChangeHistory();

            return UniTask.CompletedTask;
        }

        public void Initialize()
        {
            _mixerConfiguration = _staticDataService.GetConfiguration<AudioMixerConfiguration>();
            _audioMixer = _mixerConfiguration.AudioMixer;

            SetMusicVolume(_mixerConfiguration.DefaultMusicVolumePercent);
            SetEffectsVolume(_mixerConfiguration.DefaultEffectsVolumePercent);
        }

        public void SetMusicVolume(float percent)
        {
            _lastMusicVolumePercent.Set(percent);
            SetVolume(_mixerConfiguration.MusicMixerParameter, percent);
        }
            
        public void SetEffectsVolume(float percent)
        {
            _lastEffectVolumePercent.Set(percent);
            SetVolume(_mixerConfiguration.EffectsMixerParameter, percent);
        }

        public void Mute() =>
            SetVolume(_mixerConfiguration.MasterMixerParameter, MinPercent);

        public void Unmute() =>
            SetVolume(_mixerConfiguration.MasterMixerParameter, MaxPercent);

        private void SetVolume(string mixerParameter, float percent)
        {
            _floatValidator.BetweenZeroAndOne(percent);

            _audioMixer.SetFloat(mixerParameter, Mathf.Log10(percent) * AttenuationLevelMultiplier);
        }
    }
}