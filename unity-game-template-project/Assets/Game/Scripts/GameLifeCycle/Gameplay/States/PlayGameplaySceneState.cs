using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.UI.Gameplay.Signals;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Modules.Analytics;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;
using Modules.AudioManagement.Systems;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public sealed class PlayGameplaySceneState : LevelGameplayState
    {
        public PlayGameplaySceneState(SceneStateMachine stateMachine, ISignalBus signalBus,
            ILogSystem logSystem, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator)
            : base(stateMachine, signalBus, logSystem, analyticsSystem, musicPlayer, resetObjects, 
                  loadingCurtain, levelConfigurator)
        {
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            StateSignalBus.Subscribe<PauseSignal>(OnPauseSignal);

            RestoreGameTime();
            PlayMusic();
            HideCurtain();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateSignalBus.Unsubscribe<PauseSignal>(OnPauseSignal);
        }

        private async void OnPauseSignal() =>
            await SwitchPauseState();
    }
}
