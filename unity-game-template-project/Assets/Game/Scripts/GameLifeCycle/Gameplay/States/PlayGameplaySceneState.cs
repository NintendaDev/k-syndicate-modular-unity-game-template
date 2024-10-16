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
using Modules.MusicManagement.Systems;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public sealed class PlayGameplaySceneState : LevelGameplayState
    {
        public PlayGameplaySceneState(SceneStateMachine stateMachine, IEventBus eventBus,
            ILogSystem logSystem, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator)
            : base(stateMachine, eventBus, logSystem, analyticsSystem, musicPlayer, resetObjects, 
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
