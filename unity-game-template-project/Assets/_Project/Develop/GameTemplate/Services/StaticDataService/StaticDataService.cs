using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.Log;
using GameTemplate.Services.SaveLoad;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameTemplate.Services.Analytics;
using GameTemplate.Systems.Performance;
using GameTemplate.Level.Configurations;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Data;
using GameTemplate.Services.Wallet;
using GameTemplate.Infrastructure.LanguageSystem;
using GameTemplate.UI.Services.Popups;

namespace GameTemplate.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private IAssetProvider _assetProvider;
        private ILogService _logService;
        private StaticDataServiceConfiguration _serviceConfiguration;
        private Dictionary<Type, UnityEngine.Object> _configurations = new();

        public StaticDataService(IAssetProvider assetProvider, ILogService logService,
            StaticDataServiceConfiguration serviceConfiguration)
        {
            _assetProvider = assetProvider;
            _logService = logService;
            _serviceConfiguration = serviceConfiguration;
        }

        public async UniTask InitializeAsync()
        {
            List<UniTask> tasks = new List<UniTask>()
            {
                InitializeConfiguration<AdvertisimentsConfiguration>(),
                InitializeConfiguration<AnalyticsConfiguration>(),
                InitializeConfiguration<AudioMixerConfiguration>(),
                InitializeConfiguration<SaveConfiguration>(),
                InitializeConfiguration<DevicesPerformanceConfigurations>(),
                InitializeConfiguration<LevelsConfigurationsHub>(),
                InitializeConfiguration<GameHubConfiguration>(),
                InitializeConfiguration<InfrastructureAssetsConfiguration>(),
                InitializeConfiguration<DefaultProgressConfiguration>(),
                InitializeConfiguration<WalletSpritesConfiguration>(),
                InitializeConfiguration<LocalizationDatabase>(),
                InitializeConfiguration<PopupsAssetsConfiguration>(),
            };

            await UniTask.WhenAll(tasks);
        }

        public TConfigType GetConfiguration<TConfigType>() where TConfigType : ScriptableObject
        {
            Type configurationType = typeof(TConfigType);

            if (_configurations.ContainsKey(configurationType) == false)
                throw new Exception($"Configuration type {configurationType} is not exists");

            return _configurations[configurationType] as TConfigType;
        }

        private async UniTask InitializeConfiguration<TConfigType>() where TConfigType : ScriptableObject
        {
            TConfigType configuration = await _assetProvider.LoadOneByLabelAsync<TConfigType>(_serviceConfiguration.ConfigurationsAssetLabel);
            Type configurationType = typeof(TConfigType);

            if (configuration == null)
                throw new Exception($"Configuration type {configurationType} was not found");

            _configurations.Add(configurationType, configuration);

            _logService.Log($"Configuration type {configurationType} was initialized");
        }
    }
}
