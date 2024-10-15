using Modules.MusicManagement.Clip;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.MusicManagement.Player
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private AudioSource _audioSource;

        private int _pauseTimeSamples;

        public bool IsPlaying => _audioSource.isPlaying;

        public bool IsPausing { get; private set; }

        public void Set(IAudioClip audioClip) =>
            Set(audioClip.Clip);

        public void Set(AudioClip audioClip) =>
            _audioSource.clip = audioClip;

        public void Play()
        {
            _audioSource.Play();
            IsPausing = false;
        }

        public void Stop()
        {
            _audioSource.Stop();
            IsPausing = false;
        }

        public void Pause()
        {
            _pauseTimeSamples = _audioSource.timeSamples;

            Stop();
            IsPausing = true;
        }

        public void Unpause()
        {
            _audioSource.Play();
            _audioSource.timeSamples = _pauseTimeSamples;
            IsPausing = false;
        }
    }
}
