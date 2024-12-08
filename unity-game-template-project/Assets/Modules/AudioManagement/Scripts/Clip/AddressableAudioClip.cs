using Cysharp.Threading.Tasks;
using System;
using Modules.AssetsManagement.AddressablesOperations;
using UnityEngine;

namespace Modules.AudioManagement.Clip
{
    public sealed class AddressableAudioClip : IDisposable, IAudioClip
    {
        private readonly IAddressablesService _addressablesService;
        private IReferenceAudio _audioReferencer;
        private bool _isInitialized;

        public AddressableAudioClip(IAddressablesService addressablesService)
        {
            _addressablesService = addressablesService;
        }

        public AudioClip Clip { get; private set; }
        
        public void Dispose()
        {
            if (_isInitialized == false)
                return;

            _addressablesService.Release(_audioReferencer.AudioReference);
            _isInitialized = false;
        }

        public async UniTask<bool> TryInitializeAsync(IReferenceAudio audioReferencer)
        {
            if (audioReferencer.AudioReference.RuntimeKeyIsValid() == false)
                return false;

            _audioReferencer = audioReferencer;
            Clip = await _addressablesService.LoadAsync<AudioClip>(_audioReferencer.AudioReference);
            _isInitialized = true;

            return true;
        }
    }
}
