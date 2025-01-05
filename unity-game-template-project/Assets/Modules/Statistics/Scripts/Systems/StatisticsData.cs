using System.Collections.Generic;
using Modules.Statistics.Types;

namespace Modules.Statistics.Systems
{
    [System.Serializable]
    public sealed class StatisticsData
    {
        public StatisticsData(Dictionary<StatisticType, int> data)
        {
            Data = data;
        }

        public Dictionary<StatisticType, int> Data { get; }
    }
}
