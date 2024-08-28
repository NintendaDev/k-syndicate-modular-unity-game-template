using Cysharp.Threading.Tasks;
using ExternalLibraries.Types.MemorizedValues;
using GameTemplate.Infrastructure.Data;
using GameTemplate.Infrastructure.Types;
using GameTemplate.Services.Log;
using GameTemplate.Services.StaticData;
using System.Collections.Generic;

namespace GameTemplate.Services.Wallet
{
    public class WalletService : IncreasedSaveableObject<CurrencyType>, IWalletService
    {
        private readonly IStaticDataService _staticDataService;

        public WalletService(ILogService logService, IStaticDataService staticDataService) : base (logService)
        {
            _staticDataService = staticDataService;
        }

        public bool TrySpend(CurrencyType currencyType, long price)
        {
            long amount = GetAmount(currencyType);

            if (amount < price)
                return false;

            amount -= price;
            SetAmount(currencyType, amount);

            return true;
        }

        public override async UniTask SaveProgress(PlayerProgress progress)
        {
            await base.SaveProgress(progress);

            progress.WalletsData = new WalletsData(Data);
        }
            
        protected override bool IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData)
        {
            DefaultProgressConfiguration defaultProgressConfiguration = _staticDataService.GetConfiguration<DefaultProgressConfiguration>();
            defaultData = null;

            if (defaultProgressConfiguration.WalletsData.Count == 0)
                return false;

            defaultData = CreateEmptyData();

            foreach(KeyValuePair<int, long> defailtDataPair in defaultProgressConfiguration.WalletsData)
                defaultData[defailtDataPair.Key] = new LongMemorizedValue(defailtDataPair.Value);

            return true;
        }

        protected override bool IsExistSavedData(PlayerProgress progress, out IReadOnlyDictionary<int, LongMemorizedValue> savedData)
        {
            savedData = null;

            if (progress.WalletsData == null || progress.WalletsData.Data == null)
                return false;

            savedData = progress.WalletsData.Data;

            return true;
        }
    }
}