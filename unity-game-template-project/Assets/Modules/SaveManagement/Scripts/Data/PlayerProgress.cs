namespace Modules.SaveManagement.Data
{
    [System.Serializable]
    public abstract class PlayerProgress : IPlayerProgress
    {
        public abstract bool TryGetProgressData<TData>(out TData data);

        public abstract void SetProgressData<TData>(TData data);
    }
}