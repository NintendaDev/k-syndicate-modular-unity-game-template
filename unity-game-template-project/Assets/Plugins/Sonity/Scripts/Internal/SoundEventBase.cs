// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundEventBase : ScriptableObject {

        public SoundEventInternals internals = new SoundEventInternals();

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

        /// <summary>
        /// Loads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of this <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        public void LoadAudioData() {
            LoadUnloadAudioData(true);
        }

        /// <summary>
        /// Unloads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of this <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        public void UnloadAudioData() {
            LoadUnloadAudioData(false);
        }

        private void LoadUnloadAudioData(bool load) {
            if (internals.soundContainers.Length == 0) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"{nameof(NameOf.SoundEvent)} " + name + $" has no {nameof(NameOf.SoundContainer)}.", this);
                }
            } else {
                for (int i = 0; i < internals.soundContainers.Length; i++) {
                    if (internals.soundContainers[i] == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"{nameof(NameOf.SoundEvent)} " + name + $" {nameof(NameOf.SoundContainer)} " + i + " is null.", this);
                        }
                    } else {
                        internals.soundContainers[i].internals.LoadUnloadAudioData(load, internals.soundContainers[i]);
                    }
                }
            }
        }

        public bool IsNull() {
            for (int i = 0; i < internals.soundContainers.Length; i++) {
                // Checks if the SoundEvent is null
                if (internals.soundContainers[i] == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity: \"" + name + $"\" ({nameof(NameOf.SoundEvent)}) has null {nameof(NameOf.SoundContainer)}s.", this);
                    }
                    return true;
                } else {
                    // Checks if the AudioClips are not empty
                    if (internals.soundContainers[i].internals.audioClips.Length == 0) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity: \"" + internals.soundContainers[i].name + $"\" ({nameof(NameOf.SoundContainer)}) has no {nameof(AudioClip)}s.", internals.soundContainers[i]);
                        }
                        return true;
                    } else {
                        for (int ii = 0; ii < internals.soundContainers[i].internals.audioClips.Length; ii++) {
                            // Checks if the AudioClips are null
                            if (internals.soundContainers[i].internals.audioClips[ii] == null) {
                                if (ShouldDebug.Warnings()) {
                                    Debug.LogWarning($"Sonity: \"" + internals.soundContainers[i].name + $"\" ({nameof(NameOf.SoundContainer)}) has null {nameof(AudioClip)}s.", internals.soundContainers[i]);
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}