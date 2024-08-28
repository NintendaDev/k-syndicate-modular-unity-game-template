using GameTemplate.Services.Progress;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameTemplate.Services.SaveLoad
{
    public class SceneSaveButtons : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;
        private IEnumerable<IProgressLoader> _progressLoaders;
        private IPersistentProgressService _persistentProgressService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IEnumerable<IProgressLoader> progressLoaders,
            IPersistentProgressService persistentProgressService)
        {
            _saveLoadService = saveLoadService;
            _progressLoaders = progressLoaders;
            _persistentProgressService = persistentProgressService;
        }

        [Button, DisableInEditorMode]
        public void Save() =>
            _saveLoadService.SendSaveSignal();

        [Button, DisableInEditorMode]
        public void Load()
        {
            _persistentProgressService.Progress = _saveLoadService.Load();

            foreach (IProgressLoader progressLoader in _progressLoaders)
                progressLoader.LoadProgress(_persistentProgressService.Progress);
        }

        [Button, DisableInEditorMode]
        public void Clear()
        {
            _saveLoadService.Clear();
            Load();
        }
    }
}
