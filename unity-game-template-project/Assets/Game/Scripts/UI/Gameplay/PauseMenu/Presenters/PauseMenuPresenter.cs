using System;
using Game.UI.Gameplay.PauseMenu.Views;
using Game.UI.Gameplay.Signals;
using Modules.EventBus;

namespace Game.UI.Gameplay.PauseMenu.Presenters
{
    public sealed class PauseMenuPresenter : IDisposable
    {
        private readonly PauseMenuView _view;
        private readonly ISignalBus _signalBus;

        public PauseMenuPresenter(PauseMenuView view, ISignalBus signalBus)
        {
            _view = view;
            _signalBus = signalBus;

            _view.BackButtonClicked += OnBackButtonClick;
            _view.ExitButtonClicked += OnExitButtonClick;
        }

        public void Dispose()
        {
            _view.BackButtonClicked -= OnBackButtonClick;
            _view.ExitButtonClicked -= OnExitButtonClick;
        }

        private void OnBackButtonClick() =>
            _signalBus.Invoke<PlaySignal>();

        private void OnExitButtonClick() =>
            _signalBus.Invoke<ExitSignal>();
    }
}
