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
using Modules.AudioManagement.Systems;
using Modules.SaveSystem.Signals;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public sealed class FinishGameplaySceneState : LevelGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IAdvertisementsSystem _advertisementsSystem;

        public FinishGameplaySceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
            ISignalBus signalBus, ILogSystem logSystem, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, IAdvertisementsSystem advertisementsSystem)
            : base(sceneStateMachine, signalBus, logSystem, analyticsSystem, musicPlayer, resetObjects, 
                loadingCurtain, levelConfigurator)
        {
            _gameStateMachine = gameStateMachine;
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
            StateSignalBus.Invoke<SaveSignal>();

            await Exit();
            await _gameStateMachine.SwitchState<GameHubGameState>();
        }
    }
}
