using System.Text;

namespace Modules.Logging
{
    public sealed class TemplateLogger : ILogSystem
    {
        private readonly ILogSystem _logSystem;
        private readonly StringBuilder _stringBuilder = new();
        private string _prefix = string.Empty;

        public TemplateLogger(ILogSystem logSystem)
        {
            _logSystem = logSystem;
        }

        public void SetPrefix(string prefix) => _prefix = prefix;
        
        public void Log(string message) => _logSystem.Log(BuildMessage(message));

        public void LogError(string message) => _logSystem.LogError(BuildMessage(message));

        public void LogWarning(string message) => _logSystem.LogError(BuildMessage(message));

        private string BuildMessage(string message)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(_prefix);
            _stringBuilder.Append(message);
            
            return _stringBuilder.ToString();
        }
    }
}