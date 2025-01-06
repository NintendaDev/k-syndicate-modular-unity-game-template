// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundDataGroupBase : ScriptableObject {

        public SoundDataGroupInternals internals = new SoundDataGroupInternals();

        public string assetGuid;
        public long assetGuidHash;

        /// <summary>
        /// The ID of an asset is cached in the editor because in some cases when using adressables a single scriptable object might be instantiated multiple times.
        /// This might lead to problems where a simple "Object == Object" check will return false, but a "ID == ID" check will return true.
        /// If you are upgrading Sonity and use adressable assets you probably want to select all Sonity scriptable objects and run:
        /// "Tools/Sonity 🔊/Set Selected Assets as Dirty (Force Reserialize for Resave)" which will force the assets to cache the GUIDs.
        /// You can search t:SoundEvent, t:SoundContainer, t:SoundTag, t:SoundPolyGroup, t:SoundDataGroup, t:SoundMix, t:SoundPhysicsCondition to find all objects.
        /// If no GUID is cached then it will use the InstanceID in builds.
        /// The free trial version doesn't have full functionality.
        /// </summary>
        public long GetAssetID() {
            if (String.IsNullOrEmpty(assetGuid)) {
#if UNITY_EDITOR && !SONITY_DLL_RUNTIME
                assetGuid = EditorAssetGuid.GetAssetGuid(this);
                assetGuidHash = EditorAssetGuid.GetInt64HashFromString(assetGuid);
                // Force save the new values
                UnityEditor.EditorUtility.SetDirty(this);
                return assetGuidHash;
#else
                return GetInstanceID();
#endif
            }
            return assetGuidHash;
        }
    }
}