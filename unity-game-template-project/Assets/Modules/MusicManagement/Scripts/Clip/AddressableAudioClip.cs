using Cysharp.Threading.Tasks;
using System;
using Modules.AssetsManagement.AddressablesServices;
using UnityEngine;

namespace Modules.MusicManagement.Clip
{
    public class AddressableAudioClip : IDisposable, IAudioClip
    {
        private readonly IAddressablesService _addressablesService;

        public AddressableAudioClip(IAddressablesService addressablesService)
        {
            _addressablesService = addressablesService;
        }

        private IReferenceAudio _audioReferencer;

        public AudioClip Clip { get; private set; }

        private bool _isInitialized;

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
            Clip = await _addressablesService.LoadAsync(_audioReferencer.AudioReference);
            _isInitialized = true;

            return true;
        }
    }
}
