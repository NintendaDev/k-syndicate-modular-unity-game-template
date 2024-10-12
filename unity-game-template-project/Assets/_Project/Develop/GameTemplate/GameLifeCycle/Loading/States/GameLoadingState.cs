using Cysharp.Threading.Tasks;
using Modules.SceneManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.UI.LoadingCurtain;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading.States
{
    public class GameLoadingState : GameState
    {
        private ISceneLoader _sceneLoader;
        private ILoadingCurtain _loadingCurtain;
        private GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;

        public GameLoadingState(GameStateMachine stateMachine, IEventBus eventBus, ISceneLoader sceneLoader, 
            ILoadingCurtain loadingCurtain, ILogSystem logSystem, 
            GameLoadingAssetsConfiguration gameLoadingAssetsConfiguration)
            : base(stateMachine, eventBus, logSystem)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.Show();
            await _sceneLoader.Load(_gameLoadingAssetsConfiguration.GameLoadingScene);

            _loadingCurtain.Hide();
        }
    }
}