using GameTemplate.Infrastructure.Data;

namespace GameTemplate.Services.Progress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}