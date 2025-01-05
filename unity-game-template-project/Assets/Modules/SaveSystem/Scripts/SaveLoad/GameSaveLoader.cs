using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.SaveSystem.Repositories;

namespace Modules.SaveSystem.SaveLoad
{
    public sealed class GameSaveLoader : IGameSaveLoader
    {
        private readonly IGameRepository _repository;
        private readonly IEnumerable<IGameSerializer> _serializers;

        public GameSaveLoader(IGameRepository repository, IEnumerable<IGameSerializer> serializers)
        {
            _repository = repository;
            _serializers = serializers;
        }

        public async UniTask SaveAsync()
        {
            var gameState = new Dictionary<string, string>();
            
            foreach (IGameSerializer serializer in _serializers)
                serializer.Serialize(gameState);

            await _repository.SetStateAsync(gameState);
        }

        public async UniTask<bool> TryLoadAsync()
        {
            Dictionary<string, string> gameState = await _repository.GetStateAsync();

            if (gameState == null || gameState.Count == 0)
                return false;
            
            foreach (IGameSerializer serializer in _serializers)
                serializer.Deserialize(gameState);
            
            return true;
        }
    }
}