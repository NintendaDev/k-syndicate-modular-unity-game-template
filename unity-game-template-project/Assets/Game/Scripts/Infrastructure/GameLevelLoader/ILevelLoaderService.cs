using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Levels;

namespace GameTemplate.Services.GameLevelLoader
{
    public interface ILevelLoaderService : IFastLoadLevel, IFastLoadInitialize, ICurrentLevelConfiguration
    {
        public void Initialize();

        public UniTask LoadLevelAsync(LevelCode levelCode);
    }
}