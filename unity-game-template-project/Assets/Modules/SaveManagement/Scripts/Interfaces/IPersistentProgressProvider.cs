using Modules.SaveManagement.Data;

namespace Modules.SaveManagement.Interfaces
{
    public interface IPersistentProgressProvider
    {
        public PlayerProgress Progress { get; set; }
    }
}