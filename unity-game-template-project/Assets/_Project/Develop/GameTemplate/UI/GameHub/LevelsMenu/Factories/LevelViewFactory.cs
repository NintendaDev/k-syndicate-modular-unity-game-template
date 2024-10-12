using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.LanguageSystem.Processors;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.UI.GameHub.LevelsMenu.Presenters;
using GameTemplate.UI.GameHub.LevelsMenu.Views;
using System;
using System.Collections.Generic;
using GameTemplate.Core;
using Modules.AssetManagement.StaticData;
using Zenject;

namespace GameTemplate.UI.GameHub.LevelsMenu.Factories
{
    public class LevelViewFactory : PrefabFactoryAsync<LevelView>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IEventBus _eventBus;
        private readonly LocalizedTermProcessorLinker _localizedTermProcessorLinker;
        private readonly DictionaryDatabase<LevelView, Action> _destroyCallbacks = new();
        private GameHubConfiguration _gameHubConfiguration;
        private List<IDisposable> _disposableObjects = new();

        public LevelViewFactory(IInstantiator instantiator, IComponentAssetService componentAssetService,
            IStaticDataService staticDataService, IEventBus eventBus,
            LocalizedTermProcessorLinker localizedTermProcessorLinker) 
            : base(instantiator, componentAssetService)
        {
            _staticDataService = staticDataService;
            _eventBus = eventBus;
            _localizedTermProcessorLinker = localizedTermProcessorLinker;
        }

        public override void Dispose()
        {
            base.Dispose();

            _disposableObjects.ForEach(x => x.Dispose());
        }

        public async UniTask<LevelView> CreateAsync(LevelConfiguration levelConfiguration)
        {
            if (_gameHubConfiguration == null)
                _gameHubConfiguration = _staticDataService.GetConfiguration<GameHubConfiguration>();

            LevelView levelView = await CreateAsync(_gameHubConfiguration.LevelViewPrebafReference.AssetGUID);
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