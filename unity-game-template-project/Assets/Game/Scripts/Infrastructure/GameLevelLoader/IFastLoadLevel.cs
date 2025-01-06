using Cysharp.Threading.Tasks;

namespace Game.Services.GameLevelLoader
{
    public interface IFastLoadLevel
    {
        public UniTask FastLoadLevelAsync();
    }
}