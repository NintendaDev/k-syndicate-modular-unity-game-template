using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using GameTemplate.Infrastructure.Configurations;
using GameTemplate.Infrastructure.Language.Processors;
using GameTemplate.Infrastructure.Levels.Configurations;
using GameTemplate.Infrastructure.Signals;
using GameTemplate.Services.StaticData;
using GameTemplate.UI.GameHub.LevelsMenu.Presenters;
using GameTemplate.UI.GameHub.LevelsMenu.Views;
using System;
using System.Collections.Generic;
using Zenject;

namespace GameTemplate.UI.GameHub.LevelsMenu.Factories
{
    public class LevelViewFactory : PrefabFactoryAsync<LevelView>
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IEventBus _eventBus;
        private readonly LocalizedTermProcessorLinker _LocalizedTermProcessorLinker;
        private GameHubConfiguration _gameHubConfiguration;
        private List<IDisposable> _disposableObjects = new();

        public LevelViewFactory(IInstantiator instantiator, IComponentAssetProvider componentAssetProvider,
            IStaticDataService staticDataService, IEventBus eventBus,
            LocalizedTermProcessorLinker LocalizedTermProcessorLinker) 
            : base(instantiator, componentAssetProvider)
        {
            _staticDataService = staticDataService;
            _eventBus = eventBus;
            _LocalizedTermProcessorLinker = LocalizedTermProcessorLinker;
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

            _LocalizedTermProcessorLinker.Link(levelConfiguration.Title, levelView.SetTitle);
            levelView.Set(levelConfiguration.LevelCode);

            return levelView;
        }
    }
}