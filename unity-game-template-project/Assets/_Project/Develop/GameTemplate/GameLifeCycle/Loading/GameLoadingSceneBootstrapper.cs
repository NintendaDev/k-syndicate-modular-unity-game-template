using Cysharp.Threading.Tasks;
using Modules.StateMachines;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Log;
using Zenject;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class GameLoadingSceneBootstrapper : IInitializable
    {
        private readonly SceneStateMachine _sceneStateMachine;
        private readonly StatesFactory _statesFactory;
        private readonly ILogService _logService;

        private GameLoadingSceneBootstrapper(SceneStateMachine sceneStateMachine, StatesFactory statesFactory, ILogService logService)
        {
            _sceneStateMachine = sceneStateMachine;
            _statesFactory = statesFactory;
            _logService = logService;
        }

        public void Initialize()
        {
            _logService.Log("Start loading scene bootstraping");

            _sceneStateMachine.RegisterState(_statesFactory.Create<DownloadAccountInfoSceneState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<LoadPlayerProgressSceneState>());
            _sceneStateMachine.RegisterState(_statesFactory.Create<FinishLoadingSceneState>());

            _logService.Log("Finish loading scene bootstraping");

            _sceneStateMachine.SwitchState<DownloadAccountInfoSceneState>().Forget();
        }
    }
}
