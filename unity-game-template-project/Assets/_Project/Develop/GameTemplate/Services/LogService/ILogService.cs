namespace GameTemplate.Services.Log
{
    public interface ILogService
    {
        public void Log(string message);

        public void LogError(string message);

        public void LogWarning(string message);
    }
}