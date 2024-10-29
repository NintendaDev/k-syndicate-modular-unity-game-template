using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.AudioManagement.Clip
{
    public interface IReferenceAudio
    {
        public AssetReferenceT<AudioClip> AudioReference { get; }
    }
}
