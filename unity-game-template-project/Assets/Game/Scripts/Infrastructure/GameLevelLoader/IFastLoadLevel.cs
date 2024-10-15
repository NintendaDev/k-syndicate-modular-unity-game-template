using Cysharp.Threading.Tasks;

namespace GameTemplate.Services.GameLevelLoader
{
    public interface IFastLoadLevel
    {
        public UniTask FastLoadLevelAsync();
    }
}