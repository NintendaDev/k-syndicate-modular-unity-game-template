using Modules.SaveManagement.Data;
using Modules.SaveManagement.Interfaces;

namespace Modules.SaveManagement.Persistent
{
    public class PersistentProgressProvider : IPersistentProgressProvider
    {
        public PlayerProgress Progress { get; set; }
    }
}