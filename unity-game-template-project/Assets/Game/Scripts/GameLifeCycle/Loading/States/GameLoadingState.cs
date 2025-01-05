using Cysharp.Threading.Tasks;
using Modules.SceneManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using Modules.LoadingCurtain;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading.States
{
    public sealed class GameLoadingState : GameState
    {
        private ISingleSceneLoader _singleSceneLoader;
        private ILoadingCurtain _loadingCurtain;
        private GameLoadingAssetsConfiguration _gameLoadingAssetsConfiguration;

        public GameLoadingState(GameStateMachine stateMachine, ISignalBus signalBus, ISingleSceneLoader singleSceneLoader, 
            ILoadingCurtain loadingCurtain, ILogSystem logSystem, 
            GameLoadingAssetsConfiguration gameLoadingAssetsConfiguration)
            : base(stateMachine, signalBus, logSystem)
        {
            _singleSceneLoader = singleSceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameLoadingAssetsConfiguration = gameLoadingAssetsConfiguration;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _loadingCurtain.ShowWithoutProgressBar();
            await _singleSceneLoader.Load(_gameLoadingAssetsConfiguration.GameLoadingScene);
        }
    }
}