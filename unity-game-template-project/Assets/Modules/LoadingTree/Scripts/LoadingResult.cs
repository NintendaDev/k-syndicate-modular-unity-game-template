namespace Modules.LoadingTree
{
    public struct LoadingResult
    {
        public LoadingResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }

        public string Message { get; }
        
        public static LoadingResult Success() => new(true, string.Empty);
        
        public static LoadingResult Error(string error) => new(false, string.Empty);
    }
}