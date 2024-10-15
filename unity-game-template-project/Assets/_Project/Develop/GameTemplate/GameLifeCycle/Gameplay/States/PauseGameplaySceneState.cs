using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.UI.Gameplay.Signals;
using Modules.LoadingCurtain;
using System.Collections.Generic;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Logging;
using Modules.MusicManagement.Systems;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public class PauseGameplaySceneState : LevelGameplayState
    {
        public PauseGameplaySceneState(SceneStateMachine stateMachine, ILogSystem logSystem,
            IEventBus eventBus, IAnalyticsService analyticsService, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator)
            : base(stateMachine, eventBus, logSystem, analyticsService, musicPlayer, 
                  resetObjects, loadingCurtain, levelConfigurator)
        {
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            StateEventBus.Subscribe<PlaySignal>(OnPlaySignal);
            StateEventBus.Subscribe<ExitSignal>(OnExitSignal);

            PauseMusic();
            StopGameTime();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateEventBus.Unsubscribe<PlaySignal>(OnPlaySignal);
            StateEventBus.Unsubscribe<ExitSignal>(OnExitSignal);
        }

        private async void OnPlaySignal() =>
           await SwitchPlayState();

        private async void OnExitSignal() =>
            await SwitchFinishState();
    }
}
