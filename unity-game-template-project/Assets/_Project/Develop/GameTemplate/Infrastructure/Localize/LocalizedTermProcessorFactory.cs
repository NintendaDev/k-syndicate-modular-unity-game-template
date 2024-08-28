using GameTemplate.Infrastructure.Language.Processors;
using GameTemplate.Services.Localization;
using System;
using System.Collections.Generic;

namespace GameTemplate.Infrastructure.Language
{
    public class LocalizedTermProcessorFactory : IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private List<IDisposable> _disposableObjects = new();

        public LocalizedTermProcessorFactory(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Dispose() =>
            _disposableObjects.ForEach(x => x.Dispose());

        public LocalizedTermProcessor Create(string term, Action<string> onChangeLocalizationCallback)
        {
            LocalizedTermProcessor termProcessor = new(_localizationService);
            termProcessor.Initialize(term, onChangeLocalizationCallback);
            _disposableObjects.Add(termProcessor);

            return termProcessor;
        }
    }
}
