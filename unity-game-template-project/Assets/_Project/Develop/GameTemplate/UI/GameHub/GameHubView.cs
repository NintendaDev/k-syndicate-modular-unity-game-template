using GameTemplate.UI.GameHub.LevelsMenu.Views;
using GameTemplate.UI.GameHub.SettinsMenu.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.UI.GameHub.MainMenu.Views
{
    public class GameHubView : MonoBehaviour
    {
        [SerializeField, Required] private MainMenuView _mainMenuView;
        [SerializeField, Required] private LevelsMenuView _levelsMenuView;
        [SerializeField, Required] private SettingsMenuView _settingsMenuView;

        private void Start()
        {
            EnableMainMenuView();
        }

        private void OnEnable()
        {
            _mainMenuView.SettingButtonClicked += OnSettingsButtonClick;
            _mainMenuView.LevelsButtonClicked += OnLevelsButtonClick;
            _levelsMenuView.BackButtonClicked += OnLevelsMenuBackClick;
            _settingsMenuView.BackButtonClicked += OnSettingsMenuBackButtonClick;
        }

        private void OnDisable()
        {
            _mainMenuView.SettingButtonClicked -= OnSettingsButtonClick;
            _mainMenuView.LevelsButtonClicked -= OnLevelsButtonClick;
            _levelsMenuView.BackButtonClicked += OnLevelsMenuBackClick;
            _settingsMenuView.BackButtonClicked -= OnSettingsMenuBackButtonClick;
        }

        private void EnableMainMenuView()
        {
            DisableAllViews();
            _mainMenuView.Enable();
        }

        private void DisableAllViews()
        {
            _mainMenuView.Disable();
            _settingsMenuView.Disable();
            _levelsMenuView.Disable();
        }

        private void OnSettingsButtonClick() =>
            EnableSettingsView();

        private void EnableSettingsView()
        {
            DisableAllViews();
            _settingsMenuView.Enable();
        }

        private void OnLevelsButtonClick() =>
            EnableLevelsView();

        private void EnableLevelsView()
        {
            DisableAllViews();
            _levelsMenuView.Enable();
        }

        private void OnLevelsMenuBackClick() =>
            EnableMainMenuView();

        private void OnSettingsMenuBackButtonClick() =>
            EnableMainMenuView();
    }
}
