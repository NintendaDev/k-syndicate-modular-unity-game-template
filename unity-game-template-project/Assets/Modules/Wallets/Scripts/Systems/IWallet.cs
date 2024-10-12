using Modules.SaveManagement.Types;
using Modules.Wallet.Types;

namespace Modules.Wallets.Systems
{
    public interface IWallet : IIncreasedSaveableObject<CurrencyType>
    {
        public bool IsChanged { get; }

        public bool TrySpend(CurrencyType currency, long price);
    }
}