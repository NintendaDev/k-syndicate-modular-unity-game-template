using UnityEngine;

namespace GameTemplate.Infrastructure.Music
{
    public interface IAudioClip
    {
        public AudioClip Clip { get; }
    }
}