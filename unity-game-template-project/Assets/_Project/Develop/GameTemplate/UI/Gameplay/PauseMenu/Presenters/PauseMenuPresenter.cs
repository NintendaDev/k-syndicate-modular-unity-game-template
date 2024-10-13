using GameTemplate.UI.Gameplay.PauseMenu.Views;
using GameTemplate.UI.Gameplay.Signals;
using System;
using Modules.EventBus;

namespace GameTemplate.UI.Gameplay.PauseMenu.Presenters
{
    public class PauseMenuPresenter : IDisposable
    {
        private readonly PauseMenuView _view;
        private readonly IEventBus _eventBus;

        public PauseMenuPresenter(PauseMenuView view, IEventBus eventBus)
        {
            _view = view;
            _eventBus = eventBus;

            _view.BackButtonClicked += OnBackButtonClick;
            _view.ExitButtonClicked += OnExitButtonClick;
        }

        public void Dispose()
        {
            _view.BackButtonClicked -= OnBackButtonClick;
            _view.ExitButtonClicked -= OnExitButtonClick;
        }

        private void OnBackButtonClick() =>
            _eventBus.Invoke<PlaySignal>();

        private void OnExitButtonClick() =>
            _eventBus.Invoke<ExitSignal>();
    }
}
