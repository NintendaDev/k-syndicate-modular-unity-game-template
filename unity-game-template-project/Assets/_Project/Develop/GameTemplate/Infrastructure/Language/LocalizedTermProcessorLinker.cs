using GameTemplate.UI.Core;
using GameTemplate.UI.Core.Buttons;
using System;
using TMPro;

namespace GameTemplate.Infrastructure.LanguageSystem
{
    public class LocalizedTermProcessorLinker
    {
        private readonly LocalizedTermProcessorFactory _localizedTermProcessorFactory;

        public LocalizedTermProcessorLinker(LocalizedTermProcessorFactory localizedTermProcessorFactory) =>
            _localizedTermProcessorFactory = localizedTermProcessorFactory;

        public void Link(TMP_Text label) =>
            Link(label.text, (translation) => label.text = translation);

        public void Link(UIText uiText) =>
            Link(uiText.Text, (translation) => uiText.SetText(translation));

        public void Link(UITextButton textButton) =>
            Link(textButton.Title, (translation) => textButton.SetTitle(translation));

        public void Link(string term, Action<string> onLocalizationChangeCallback) =>
            _localizedTermProcessorFactory.Create(term, onLocalizationChangeCallback);
    }
}
