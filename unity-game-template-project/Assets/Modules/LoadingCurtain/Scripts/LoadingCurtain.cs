using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.LoadingCurtain
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField, Required] private CanvasGroup _canvasGroup;
        [SerializeField, MinValue(0), Unit(Units.Second)] private float _fadeTime = 1f;

        private Coroutine _fadeInCoroutine;

        [Button, DisableInEditorMode]
        public void Show()
        {
            if (_fadeInCoroutine != null)
                StopCoroutine(_fadeInCoroutine);

            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
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