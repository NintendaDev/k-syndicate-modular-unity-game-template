// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using UnityEngine.Audio; // Used with SONITY_ENABLE_ADRESSABLE_AUDIOMIXER

namespace Sonity.Internal {
    
    [Serializable]
    public class SoundManagerInternalsSettings {

        public bool settingExpandBase = true;
        public bool settingsExpandAdvanced = false;

        public bool disablePlayingSounds = false;

        public SoundTimeScaleMode soundTimeScale = SoundTimeScaleMode.UnscaledTime;

        public bool globalPause = false;

        public float globalVolumeRatio = 1f;
        // Decibel only used in the editor
        public float globalVolumeDecibel = 0f;

        public bool volumeIncreaseEnable = false;

        public bool dontDestroyOnLoad = true;
        public bool debugWarnings = true;
        public bool debugInPlayMode = true;

        public bool GetShouldDebugWarnings() {
            if (debugWarnings) {
                if (Application.isPlaying) {
                    return debugInPlayMode;
                } else {
                    return true;
                }
            }
            return false;
        }

#if UNITY_EDITOR
        public bool guiWarnings = true;
        public bool GetShouldDebugGuiWarnings() {
            return guiWarnings;
        }
#endif

        public bool adressableAudioMixerUse = false;

#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
        public UnityEngine.AddressableAssets.AssetReference adressableAudioMixerReference;
        private AudioMixer adressableAudioMixerAsset;

        public AudioMixer GetAdressableAudioMixer() {
            if (adressableAudioMixerUse) {
                if (adressableAudioMixerAsset != null) {
                    return adressableAudioMixerAsset;
                } else {
                    Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer AssetReference is null.");
                }
            } else {
                Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer is disabled but still is trying to be used.");
            }
            return null;
        }

        public AudioMixerGroup AdressableAudioMixerGroupConvert(AudioMixerGroup audioMixerGroup) {
            // Ignore SoundEvents without assigned AudioMixerGroups
            if (audioMixerGroup != null) {
                // Find the group with the same name in the Adressable AudioMixer
                if (adressableAudioMixerReference != null && adressableAudioMixerReference.RuntimeKeyIsValid()) {
                    return SoundManagerBase.Instance.Internals.settings.GetAdressableAudioMixer().FindMatchingGroups(audioMixerGroup.name)[0];
                } else {
                    Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer AssetReference is null.");
                }
            }
            return audioMixerGroup;
        }

        public void LoadAdressableAudioMixer() {
            if (adressableAudioMixerUse) {
                if (adressableAudioMixerAsset == null) {
                    if (adressableAudioMixerReference != null && adressableAudioMixerReference.RuntimeKeyIsValid()) {
                        UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AudioMixer> audioMixerLoadHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<AudioMixer>(adressableAudioMixerReference);
                        audioMixerLoadHandle.WaitForCompletion();
                        if (audioMixerLoadHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded) {
                            if (audioMixerLoadHandle.Result != null) {
                                adressableAudioMixerAsset = audioMixerLoadHandle.Result;
                            } else {
                                Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer is loaded but null.");
                            }
                        } else {
                            Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer AssetReference {adressableAudioMixerReference} failed to load.");
                        }
                    } else {
                        Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer AssetReference is null.");
                    }
                }
            } else {
                Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: Adressable AudioMixer is disabled but still is trying to be used.");
            }
        }
#endif

        public SoundTagBase globalSoundTag;
        public float distanceScale = 1f;
        public bool overrideListenerDistance = false;
        public float overrideListenerDistanceAmount = 100f;
        public bool speedOfSoundEnabled = false;
        public float speedOfSoundScale = 1f;

        /// <summary>
        /// Returns the Sound of Speed delay in seconds
        /// Distance is the unscaled distance between the <see cref="AudioListener"/> and the <see cref="Voice"/>
        /// </summary>
        public float GetSpeedOfSoundDelay(float distance) {
            if (speedOfSoundEnabled) {
                // Base value is 340 unity units per second. 1/340 = 0.00294117647058823529
                return distance * 0.002941f * speedOfSoundScale;
            } else {
                return 0f;
            }
        }

        public int voicePreload = 32;
        public int voiceLimit = 32;
        public float voiceStopTime = 5f;
        public int voiceEffectLimit = 32;
    }
}