using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.Progress
{
    public interface IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}