using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.StaticData;

namespace GameTemplate.Infrastructure.Data
{
    public class DefaultPlayerProgressMaker : IDefaultPlayerProgress
    {
        private readonly IStaticDataService _staticDataService;

        public DefaultPlayerProgressMaker(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public PlayerProgress Make()
        {
            PlayerProgress progress = new();

            AudioMixerConfiguration audioMixerConfiguration = _staticDataService.GetConfiguration<AudioMixerConfiguration>();
            AudioMixerServiceData audioMixerServiceData = new(audioMixerConfiguration.DefaultMusicVolumePercent,
                audioMixerConfiguration.DefaultEffectsVolumePercent);

            progress.AudioMixerServiceData = audioMixerServiceData;

            return progress;
        }
    }
}
