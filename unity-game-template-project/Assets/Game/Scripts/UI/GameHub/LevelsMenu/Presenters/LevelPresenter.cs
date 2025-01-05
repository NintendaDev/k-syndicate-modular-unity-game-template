using GameTemplate.UI.GameHub.LevelsMenu.Views;
using GameTemplate.UI.GameHub.Signals;
using System;
using Modules.EventBus;

namespace GameTemplate.UI.GameHub.LevelsMenu.Presenters
{
    public sealed class LevelPresenter : IDisposable
    {
        private readonly LevelView _levelView;
        private readonly ISignalBus _signalBus;

        public LevelPresenter(LevelView levelView, ISignalBus signalBus)
        {
            _levelView = levelView;
            _signalBus = signalBus;

            _levelView.Clicked += OnClick;
        }

        public void Dispose()
        {
            _levelView.Clicked -= OnClick;
        }

        private void OnClick() =>
            _signalBus.Invoke(new LevelLoadSignal(_levelView.LevelCode));
    }
}
