using System;
using Modules.Statistics.Types;

namespace Modules.Statistics.Systems
{
    public interface IPlayerStatisticsService
    {
        public event Action<StatisticType, int> Updated;

        public void Add(StatisticType objectType, int amount);

        public int Get(StatisticType objectType);

        public void Set(StatisticType objectType, int amount);
    }
}