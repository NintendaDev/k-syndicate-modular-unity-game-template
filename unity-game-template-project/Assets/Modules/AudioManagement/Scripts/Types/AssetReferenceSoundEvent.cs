using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Modules.AudioManagement.Types
{
    [Serializable]
    public sealed class AssetReferenceSoundEvent : AssetReference
    {
        public AssetReferenceSoundEvent(string guid) : base(guid)
        {
        }

        public override bool ValidateAsset(Object obj)
        {
            return obj is Sonity.SoundEvent;
        }
        
        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            
            return asset is Sonity.SoundEvent;
#else
        return false;
#endif
        }
    }
}