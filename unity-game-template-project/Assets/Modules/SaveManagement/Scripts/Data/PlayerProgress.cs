namespace Modules.SaveManagement.Data
{
    [System.Serializable]
    public abstract class PlayerProgress : IPlayerProgress
    {
        public abstract TData GetProgressData<TData>();

        public abstract void SetProgressData<TData>(TData data);
    }
}