using Cysharp.Threading.Tasks;
using GameTemplate.Core;
using GameTemplate.GameLifeCycle.GameHub.States;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.Log;
using GameTemplate.Services.MusicPlay;
using GameTemplate.Services.SaveLoad;
using GameTemplate.UI.LoadingCurtain;
using System.Collections.Generic;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class FinishGameplaySceneState : LevelGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISaveSignal _saveSignaller;
        private readonly IInterstitialAdvertisimentShower _interstitialAdvertisimentShower;

        public FinishGameplaySceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, IEventBus eventBus, 
            ILogService logService, IAnalyticsService analyticsService, IMusicPlay musicPlayer, ISaveSignal saveSignaller,
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, ICurrentLevelConfiguration levelConfigurator, 
            IInterstitialAdvertisimentShower interstitialAdvertisimentShower)
            : base(sceneStateMachine, eventBus, logService, analyticsService, musicPlayer, 
                  resetObjects, loadingCurtain, levelConfigurator)
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
