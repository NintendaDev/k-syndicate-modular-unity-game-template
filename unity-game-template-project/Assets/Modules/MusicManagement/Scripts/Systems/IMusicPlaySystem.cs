using Cysharp.Threading.Tasks;
using Modules.MusicManagement.Clip;
using UnityEngine;

namespace Modules.MusicManagement.Systems
{
    public interface IMusicPlaySystem : IMusicPlay
    {
        public UniTask InitializeAsync();

        public void Set(AudioClip clip);

        public void Set(IAudioClip clip);
    }
}
