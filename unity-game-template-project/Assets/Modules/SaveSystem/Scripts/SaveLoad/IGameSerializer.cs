using System.Collections.Generic;

namespace Modules.SaveSystem.SaveLoad
{
    public interface IGameSerializer
    {
        public void Serialize(IDictionary<string, string> saveState);
        
        public void Deserialize(IDictionary<string, string> loadState);
    }
}