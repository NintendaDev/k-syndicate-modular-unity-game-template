using System;
using System.Collections.Generic;
using GameTemplate.Level.Configurations;
using GameTemplate.Infrastructure.Configurations;
using Modules.Advertisements.Configurations;
using Modules.Analytics.Configurations;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.Device.Performance.Configurations;
using Modules.Localization.Systems.Demo;
using Modules.Logging;
using Modules.AudioManagement.Configurations;
using Modules.PopupsSystem.Configurations;
using Modules.SaveManagement.Systems;
using Modules.Wallets.Configurations;

namespace GameTemplate.Services.StaticData
{
    public sealed class GameTemplateStaticDataService : StaticDataService
    {
        public GameTemplateStaticDataService(IAddressablesService addressablesService, ILogSystem logSystem, 
            StaticDataServiceConfiguration serviceConfiguration) 
            : base(addressablesService, logSystem, serviceConfiguration)
        {
        }

        protected override IEnumerable<Action> GetConfigurationInitializationTasks()
        {
            return new List<Action>()
            {
                InitializeConfiguration<AdvertisementsConfiguration>,
                InitializeConfiguration<AnalyticsConfiguration>,
                InitializeConfiguration<AudioMixerConfiguration>,
                InitializeConfiguration<SaveConfiguration>,
                InitializeConfiguration<DevicesPerformanceConfigurations>,
                InitializeConfiguration<LevelsConfigurationsHub>,
                InitializeConfiguration<GameHubConfiguration>,
                InitializeConfiguration<InfrastructureAssetsConfiguration>,
                InitializeConfiguration<DefaultWalletsAmountConfiguration>,
                InitializeConfiguration<WalletAssetsConfiguration>,
                InitializeConfiguration<LocalizationDatabase>,
                InitializeConfiguration<PopupsAssetsConfiguration>,
                InitializeConfiguration<MusicPlayerConfiguration>,
            };
        }
    }
}
