using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.LoadingCurtain
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progressLabel;
        [SerializeField] private Image _fill;
        
        public void Show() => gameObject.SetActive(true);
        
        public void Hide() => gameObject.SetActive(false);

        public void SetProgress(float progress)
        {
            _fill.fillAmount = progress;
            _progressLabel.text = $"{progress * 100:F0}%";
        }
    }
}