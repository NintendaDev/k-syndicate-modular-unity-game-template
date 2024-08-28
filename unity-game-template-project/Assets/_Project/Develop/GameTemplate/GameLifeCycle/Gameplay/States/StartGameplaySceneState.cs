using Cysharp.Threading.Tasks;
using GameTemplate.Core;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.Log;
using GameTemplate.Services.MusicPlay;
using GameTemplate.UI.LoadingCurtain;
using System.Collections.Generic;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class StartGameplaySceneState : LevelGameplayState
    {
        private readonly IInterstitialAdvertisimentShower _interstitialAdvertisimentShower;

        public StartGameplaySceneState(SceneStateMachine stateMachine, ILogService logService,
            IEventBus eventBus, IAnalyticsService analyticsService, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, ICurrentLevelConfiguration levelConfigurator, 
            IInterstitialAdvertisimentShower interstitialAdvertisimentShower)
            : base(stateMachine, eventBus, logService, analyticsService, musicPlayer, resetObjects, 
                  loadingCurtain, levelConfigurator)
        {
            _interstitialAdvertisimentShower = interstitialAdvertisimentShower;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            ShowCurtain();
            ResetGameplay();

            _interstitialAdvertisimentShower.Initialize(AdvertisementPlacement.LevelStart);
            _interstitialAdvertisimentShower.Finished += OnInterstitialFinished;

            if (_interstitialAdvertisimentShower.TryStartAdvertisementBehaviour() == false)
                await SwitchNextState();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _interstitialAdvertisimentShower.Finished -= OnInterstitialFinished;
            _interstitialAdvertisimentShower.Reset();
        }

        protected async void OnInterstitialFinished(bool isSuccess) =>
            await SwitchNextState();

        protected async UniTask SwitchNextState() =>
            await SwitchPlayState();
    }
}