using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Modules.Logging;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Types;
using Modules.Statistics.Data;
using Modules.Statistics.Systems;
using Modules.Statistics.Types;
using Modules.Types.MemorizedValues.Core;

namespace GameTemplate.Services.PlayerStatistics
{
    public class PlayerStatisticsService : IncreasedSaveableObject<StatisticType>, IPlayerStatisticsService
    {
        public PlayerStatisticsService(ILogSystem logSystem) : base(logSystem)
        {
        }

        public override UniTask SaveProgress(PlayerProgress progress)
        {
            progress.SetProgressData(new StatisticsData(Data));

            return UniTask.CompletedTask;
        }

        protected override bool IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData)
        {
            defaultData = null;

            return false;
        }

        protected override bool IsExistSavedData(PlayerProgress progress, 
            out IReadOnlyDictionary<int, LongMemorizedValue> savedData)
        {
            savedData = null;
            StatisticsData statisticsData = progress.GetProgressData<StatisticsData>();

            if (statisticsData == null || statisticsData.Data == null)
                return false;

            savedData = statisticsData.Data;

            return true;
        }
    }
}