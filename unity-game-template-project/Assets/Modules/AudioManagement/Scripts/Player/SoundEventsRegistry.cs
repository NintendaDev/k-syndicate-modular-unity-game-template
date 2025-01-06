using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.AssetsManagement.AddressablesOperations;
using Modules.AssetsManagement.StaticData;
using Modules.AudioManagement.Configurations;
using Modules.AudioManagement.Types;
using Sonity;

namespace Modules.AudioManagement.Player
{
    public sealed class SoundEventsRegistry : IDisposable
    {
        private readonly IAddressablesService _addressablesService;
        private readonly IStaticDataService _staticDataService;
        private readonly Dictionary<AudioCode, SoundEvent> _loadedSoundEvents = new();
        private readonly Dictionary<AudioCode, AssetReferenceSoundEvent> _loadedSoundReferences = new();
        private SoundConfiguration _soundConfiguration;
        private AudioCode _loadLockAudioCode;

        public SoundEventsRegistry(IAddressablesService addressablesService, IStaticDataService staticDataService)
        {
            _addressablesService = addressablesService;
            _staticDataService = staticDataService;
        }

        public void Dispose()
        {
            ReleaseSounds();
        }

        public void Initialize()
        {
            _soundConfiguration = _staticDataService.GetConfiguration<SoundConfiguration>();
        }
        
        public bool IsExistSoundEvent(AudioCode soundCode, out SoundEvent soundEvent) =>
            _loadedSoundEvents.TryGetValue(soundCode, out soundEvent);

        public async UniTask<bool> TryLoadAudioAsync(AudioCode audioCode)
        {
            if (_loadedSoundReferences.ContainsKey(audioCode))
                return true;
            
            if (_loadLockAudioCode == audioCode)
                return false;

            if (_soundConfiguration.IsExistAudioAsset(audioCode, out AudioAsset audioAsset) == false)
                return false;
            
            _loadLockAudioCode = audioCode;
            
            SoundEvent soundEvent = await _addressablesService
                .LoadAsync<SoundEvent>(audioAsset.Reference);
                
            _loadedSoundEvents.Add(audioAsset.Code, soundEvent);
            
            if (_loadedSoundReferences.ContainsKey(audioCode))
                _loadedSoundReferences[audioAsset.Code] = audioAsset.Reference;
            else
                _loadedSoundReferences.Add(audioAsset.Code, audioAsset.Reference);

            _loadLockAudioCode = AudioCode.None;

            return true;
        }

        public void ReleaseSounds()
        {
            foreach (AssetReferenceSoundEvent soundReference in _loadedSoundReferences.Values)
                _addressablesService.Release(soundReference);
            
            _loadedSoundReferences.Clear();
            _loadedSoundEvents.Clear();
        }
    }
}