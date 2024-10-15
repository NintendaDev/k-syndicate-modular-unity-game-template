using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Advertisements;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Advertisiments;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.MusicPlay;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class StartGameplaySceneState : LevelGameplayState
    {
        private readonly IInterstitialAdvertisimentShower _interstitialAdvertisimentShower;

        public StartGameplaySceneState(SceneStateMachine stateMachine, ILogSystem logSystem,
            IEventBus eventBus, IAnalyticsService analyticsService, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, 
            IInterstitialAdvertisimentShower interstitialAdvertisimentShower)
            : base(stateMachine, eventBus, logSystem, analyticsService, musicPlayer, resetObjects, 
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