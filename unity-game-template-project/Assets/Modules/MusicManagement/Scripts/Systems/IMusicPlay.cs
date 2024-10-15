namespace Modules.MusicManagement.Systems
{
    public interface IMusicPlay
    {
        public bool IsPlaying { get; }

        public bool IsPausing { get; }

        public void PlayOrUnpause();

        public void Play();

        public void Stop();

        public void Pause();
    }
}
