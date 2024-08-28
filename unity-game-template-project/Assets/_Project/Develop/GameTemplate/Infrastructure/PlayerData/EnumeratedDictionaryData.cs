using ExternalLibraries.Types.MemorizedValues;
using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate.Infrastructure.Data
{
    [System.Serializable]
    public class EnumeratedDictionaryData
    {
        [SerializeField] private Dictionary<int, LongMemorizedValue> _data;

        public EnumeratedDictionaryData(Dictionary<int, LongMemorizedValue> data) =>
            _data = data;

        public IReadOnlyDictionary<int, LongMemorizedValue> Data => _data;
    }
}
