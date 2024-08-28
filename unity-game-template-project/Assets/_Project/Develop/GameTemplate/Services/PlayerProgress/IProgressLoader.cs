using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.Progress
{
    public interface IProgressLoader
    {
        public UniTask LoadProgress(PlayerProgress progress);
    }
}