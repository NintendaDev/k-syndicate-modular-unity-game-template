using Cysharp.Threading.Tasks;
using Modules.SaveManagement.Data;

namespace Modules.SaveManagement.Interfaces
{
    public interface IProgressSaver
    {
        public UniTask SaveProgress(PlayerProgress progress);
    }
}