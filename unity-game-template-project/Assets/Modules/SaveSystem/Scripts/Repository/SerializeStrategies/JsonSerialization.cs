using System.Collections.Generic;
using Newtonsoft.Json;

namespace Modules.SaveSystem.Repositories.SerializeStrategies
{
    public sealed class JsonSerialization : ISerialization
    {
        public string Serialize(Dictionary<string, string> data) => JsonConvert.SerializeObject(data);

        public Dictionary<string, string> Deserialize(string json) =>
            JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }
}