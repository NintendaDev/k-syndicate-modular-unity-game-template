using Cysharp.Threading.Tasks;
using GameTemplate.Services.Log;
using GameTemplate.Services.Progress;
using System;
using System.Collections.Generic;
using System.Threading;
using Sirenix.Utilities;
using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.SaveLoad
{
    public abstract class SaveLoadService : IDisposable, ISaveLoadService
    {
        private readonly IEnumerable<IProgressSaver> _progressSavers;
        private readonly IPersistentProgressService _persistentProgressService;
        private CancellationToken _saveProcessCancelationToken;
        private float _saveSecondsPeriod = 1f;
        private bool _isNeedSave;
        private CancellationTokenSource _tokenSource = new();

        public SaveLoadService(IEnumerable<IProgressSaver> progressSavers, IPersistentProgressService persistentProgressService, 
            ILogService logService)
        {
            _progressSavers = progressSavers;
            _persistentProgressService = persistentProgressService;
            LogService = logService;
            _saveProcessCancelationToken = _tokenSource.Token;
            StartSaveProcessAsync().Forget();
        }

        protected ILogService LogService { get; }

        public void Dispose()
        {
            _tokenSource.Cancel();
        }

        public virtual UniTask InitializeAsync() =>
            default;

        public abstract bool IsEnableEncryption();

        public abstract string GetEncryptionPassword();

        public void SendSaveSignal() => _isNeedSave = true;

        public abstract PlayerProgress Load();

        public abstract void Save(string rawData);

        public abstract void Clear();

        public abstract string GetRawSaveData();

        public abstract void Save(PlayerProgress progress);

        protected PlayerProgress GetDefaultProgress() =>
            new PlayerProgress();

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
            _progressSavers.ForEach(x => saveTasks.Add(x.SaveProgress(_persistentProgressService.Progress)));
            await UniTask.WhenAll(saveTasks);

            Save(_persistentProgressService.Progress);

            LogService.Log("Game is saved...");
        }
    }
}
