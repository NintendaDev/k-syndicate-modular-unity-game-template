namespace Modules.SaveManagement.Data
{
    public interface IPlayerProgress
    {
        public bool TryGetProgressData<TData>(out TData data);
        
        public void SetProgressData<TData>(TData data);
    }
}