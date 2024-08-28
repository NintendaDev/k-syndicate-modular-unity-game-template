using System;
using System.Collections.Generic;

namespace GameTemplate.Infrastructure.Types
{
    public interface IIncreasedSaveableObject<TObjectEnum> 
        where TObjectEnum : Enum
    {
        public event Action<TObjectEnum, long, long> Updated;

        public IEnumerable<TObjectEnum> AvailableTypes { get; }

        public void AddAmount(TObjectEnum objectType, long amount);

        public long GetAmount(TObjectEnum objectType);

        public void SetAmount(TObjectEnum objectType, long amount);
    }
}