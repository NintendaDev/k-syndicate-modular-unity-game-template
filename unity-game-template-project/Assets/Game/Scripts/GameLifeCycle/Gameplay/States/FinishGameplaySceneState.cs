using Cysharp.Threading.Tasks;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Game.GameLifeCycle.GameHub.States;
using Game.Infrastructure.StateMachineComponents;
using Game.Services.GameLevelLoader;
using Modules.Advertisements.AnalyticsAddon;
using Modules.Advertisements.Types;
using Modules.Analytics;
using Modules.AudioManagement.Player;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;
using Modules.SaveSystem.Signals;

namespace Game.GameLifeCycle.Gameplay.States
{
    public sealed class FinishGameplaySceneState : LevelGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly AdvertisementsFacade _advertisementsFacade;

        public FinishGameplaySceneState(GameStateMachine gameStateMachine, SceneStateMachine sceneStateMachine, 
            ISignalBus signalBus, ILogSystem logSystem, IAnalyticsSystem analyticsSystem, 
            IAudioAssetPlayer audioAssetPlayer, IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, AdvertisementsFacade advertisementsFacade)
            : base(sceneStateMachine, signalBus, logSystem, analyticsSystem, audioAssetPlayer, resetObjects, 
                loadingCurtain, levelConfigurator)
        {
            _gameStateMachine = gameStateMachine;
            _advertisementsFacade = advertisementsFacade;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            RestoreGameTime();

            if (_advertisementsFacade.TryShowInterstitial(AdvertisementPlacement.EndLevel,
                    onCloseCallback: OnAdvertisimentFinish) == false)
            {
                await SaveAndSwitchGameHubState();
            }
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
