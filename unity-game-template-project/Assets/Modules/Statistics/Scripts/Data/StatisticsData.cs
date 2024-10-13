using System.Collections.Generic;
using Modules.Types.MemorizedValues;
using Modules.Types.MemorizedValues.Core;

namespace Modules.Statistics.Data
{
    [System.Serializable]
    public class StatisticsData : EnumeratedDictionaryData
    {
        public StatisticsData(Dictionary<int, LongMemorizedValue> statisticsData) : base(statisticsData)
        {
        }
    }
}
