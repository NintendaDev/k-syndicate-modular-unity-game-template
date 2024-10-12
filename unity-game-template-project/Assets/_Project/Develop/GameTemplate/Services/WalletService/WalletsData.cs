using Modules.Types.MemorizedValues;
using GameTemplate.Infrastructure.Data;
using System.Collections.Generic;

namespace GameTemplate.Services.Wallet
{
    [System.Serializable]
    public class WalletsData : EnumeratedDictionaryData
    {
        public WalletsData(Dictionary<int, LongMemorizedValue> walletData) : base(walletData)
        {
        }
    }
}
