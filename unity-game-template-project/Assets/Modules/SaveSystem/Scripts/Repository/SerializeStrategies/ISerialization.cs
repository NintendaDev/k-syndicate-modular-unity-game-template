using System.Collections.Generic;

namespace Modules.SaveSystem.Repositories.SerializeStrategies
{
    public interface ISerialization
    {
        public string Serialize(Dictionary<string, string> data);
        
        public Dictionary<string, string> Deserialize(string json);
    }
}