using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;

namespace GameTemplate.Infrastructure.SaveManagement.Defaults
{
    public class DefaultPlayerProgressProvider : IDefaultPlayerProgress
    {
        private readonly IStaticDataService _staticDataService;

        public DefaultPlayerProgressProvider(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public PlayerProgress GetDefaultProgress()
        {
            PlayerProgress progress = new GameTemplatePlayerProgress();

            AudioMixerConfiguration audioMixerConfiguration =
                _staticDataService.GetConfiguration<AudioMixerConfiguration>();
            
            AudioMixerServiceData audioMixerServiceData = new(audioMixerConfiguration.DefaultMusicVolumePercent,
                audioMixerConfiguration.DefaultEffectsVolumePercent);
            
            progress.SetProgressData(audioMixerServiceData);

            return progress;
        }
    }
}
