using Game.UI.Gameplay.PauseMenu.Views;
using Game.UI.Gameplay.PlayMenu.Views;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.UI.Gameplay
{
    public sealed class GameplayMenuView : MonoBehaviour
    {
        [SerializeField, Required] private PlayMenuView _playMenu;
        [SerializeField, Required] private PauseMenuView _pauseMenu;

        private void OnEnable()
        {
            _playMenu.PauseButtonClicked += OnPauseButtonClick;
            _pauseMenu.BackButtonClicked += OnPauseMenuBackButtonClick;
        }

        private void OnDisable()
        {
            _playMenu.PauseButtonClicked -= OnPauseButtonClick;
            _pauseMenu.BackButtonClicked -= OnPauseMenuBackButtonClick;
        }

        private void Start()
        {
            EnablePlayMenu();
        }

        private void OnPauseButtonClick() =>
            EnablePauseMenu();

        private void EnablePauseMenu()
        {
            DisableAllViews();
            _pauseMenu.Enable();
        }

        private void DisableAllViews()
        {
            _playMenu.Disable();
            _pauseMenu.Disable();
        }

        private void OnPauseMenuBackButtonClick() =>
            EnablePlayMenu();

        private void EnablePlayMenu()
        {
            DisableAllViews();
            _playMenu.Enable();
        }
    }
}
