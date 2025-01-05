using Cysharp.Threading.Tasks;

namespace Modules.SaveSystem.SaveStrategies
{
    public interface IStorage
    {
        public UniTask<(bool, string)> TryReadAsync();
        
        public UniTask WriteAsync(string data);
    }
}