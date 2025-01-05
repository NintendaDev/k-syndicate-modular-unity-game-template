using System.IO;
using Cysharp.Threading.Tasks;

namespace Modules.SaveSystem.SaveStrategies
{
    public sealed class FileStorage : IStorage
    {
        private readonly string _filePath;

        public FileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public UniTask<(bool, string)> TryReadAsync()
        {
            if (File.Exists(_filePath) == false)
                return UniTask.FromResult((true, string.Empty));

            string data = File.ReadAllText(_filePath);

            if (string.IsNullOrEmpty(data))
                return UniTask.FromResult((false, data));

            return UniTask.FromResult((true, data));
        }

        public UniTask WriteAsync(string data)
        {
            File.WriteAllText(_filePath, data);
            
            return UniTask.CompletedTask;
        }
    }
}