using Game.Infrastructure.Levels;
using Modules.EventBus;

namespace Game.UI.GameHub.Signals
{
    public sealed class LevelLoadSignal : IPayloadSignal
    {
        public LevelLoadSignal(LevelCode levelCode) =>
            LevelCode = levelCode;

        public LevelCode LevelCode { get; }
    }
}
