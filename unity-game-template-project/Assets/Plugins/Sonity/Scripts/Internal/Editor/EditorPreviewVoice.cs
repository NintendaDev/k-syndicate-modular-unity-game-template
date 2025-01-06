// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorPreviewVoice {

        public EditorPreviewSoundData previewSoundContainerSetting;
        public SoundEventModifier soundEventModifier;
        public VoiceFade voiceFade = new VoiceFade();
        public GameObject gameObject;
        public AudioSource audioSource;
        public VoiceEffect voiceEffect;

        public bool played = false;
        public float savedDelay = 0f;
        public float savedPitchRatio = 1f;
        public float savedVolumeRandomRatio = 1f;
        public float intensityCurrent = 1f;
        public bool isTriggerOn = false;

        private bool clickFadeActive = false;
        private float clickFadeStartTime = 0f;

        public void Play(bool isTriggerOn = false) {
            this.isTriggerOn = isTriggerOn;
            clickFadeActive = false;

            // SoundContainer can be null if SoundTag has SE with null SC
            if (audioSource == null || previewSoundContainerSetting.soundContainer == null || previewSoundContainerSetting.soundContainer.internals.audioClips.Length == 0) {
                return;
            }

            if (previewSoundContainerSetting != null) {
                // Initialize AudioSource
                if (previewSoundContainerSetting.soundContainer.internals.audioClips.Length == 0) {
                    return;
                }
                audioSource.clip = previewSoundContainerSetting.soundContainer.internals.audioClips[UnityEngine.Random.Range(0, previewSoundContainerSetting.soundContainer.internals.audioClips.Length)];

                audioSource.playOnAwake = false;
                audioSource.dopplerLevel = 0f;

                savedPitchRatio = previewSoundContainerSetting.soundContainer.internals.data.GetPitchBaseAndRandom();

                savedVolumeRandomRatio = previewSoundContainerSetting.soundContainer.internals.data.GetVolumeRatioRandom();
            }

            voiceFade.Reset();

            UpdateSoundContainerModifier();
            savedDelay = soundEventModifier.delay + previewSoundContainerSetting.timelineSoundContainerData.delay;

            InternalPlayDelay(0f);
        }

        public void InternalPlayDelay(float timeSinceStart) {
            if (!played && timeSinceStart >= savedDelay) {
                played = true;
                clickFadeActive = false;

                // SoundContainer can be null if SoundTag has SE with null SC
                if (previewSoundContainerSetting.soundContainer == null) {
                    return;
                }

                voiceFade.SetToFadeIn(soundEventModifier);
                UpdateVoice();

                if (previewSoundContainerSetting != null) {
                    audioSource.Play();

                    if (previewSoundContainerSetting.soundContainer.internals.data.randomStartPosition) {
                        // Random Start Position
                        int clipSamples = audioSource.clip.samples;
                        audioSource.timeSamples =
                            (int)Mathf.Clamp(
                                UnityEngine.Random.Range(
                                    clipSamples * previewSoundContainerSetting.soundContainer.internals.data.randomStartPositionMin,
                                    clipSamples * previewSoundContainerSetting.soundContainer.internals.data.randomStartPositionMax
                                    ),
                                0f, clipSamples - 1
                            );

                    } else {
                        // Start Offset
                        audioSource.timeSamples =
                            (int)Mathf.Clamp(
                                audioSource.clip.samples
                                * previewSoundContainerSetting.soundContainer.internals.data.GetStartPosition(soundEventModifier),
                                0f, audioSource.clip.samples - 1
                                );
                    }
                }
            }
        }

        public void UpdateSoundContainerModifier() {
            if (previewSoundContainerSetting != null) {
                soundEventModifier = new SoundEventModifier();
                for (int ii = 0; ii < previewSoundContainerSetting.soundEventModifierList.Count; ii++) {
                    if (previewSoundContainerSetting.soundEventModifierList != null) {
                        soundEventModifier.AddValuesTo(previewSoundContainerSetting.soundEventModifierList[ii]);
                    }
                }
            }
        }

        public float GetAudioSourceClipLengthSeconds(bool pitchSpeed) {
            if (audioSource.clip == null) {
                return 0f;
            }
            if (pitchSpeed) {
                // Avoid divide by zero
                if (audioSource.pitch == 0f) {
                    return audioSource.clip.length;
                }
                // Abs so that backwards audioSource with negative pitch does not return a negative number
                return audioSource.clip.length / Mathf.Abs(audioSource.pitch);
            } else {
                return audioSource.clip.length;
            }
        }

        public float GetAudioSourceTimeSeconds(bool pitchSpeed) {
            if (pitchSpeed) {
                // Avoid divide by zero
                if (audioSource.pitch == 0f) {
                    return audioSource.time;
                }
                // Abs so that backwards audioSource with negative pitch does not return a negative number
                return audioSource.time / Mathf.Abs(audioSource.pitch);
            } else {
                return audioSource.time;
            }
        }

        public void UpdateVoice() {
            if (previewSoundContainerSetting != null && previewSoundContainerSetting.soundContainer != null) {
                Vector3 position = EditorPreviewSound.GetEditorSetting().position;
                if (previewSoundContainerSetting.soundContainer.internals.data.lockAxisEnable) {
                    position = AxisLock.Lock(
                        position,
                        previewSoundContainerSetting.soundContainer.internals.data.lockAxis,
                        previewSoundContainerSetting.soundContainer.internals.data.lockAxisPosition
                        );
                }
                audioSource.transform.position = position * 10f;

                float distance = 0f;

                if (previewSoundContainerSetting.soundContainer.internals.data.distanceEnabled) {
                    if (previewSoundContainerSetting.soundContainer.internals.data.distanceScale == 0f) {
                        distance = 1f;
                    } else {
                        distance = Vector3.Distance(Vector3.zero, position);
                        // If is previewing a SoundEvent then calculate relative distance scale
                        if (previewSoundContainerSetting.soundEvent != null) {
                            float distanceHighest = 0f;
                            for (int i = 0; i < previewSoundContainerSetting.soundEvent.internals.soundContainers.Length; i++) {
                                if (previewSoundContainerSetting.soundEvent.internals.soundContainers[i] != null) {
                                    if (previewSoundContainerSetting.soundEvent.internals.soundContainers[i].internals.data.distanceEnabled) {
                                        if (distanceHighest < previewSoundContainerSetting.soundEvent.internals.soundContainers[i].internals.data.distanceScale) {
                                            distanceHighest = previewSoundContainerSetting.soundEvent.internals.soundContainers[i].internals.data.distanceScale;
                                        }
                                    }
                                }
                            }
                            if (distanceHighest > 0f) {
                                distance = distance / (previewSoundContainerSetting.soundContainer.internals.data.distanceScale / distanceHighest);
                            }
                        }
                    }
                }
                
                audioSource.loop = previewSoundContainerSetting.soundContainer.internals.data.loopEnabled;

                float reverbZoneMixRatio = previewSoundContainerSetting.soundContainer.internals.data.GetReverbZoneMixRatio(soundEventModifier, distance, intensityCurrent);
                // Range 0 to 1 is linear, 1.1 is 10 db boost(* 3.1622776601683795)
                if (reverbZoneMixRatio > 1f) {
                    reverbZoneMixRatio = reverbZoneMixRatio * 0.031622776601683795f + 1f;
                }
                audioSource.reverbZoneMix = reverbZoneMixRatio;

                float angleToCenter = AngleAroundAxis.Get(EditorPreviewSound.GetEditorSetting().position, Vector3.forward, Vector3.up);
                audioSource.panStereo = previewSoundContainerSetting.soundContainer.internals.data.GetStereoPan(soundEventModifier, angleToCenter);

                audioSource.bypassReverbZones = previewSoundContainerSetting.soundContainer.internals.data.GetBypassReverbZones(soundEventModifier);

                audioSource.bypassEffects = previewSoundContainerSetting.soundContainer.internals.data.GetBypassVoiceEffects(soundEventModifier);

                audioSource.bypassListenerEffects = previewSoundContainerSetting.soundContainer.internals.data.GetBypassListenerEffects(soundEventModifier);

                audioSource.spatialize = previewSoundContainerSetting.soundContainer.internals.data.hrtfPluginSpatialize;
                audioSource.spatializePostEffects = previewSoundContainerSetting.soundContainer.internals.data.hrtfPluginSpatializePostEffects;

                audioSource.dopplerLevel = previewSoundContainerSetting.soundContainer.internals.data.dopplerAmount;

                if (previewSoundContainerSetting.soundContainer.internals.data.distanceEnabled) {
                    audioSource.maxDistance = Mathf.Clamp(previewSoundContainerSetting.soundContainer.internals.data.distanceScale * soundEventModifier.distanceScale, 0f, 1f);
                } else {
                    audioSource.maxDistance = Mathf.Infinity;
                }

                if (previewSoundContainerSetting.soundContainer.internals.data.GetReverse(soundEventModifier)) {
                    // Reverse
                    audioSource.pitch = -savedPitchRatio * soundEventModifier.pitchRatio;
                } else {
                    audioSource.pitch = savedPitchRatio * soundEventModifier.pitchRatio;
                }
                audioSource.pitch *= previewSoundContainerSetting.soundContainer.internals.data.GetPitchRatioIntensity(intensityCurrent);

                audioSource.rolloffMode = AudioRolloffMode.Custom;
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, AnimationCurve.Linear(0f, 1f, 1f, 1f));

                float newVolume = VolumeScale.volumeIncrease60dbAudioListenerRatioLowerBack;

                newVolume *= previewSoundContainerSetting.soundContainer.internals.data.GetVolume(soundEventModifier, distance, intensityCurrent);

                newVolume = newVolume * voiceFade.GetVolume(soundEventModifier)
                    * previewSoundContainerSetting.timelineSoundContainerData.GetVolumeRatio() * savedVolumeRandomRatio;

                // SoundEvent Volume
                // SE is null when previewing SC
                if (previewSoundContainerSetting.soundEvent != null) {
                    newVolume *= previewSoundContainerSetting.soundEvent.internals.data.GetVolumeRatioClamped();
                    // SoundVolumeGroup
                    newVolume *= previewSoundContainerSetting.soundEvent.internals.data.GetSoundVolumeGroupRatio();
                }

                if (previewSoundContainerSetting.soundContainer.internals.data.preventEndClicks
                    && !previewSoundContainerSetting.soundContainer.internals.data.loopEnabled
                    && GetAudioSourceClipLengthSeconds(true) > 0.1f
                    ) {
                    if (GetAudioSourceTimeSeconds(true) >= GetAudioSourceClipLengthSeconds(true) - 0.1f) {
                        if (!clickFadeActive && GetAudioSourceClipLengthSeconds(true) - GetAudioSourceTimeSeconds(true) < 0.1f) {
                            clickFadeActive = true;
                            clickFadeStartTime = SoundTimeScale.GetTimeEditor();
                        }
                        if (clickFadeActive) {
                            // * 10 to scale to 0.1s, * 2 - 1 for silence at 0.05s
                            newVolume *= LogLinExp.Get(Mathf.Clamp((0.1f - (SoundTimeScale.GetTimeEditor() - clickFadeStartTime)) * 10f * 2f - 1f, 0f, 1f), -2f);
                        }
                    } else {
                        clickFadeActive = false;
                    }
                }

                audioSource.volume = newVolume;

                audioSource.spatialBlend = previewSoundContainerSetting.soundContainer.internals.data.GetSpatialBlend(soundEventModifier, distance, intensityCurrent);

                audioSource.spread = previewSoundContainerSetting.soundContainer.internals.data.GetSpatialSpread(distance, intensityCurrent) * 360f;

                // SE is null when previewing SC
                if (previewSoundContainerSetting.soundEvent == null) {
                    if (previewSoundContainerSetting.soundContainer.internals.data.previewAudioMixerGroup != null) {
                        audioSource.outputAudioMixerGroup = previewSoundContainerSetting.soundContainer.internals.data.previewAudioMixerGroup;
                    }
                } else {
                    if (previewSoundContainerSetting.soundEvent.internals.data.previewAudioMixerGroup == null) {
                        audioSource.outputAudioMixerGroup = previewSoundContainerSetting.soundEvent.internals.data.audioMixerGroup;
                    } else {
                        audioSource.outputAudioMixerGroup = previewSoundContainerSetting.soundEvent.internals.data.previewAudioMixerGroup;
                    }
                }

                // Voice Effects
                if (previewSoundContainerSetting.soundContainer.internals.data.GetVoiceEffectsEnabled()) {
                    voiceEffect.SetEnabled(true);

                    voiceEffect.DistortionSetEnabled(previewSoundContainerSetting.soundContainer.internals.data.GetDistortionEnabled());
                    if (voiceEffect.DistortionGetEnabled()) {
                        voiceEffect.DistortionSetValue(previewSoundContainerSetting.soundContainer.internals.data.GetDistortion(soundEventModifier, distance, intensityCurrent));
                    }

                    voiceEffect.LowpassSetEnabled(previewSoundContainerSetting.soundContainer.internals.data.GetLowpassEnabled());
                    if (voiceEffect.LowpassGetEnabled()) {
                        voiceEffect.LowpassSetValue(
                            previewSoundContainerSetting.soundContainer.internals.data.GetLowpassFrequency(distance, intensityCurrent),
                            previewSoundContainerSetting.soundContainer.internals.data.GetLowpassAmount(distance, intensityCurrent)
                        );
                    }

                    voiceEffect.HighpassSetEnabled(previewSoundContainerSetting.soundContainer.internals.data.GetHighpassEnabled());
                    if (voiceEffect.HighpassGetEnabled()) {
                        voiceEffect.HighpassSetValue(
                            previewSoundContainerSetting.soundContainer.internals.data.GetHighpassFrequency(distance, intensityCurrent),
                            previewSoundContainerSetting.soundContainer.internals.data.GetHighpassAmount(distance, intensityCurrent)
                        );
                    }
                } else {
                    voiceEffect.SetEnabled(false);
                }
            }
        }
    }
}
#endif