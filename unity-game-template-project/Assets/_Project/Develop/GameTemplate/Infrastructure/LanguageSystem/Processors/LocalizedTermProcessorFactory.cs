using GameTemplate.Services.Localization;
using System;
using System.Collections.Generic;

namespace GameTemplate.Infrastructure.LanguageSystem.Processors
{
public class LocalizedTermProcessorFactory : IDisposable
    {
        private readonly ILocalizationService _localizationService;
        private readonly List<IDisposable> _disposableObjects = new();
        private readonly Dictionary<Type, LocalizedTermProcessor> _createdProcessors = new();

        public LocalizedTermProcessorFactory(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void Dispose() =>
            _disposableObjects.ForEach(x => x.Dispose());

        public void DestroyProcessor(Action<string> onChangeLocalizationCallback)
        {
            Type callbackType = onChangeLocalizationCallback.GetType();

            if (_createdProcessors.TryGetValue(callbackType, out LocalizedTermProcessor termProcessor) == false)
                return;
            
            termProcessor.Dispose();

            if (_disposableObjects.Contains(termProcessor))
                _disposableObjects.Remove(termProcessor);
        }

        public LocalizedTermProcessor Create(string term, Action<string> onChangeLocalizationCallback)
        {
            Type callbackType = onChangeLocalizationCallback.GetType();
            
            LocalizedTermProcessor termProcessor;

            if (_createdProcessors.TryGetValue(callbackType, out termProcessor) == false)
            {
                termProcessor = new(_localizationService);
                _createdProcessors.Add(callbackType, termProcessor);
                _disposableObjects.Add(termProcessor);
            }
            
            termProcessor.Initialize(term, onChangeLocalizationCallback);
            
            return termProcessor;
        }
    }
}
