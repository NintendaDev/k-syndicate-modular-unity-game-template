using Cysharp.Threading.Tasks;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Game.Infrastructure.StateMachineComponents;
using Game.Services.GameLevelLoader;
using Modules.Advertisements.Systems;
using Modules.Analytics;
using Modules.AudioManagement.Player;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;

namespace Game.GameLifeCycle.Gameplay.States
{
    public sealed class StartGameplaySceneState : LevelGameplayState
    {
        private readonly IAdvertisementsSystem _advertisementsSystem;

        public StartGameplaySceneState(SceneStateMachine stateMachine, ILogSystem logSystem,
            ISignalBus signalBus, IAnalyticsSystem analyticsSystem, IAudioAssetPlayer audioAssetPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, IAdvertisementsSystem advertisementsSystem)
            : base(stateMachine, signalBus, logSystem, analyticsSystem, audioAssetPlayer, resetObjects, 
                  loadingCurtain, levelConfigurator)
        {
            _advertisementsSystem = advertisementsSystem;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            ShowCurtain();
            ResetGameplay();

            if (_advertisementsSystem.TryShowInterstitial(onCloseCallback: OnInterstitialFinished) == false)
                await SwitchNextState();
        }

        private async void OnInterstitialFinished() =>
            await SwitchNextState();

        private async UniTask SwitchNextState() =>
            await SwitchPlayState();
    }
}