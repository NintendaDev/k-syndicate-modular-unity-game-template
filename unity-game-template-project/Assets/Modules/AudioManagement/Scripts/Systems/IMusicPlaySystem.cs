using Cysharp.Threading.Tasks;
using Modules.AudioManagement.Clip;
using UnityEngine;

namespace Modules.AudioManagement.Systems
{
    public interface IMusicPlaySystem : IMusicPlay
    {
        public UniTask InitializeAsync(IAudioClip audioClip);
        
        public UniTask InitializeAsync();
        
        public void Set(AudioClip clip);

        public void Set(IAudioClip clip);
    }
}
