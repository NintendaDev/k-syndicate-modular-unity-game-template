using Cysharp.Threading.Tasks;
using Game.Infrastructure.StateMachineComponents;
using Game.Infrastructure.StateMachineComponents.States;
using Modules.Analytics;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;
using Modules.SaveSystem.SaveLoad;

namespace Game.GameLifeCycle.Loading
{
    public sealed class LoadPlayerProgressSceneState : AnalyticsSceneState
    {
        private readonly IGameSaveLoader _gameSaveLoader;
        private readonly IDefaultSaveLoader _defaultSaveLoader;

        public LoadPlayerProgressSceneState(SceneStateMachine stateMachine, ISignalBus signalBus, ILogSystem logSystem,
            IGameSaveLoader gameSaveLoader, IDefaultSaveLoader defaultSaveLoader, IAnalyticsSystem analyticsSystem, 
            IStaticDataService staticDataService) 
            : base(stateMachine, signalBus, logSystem, analyticsSystem, staticDataService)
        {
            _gameSaveLoader = gameSaveLoader;
            _defaultSaveLoader = defaultSaveLoader;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            SendAnalyticsEvent(AnalyticsConfiguration.LoadProgressStageEvent);

            if (await _gameSaveLoader.TryLoadAsync() == false)
                _defaultSaveLoader.LoadDefaultSave();

            await StateMachine.SwitchState<FinishLoadingSceneState>();
        }
    }
}
