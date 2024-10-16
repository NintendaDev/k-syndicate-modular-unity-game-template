using Modules.Types.MemorizedValues;
using System.Collections.Generic;
using Modules.Types.MemorizedValues.Core;

namespace Modules.Wallets.Data
{
    [System.Serializable]
    public sealed class WalletsData : EnumeratedDictionaryData
    {
        public WalletsData(Dictionary<int, LongMemorizedValue> walletData) : base(walletData)
        {
        }
    }
}
