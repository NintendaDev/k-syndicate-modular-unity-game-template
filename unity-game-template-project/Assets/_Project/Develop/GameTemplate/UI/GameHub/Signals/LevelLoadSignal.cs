using GameTemplate.Infrastructure.Levels;
using Modules.EventBus;

namespace GameTemplate.UI.GameHub.Signals
{
    public class LevelLoadSignal : IPayloadSignal
    {
        public LevelLoadSignal(LevelCode levelCode) =>
            LevelCode = levelCode;

        public LevelCode LevelCode { get; }
    }
}
