using ExternalLibraries.Types.MemorizedValues;
using GameTemplate.Infrastructure.Data;
using System.Collections.Generic;

namespace GameTemplate.Services.PlayerStatistics
{
    [System.Serializable]
    public class StatisticsData : EnumeratedDictionaryData
    {
        public StatisticsData(Dictionary<int, LongMemorizedValue> statisticsData) : base(statisticsData)
        {
        }
    }
}
