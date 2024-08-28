using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.Progress
{
    public interface IProgressSaver : IProgressLoader
    {
        public UniTask SaveProgress(PlayerProgress progress);
    }
}