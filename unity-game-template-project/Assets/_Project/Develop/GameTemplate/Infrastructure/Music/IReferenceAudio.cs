using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameTemplate.Infrastructure.Music
{
    public interface IReferenceAudio
    {
        public AssetReferenceT<AudioClip> AudioReference { get; }
    }
}
