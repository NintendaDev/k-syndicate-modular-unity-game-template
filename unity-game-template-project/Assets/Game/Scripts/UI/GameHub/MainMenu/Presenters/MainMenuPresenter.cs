using System;
using Game.UI.GameHub.MainMenu.Views;
using Game.UI.GameHub.Signals;
using Modules.Authorization.Interfaces;
using Modules.EventBus;

namespace Game.UI.GameHub.MainMenu.Presenters
{
    public sealed class MainMenuPresenter : IDisposable
    {
        private readonly MainMenuView _view;
        private readonly ISignalBus _signalBus;

        public MainMenuPresenter(MainMenuView view, ISignalBus signalBus, ILoginInfo loginInformer) 
        {
            _view = view;
            _signalBus = signalBus;

            if (loginInformer.IsLogined)
                view.DisableLoginButton();
            else
                view.EnableLoginButton();

            _view.LoginClicked += OnLoginClick;
        }

        public void Dispose() =>
            _view.LoginClicked -= OnLoginClick;

        private void OnLoginClick() =>
            _signalBus.Invoke<LoginSignal>();
    }
}
