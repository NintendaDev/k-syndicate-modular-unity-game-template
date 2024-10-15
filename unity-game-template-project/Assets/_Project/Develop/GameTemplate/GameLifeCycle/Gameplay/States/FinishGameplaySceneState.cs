using Cysharp.Threading.Tasks;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;
using Modules.MusicManagement.Systems;
using Modules.SaveManagement.Interfaces;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class FinishGameplaySceneState : LevelGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveSignal _saveSignaller;
        private readonly IInterstitialAdvertisimentShower _interstitialAdvertisimentShower;

        public FinishGameplaySceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
            IEventBus eventBus, ILogSystem logSystem, IAnalyticsService analyticsService, IMusicPlay musicPlayer, 
            ISaveSignal saveSignaller, IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, 
            IInterstitialAdvertisimentShower interstitialAdvertisimentShower)
            : base(sceneStateMachine, eventBus, logSystem, analyticsService, musicPlayer, resetObjects, 
                loadingCurtain, levelConfigurator)
        {
            _gameStateMachine = gameStateMachine;
            _saveSignaller = saveSignaller;
            _interstitialAdvertisimentShower = interstitialAdvertisimentShower;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            RestoreGameTime();

            _interstitialAdvertisimentShower.Initialize(AdvertisementPlacement.LevelEnd);
            _interstitialAdvertisimentShower.Finished += OnAdvertisimentFinish;

            if (_interstitialAdvertisimentShower.TryStartAdvertisementBehaviour() == false)
                await SaveAndSwitchGameHubState();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _interstitialAdvertisimentShower.Finished -= OnAdvertisimentFinish;
            _interstitialAdvertisimentShower.Reset();
        }

        private async void OnAdvertisimentFinish(bool isSuccess) =>
            await SaveAndSwitchGameHubState();

        private async UniTask SaveAndSwitchGameHubState()
        {
            _saveSignaller.SendSaveSignal();

            await Exit();
            await _gameStateMachine.SwitchState<GameHubGameState>();
        }
    }
}
