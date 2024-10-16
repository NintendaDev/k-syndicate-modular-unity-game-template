using Cysharp.Threading.Tasks;
using Modules.Core.UI;
using Modules.Localization.Systems;
using Modules.PopupsSystem.Configurations;
using UnityEngine;
using Zenject;

namespace Modules.PopupsSystem.UI.Simple
{
    public sealed class SimplePopup : PopupBase<bool>
    {
        [SerializeField] private UIText _headerLabel;
        [SerializeField] private UIText _messageLabel;
        [SerializeField] private UITextButton _button;
        
        private ITranslation _translator;
        private bool _isInitialized;

        [Inject]
        public void Construct(ITranslation translator) =>
            _translator = translator;
        
        public void Initialize(SimplePopupConfig popupConfig)
        {
            _headerLabel.SetText(_translator.MakeTranslatedTextByTerm(popupConfig.Header));
            _messageLabel.SetText(_translator.MakeTranslatedTextByTerm(popupConfig.Message));
            _button.SetTitle(_translator.MakeTranslatedTextByTerm(popupConfig.ButtonText));

            _isInitialized = true;
        }

        public override async UniTask<bool> Show()
        {
            if (_isInitialized == false)
                throw new System.Exception($"{nameof(SimplePopup)} is not initialized");

            return await base.Show();
        }

        protected override void Subscribe() =>
            _button.Clicked += OnClick;

        private void OnClick() => 
            SetPopupResult(true);

        protected override void Unsubscribe() =>
            _button.Clicked -= OnClick;
    }
}