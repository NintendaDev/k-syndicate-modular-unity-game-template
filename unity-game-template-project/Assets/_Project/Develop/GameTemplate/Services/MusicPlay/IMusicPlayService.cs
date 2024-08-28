using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Music;
using UnityEngine;

namespace GameTemplate.Services.MusicPlay
{
    public interface IMusicPlayService : IMusicPlay
    {
        public UniTask InitializeAsync();

        public void Set(AudioClip clip);

        public void Set(IAudioClip clip);
    }
}
