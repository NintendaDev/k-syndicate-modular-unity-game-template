using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Modules.SaveSystem.Repositories
{
    public interface IGameRepository
    {
        public UniTask<Dictionary<string, string>> GetStateAsync();
        
        public UniTask SetStateAsync(Dictionary<string, string> gameState);
    }
}