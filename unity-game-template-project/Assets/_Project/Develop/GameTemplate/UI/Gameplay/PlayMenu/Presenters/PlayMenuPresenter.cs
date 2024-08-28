using GameTemplate.Infrastructure.Signals;
using GameTemplate.UI.Gameplay.PlayMenu.Views;
using GameTemplate.UI.Gameplay.Signals;
using System;

namespace GameTemplate.UI.Gameplay.Presenters
{
    public class PlayMenuPresenter : IDisposable
    {
        private readonly PlayMenuView _view;
        private readonly IEventBus _eventBus;

        public PlayMenuPresenter(PlayMenuView view, IEventBus eventBus)
        {
            _view = view;
            _eventBus = eventBus;

            _view.PauseButtonClicked += OnPauseButtonClick;
        }

        public void Dispose()
        {
            _view.PauseButtonClicked -= OnPauseButtonClick;
        }

        private void OnPauseButtonClick() =>
            _eventBus.Invoke<PauseSignal>();
    }
}
