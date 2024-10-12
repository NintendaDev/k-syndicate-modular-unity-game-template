using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.Progress;
using GameTemplate.Services.SaveLoad;
using System.Collections.Generic;
using Modules.AssetManagement.StaticData;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class LoadPlayerProgressSceneState : AnalyticsSceneState
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly List<IProgressLoader> _progressLoaders;

        public LoadPlayerProgressSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem,
            ISaveLoadService saveLoadService, List<IProgressLoader> progressLoaders,
            IPersistentProgressService persistentProgressService, IAnalyticsService analyticsService, 
            IStaticDataService staticDataService) 
            : base(stateMachine, eventBus, logSystem, analyticsService, staticDataService)
        {
            _saveLoadService = saveLoadService;
            _persistentProgressService = persistentProgressService;
            _progressLoaders = progressLoaders;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            SendAnalyticsEvent(AnalyticsConfiguration.LoadProgressStageEvent);
            await _saveLoadService.InitializeAsync();

            _persistentProgressService.Progress = _saveLoadService.Load();

            List<UniTask> progressLoadTasks = new();
            _progressLoaders.ForEach(x => progressLoadTasks
                .Add(x.LoadProgress(_persistentProgressService.Progress)));

            await UniTask.WhenAll(progressLoadTasks);

            await StateMachine.SwitchState<FinishLoadingSceneState>();
        }
    }
}
