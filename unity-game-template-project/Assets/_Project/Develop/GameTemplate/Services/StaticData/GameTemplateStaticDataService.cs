using GameTemplate.Services.Advertisiments;
using System;
using System.Collections.Generic;
using GameTemplate.Services.Analytics;
using GameTemplate.Systems.Performance;
using GameTemplate.Level.Configurations;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.UI.Services.Popups;
using GameTemplate.Infrastructure.LanguageSystem.Localization;
using Modules.AssetsManagement.AddressablesServices;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Mixer;
using Modules.Logging;
using Modules.SaveManagement.Systems;
using Modules.Wallets.Configurations;

namespace GameTemplate.Services.StaticData
{
    public class GameTemplateStaticDataService : StaticDataService
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
                InitializeConfiguration<AdvertisimentsConfiguration>,
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
            };
        }
    }
}
