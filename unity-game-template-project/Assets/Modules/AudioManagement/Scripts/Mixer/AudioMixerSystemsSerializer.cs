using Modules.SaveSystem.SaveLoad;

namespace Modules.AudioManagement.Mixer
{
    public sealed class AudioMixerSystemsSerializer : GameSerializer<AudioMixerSystem, AudioMixerData>
    {
        public AudioMixerSystemsSerializer(AudioMixerSystem service) : base(service)
        {
        }

        protected override AudioMixerData Serialize(AudioMixerSystem service) =>
            new AudioMixerData(service.MusicPercentVolume, service.EffectsPercentVolume);

        protected override void Deserialize(AudioMixerSystem service, AudioMixerData data)
        {
            service.Reset();
            service.SetMusicPercentVolume(data.MusicPercentVolume);
            service.SetEffectsPercentVolume(data.EffectsPercentVolume);
        }
    }
}