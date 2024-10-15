using Cysharp.Threading.Tasks;
using Modules.SceneManagement;
using GameTemplate.GameLifeCycle.Gameplay;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Infrastructure.Levels;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.UI.GameHub.Signals;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.GameHub.States
{
    public class GameHubGameState : GameState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly ISceneLoader _sceneLoader;
        private readonly IFastLoadInitialize _levelLoaderInitializer;
        private readonly GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;

        public GameHubGameState(GameStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem, 
            ILoadingCurtain loadingCurtain, ISceneLoader sceneLoader, IFastLoadInitialize levelLoaderInitializer,
            GameLoadingAssetsConfiguration gameLoadingAssetsConfiguration)
            : base(stateMachine, eventBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _levelLoaderInitializer = levelLoaderInitializer;
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.Show();
            await _sceneLoader.Load(_gameLoadingAssetsConfiguration.GameHubScene);

            StateEventBus.Subscribe<LevelLoadSignal>(OnLevelLoadEventRequest);
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateEventBus.Unsubscribe<LevelLoadSignal>(OnLevelLoadEventRequest);
        }

        private void OnLevelLoadEventRequest(LevelLoadSignal levelLoadSignal) =>
            LoadLevelAndSwitchStateAsync(levelLoadSignal.LevelCode).Forget();
            
        private async UniTask LoadLevelAndSwitchStateAsync(LevelCode levelCode)
        {
            _levelLoaderInitializer.InitializeFastLoad(levelCode);
            await StateMachine.SwitchState<GameplayGameState>();
        }
    }
}
