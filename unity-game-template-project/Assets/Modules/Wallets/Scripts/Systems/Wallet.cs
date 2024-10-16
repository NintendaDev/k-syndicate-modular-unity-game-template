using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Types;
using Modules.Types.MemorizedValues.Core;
using Modules.Wallet.Types;
using Modules.Wallets.Configurations;
using Modules.Wallets.Data;

namespace Modules.Wallets.Systems
{
    public sealed class Wallet : IncreasedSaveableObject<CurrencyType>, IWallet
    {
        private readonly IStaticDataService _staticDataService;

        public Wallet(ILogSystem logSystem, IStaticDataService staticDataService) : base (logSystem)
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

            progress.SetProgressData(new WalletsData(Data));
        }
            
        protected override bool IsExistDefaultData(out Dictionary<int, LongMemorizedValue> defaultData)
        {
            DefaultWalletsAmountConfiguration defaultProgressAmountConfiguration = 
                _staticDataService.GetConfiguration<DefaultWalletsAmountConfiguration>();
            
            defaultData = null;

            if (defaultProgressAmountConfiguration.WalletsData.Count == 0)
                return false;

            defaultData = CreateEmptyData();

            foreach(KeyValuePair<int, long> defailtDataPair in defaultProgressAmountConfiguration.WalletsData)
                defaultData[defailtDataPair.Key] = new LongMemorizedValue(defailtDataPair.Value);

            return true;
        }

        protected override bool IsExistSavedData(PlayerProgress progress, out IReadOnlyDictionary<int, LongMemorizedValue> savedData)
        {
            savedData = null;

            WalletsData walletsData = progress.GetProgressData<WalletsData>();

            if (walletsData == null || walletsData.Data == null)
                return false;

            savedData = walletsData.Data;

            return true;
        }
    }
}