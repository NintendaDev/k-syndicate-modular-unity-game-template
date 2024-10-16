using UnityEngine;

namespace Modules.Logging
{
    public sealed class LogSystem : ILogSystem
    {
        public void Log(string message) => 
            Debug.Log(message);
        
        public void LogError(string message) => 
            Debug.LogError(message);

        public void LogWarning(string message) => 
            Debug.LogWarning(message);
    }
}