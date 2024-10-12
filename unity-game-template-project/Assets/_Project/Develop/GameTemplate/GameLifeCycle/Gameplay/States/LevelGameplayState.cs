using Cysharp.Threading.Tasks;
using GameTemplate.Core;
using GameTemplate.Services.Analytics;
using GameTemplate.Services.GameLevelLoader;
using GameTemplate.Services.MusicPlay;
using GameTemplate.Infrastructure.StateMachineComponents.States;
using GameTemplate.UI.LoadingCurtain;
using System.Collections.Generic;
using UnityEngine;
using GameTemplate.Infrastructure.StateMachineComponents;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.Infrastructure.Signals;
using Modules.Logging;

namespace GameTemplate.GameLifeCycle.Gameplay.StandardLevelStates
{
    public abstract class LevelGameplayState : SceneState
    {
        private readonly float _originalTimeScale;
        private readonly IEnumerable<IReset> _resetObjects;
        private readonly ILoadingCurtain _loadingCurtain;

        public LevelGameplayState(SceneStateMachine stateMachine, IEventBus eventBus, ILogSystem logSystem,
            IAnalyticsService analyticsService, IMusicPlay musicPlayer,
            IEnumerable<IReset> resetObjects, ILoadingCurtain loadingCurtain, ICurrentLevelConfiguration levelConfigurator) 
            : base(stateMachine, eventBus, logSystem)
        {
            MusicPlay = musicPlayer;
            _originalTimeScale = Time.timeScale;
            _resetObjects = resetObjects;
            _loadingCurtain = loadingCurtain;
            CurrentLevelConfiguration = levelConfigurator.CurrentLevelConfiguration;
            AnalyticsService = analyticsService;
        }

        protected IMusicPlay MusicPlay { get; private set; }

        protected LevelConfiguration CurrentLevelConfiguration { get; private set; }

        protected IAnalyticsService AnalyticsService { get; private set; }

        protected void ResetGameplay()
        {
            foreach (IReset resetObject in _resetObjects)
                resetObject.Reset();
        }

        protected void StopGameTime() =>
            Time.timeScale = 0;

        protected void RestoreGameTime() =>
            Time.timeScale = _originalTimeScale;

        protected void PlayMusic() =>
            MusicPlay.PlayOrUnpause();

        protected void PauseMusic() =>
            MusicPlay.Pause();

        protected void ShowCurtain() =>
            _loadingCurtain.Show();

        protected void HideCurtain() =>
            _loadingCurtain.Hide();

        protected async UniTask SwitchStartState() =>
            await StateMachine.SwitchState<StartGameplaySceneState>();

        protected async UniTask SwitchPlayState() =>
            await StateMachine.SwitchState<PlayGameplaySceneState>();

        protected async UniTask SwitchPauseState() =>
            await StateMachine.SwitchState<PauseGameplaySceneState>();

        protected async UniTask SwitchFinishState() =>
            await StateMachine.SwitchState<FinishGameplaySceneState>();
    }
}
