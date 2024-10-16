using GameTemplate.UI.GameHub.LevelsMenu.Views;
using GameTemplate.UI.GameHub.Signals;
using System;
using Modules.EventBus;

namespace GameTemplate.UI.GameHub.LevelsMenu.Presenters
{
    public sealed class LevelPresenter : IDisposable
    {
        private readonly LevelView _levelView;
        private readonly IEventBus _eventBus;

        public LevelPresenter(LevelView levelView, IEventBus eventBus)
        {
            _levelView = levelView;
            _eventBus = eventBus;

            _levelView.Clicked += OnClick;
        }

        public void Dispose()
        {
            _levelView.Clicked -= OnClick;
        }

        private void OnClick() =>
            _eventBus.Invoke(new LevelLoadSignal(_levelView.LevelCode));
    }
}
