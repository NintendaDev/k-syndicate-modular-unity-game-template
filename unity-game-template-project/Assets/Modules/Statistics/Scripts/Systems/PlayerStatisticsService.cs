using System;
using Modules.Statistics.Types;
using Modules.Storage;

namespace Modules.Statistics.Systems
{
    public sealed class PlayerStatisticsService : IPlayerStatisticsService
    {
        private readonly MultiValueAccounter<StatisticType> _multiValueAccounter = new();
        
        public event Action<StatisticType, int> Updated;
        
        public void Add(StatisticType objectType, int amount)
        {
            _multiValueAccounter.Increase(objectType, amount);

            Updated?.Invoke(objectType, Get(objectType));
        }

        public int Get(StatisticType objectType) => _multiValueAccounter.Get(objectType);

        public void Set(StatisticType objectType, int amount)
        {
            _multiValueAccounter.Set(objectType, amount);
            
            Updated?.Invoke(objectType, Get(objectType));
        }
    }
}