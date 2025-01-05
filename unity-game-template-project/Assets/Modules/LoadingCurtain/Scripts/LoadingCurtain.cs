using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.LoadingCurtain
{
    public sealed class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField, Required] private CanvasGroup _canvasGroup;
        [SerializeField, MinValue(0), Unit(Units.Second)] private float _fadeTime = 1f;
        [SerializeField, Required] private ProgressBar _progressBar;

        private Coroutine _fadeInCoroutine;

        public void ShowWithoutProgressBar()
        {
            Show();
            DisableProgressBar();
        }

        public void ShowWithProgressBar()
        {
            Show();
            EnableProgressBar();
        }

        [Button, DisableInEditorMode]
        public void Hide()
        {
            if (gameObject.activeSelf == false)
                return;

            if (_fadeInCoroutine != null)
                StopCoroutine(_fadeInCoroutine);

            _fadeInCoroutine = StartCoroutine(FadeInCoroutine());
        }

        [Button, DisableInEditorMode]
        public void SetProgress(float progress) => _progressBar.SetProgress(progress);

        [Button, DisableInEditorMode]
        public void EnableProgressBar() => _progressBar.Show();

        [Button, DisableInEditorMode]
        public void DisableProgressBar() => _progressBar.Hide();

        private void Show()
        {
            if (_fadeInCoroutine != null)
                StopCoroutine(_fadeInCoroutine);

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
        }

        private IEnumerator FadeInCoroutine()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.unscaledDeltaTime / _fadeTime;

                yield return null;
            }
      
            gameObject.SetActive(false);
        }
    }
}