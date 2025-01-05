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
    public sealed class PauseGameplaySceneState : LevelGameplayState
    {
        public PauseGameplaySceneState(SceneStateMachine stateMachine, ILogSystem logSystem,
            ISignalBus signalBus, IAnalyticsSystem analyticsSystem, IMusicPlay musicPlayer, 
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, 
            ICurrentLevelConfiguration levelConfigurator)
            : base(stateMachine, signalBus, logSystem, analyticsSystem, musicPlayer, 
                  resetObjects, loadingCurtain, levelConfigurator)
        {
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            StateSignalBus.Subscribe<PlaySignal>(OnPlaySignal);
            StateSignalBus.Subscribe<ExitSignal>(OnExitSignal);

            PauseMusic();
            StopGameTime();
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            StateSignalBus.Unsubscribe<PlaySignal>(OnPlaySignal);
            StateSignalBus.Unsubscribe<ExitSignal>(OnExitSignal);
        }

        private async void OnPlaySignal() =>
           await SwitchPlayState();

        private async void OnExitSignal() =>
            await SwitchFinishState();
    }
}
