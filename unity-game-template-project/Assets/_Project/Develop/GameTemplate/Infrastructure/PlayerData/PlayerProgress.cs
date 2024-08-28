using GameTemplate.Services.AudioMixer;
using GameTemplate.Services.PlayerStatistics;
using GameTemplate.Services.Wallet;
using UnityEngine;

namespace GameTemplate.Infrastructure.Data
{
    [System.Serializable]
    public class PlayerProgress
    {
        [SerializeField] private WalletsData _walletsData;
        [SerializeField] private StatisticsData _statisticsData;
        [SerializeField] private AudioMixerServiceData _audioMixerServiceData;

        public WalletsData WalletsData
        {
            get => _walletsData;
            set => _walletsData = value;
        }

        public StatisticsData StatisticsData
        {
            get => _statisticsData;
            set => _statisticsData = value;
        }

        public AudioMixerServiceData AudioMixerServiceData
        {
            get => _audioMixerServiceData;
            set => _audioMixerServiceData = value;
        }
    }
}