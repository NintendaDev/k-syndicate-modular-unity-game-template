using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Modules.Logging;

namespace Modules.AssetManagement.StaticData
{
    public abstract class StaticDataService : IStaticDataService
    {
        private IAddressablesService _addressablesService;
        private ILogSystem _logSystem;
        private StaticDataServiceConfiguration _serviceConfiguration;
        private Dictionary<Type, UnityEngine.Object> _configurations = new();
        private ScriptableObject[] _rawConfigurations;

        public StaticDataService(IAddressablesService addressablesService, ILogSystem logSystem,
            StaticDataServiceConfiguration serviceConfiguration)
        {
            _addressablesService = addressablesService;
            _logSystem = logSystem;
            _serviceConfiguration = serviceConfiguration;
        }

        public async UniTask InitializeAsync()
        {
            await LoadRawConfigurations();
            
            IEnumerable<Action> configurationsInitializeTasks = GetConfigurationInitializationTasks();
            
            foreach (Action initializeTaskAction in configurationsInitializeTasks)
                initializeTaskAction.Invoke();

            _rawConfigurations = null;
        }

        public TConfigType GetConfiguration<TConfigType>() where TConfigType : ScriptableObject
        {
            Type configurationType = typeof(TConfigType);

            if (_configurations.ContainsKey(configurationType) == false)
                throw new Exception($"Configuration type {configurationType} is not exists");

            return _configurations[configurationType] as TConfigType;
        }

        protected abstract IEnumerable<Action> GetConfigurationInitializationTasks();

        private async UniTask LoadRawConfigurations()
        {
            _rawConfigurations = await _addressablesService
                .LoadByLabelAsync<ScriptableObject>(_serviceConfiguration.ConfigurationsAssetLabel);
        }

        protected void InitializeConfiguration<TConfigType>() where TConfigType : ScriptableObject
        {
            TConfigType configuration = _rawConfigurations.Where(x => x is TConfigType)
                .Cast<TConfigType>()
                .FirstOrDefault();
            
            Type configurationType = typeof(TConfigType);

            if (configuration == null)
                throw new Exception($"Configuration type {configurationType} was not found");
            
            _configurations.Add(configurationType, configuration);

            _logSystem.Log($"Configuration type {configurationType} was initialized");
        }
    }
}
