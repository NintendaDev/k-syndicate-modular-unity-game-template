using GameTemplate.Core;
using GameTemplate.UI.Core.Buttons;
using GameTemplate.UI.GameHub.LevelsMenu.Views;
using GameTemplate.UI.Wallets;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTemplate.UI.GameHub.MainMenu.Views
{
    public class MainMenuView : EnableDisableBehaviour, IWalletPanelView
    {
        [SerializeField, Required] private UIButton _settingsButton;
        [SerializeField, Required] private UITextButton _loginButton;
        [SerializeField, Required] private UITextButton _levelsButton;
        [SerializeField, Required] private WalletsPanelView _walletsPanelView;
        
        public event Action SettingButtonClicked;

        public event Action LoginClicked;

        public event Action LevelsButtonClicked;

        private void OnEnable()
        {
            _settingsButton.Clicked += OnSettingButtonClick;
            _loginButton.Clicked += OnLoginClicked;
            _levelsButton.Clicked += OnLevelsClick;
        }

        private void OnDisable()
        {
            _settingsButton.Clicked -= OnSettingButtonClick;
            _loginButton.Clicked -= OnLoginClicked;
            _levelsButton.Clicked -= OnLevelsClick;
        }

        public void Link(IEnumerable<WalletView> walletViews) =>
            _walletsPanelView.Link(walletViews);

        public void EnableLoginButton() =>
            _loginButton.Enable();

        public void DisableLoginButton() =>
            _loginButton.Disable();

        private void OnSettingButtonClick() =>
            SettingButtonClicked?.Invoke();

        private void OnLoginClicked() =>
            LoginClicked?.Invoke();

        private void OnLevelsClick() =>
            LevelsButtonClicked?.Invoke();
    }
}
