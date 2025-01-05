using System;
using Cysharp.Threading.Tasks;
using Modules.EventBus;
using Modules.SaveSystem.Signals;
using R3;
using Zenject;

namespace Modules.SaveSystem.SaveLoad
{
    public sealed class SaveLoadController : IInitializable, IDisposable
    {
        private const float SavePeriodSeconds = 1f;
        private readonly IGameSaveLoader _gameSaveLoader;
        private readonly ISignalBus _signalBus;
        private bool _isRequiredSave;
        private CompositeDisposable _disposables = new();
        private UniTask _saveTask;

        public SaveLoadController(IGameSaveLoader gameSaveLoader, ISignalBus signalBus)
        {
            _gameSaveLoader = gameSaveLoader;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SaveSignal>(OnSaveSignal);

            Observable
                .Interval(TimeSpan.FromSeconds(SavePeriodSeconds))
                .Subscribe(_ => StartSaveBehaviour())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<SaveSignal>(OnSaveSignal);
            _disposables.Clear();
        }

        private void OnSaveSignal()
        {
            _isRequiredSave = true;
        }

        private void StartSaveBehaviour()
        {
            if (_saveTask.Status != UniTaskStatus.Succeeded)
                return;
            
            _saveTask = StartSaveBehaviourAsync();
            _saveTask.Forget();
        }

        private async UniTask StartSaveBehaviourAsync()
        {
            if (_isRequiredSave == false)
                return;
            
            await _gameSaveLoader.SaveAsync();
            _isRequiredSave = false;
        }
    }
}