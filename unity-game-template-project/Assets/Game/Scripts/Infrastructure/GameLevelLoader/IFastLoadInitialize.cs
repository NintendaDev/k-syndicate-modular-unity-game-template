using GameTemplate.Infrastructure.Levels;

namespace GameTemplate.Services.GameLevelLoader
{
    public interface IFastLoadInitialize
    {
        public void InitializeFastLoad(LevelCode levelCode);
    }
}