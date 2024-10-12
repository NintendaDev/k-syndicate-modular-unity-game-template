using Cysharp.Threading.Tasks;
using Modules.SaveManagement.Data;

namespace Modules.SaveManagement.Interfaces
{
    public interface IProgressSaver : IProgressLoader
    {
        public UniTask SaveProgress(PlayerProgress progress);
    }
}