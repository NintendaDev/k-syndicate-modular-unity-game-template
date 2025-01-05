using System.Collections.Generic;
using Modules.Wallet.Types;

namespace Modules.Wallets.Systems
{
    [System.Serializable]
    public struct WalletData
    {
        public WalletData(Dictionary<CurrencyType, int> data)
        {
            Data = data;
        }

        public Dictionary<CurrencyType, int> Data { get; }
    }
}
