using Modules.AudioManagement.Mixer;
using Modules.SaveSystem.SaveLoad;

namespace Game.Scripts.Application.SaveLoad.Serializers
{
    public sealed class AudioMixerSerializer : GameSerializer<AudioMixerSystem, AudioMixerData>
    {
        public AudioMixerSerializer(AudioMixerSystem service) : base(service)
        {
        }

        protected override AudioMixerData Serialize(AudioMixerSystem service) => 
            new AudioMixerData(service.MusicPercentVolume, service.EffectsPercentVolume);

        protected override void Deserialize(AudioMixerSystem audioMixerSystem, AudioMixerData data)
        {
            audioMixerSystem.Reset();
            audioMixerSystem.SetMusicPercentVolume(data.MusicPercentVolume);
            audioMixerSystem.SetEffectsPercentVolume(data.EffectsPercentVolume);
        }
    }
}