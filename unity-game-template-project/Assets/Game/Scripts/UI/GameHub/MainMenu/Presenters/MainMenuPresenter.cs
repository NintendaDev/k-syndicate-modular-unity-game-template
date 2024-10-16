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
        private readonly IEventBus _eventBus;

        public MainMenuPresenter(MainMenuView view, IEventBus eventBus, ILoginInfo loginInformer) 
        {
            _view = view;
            _eventBus = eventBus;

            if (loginInformer.IsLogined)
                view.DisableLoginButton();
            else
                view.EnableLoginButton();

            _view.LoginClicked += OnLoginClick;
        }

        public void Dispose() =>
            _view.LoginClicked -= OnLoginClick;

        private void OnLoginClick() =>
            _eventBus.Invoke<LoginSignal>();
    }
}
