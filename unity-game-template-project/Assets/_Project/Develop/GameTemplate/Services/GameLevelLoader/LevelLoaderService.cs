using Cysharp.Threading.Tasks;
using Modules.SceneManagement;
using GameTemplate.Infrastructure.Levels;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.Level.Configurations;
using Modules.AssetManagement.StaticData;

namespace GameTemplate.Services.GameLevelLoader
{
    public class LevelLoaderService : ILevelLoaderService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneLoader _sceneLoader;
        private LevelCode _levelCodeForFastLoading;
        private LevelsConfigurationsHub _levelsConfigurations;

        public LevelLoaderService(IStaticDataService staticDataService, ISceneLoader sceneLoader)
        {
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
        }

        public LevelConfiguration CurrentLevelConfiguration { get; private set; }

        public async UniTask FastLoadLevelAsync() =>
            await LoadLevelAsync(_levelCodeForFastLoading);

        public void Initialize()
        {
            _levelsConfigurations = _staticDataService.GetConfiguration<LevelsConfigurationsHub>();
        }

        public async UniTask LoadLevelAsync(LevelCode levelCode)
        {
            if (_levelsConfigurations.TryGetLevelConfiguration(levelCode,
                    out LevelConfiguration levelConfiguration) == false)
            {
                throw new System.Exception($"Level configuration with code {levelCode} was not found");
            }
                

            CurrentLevelConfiguration = levelConfiguration;

            await _sceneLoader.Load(levelConfiguration.SceneAddress);
        }

        public void InitializeFastLoad(LevelCode levelCode)
        {
            _levelCodeForFastLoading = levelCode;
        }
    }
}
