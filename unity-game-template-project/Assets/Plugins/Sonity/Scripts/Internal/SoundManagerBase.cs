// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    // ExecuteInEditMode so that e.g. SoundContainer & SoundTrigger distanceScale, guiWarnings, GetIsInSolo can be checked in editor
    [ExecuteInEditMode]
    public abstract class SoundManagerBase : MonoBehaviour {

        /// <summary>
        /// The static instance of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        public static SoundManagerBase Instance { get; private set; }

        [SerializeField]
        private SoundManagerInternals internals = new SoundManagerInternals();

        /// <summary>
        /// The internal data of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        public SoundManagerInternals Internals { get { return internals; } private set { internals = value; } }

        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
                // Needed for disabling domain reloading
                internals.isGoingToDelete = false;
                internals.cachedSoundManagerTransform = GetComponent<Transform>();
            } else if (Instance != this) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"There can only be one Sonity.{nameof(NameOf.SoundManager)} instance per scene.", this);
                }
                // So that it does not run the rest of the Awake and Update code
                internals.isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }

        // Older version doesn't have the SubsystemRegistration load type
#if UNITY_2019_2_OR_NEWER
        // Needed for disabling domain reloading
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RuntimeInitializeOnLoad() {
            if (Instance != null) {
                Instance.Awake();
            }
        }
#endif
        void OnApplicationPause(bool pauseStatus) {
            internals.onApplicationIsPaused = pauseStatus;
        }

        private void Awake() {
            InstanceCheck();
            if (!internals.isGoingToDelete && Application.isPlaying) {
#if !SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
                if (internals.settings.adressableAudioMixerUse) {
                    Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: \"Adressable AudioMixer\" is enabled but the Script Define Symbol \"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\" is not added.");
                }
#endif
#if SONITY_ENABLE_VOLUME_INCREASE
                if (!internals.settings.volumeIncreaseEnable) {
                    Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: \"Enable Volume Increase\" is disabled but the Script Define Symbol \"SONITY_ENABLE_VOLUME_INCREASE\" exists.");
                }
#else
                if (internals.settings.volumeIncreaseEnable) {
                    Debug.LogError($"Sonity.{nameof(NameOf.SoundManager)}: \"Enable Volume Increase\" is enabled but the Script Define Symbol \"SONITY_ENABLE_VOLUME_INCREASE\" is not added.");
                }
#endif
                internals.AwakeCheck();
                // Update time value based on the selected time scale
                SoundTimeScale.UpdateTime(Internals.settings.soundTimeScale);
            }
        }

        private void Update() {
            if (!internals.isGoingToDelete) {
#if UNITY_EDITOR
                if (Application.isPlaying) {
#endif
                    internals.ManagedUpdate();
#if UNITY_EDITOR
                } else {
                    InstanceCheck();
                }
#endif
            }
        }

        private void OnDestroy() {
            // So that if the SoundManager is destroyed during eg. scene switching it will stop all playing sounds
            if (Application.isPlaying) {
                internals.Destroy();
            }
            // Needed for disabling domain reloading. If the current SoundManager is removed, then find any other one.
            if (Instance == this) {
                Instance = null;
#if UNITY_2022_3_OR_NEWER && !SONITY_DLL_RUNTIME
                // FindFirstObjectByType is slower than FindAnyObjectByType but is more consistent
                SoundManagerBase tempSoundManagerBase = UnityEngine.Object.FindFirstObjectByType<SoundManagerBase>();
#else
                SoundManagerBase tempSoundManagerBase = UnityEngine.Object.FindObjectOfType<SoundManagerBase>();
#endif
                if (tempSoundManagerBase != null) {
                    tempSoundManagerBase.InstanceCheck();
                }
            }
        }

#if UNITY_EDITOR
        private void OnGUI() {
            // Double check for DLL
            if (Application.isEditor) {
                // For live SoundEvent debugging
                EditorDebugDrawInGameView.UpdateDraw();
            }
        }
#endif

        private void OnApplicationQuit() {
            internals.applicationIsQuitting = true;
        }
    }
}