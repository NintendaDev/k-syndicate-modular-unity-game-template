namespace Modules.AudioManagement.Mixer
{
    public interface IAudioMixerSystem
    {
        public float EffectsPercentVolume { get; }

        public float MusicPercentVolume { get; }

        public bool IsChanged { get; }

        public void Initialize();

        public void SetEffectsVolume(float percent);

        public void SetMusicVolume(float percent);

        public void Mute();

        public void Unmute();
    }
}