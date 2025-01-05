namespace Modules.LoadingCurtain
{
    public interface ILoadingCurtain
    {
        public void ShowWithProgressBar();

        public void ShowWithoutProgressBar();

        public void Hide();

        public void EnableProgressBar();
        
        public void DisableProgressBar();
        
        public void SetProgress(float progress);
    }
}