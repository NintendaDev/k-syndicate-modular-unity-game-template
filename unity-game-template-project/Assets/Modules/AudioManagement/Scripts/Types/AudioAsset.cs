using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AudioManagement.Types
{
    [Serializable]
    public struct AudioAsset
    {
        [SerializeField, Required] private AssetReferenceSoundEvent _reference;
        [SerializeField] private AudioCode _code;

        public AudioAsset(AudioCode code, AssetReferenceSoundEvent reference)
        {
            _code = code;
            _reference = reference;
        }
        
        public AssetReferenceSoundEvent Reference => _reference;
        
        public AudioCode Code => _code;
    }
}