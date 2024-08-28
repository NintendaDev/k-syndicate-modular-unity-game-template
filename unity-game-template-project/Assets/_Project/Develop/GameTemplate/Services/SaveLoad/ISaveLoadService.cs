using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.SaveLoad
{
    public interface ISaveLoadService : ISaveSignal
    {
        public UniTask InitializeAsync();

        public bool IsEnableEncryption();

        public string GetEncryptionPassword();

        public PlayerProgress Load();

        public void Save(string rawData);

        public void Save(PlayerProgress progress);

        public void Clear();

        public string GetRawSaveData();
    }
}