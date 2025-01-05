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
    public sealed class GameHubGameState : GameState
    {
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly ISingleSceneLoader _singleSceneLoader;
        private readonly IFastLoadInitialize _levelLoaderInitializer;
        private readonly GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;

        public GameHubGameState(GameStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem, 
            ILoadingCurtain loadingCurtain, ISingleSceneLoader singleSceneLoader, IFastLoadInitialize levelLoaderInitializer,
            GameLoadingAssetsConfiguration gameLoadingAssetsConfiguration)
            : base(stateMachine, signalBus, logSystem)
        {
            _loadingCurtain = loadingCurtain;
            _singleSceneLoader = singleSceneLoader;
            _levelLoaderInitializer = levelLoaderInitializer;
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.ShowWithoutProgressBar();
            await _singleSceneLoader.Load(_gameLoadingAssetsConfiguration.GameHubScene);

            StateSignalBus.Subscribe<LevelLoadSignal>(OnLevelLoadEventRequest);
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateSignalBus.Unsubscribe<LevelLoadSignal>(OnLevelLoadEventRequest);
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
