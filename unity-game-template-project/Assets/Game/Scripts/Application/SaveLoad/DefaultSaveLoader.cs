using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.SaveSystem.SaveLoad;
using Modules.Wallet.Types;
using Modules.Wallets.Configurations;
using Modules.Wallets.Systems;

namespace Game.Application.SaveLoad
{
    public sealed class DefaultSaveLoader : IDefaultSaveLoader
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IWallet _wallet;
        private readonly IAudioMixerSystem _mixerSystem;

        public DefaultSaveLoader(IStaticDataService staticDataService, IWallet wallet, IAudioMixerSystem mixerSystem)
        {
            _staticDataService = staticDataService;
            _wallet = wallet;
            _mixerSystem = mixerSystem;
        }
        
        public void LoadDefaultSave()
        {
            LoadAudioMixer();
            LoadWallet();
        }

        private void LoadAudioMixer()
        {
            AudioMixerConfiguration configuration = _staticDataService.GetConfiguration<AudioMixerConfiguration>();
            
            _mixerSystem.Reset();
            _mixerSystem.SetEffectsPercentVolume(configuration.DefaultEffectsVolumePercent);
            _mixerSystem.SetEffectsPercentVolume(configuration.DefaultMusicVolumePercent);
        }

        private void LoadWallet()
        {
            DefaultWalletsAmountConfiguration configuration = 
                _staticDataService.GetConfiguration<DefaultWalletsAmountConfiguration>();

            foreach (CurrencyValue currencyValue in configuration.WalletsData)
                _wallet.Set(currencyValue.CurrencyType, currencyValue.Amount);
        }
    }
}