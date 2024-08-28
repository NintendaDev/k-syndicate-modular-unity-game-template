using GameTemplate.Infrastructure.Levels.Configurations;

namespace GameTemplate.Services.GameLevelLoader
{
    public interface ICurrentLevelConfiguration
    {
        public LevelConfiguration CurrentLevelConfiguration { get; }
    }
}