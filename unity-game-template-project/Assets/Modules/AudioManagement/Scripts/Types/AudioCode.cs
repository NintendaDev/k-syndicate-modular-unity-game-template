namespace Modules.AudioManagement.Types
{
    public enum AudioCode
    {
        None,
        GameHubMusic,
        LevelMusic,
        ButtonClick,
        ButtonHighlight
    }

    public static class AudioCodeExtensions
    {
        public static string ConvertToSonityName(this AudioCode audioCode) => audioCode.ToString();
    }
}