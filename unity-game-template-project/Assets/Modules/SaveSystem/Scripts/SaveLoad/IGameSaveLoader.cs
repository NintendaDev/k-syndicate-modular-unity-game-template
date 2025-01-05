using Cysharp.Threading.Tasks;

namespace Modules.SaveSystem.SaveLoad
{
    public interface IGameSaveLoader
    {
        public UniTask SaveAsync();
        
        public UniTask<bool> TryLoadAsync();
    }
}