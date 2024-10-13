using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.Services.Analytics;
using System.Collections.Generic;
using GameTemplate.Infrastructure.SaveManagement;
using Modules.AssetsManagement.StaticData;
using Modules.EventBus;
using Modules.Logging;
using Modules.SaveManagement.Interfaces;

namespace GameTemplate.GameLifeCycle.Loading
{
    public class LoadPlayerProgressSceneState : AnalyticsSceneState
    {
        private readonly ISaveLoadSystem _saveLoadSystem;
        private readonly IPersistentProgressProvider _persistentProgressProvider;
        private readonly IDefaultPlayerProgress _defaultPlayerProgressProvider;
        private readonly List<IProgressLoader> _progressLoaders;

        public LoadPlayerProgressSceneState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem,
            ISaveLoadSystem saveLoadSystem, List<IProgressLoader> progressLoaders,
            IPersistentProgressProvider persistentProgressProvider, IAnalyticsService analyticsService, 
            IStaticDataService staticDataService, IDefaultPlayerProgress defaultPlayerProgressProvider) 
            : base(stateMachine, eventBus, logSystem, analyticsService, staticDataService)
        {
            _saveLoadSystem = saveLoadSystem;
            _persistentProgressProvider = persistentProgressProvider;
            _defaultPlayerProgressProvider = defaultPlayerProgressProvider;
            _progressLoaders = progressLoaders;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            SendAnalyticsEvent(AnalyticsConfiguration.LoadProgressStageEvent);
            await _saveLoadSystem.InitializeAsync();

            _persistentProgressProvider.Progress = _saveLoadSystem.Load<GameTemplatePlayerProgress>();

            if (_persistentProgressProvider.Progress == null)
                _persistentProgressProvider.Progress = _defaultPlayerProgressProvider.GetDefaultProgress();

            List<UniTask> progressLoadTasks = new();
            _progressLoaders.ForEach(x => progressLoadTasks
                .Add(x.LoadProgress(_persistentProgressProvider.Progress)));

            await UniTask.WhenAll(progressLoadTasks);

            await StateMachine.SwitchState<FinishLoadingSceneState>();
        }
    }
}
