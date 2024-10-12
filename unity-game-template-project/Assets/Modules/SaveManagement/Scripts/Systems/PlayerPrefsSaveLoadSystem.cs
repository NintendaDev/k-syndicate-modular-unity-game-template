using Cysharp.Threading.Tasks;
using Modules.Extensions;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.StaticData;
using Modules.Logging;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;
using UnityEngine;

namespace Modules.SaveManagement.Systems
{
    public class PlayerPrefsSaveLoadSystem : SaveLoadSystem
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IDefaultPlayerProgress _defaultPlayerProgress;
        private SaveConfiguration _saveConfiguration;

        public PlayerPrefsSaveLoadSystem(IEnumerable<IProgressSaver> progressSavers, 
            IPersistentProgressProvider persistentProgressProvider, IStaticDataService staticDataService, 
            IDefaultPlayerProgress defaultPlayerProgress, ILogSystem logSystem) 
            : base(progressSavers, persistentProgressProvider, logSystem)
        {
            _staticDataService = staticDataService;
            _defaultPlayerProgress = defaultPlayerProgress;
        }

        public override async UniTask InitializeAsync()
        {
            await base.InitializeAsync();

            _saveConfiguration = _staticDataService.GetConfiguration<SaveConfiguration>();
        }

        public override bool IsEnableEncryption() =>
            _saveConfiguration.IsEnableEncryption;

        public override string GetEncryptionPassword() =>
            _saveConfiguration.Password;

        public override PlayerProgress Load<TProgress>()
        {
            string serializedProgress = GetRawSaveData();

            PlayerProgress progress;

            if (string.IsNullOrEmpty(serializedProgress))
                progress = _defaultPlayerProgress.GetDefaultProgress();
            else
                progress = serializedProgress.ToDeserialized<TProgress>();

            return progress;
        }
            
        public override void Save(string rawData)
        {
            string savedProgress = rawData;

            if (TryEncrypt(rawData, out string encryptedProgress))
                savedProgress = encryptedProgress;

            PlayerPrefs.SetString(_saveConfiguration.SaveKey, savedProgress);
        }
        
        public override void Save(PlayerProgress progress) =>
            Save(progress.ToJson());

        public override void Clear() =>
            PlayerPrefs.DeleteKey(_saveConfiguration.SaveKey);

        public override string GetRawSaveData()
        {
            string rawProgress = PlayerPrefs.GetString(_saveConfiguration.SaveKey);

            if (TryDecrypt(rawProgress, out string decryptedProgress))
                rawProgress = decryptedProgress;

            return rawProgress;
        }

        private bool TryDecrypt(string encryptedData, out string decryptedData) =>
            TryMakeEncryptionOperation(encryptedData, 
                (x) => x.Decrypt(_saveConfiguration.Password), out decryptedData);

        private bool TryMakeEncryptionOperation(string sourceString, Func<string, string> encryptFunction, 
            out string outputString)
        {
            outputString = string.Empty;

            if (_saveConfiguration.IsEnableEncryption == false)
                return false;

            outputString = encryptFunction(sourceString);

            return true;
        }

        private bool TryEncrypt(string sourceData, out string encryptedData) =>
            TryMakeEncryptionOperation(sourceData, 
                (x) => x.Encrypt(_saveConfiguration.Password), out encryptedData);
    }
}
