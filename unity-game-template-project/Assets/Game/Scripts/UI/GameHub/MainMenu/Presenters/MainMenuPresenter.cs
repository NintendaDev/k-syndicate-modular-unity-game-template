using GameTemplate.UI.GameHub.MainMenu.Views;
using GameTemplate.UI.GameHub.Signals;
using System;
using Modules.Authorization.Interfaces;
using Modules.EventBus;

namespace GameTemplate.UI.GameHub.MainMenu.Presenters
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
