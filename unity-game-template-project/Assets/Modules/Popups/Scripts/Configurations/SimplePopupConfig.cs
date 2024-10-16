namespace Modules.PopupsSystem.Configurations
{
    public sealed class SimplePopupConfig
    {
        public SimplePopupConfig(string header, string message, string buttonText) 
        {
            Header = header;
            Message = message;
            ButtonText = buttonText;
        }

        public string Header { get; }

        public string Message { get; }

        public string ButtonText { get; }
    }
}