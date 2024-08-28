using GameTemplate.Infrastructure.Types;

namespace GameTemplate.Services.Wallet
{
    public interface IWalletService : IIncreasedSaveableObject<CurrencyType>
    {
        public bool IsChanged { get; }

        public bool TrySpend(CurrencyType currency, long price);
    }
}