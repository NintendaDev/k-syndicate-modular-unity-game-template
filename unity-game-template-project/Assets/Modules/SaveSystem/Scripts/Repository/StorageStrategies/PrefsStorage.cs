using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.SaveSystem.SaveStrategies
{
    public sealed class PrefsStorage : IStorage
    {
        private readonly string _key;

        public PrefsStorage(string key)
        {
            _key = key;
        }

        public UniTask<(bool, string)> TryReadAsync()
        {
            string data = PlayerPrefs.GetString(_key);
            bool isSuccess = string.IsNullOrEmpty(data) == false;
            
            return UniTask.FromResult((isSuccess, data));
        }

        public UniTask WriteAsync(string data)
        {
            PlayerPrefs.SetString(_key, data);
            
            return UniTask.CompletedTask;
        }
    }
}