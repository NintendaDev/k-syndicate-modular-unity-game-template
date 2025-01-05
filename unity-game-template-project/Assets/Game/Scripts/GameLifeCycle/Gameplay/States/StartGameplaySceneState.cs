using Cysharp.Threading.Tasks;
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

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public sealed class StartGameplaySceneState : LevelGameplayState
    {
        private readonly IAdvertisementsSystem _advertisementsSystem;

        public StartGameplaySceneState(SceneStateMachine stateMachine, ILogSystem logSystem,
            ISignalBus signalBus, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator, IAdvertisementsSystem advertisementsSystem)
            : base(stateMachine, signalBus, logSystem, analyticsSystem, musicPlayer, resetObjects, 
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