using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.GameLevelLoader;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Modules.Advertisements.Systems;
using Modules.Analytics;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;
using Modules.MusicManagement.Systems;
using Modules.SaveManagement.Interfaces;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public sealed class FinishGameplaySceneState : LevelGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveSignal _saveSignaller;
        private readonly IAdvertisementsSystem _advertisementsSystem;

        public FinishGameplaySceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
            IEventBus eventBus, ILogSystem logSystem, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            ISaveSignal saveSignaller, IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, IAdvertisementsSystem advertisementsSystem)
            : base(sceneStateMachine, eventBus, logSystem, analyticsSystem, musicPlayer, resetObjects, 
                loadingCurtain, levelConfigurator)
        {
            _gameStateMachine = gameStateMachine;
            _saveSignaller = saveSignaller;
            _advertisementsSystem = advertisementsSystem;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            RestoreGameTime();

            if (_advertisementsSystem.TryShowInterstitial(onCloseCallback: OnAdvertisimentFinish) == false)
                await SaveAndSwitchGameHubState();
        }

        private async void OnAdvertisimentFinish() =>
            await SaveAndSwitchGameHubState();

        private async UniTask SaveAndSwitchGameHubState()
        {
            _saveSignaller.SendSaveSignal();

            await Exit();
            await _gameStateMachine.SwitchState<GameHubGameState>();
        }
    }
}
