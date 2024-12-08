using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.UI.GameHub.LevelsMenu.Presenters;
using GameTemplate.UI.GameHub.LevelsMenu.Views;
using System;
using System.Collections.Generic;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.AssetsManagement.StaticData;
using Modules.Core.Systems;
using Modules.EventBus;
using Modules.Localization.Processors;
using Modules.ObjectsManagement.Factories;
using Zenject;

namespace GameTemplate.UI.GameHub.LevelsMenu.Factories
{
    public sealed class LevelViewFactory : IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IEventBus _eventBus;
        private readonly LocalizedTermProcessorLinker _localizedTermProcessorLinker;
        private readonly DictionaryDatabase<LevelView, Action> _destroyCallbacks = new();
        private readonly PrefabFactoryAsync<LevelView> _prefabFactory;
        private GameHubConfiguration _gameHubConfiguration;
        private List<IDisposable> _disposableObjects = new();

        public LevelViewFactory(IInstantiator instantiator, IAddressablesService addressablesService,
            IStaticDataService staticDataService, IEventBus eventBus,
            LocalizedTermProcessorLinker localizedTermProcessorLinker)
        {
            _staticDataService = staticDataService;
            _eventBus = eventBus;
            _prefabFactory = new PrefabFactoryAsync<LevelView>(instantiator, addressablesService);
            _localizedTermProcessorLinker = localizedTermProcessorLinker;
        }

        public void Dispose()
        {
            _disposableObjects.ForEach(x => x.Dispose());
            _prefabFactory.Dispose();
        }

        public async UniTask<LevelView> CreateAsync(LevelConfiguration levelConfiguration)
        {
            if (_gameHubConfiguration == null)
                _gameHubConfiguration = _staticDataService.GetConfiguration<GameHubConfiguration>();

            LevelView levelView = await _prefabFactory.CreateAsync(_gameHubConfiguration.LevelViewPrebafReference);
            var presenter = new LevelPresenter(levelView, _eventBus);
            _disposableObjects.Add(presenter);

            Action destroyCallback = () => OnLevelViewDestroy(levelView);
            _destroyCallbacks.Add(levelView, destroyCallback);
            levelView.Destroyed += destroyCallback;

            _localizedTermProcessorLinker.Link(levelConfiguration.Title, levelView.SetTitle);
            levelView.Set(levelConfiguration.LevelCode);

            return levelView;
        }

        private void OnLevelViewDestroy(LevelView levelView)
        {
            if (_destroyCallbacks.TryPopValue(levelView, out Action destroyCallback))
                levelView.Destroyed -= destroyCallback;
            
            _localizedTermProcessorLinker.Unlink(levelView.SetTitle);
        }
    }
}