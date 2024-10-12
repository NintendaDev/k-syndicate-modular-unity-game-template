namespace Modules.SaveManagement.Data
{
    public interface IPlayerProgress
    {
        public TData GetProgressData<TData>();
        
        public void SetProgressData<TData>(TData data);
    }
}