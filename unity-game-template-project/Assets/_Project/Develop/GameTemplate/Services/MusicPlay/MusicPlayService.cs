using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.Music;
using UnityEngine;

namespace GameTemplate.Services.MusicPlay
{
    public class MusicPlayService : IMusicPlayService
    {
        private readonly MusicPlayerFactory _musicPlayerFactory;
        private MusicPlayer _musicPlayer;
        
        public MusicPlayService(MusicPlayerFactory musicPlayerFactory)
        {
            _musicPlayerFactory = musicPlayerFactory;
        }

        public bool IsPlaying => _musicPlayer.IsPlaying;

        public bool IsPausing => _musicPlayer.IsPausing;

        public async UniTask InitializeAsync(IAudioClip audioClip)
        {
            await InitializeAsync();
            Set(audioClip);
        }

        public async UniTask InitializeAsync()
        {
            if (_musicPlayer == null)
                _musicPlayer = await _musicPlayerFactory.CreateAsync();
        }

        public void Set(AudioClip clip) =>
            _musicPlayer.Set(clip);

        public void Set(IAudioClip clip) =>
            _musicPlayer.Set(clip);

        public void PlayOrUnpause()
        {
            if (_musicPlayer.IsPlaying)
                return;

            if (_musicPlayer.IsPausing)
                _musicPlayer.Unpause();
            else
                _musicPlayer.Play();
        }

        public void Play() =>
            _musicPlayer.Play();

        public void Stop() =>
            _musicPlayer.Stop();

        public void Pause() =>
            _musicPlayer.Pause();

        public void Unpause() =>
            _musicPlayer.Unpause();
    }
}
