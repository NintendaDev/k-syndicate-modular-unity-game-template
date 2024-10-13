using Cysharp.Threading.Tasks;
using Modules.SaveManagement.Data;

namespace Modules.SaveManagement.Interfaces
{
    public interface IProgressLoader
    {
        public UniTask LoadProgress(PlayerProgress progress);
    }
}