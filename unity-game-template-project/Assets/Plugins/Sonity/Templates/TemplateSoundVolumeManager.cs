// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using UnityEngine.Audio;

namespace SonityTemplate {

    /// <summary>
    /// Template of a singleton <see cref="AudioMixer"/> volume controller.
    /// The default settings work with the provided "TemplateAudioMixer.mixer".
    /// Add to a GameObject in the scene and use like this:
    /// SonityTemplate.TemplateSoundVolumeManager.Instance.SetVolumeMaster(1f);
    /// </summary>
    [AddComponentMenu("Sonity/Template - Sound Volume Manager")]
    public class TemplateSoundVolumeManager : MonoBehaviour {

        public AudioMixer audioMixer;

        // This sets the volume range in decibel scaled from 0-1
        private float lowestVolumeDb = -80f;

        // The names need to match the names of the exposed parameters in the mixer
        private string parameterNameMaster = "Master_Volume";
        private float volumeOffsetDbMaster = 0f;

        private string parameterNameAMB = "AMB_Volume";
        private float volumeOffsetDbAMB = 0f;

        private string parameterNameMUS = "MUS_Volume";
        private float volumeOffsetDbMUS = 0f;

        private string parameterNameSFX = "SFX_Volume";
        private float volumeOffsetDbSFX = 0f;

        private string parameterNameUI = "UI_Volume";
        private float volumeOffsetDbUI = 0f;

        private string parameterNameVO = "VO_Volume";
        private float volumeOffsetDbVO = 0f;

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeMaster(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbMaster;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameMaster, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeAMB(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbAMB;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameAMB, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeMUS(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbMUS;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameMUS, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeSFX(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbSFX;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameSFX, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeUI(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbUI;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameUI, volumeLinear);
        }

        /// <summary>
        /// Set <see cref="AudioMixer"/> volumes
        /// Input is range 0 to 1
        /// Its converted to a -80 to -0dB scale (the volume range is assignable with lowestVolumeDb)
        /// 0 will clamp to -80dB (-infinity in the <see cref="AudioMixer"/>)
        /// </summary>
        public void SetVolumeVO(float volumeLinear) {
            if (audioMixer == null) {
#if UNITY_EDITOR
                Debug.LogWarning($"You need to assign an {nameof(AudioMixer)} to set volumes.", this);
#endif
                return;
            }
            volumeLinear = Mathf.Clamp(volumeLinear, 0f, 1f);
            // Invert and convert to dB
            volumeLinear = (1f - volumeLinear) * lowestVolumeDb;
            // Snap to -infinity
            if (volumeLinear <= lowestVolumeDb) {
                volumeLinear = -80f;
            } else {
                volumeLinear += volumeOffsetDbVO;
            }
            // Set volume
            audioMixer.SetFloat(parameterNameVO, volumeLinear);
        }

        // Singleton Instance
        public bool useDontDestroyOnLoad = true;
        private bool isGoingToDelete;
#if UNITY_EDITOR
        private bool debugInstanceDestroyed = true;
#endif

        public static TemplateSoundVolumeManager Instance {
            get;
            private set;
        }

        private void Awake() {
            InstanceCheck();
            if (useDontDestroyOnLoad && !isGoingToDelete) {
                // DontDestroyOnLoad only works for root GameObjects
                gameObject.transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Checks if there are multiple instances this script, if so it destroys one of them
        private void InstanceCheck() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
#if UNITY_EDITOR
                if (debugInstanceDestroyed) {
                    Debug.LogWarning($"There can only be one {nameof(TemplateSoundVolumeManager)} instance per scene.", this);
                }
#endif
                // So that it does not run the rest of the Awake and Update code
                isGoingToDelete = true;
                if (Application.isPlaying) {
                    Destroy(gameObject);
                }
            }
        }
    }
}