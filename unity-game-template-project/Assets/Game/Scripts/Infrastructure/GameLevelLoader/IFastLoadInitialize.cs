using Game.Infrastructure.Levels;

namespace Game.Services.GameLevelLoader
{
    public interface IFastLoadInitialize
    {
        public void InitializeFastLoad(LevelCode levelCode);
    }
}