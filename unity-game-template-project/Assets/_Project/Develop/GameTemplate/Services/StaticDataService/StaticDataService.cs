using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.SaveLoad;
using System;
using System.Collections.Generic;
using GameTemplate.Services.Analytics;
using GameTemplate.Systems.Performance;
using GameTemplate.Level.Configurations;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Data;
using GameTemplate.Services.Wallet;
using GameTemplate.UI.Services.Popups;
using GameTemplate.Infrastructure.LanguageSystem.Localization;
using Modules.AssetManagement;
using Modules.AssetManagement.StaticData;
using Modules.Logging;

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
                InitializeConfiguration<DefaultProgressConfiguration>,
                InitializeConfiguration<WalletSpritesConfiguration>,
                InitializeConfiguration<LocalizationDatabase>,
                InitializeConfiguration<PopupsAssetsConfiguration>,
            };
        }
    }
}
