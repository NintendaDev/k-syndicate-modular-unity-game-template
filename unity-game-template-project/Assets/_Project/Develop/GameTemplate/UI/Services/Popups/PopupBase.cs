using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameTemplate.UI.Services.Popups
{
    public abstract class PopupBase<TResult> : MonoBehaviour
    {
        [SerializeField, Required] private Canvas _popupCanvas;

        private UniTaskCompletionSource<TResult> _taskCompletionSource;

        private void Awake() => 
            OnAwake();

        private void OnDestroy() =>
            Unsubscribe();

        public virtual async UniTask<TResult> Show()
        {
            _taskCompletionSource = new UniTaskCompletionSource<TResult>();
            Subscribe();
            _popupCanvas.enabled = true;

            return await _taskCompletionSource.Task;
        }

        public void Hide() => _popupCanvas.enabled = false;

        public void Destroy() => Destroy(gameObject);
        
        protected void SetPopupResult(TResult result) =>
            _taskCompletionSource.TrySetResult(result);

        protected virtual void OnAwake() => Hide();

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();
    }
}