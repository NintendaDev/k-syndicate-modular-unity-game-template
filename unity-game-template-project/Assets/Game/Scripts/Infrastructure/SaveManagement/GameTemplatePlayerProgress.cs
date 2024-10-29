using System;
using System.Collections.Generic;
using Modules.AudioManagement.Mixer;
using UnityEngine;
using Modules.SaveManagement.Data;
using Modules.Statistics.Data;
using Modules.Wallets.Data;

namespace GameTemplate.Infrastructure.SaveManagement
{
    [System.Serializable]
    public sealed class GameTemplatePlayerProgress : PlayerProgress
    {
        [SerializeField] private WalletsData _walletsData;
        [SerializeField] private StatisticsData _statisticsData;
        [SerializeField] private AudioMixerServiceData _audioMixerServiceData;

        private Dictionary<Type, Func<object>> _dataMapping;

        public GameTemplatePlayerProgress()
        {
            _dataMapping = new Dictionary<Type, Func<object>>()
            {
                {typeof(WalletsData), () => _walletsData},
                {typeof(StatisticsData), () => _statisticsData},
                {typeof(AudioMixerServiceData), () => _audioMixerServiceData},
            };
        }

        public override bool TryGetProgressData<TData>(out TData data)
        {
            Type dataType = typeof(TData);
            
            if (_dataMapping.ContainsKey(dataType) == false)
                throw new KeyNotFoundException("There is no data associated with this type.");
            
            data = (TData)_dataMapping[dataType].Invoke();
            
            return data != null;
        }

        public override void SetProgressData<TData>(TData data)
        {
            if (data is WalletsData walletsData)
                _walletsData = walletsData;
            else if (data is StatisticsData statisticsData)
                _statisticsData = statisticsData;
            else if (data is AudioMixerServiceData serviceData)
                _audioMixerServiceData = serviceData;
            else
                throw new InvalidCastException("Progress data type is not supported");
        }
    }
}