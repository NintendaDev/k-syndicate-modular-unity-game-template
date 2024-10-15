using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.MusicManagement.Clip
{
    public interface IReferenceAudio
    {
        public AssetReferenceT<AudioClip> AudioReference { get; }
    }
}
