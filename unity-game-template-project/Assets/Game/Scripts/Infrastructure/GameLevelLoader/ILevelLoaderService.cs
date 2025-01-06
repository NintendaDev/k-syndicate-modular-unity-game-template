using Cysharp.Threading.Tasks;
using Game.Infrastructure.Levels;

namespace Game.Services.GameLevelLoader
{
    public interface ILevelLoaderService : IFastLoadLevel, IFastLoadInitialize, ICurrentLevelConfiguration
    {
        public void Initialize();

        public UniTask LoadLevelAsync(LevelCode levelCode);
    }
}