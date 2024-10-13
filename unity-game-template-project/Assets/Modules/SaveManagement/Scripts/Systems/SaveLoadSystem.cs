using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Sirenix.Utilities;
using Modules.Logging;
using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;

namespace Modules.SaveManagement.Systems
{
    public abstract class SaveLoadSystem : IDisposable, ISaveLoadSystem
    {
        private readonly IEnumerable<IProgressSaver> _progressSavers;
        private readonly IPersistentProgressProvider _persistentProgressProvider;
        private readonly float _saveSecondsPeriod = 1f;
        private CancellationToken _saveProcessCancelationToken;
        private bool _isNeedSave;
        private CancellationTokenSource _tokenSource = new();

        public SaveLoadSystem(IEnumerable<IProgressSaver> progressSavers, 
            IPersistentProgressProvider persistentProgressProvider, ILogSystem logSystem)
        {
            _progressSavers = progressSavers;
            _persistentProgressProvider = persistentProgressProvider;
            LogSystem = logSystem;
            _saveProcessCancelationToken = _tokenSource.Token;
            StartSaveProcessAsync().Forget();
        }

        protected ILogSystem LogSystem { get; }

        public void Dispose()
        {
            _tokenSource.Cancel();
        }

        public virtual UniTask InitializeAsync() =>
            default;

        public abstract bool IsEnableEncryption();

        public abstract string GetEncryptionPassword();

        public void SendSaveSignal() => _isNeedSave = true;

        public abstract PlayerProgress Load<TProgress>() where TProgress : PlayerProgress;

        public abstract void Save(string rawData);

        public abstract void Clear();

        public abstract string GetRawSaveData();

        public abstract void Save(PlayerProgress progress);

        private async UniTaskVoid StartSaveProcessAsync()
        {
            bool isEnd = false;

            while (isEnd == false)
            {
                if (_isNeedSave)
                {
                    await Save();
                    _isNeedSave = false;
                }

                await UniTask.WaitForSeconds(_saveSecondsPeriod, ignoreTimeScale: true);

                if (_saveProcessCancelationToken.IsCancellationRequested)
                    isEnd = true;
            }
        }

        private async UniTask Save()
        {
            List<UniTask> saveTasks = new();
            _progressSavers.ForEach(x => saveTasks.Add(x.SaveProgress(_persistentProgressProvider.Progress)));
            await UniTask.WhenAll(saveTasks);

            Save(_persistentProgressProvider.Progress);

            LogSystem.Log("Game is saved...");
        }
    }
}
