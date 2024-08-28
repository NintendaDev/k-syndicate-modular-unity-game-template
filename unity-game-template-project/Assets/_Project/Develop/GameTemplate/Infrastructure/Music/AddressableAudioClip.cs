using Cysharp.Threading.Tasks;
using GameTemplate.Infrastructure.AssetManagement;
using System;
using UnityEngine;

namespace GameTemplate.Infrastructure.Music
{
    public class AddressableAudioClip : IDisposable, IAudioClip
    {
        private readonly IAssetProvider _assetProvider;

        public AddressableAudioClip(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        private IReferenceAudio _audioReferencer;

        public AudioClip Clip { get; private set; }

        private bool _isInitialized;

        public void Dispose()
        {
            if (_isInitialized == false)
                return;

            _assetProvider.Release(_audioReferencer.AudioReference);
            _isInitialized = false;
        }

        public async UniTask<bool> TryInitializeAsync(IReferenceAudio audioReferencer)
        {
            if (audioReferencer.AudioReference.RuntimeKeyIsValid() == false)
                return false;

            _audioReferencer = audioReferencer;
            Clip = await _assetProvider.LoadAsync(_audioReferencer.AudioReference);
            _isInitialized = true;

            return true;
        }
    }
}
