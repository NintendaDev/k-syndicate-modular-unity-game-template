using System;
using Modules.Core.UI;
using Modules.Localization.Processors.Factories;

namespace Modules.Localization.Processors
{
    public sealed class LocalizedTermProcessorLinker
    {
        private readonly LocalizedTermProcessorFactory _localizedTermProcessorFactory;

        public LocalizedTermProcessorLinker(LocalizedTermProcessorFactory localizedTermProcessorFactory) =>
            _localizedTermProcessorFactory = localizedTermProcessorFactory;

        public void Link(UIText text) =>
            Link(text.Text, text.SetText);
        
        public void Unlink(UIText uiText) =>
            Unlink(uiText.SetText);

        public void Link(UITextButton textButton) =>
            Link(textButton.Title, textButton.SetTitle);

        public void Unlink(UITextButton textButton) =>
            Unlink(textButton.SetTitle);

        public void Link(string term, Action<string> onLocalizationChangeCallback) =>
            _localizedTermProcessorFactory.Create(term, onLocalizationChangeCallback);

        public void Unlink(Action<string> onLocalizationChangeCallback) =>
            _localizedTermProcessorFactory.DestroyProcessor(onLocalizationChangeCallback);
    }
}
