namespace Modules.Logging
{
    public interface ILogSystem
    {
        public void Log(string message);

        public void LogError(string message);

        public void LogWarning(string message);
    }
}