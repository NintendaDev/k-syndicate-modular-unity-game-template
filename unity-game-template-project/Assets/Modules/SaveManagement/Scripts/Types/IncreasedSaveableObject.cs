using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Logging;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;
using Modules.Types.MemorizedValues.Core;
using Zenject;

namespace Modules.SaveManagement.Types
{
    public abstract class IncreasedSaveableObject<TObjectEnum> : IDisposable, IInitializable, 
        IProgressLoader, IProgressSaver
        where TObjectEnum : Enum
    {
        private readonly ILogSystem _logSystem;
        private const int NoneTypeEnumIndex = 0;
        private Dictionary<int, LongMemorizedValue> _data = new();
        private Array _objectEnumValues = Enum.GetValues(typeof(TObjectEnum));

        public IncreasedSaveableObject(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        public event Action<TObjectEnum, long, long> Updated;

        public bool IsChanged => _data.Values.Where(x => x.IsChanged).Count() > 0;

        public IEnumerable<TObjectEnum> AvailableTypes => 
            _data.Select(x => (TObjectEnum)(object)x.Key);

        protected Dictionary<int, LongMemorizedValue> Data => _data;

        protected ILogSystem LogSystem => _logSystem;

        public void Dispose()
        {
            _objectEnumValues = null;
        }

        public void Initialize() =>
            Clear();

        public long GetAmount(TObjectEnum objectType)
        {
            LongMemorizedValue value;
            _data.TryGetValue((int)(object)objectType, out value);

            return value.CurrentValue;
        }

        public void SetAmount(TObjectEnum objectType, long amount)
        {
            int objectTypeCode = (int)(object)objectType;
            long oldValue = _data[objectTypeCode].CurrentValue;

            if (oldValue != amount)
            {
                _data[objectTypeCode].Set(amount);
                Updated?.Invoke(objectType, oldValue, amount);
            }
        }

        public void AddAmount(TObjectEnum objectType, long amount) =>
            SetAmount(objectType, GetAmount(objectType) + amount);

        public async UniTask LoadProgress(PlayerProgress progress)
        {
            Clear();

            if (IsExistSavedData(progress, out IReadOnlyDictionary<int, LongMemorizedValue> savedData) == false)
            {
                if (IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData) == false)
                    return;
                else
                    savedData = defaultData;
            }

            foreach (KeyValuePair<int, LongMemorizedValue> objectDataPair in savedData)
            {
                int objectTypeCode = objectDataPair.Key;
                long currencyAmount = objectDataPair.Value.CurrentValue;
                TObjectEnum objectType;

                try
                {
                    objectType = (TObjectEnum)(object)objectTypeCode;
                }
                catch
                {
                    _logSystem.LogError($"Unknown currency type code {objectTypeCode} with amount {currencyAmount} " +
                                         $"on progress loading found! May be some old currency.");

                    continue;
                }

                if (objectTypeCode == NoneTypeEnumIndex)
                {
                    _logSystem.LogError($"None type {objectType} on progress loading found!");

                    continue;
                }

                SetAmount(objectType, currencyAmount);
                Updated?.Invoke(objectType, currencyAmount, currencyAmount);

                await UniTask.Yield();
            }

            _data.ForEach(x => x.Value.ResetChangeHistory());
        }

        public virtual UniTask SaveProgress(PlayerProgress progress)
        {
            _data.Values.ForEach(x => x.ResetChangeHistory());

            return UniTask.CompletedTask;
        }

        protected abstract bool IsExistSavedData(PlayerProgress progress, 
            out IReadOnlyDictionary<int, LongMemorizedValue> savedData);

        protected abstract bool IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData);

        protected void Clear()
        {
            _data.Clear();
            _data = CreateEmptyData();
        }

        protected Dictionary<int, LongMemorizedValue> CreateEmptyData()
        {
            Dictionary<int, LongMemorizedValue> data = new();

            IEnumerable<int> objectTypes = _objectEnumValues.Cast<int>();
            objectTypes = objectTypes.Where(x => x != NoneTypeEnumIndex);

            foreach (int objectType in objectTypes)
                data.Add(objectType, new LongMemorizedValue());

            return data;
        }
    }
}
