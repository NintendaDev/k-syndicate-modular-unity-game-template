using Cysharp.Threading.Tasks;
using Modules.Types.MemorizedValues;
using GameTemplate.Infrastructure.Data;
using GameTemplate.Infrastructure.Types;
using System.Collections.Generic;
using Modules.Logging;

namespace GameTemplate.Services.PlayerStatistics
{
    public class PlayerStatisticsService : IncreasedSaveableObject<StatisticType>, IPlayerStatisticsService
    {
        public PlayerStatisticsService(ILogSystem logSystem) : base(logSystem)
        {
        }

        public override UniTask SaveProgress(PlayerProgress progress)
        {
            progress.StatisticsData = new StatisticsData(Data);

            return UniTask.CompletedTask;
        }

        protected override bool IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData)
        {
            defaultData = null;

            return false;
        }

        protected override bool IsExistSavedData(PlayerProgress progress, out IReadOnlyDictionary<int, LongMemorizedValue> savedData)
        {
            savedData = null;

            if (progress.StatisticsData == null || progress.StatisticsData.Data == null)
                return false;

            savedData = progress.StatisticsData.Data;

            return true;
        }
    }
}