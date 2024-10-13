using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.MusicPlay;
using GameTemplate.UI.Gameplay.Signals;
using GameTemplate.UI.LoadingCurtain;
using System.Collections.Generic;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class PlayGameplaySceneState : LevelGameplayState
    {
        public PlayGameplaySceneState(SceneStateMachine stateMachine, IEventBus eventBus,
            ILogSystem logSystem, IAnalyticsService analyticsService, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator)
            : base(stateMachine, eventBus, logSystem, analyticsService, musicPlayer, resetObjects, 
                  loadingCurtain, levelConfigurator)
        {
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            StateEventBus.Subscribe<PauseSignal>(OnPauseSignal);

            RestoreGameTime();
            PlayMusic();
            HideCurtain();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateEventBus.Unsubscribe<PauseSignal>(OnPauseSignal);
        }

        private async void OnPauseSignal() =>
            await SwitchPauseState();
    }
}
