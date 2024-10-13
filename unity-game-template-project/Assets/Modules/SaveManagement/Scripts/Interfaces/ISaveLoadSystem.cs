using Cysharp.Threading.Tasks;
using Modules.SaveManagement.Data;

namespace Modules.SaveManagement.Interfaces
{
    public interface ISaveLoadSystem : ISaveSignal
    {
        public UniTask InitializeAsync();

        public bool IsEnableEncryption();

        public string GetEncryptionPassword();

        public PlayerProgress Load<TProgress>() where TProgress : PlayerProgress;

        public void Save(string rawData);

        public void Save(PlayerProgress progress);

        public void Clear();

        public string GetRawSaveData();
    }
}