using Game.Infrastructure.Levels.Configurations;

namespace Game.Services.GameLevelLoader
{
    public interface ICurrentLevelConfiguration
    {
        public LevelConfiguration CurrentLevelConfiguration { get; }
    }
}