// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using UnityEngine.Audio;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundContainerInternals {

        public SoundContainerInternalsData data = new SoundContainerInternalsData();

        public AudioClip[] audioClips = new AudioClip[1];

#if UNITY_EDITOR
        public string notes = "Notes";
#endif

        public void LoadUnloadAudioData(bool load, SoundContainerBase parent) {
            if (audioClips.Length == 0) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"{nameof(NameOf.SoundContainer)} " + parent.name + $" has no {nameof(AudioClip)}.", parent);
                }
            } else {
                for (int i = 0; i < audioClips.Length; i++) {
                    if (audioClips[i] == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"{nameof(NameOf.SoundContainer)} " + parent.name + $" {nameof(AudioClip)} " + i + " is null.", parent);
                        }
                    } else {
                        // Loads or Unloads the Audio Data for Unity AudioClips
                        if (load) {
                            audioClips[i].LoadAudioData();
                        } else {
                            audioClips[i].UnloadAudioData();
                        }
                    }
                }
            }
        }

        public float GetLongestAudioClipLength() {
            float length = 0f;
            if (audioClips != null) {
                for (int i = 0; i < audioClips.Length; i++) {
                    if (audioClips[i] != null) {
                        if (length < audioClips[i].length) {
                            length = audioClips[i].length;
                        }
                    }
                }
            }
            return length;
        }

        public float GetShortestAudioClipLength() {
            float length = Mathf.Infinity;
            for (int i = 0; i < audioClips.Length; i++) {
                if (audioClips[i] != null) {
                    if (length > audioClips[i].length) {
                        length = audioClips[i].length;
                    }
                }
            }
            // If no audioClip are found
            if (length == Mathf.Infinity) {
                return 0f;
            } else {
                return length;
            }
        }

        [NonSerialized]
        private int[] randomClipLast = new int[0];
        [NonSerialized]
        private int randomClipListPosition;

        private bool RandomClipLastContains(int randomClip) {
            for (int i = 0; i < randomClipLast.Length; i++) {
                if (randomClipLast[i] == randomClip) {
                    return true;
                }
            }
            return false;
        }

        public int GetRandomClip() {
            // So that it wont break when changing the length in the editor at runtime
            if (randomClipLast.Length != audioClips.Length / 2) {
                randomClipLast = new int[audioClips.Length / 2];
                randomClipListPosition = 0;
            }

            if (randomClipLast.Length == 0) {
                return 0;
            }

            int randomClip;
            // Pseudo random function remembering which clips it last played to avoid repetition
            do {
                randomClip = UnityEngine.Random.Range(0, audioClips.Length);
            } while (RandomClipLastContains(randomClip));
            randomClipLast[randomClipListPosition] = randomClip;

            randomClipListPosition++;
            if (randomClipListPosition >= randomClipLast.Length) {
                randomClipListPosition = 0;
            }

            return randomClip;
        }
    }

    [Serializable]
    public class SoundContainerInternalsData {

#if UNITY_EDITOR
        public UnityEngine.Object[] foundReferences;
        public AudioMixerGroup previewAudioMixerGroup;
#endif

        public bool expandPreview = false;
        public bool expandAudioClips = true;

        public bool previewCurves = true;

        private float GetRolloff(float value, float rolloff) {
            return LogLinExp.Get(value, rolloff);
        }

        public bool GetIntensityEnabled() {
            if (volumeIntensityEnable || reverbZoneMixIntensityEnable || spatialBlendIntensityEnable || spatialSpreadIntensityEnable || distortionIntensityEnable || lowpassIntensityEnable || highpassIntensityEnable || pitchIntensityEnable) {
                return true;
            }
            return false;
        }

        public bool GetVoiceEffectsEnabled() {
            if (GetDistortionEnabled() || GetLowpassEnabled() || GetHighpassEnabled()) {
                return true;
            }
            return false;
        }

        public bool GetDistortionEnabled() {
            return distortionEnabled;
        }

        public bool GetLowpassEnabled() {
            return lowpassEnabled;
        }

        public bool GetHighpassEnabled() {
            return highpassEnabled;
        }

        // The SoundContainer AudioMixerGroup is removed in Sonity 1.0.6
        public AudioMixerGroup audioMixerGroup;

        // Settings
        public bool settingsExpandBase = true;
        public bool settingsExpandAdvanced = false;
        public float priority = 0.5f;
        public bool distanceEnabled = true;
        public float distanceScale = 100f;
        public bool preventEndClicks = true;
        public bool loopEnabled = false;
        public bool followPosition = false;
        public bool stopIfTransformIsNull = false;
        //public bool virtualize = false; // Virtualize Todo
        public bool randomStartPosition = false;
        public float randomStartPositionMin = 0f;
        public float randomStartPositionMax = 1f;
        public float startPosition = 0f;
        public float GetStartPosition(SoundEventModifier modifier) {
            if (modifier.startPositionUse) {
                return modifier.startPosition;
            } else {
                return startPosition;
            }
        }

        // Advanced Settings

        public bool neverStealVoice = false;
        public bool neverStealVoiceEffects = false;

        public bool reverse = false;
        public bool GetReverse(SoundEventModifier modifier) {
            if (modifier.reverseUse) {
                return modifier.reverse;
            } else {
                return reverse;
            }
        }

        public bool lockAxisEnable = false;
        public AxisToLock lockAxis = AxisToLock.Z;
        public float lockAxisPosition = 0f;

        public PlayOrder playOrder;

        // Range 0 to 5
        public float dopplerAmount = 0f;

        public bool bypassReverbZones = false;
        public bool bypassVoiceEffects = false;
        public bool bypassListenerEffects = false;

        public bool GetBypassReverbZones(SoundEventModifier modifier) {
            if (modifier.bypassReverbZonesUse) {
                return modifier.bypassReverbZones;
            } else {
                return bypassReverbZones;
            }
        }

        public bool GetBypassVoiceEffects(SoundEventModifier modifier) {
            if (modifier.bypassVoiceEffectsUse) {
                return modifier.bypassVoiceEffects;
            } else {
                return bypassVoiceEffects;
            }
        }

        public bool GetBypassListenerEffects(SoundEventModifier modifier) {
            if (modifier.bypassListenerEffectsUse) {
                return modifier.bypassListenerEffects;
            } else {
                return bypassListenerEffects;
            }
        }

        // HRTF Plugin Spatialize
        public bool hrtfPluginSpatialize = false;
        public bool hrtfPluginSpatializePostEffects = false;

        // Volume

        public bool volumeExpand = true;
        // Decibel only used in the editor
        public float volumeDecibel = 0;
        public float volumeRatio = 1f;

        public float GetVolumeRatioClamped() {
            return Mathf.Clamp(volumeRatio, 0f, VolumeScale.volumeIncrease24dbMaxRatio);
        }

        public bool volumeRandomEnable = false;
        // Unipolar downwards
        public float volumeRandomRangeDecibel = -3f;

        public float volumeDistanceRolloff = -2f;
        public AnimationCurve volumeDistanceCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public bool volumeIntensityEnable = false;
        public float volumeIntensityRolloff = 2f;
        public float volumeIntensityStrength = 1f;
        public AnimationCurve volumeIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float GetVolumeRatioRandom() {
            if (volumeRandomEnable) {
                // Need to use decibel random range otherwise random range scale is weighted wrong
                return VolumeScale.ConvertDecibelToRatio(UnityEngine.Random.Range(volumeRandomRangeDecibel, 0f));
            } else {
                return 1f;
            }
        }

        public float GetVolume(SoundEventModifier modifier, float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            float volume = GetVolumeRatioClamped();

            if (distanceEnabled && distanceEnable) {
                volume *= GetVolumeDistance(distance);
            }

            if (volumeIntensityEnable && intensityEnable) {
                volume *= GetVolumeIntensity(intensity);
            }

            if (modifier.volumeUse) {
                volume *= modifier.volumeRatio;
            }

            return Mathf.Clamp(volume, 0f, VolumeScale.volumeIncrease24dbMaxRatio);
        }

        public float GetVolumeDistance(float distance) {
            return GetVolumeDistanceCrossfade(distance) * volumeDistanceCurve.Evaluate(GetRolloff(distance, volumeDistanceRolloff));
        }

        public float GetVolumeIntensity(float intensity) {
            return GetVolumeIntensityCrossfade(intensity) * ((volumeIntensityCurve.Evaluate(GetRolloff(intensity, volumeIntensityRolloff)) - 1f) * volumeIntensityStrength + 1f);
        }

        public bool volumeDistanceCrossfadeEnable = false;
        public int volumeDistanceCrossfadeTotalLayersOneBased = 2;
        public int volumeDistanceCrossfadeTotalLayers = 1;
        public int volumeDistanceCrossfadeLayerOneBased = 1;
        public int volumeDistanceCrossfadeLayer = 0;
        public float volumeDistanceCrossfadeRolloff = -2f;
        public AnimationCurve volumeDistanceCrossfadeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public void SetVolumeDistanceCrossfade(int layersOneIndexed, int thisIsOneIndexed, bool forceEnable) {
            if (forceEnable) {
                volumeDistanceCrossfadeEnable = true;
                volumeDistanceCrossfadeTotalLayersOneBased = layersOneIndexed;
            }
            volumeDistanceCrossfadeTotalLayers = layersOneIndexed - 1;
            volumeDistanceCrossfadeLayerOneBased = thisIsOneIndexed;
            volumeDistanceCrossfadeLayer = thisIsOneIndexed - 1;
        }

        private float GetVolumeDistanceCrossfade(float distance) {
            if (!volumeDistanceCrossfadeEnable) return 1f;

            float crossfade = 1f;

            // Applies distance offset
            distance = volumeDistanceCrossfadeCurve.Evaluate(GetRolloff(distance, volumeDistanceCrossfadeRolloff));

            // Calculates the Crossfade
            if (volumeDistanceCrossfadeTotalLayers > 0) {
                // Min
                if (volumeDistanceCrossfadeLayer == 0) {
                    crossfade = ((1f - distance) - 1f / volumeDistanceCrossfadeTotalLayers * (volumeDistanceCrossfadeTotalLayers - 1f)) * volumeDistanceCrossfadeTotalLayers;
                    // Max
                } else if (volumeDistanceCrossfadeLayer == volumeDistanceCrossfadeTotalLayers) {
                    crossfade = (distance - 1f / volumeDistanceCrossfadeTotalLayers * (volumeDistanceCrossfadeTotalLayers - 1f)) * volumeDistanceCrossfadeTotalLayers;
                    // Middle
                } else {
                    crossfade = -Mathf.Abs(((1f - distance) - 1f / volumeDistanceCrossfadeTotalLayers * (volumeDistanceCrossfadeTotalLayers - volumeDistanceCrossfadeLayer))) * volumeDistanceCrossfadeTotalLayers + 1f;
                }
                crossfade = Mathf.Clamp(crossfade, 0f, 1f);
            }

            // Converts from linear scale 
            crossfade = 1f - Mathf.Pow(1f - crossfade, 2f);

            return Mathf.Clamp(crossfade, 0f, 1f);
        }

        public bool volumeIntensityCrossfadeEnable = false;
        public int volumeIntensityCrossfadeTotalLayersOneBased = 2;
        public int volumeIntensityCrossfadeTotalLayers = 1;
        public int volumeIntensityCrossfadeLayerOneBased = 1;
        public int volumeIntensityCrossfadeLayer = 0;
        public float volumeIntensityCrossfadeRolloff = 2f;
        public AnimationCurve volumeIntensityCrossfadeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public void SetVolumeIntensityCrossfade(int layersOneIndexed, int thisIsOneIndexed, bool forceEnable) {
            if (forceEnable) {
                volumeIntensityEnable = true;
                volumeIntensityCrossfadeEnable = true;
            }
            volumeIntensityCrossfadeTotalLayersOneBased = layersOneIndexed;
            volumeIntensityCrossfadeTotalLayers = layersOneIndexed - 1;
            volumeIntensityCrossfadeLayerOneBased = thisIsOneIndexed;
            volumeIntensityCrossfadeLayer = thisIsOneIndexed - 1;
        }

        private float GetVolumeIntensityCrossfade(float intensity) {
            if (!volumeIntensityCrossfadeEnable) return 1f;

            float crossfade = 1f;

            // Applies distance offset
            intensity = volumeIntensityCrossfadeCurve.Evaluate(GetRolloff(intensity, volumeIntensityCrossfadeRolloff));

            // Calculates the Crossfade
            if (volumeIntensityCrossfadeTotalLayers > 0) {
                // Min
                if (volumeIntensityCrossfadeLayer == 0) {
                    crossfade = ((1f - intensity) - 1f / volumeIntensityCrossfadeTotalLayers * (volumeIntensityCrossfadeTotalLayers - 1f)) * volumeIntensityCrossfadeTotalLayers;
                    // Max
                } else if (volumeIntensityCrossfadeLayer == volumeIntensityCrossfadeTotalLayers) {
                    crossfade = (intensity - 1f / volumeIntensityCrossfadeTotalLayers * (volumeIntensityCrossfadeTotalLayers - 1f)) * volumeIntensityCrossfadeTotalLayers;
                    // Middle
                } else {
                    crossfade = -Mathf.Abs(((1f - intensity) - 1f / volumeIntensityCrossfadeTotalLayers * (volumeIntensityCrossfadeTotalLayers - volumeIntensityCrossfadeLayer))) * volumeIntensityCrossfadeTotalLayers + 1f;
                }
                crossfade = Mathf.Clamp(crossfade, 0f, 1f);
            }

            // Converts from linear scale
            crossfade = 1f - Mathf.Pow(1f - crossfade, 2f);

            return Mathf.Clamp(crossfade, 0f, 1f);
        }

        // Pitch

        public bool pitchExpand = false;

        // Used in the editor
        public float pitchSemitone = 0f;
        public float pitchRatio = 1f;

        public bool pitchRandomEnable = true;
        public float pitchRandomRangeSemitoneBipolar = 0.25f;

        public bool pitchIntensityEnable = false;

        // Used in the editor
        public float pitchIntensityLowSemitone = -12f;
        public float pitchIntensityLowRatio = 0.5f;
        public float pitchIntensityHighSemitone = 12f;
        public float pitchIntensityHighRatio = 2f;

        public float pitchIntensityBaseRatio = 0.5f;
        public float pitchIntensityBaseSemitone = -12f;
        // Used in the editor
        public float pitchIntensityRangeSemitone = 24f;
        public float pitchIntensityRangeRatio = 4f;
        public float pitchIntensityRolloff = 0f;
        public AnimationCurve pitchIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        // Pitch Intensity
        public float GetPitchRatioIntensity(float intensity) {
            if (!pitchIntensityEnable) {
                return 1f;
            }

            // Pitch Intensity is Offset so it doesnt use the pitchRatio
            return Mathf.Clamp(pitchIntensityBaseRatio * PitchScale.SemitonesToRatio(pitchIntensityRangeSemitone * pitchIntensityCurve.Evaluate(GetRolloff(intensity, pitchIntensityRolloff))), 0f, Mathf.Infinity);
        }

        public float GetPitchBaseAndRandom() {
            if (pitchRandomEnable) {
                // Need to use semitone random range otherwise random range scale is weighted wrong
                return pitchRatio * PitchScale.SemitonesToRatio(UnityEngine.Random.Range(-pitchRandomRangeSemitoneBipolar, pitchRandomRangeSemitoneBipolar));
            } else {
                return pitchRatio;
            }
        }

        // Spatial Blend

        public bool spatialBlendExpand = false;

        public float spatialBlend = 1f;

        public float spatialBlendDistanceRolloff = -4f;
        public float spatialBlendDistance3DIncrease = 0.5f;
        public AnimationCurve spatialBlendDistanceCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool spatialBlendIntensityEnable = false;
        public float spatialBlendIntensityRolloff = 4f;
        public float spatialBlendIntensityStrength = 1f;
        public AnimationCurve spatialBlendIntensityCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public float GetSpatialBlend(SoundEventModifier modifier, float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            float spatialBlend = this.spatialBlend;

            if (distanceEnabled && distanceEnable) {
                spatialBlend *= GetSpatialBlendDistance(distance);
            }

            if (spatialBlendIntensityEnable && intensityEnable) {
                spatialBlend *= GetSpatialBlendIntensity(intensity);
            }

            if (modifier.increase2DUse) {
                spatialBlend = spatialBlend * (1f - modifier.increase2D);
            }

            return Mathf.Clamp(spatialBlend, 0f, 1f);
        }

        public float GetSpatialBlendDistance(float distance) {
            return (spatialBlendDistanceCurve.Evaluate(GetRolloff(distance, spatialBlendDistanceRolloff)) - 1f) * (1f - spatialBlendDistance3DIncrease) + 1f;
        }

        public float GetSpatialBlendIntensity(float intensity) {
            return (spatialBlendIntensityCurve.Evaluate(GetRolloff(intensity, spatialBlendIntensityRolloff)) - 1f) * spatialBlendIntensityStrength + 1f;
        }

        // Spatial Spread

        public bool spatialSpreadExpand = false;
        // Range 0 to 360
        public float spatialSpreadDegrees = 180f;
        // Range 0 to 1
        public float spatialSpreadRatio = 0.5f;

        public float spatialSpreadDistanceRolloff = -4f;
        public AnimationCurve spatialSpreadDistanceCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public bool spatialSpreadIntensityEnable = false;
        public float spatialSpreadIntensityRolloff = 4f;
        public float spatialSpreadIntensityStrength = 0.5f;
        public AnimationCurve spatialSpreadIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float GetSpatialSpread(float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            float spatialSpread = spatialSpreadRatio;

            if (distanceEnabled && distanceEnable) {
                spatialSpread *= GetSpatialSpreadDistance(distance);
            }

            if (spatialSpreadIntensityEnable && intensityEnable) {
                spatialSpread *= GetSpatialSpreadIntensity(intensity);
            }

            return Mathf.Clamp(spatialSpread, 0f, 1f);
        }

        public float GetSpatialSpreadDistance(float distance) {
            return spatialSpreadDistanceCurve.Evaluate(GetRolloff(distance, spatialSpreadDistanceRolloff));
        }

        public float GetSpatialSpreadIntensity(float intensity) {
            return (spatialSpreadIntensityCurve.Evaluate(GetRolloff(intensity, spatialSpreadIntensityRolloff)) - 1f) * spatialSpreadIntensityStrength + 1f;
        }

        // Stereo Pan

        public bool stereoPanExpand = false;
        public float stereoPanOffset = 0f;
        public bool stereoPanAngleUse = false;
        public float stereoPanAngleAmount = 0.75f;
        public float stereoPanAngleRolloff = -2f;

        public float GetStereoPan(SoundEventModifier modifier, float stereoPanAngle) {
            float stereoPan = 0f;

            // Stereo pan angle is range -1 (behind left) to 1 (behind right)
            if (stereoPanAngleUse) {
                // Angle fold behind the audioListener so that there is no jump between angle -1 and 1
                if (stereoPanAngle > 0.5f) {
                    // If angle is +90 degrees
                    stereoPanAngle = -stereoPanAngle + 1f;
                } else if (stereoPanAngle < -0.5f) {
                    // If angle is -90 degrees
                    stereoPanAngle = -stereoPanAngle - 1f;
                }
                if (stereoPanAngle > 0f) {
                    stereoPan = GetRolloff(stereoPanAngle * 2f, stereoPanAngleRolloff) * stereoPanAngleAmount;
                } else {
                    stereoPan = -GetRolloff(-stereoPanAngle * 2f, stereoPanAngleRolloff) * stereoPanAngleAmount;
                }
            }

            if (modifier.stereoPanUse) {
                stereoPan += modifier.stereoPan;
            } else {
                stereoPan += stereoPanOffset;
            }
            return Mathf.Clamp(stereoPan, -1f, 1f);
        }

        // Reverb Zone Mix

        public bool reverbZoneMixExpand = false;
        // Used in the editor
        public float reverbZoneMixDecibel = 0f;
        // Max value is +10 dB (3.1622776601683795 ratio)
        // Is scaled later so 1.1 is +10dB
        public float reverbZoneMixRatio = 1f;

        public float reverbZoneMixDistanceIncrease = 0.5f;
        public float reverbZoneMixDistanceRolloff = -2f;
        public AnimationCurve reverbZoneMixDistanceCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool reverbZoneMixIntensityEnable = false;
        public float reverbZoneMixIntensityRolloff = 2f;
        public float reverbZoneMixIntensityAmount = 1f;
        public AnimationCurve reverbZoneMixIntensityCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public float GetReverbZoneMixRatio(SoundEventModifier modifier, float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            // Range 0 to 3.1622776601683795f
            float reverbZoneMix = reverbZoneMixRatio;

            if (distanceEnabled && distanceEnable) {
                reverbZoneMix *= GetReverbZoneMixRatioDistance(distance);
            }

            if (reverbZoneMixIntensityEnable && intensityEnable) {
                reverbZoneMix *= GetReverbZoneMixRatioIntensity(intensity);
            }

            if (modifier.reverbZoneMixUse) {
                reverbZoneMix *= modifier.reverbZoneMixRatio;
            }

            return Mathf.Clamp(reverbZoneMix, 0f, 3.1622776601683795f);
        }

        public float GetReverbZoneMixRatioDistance(float distance) {
            return (reverbZoneMixDistanceCurve.Evaluate(GetRolloff(distance, reverbZoneMixDistanceRolloff)) - 1f) * (1f - reverbZoneMixDistanceIncrease) + 1f;
        }

        public float GetReverbZoneMixRatioIntensity(float intensity) {
            return (reverbZoneMixIntensityCurve.Evaluate(GetRolloff(intensity, reverbZoneMixIntensityRolloff)) - 1f) * reverbZoneMixIntensityAmount + 1f;
        }

        // Distortion

        public bool distortionExpand = false;
        public bool distortionEnabled = false;
        // Range 0f to 1f
        public float distortionAmount = 0.25f;

        public bool distortionDistanceEnable = false;
        public float distortionDistanceRolloff = -4f;
        public AnimationCurve distortionDistanceCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public bool distortionIntensityEnable = false;
        public float distortionIntensityRolloff = 4f;
        public float distortionIntensityStrength = 0.5f;
        public AnimationCurve distortionIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public float GetDistortion(SoundEventModifier modifier, float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            if (!distortionEnabled) {
                return 0f;
            }

            // Range 0 to 1
            float distortionAmountTemp = distortionAmount;

            if (distanceEnabled && distortionDistanceEnable && distanceEnable) {
                distortionAmountTemp *= GetDistortionDistance(distance);
            }

            if (distortionIntensityEnable && intensityEnable) {
                distortionAmountTemp *= GetDistortionIntensity(intensity);
            }

            if (modifier.distortionIncreaseUse) {
                distortionAmountTemp = (distortionAmountTemp - 1f) * (1f - modifier.distortionIncrease) + 1f;
            }

            return Mathf.Clamp(distortionAmountTemp, 0f, 1f);
        }

        public float GetDistortionDistance(float distance) {
            return distortionDistanceCurve.Evaluate(GetRolloff(distance, distortionDistanceRolloff));
        }

        public float GetDistortionIntensity(float intensity) {
            return (distortionIntensityCurve.Evaluate(GetRolloff(intensity, distortionIntensityRolloff)) - 1f) * distortionIntensityStrength + 1f;
        }

        // Lowpass

        public bool lowpassExpand = false;
        public bool lowpassEnabled = false;
        // Range 20 to 20000 hz = engine * 19980f + 20f
        public float lowpassFrequencyEditor = 20000f;
        // Range 0 to 1 = (editor - 20f) / 19980f
        public float lowpassFrequencyEngine = 1f;
        // Range 0 to 6 dB = engine * 6f
        public float lowpassAmountEditor = 6f;
        // Range 0 to 1 = -editor / 6f
        public float lowpassAmountEngine = 1f;

        public bool lowpassDistanceEnable = false;
        public float lowpassDistanceFrequencyRolloff = -2f;
        public AnimationCurve lowpassDistanceFrequencyCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);
        public float lowpassDistanceAmountRolloff = -2f;
        public AnimationCurve lowpassDistanceAmountCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public bool lowpassIntensityEnable = false;
        public float lowpassIntensityFrequencyRolloff = 2f;
        public float lowpassIntensityFrequencyStrength = 1f;
        public AnimationCurve lowpassIntensityFrequencyCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public float lowpassIntensityAmountRolloff = 2f;
        public float lowpassIntensityAmountStrength = 1f;
        public AnimationCurve lowpassIntensityAmountCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        public float GetLowpassFrequency(float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            if (!lowpassEnabled) {
                return 0f;
            }

            float lowpassFrequency = lowpassFrequencyEngine;

            if (distanceEnabled && lowpassDistanceEnable && distanceEnable) {
                lowpassFrequency *= GetLowpassFrequencyDistance(distance);
            }

            if (lowpassIntensityEnable && intensityEnable) {
                lowpassFrequency *= GetLowpassFrequencyIntensity(intensity);
            }

            return Mathf.Clamp(lowpassFrequency, 0f, 1f);
        }

        public float GetLowpassFrequencyDistance(float distance) {
            return lowpassDistanceFrequencyCurve.Evaluate(GetRolloff(distance, lowpassDistanceFrequencyRolloff));
        }

        public float GetLowpassFrequencyIntensity(float intensity) {
            return (lowpassIntensityFrequencyCurve.Evaluate(GetRolloff(intensity, lowpassIntensityFrequencyRolloff)) - 1f) * lowpassIntensityFrequencyStrength + 1f;
        }

        public float GetLowpassAmount(float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            if (!lowpassEnabled) {
                return 0f;
            }

            float lowpassAmount = lowpassAmountEngine;

            if (distanceEnabled && lowpassDistanceEnable && distanceEnable) {
                lowpassAmount *= GetLowpassAmountDistance(distance);
            }

            if (lowpassIntensityEnable && intensityEnable) {
                lowpassAmount *= GetLowpassAmountIntensity(intensity);
            }

            return Mathf.Clamp(lowpassAmount, 0f, 1f);
        }

        public float GetLowpassAmountDistance(float distance) {
            return lowpassDistanceAmountCurve.Evaluate(GetRolloff(distance, lowpassDistanceAmountRolloff));
        }

        public float GetLowpassAmountIntensity(float intensity) {
            return (lowpassIntensityAmountCurve.Evaluate(GetRolloff(intensity, lowpassIntensityAmountRolloff)) - 1f) * lowpassIntensityAmountStrength + 1f;
        }

        // Highpass

        // Highpass
        public bool highpassExpand = false;
        public bool highpassEnabled = false;
        // Range 20 to 20000 hz = engine * 19980f + 20f
        public float highpassFrequencyEditor = 20000f;
        // Range 0 to 1 = (editor - 20f) / 19980f
        public float highpassFrequencyEngine = 1f;
        // Range 0 to 6 dB = engine * 6f
        public float highpassAmountEditor = 6f;
        // Range 0 to 1 = -editor / 6f
        public float highpassAmountEngine = 1f;

        public bool highpassDistanceEnable = false;
        public float highpassDistanceFrequencyRolloff = -2f;
        public AnimationCurve highpassDistanceFrequencyCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float highpassDistanceAmountRolloff = -2f;
        public AnimationCurve highpassDistanceAmountCurve = AnimationCurve.Linear(0, 0, 1, 1);

        public bool highpassIntensityEnable = false;
        public float highpassIntensityFrequencyRolloff = 2f;
        public float highpassIntensityFrequencyStrength = 1f;
        public AnimationCurve highpassIntensityFrequencyCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public float highpassIntensityAmountRolloff = 2f;
        public float highpassIntensityAmountStrength = 1f;
        public AnimationCurve highpassIntensityAmountCurve = AnimationCurve.Linear(0, 1, 1, 0);

        public float GetHighpassFrequency(float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            if (!highpassEnabled) {
                return 0f;
            }

            float highpassFrequency = highpassFrequencyEngine;

            if (distanceEnabled && highpassDistanceEnable && distanceEnable) {
                highpassFrequency *= GetHighpassFrequencyDistance(distance);

            }

            if (highpassIntensityEnable && intensityEnable) {
                highpassFrequency *= GetHighpassFrequencyIntensity(intensity);
            }

            return Mathf.Clamp(highpassFrequency, 0f, 1f);
        }

        public float GetHighpassFrequencyDistance(float distance) {
            return highpassDistanceFrequencyCurve.Evaluate(GetRolloff(distance, highpassDistanceFrequencyRolloff));
        }

        public float GetHighpassFrequencyIntensity(float intensity) {
            return (highpassIntensityFrequencyCurve.Evaluate(GetRolloff(intensity, highpassIntensityFrequencyRolloff)) - 1f) * highpassIntensityFrequencyStrength + 1f;
        }

        public float GetHighpassAmount(float distance, float intensity, bool distanceEnable = true, bool intensityEnable = true) {
            if (!highpassEnabled) {
                return 0f;
            }

            float highpassAmount = highpassAmountEngine;

            if (distanceEnabled && highpassDistanceEnable && distanceEnable) {
                highpassAmount *= GetHighpassAmountDistance(distance);
            }

            if (highpassIntensityEnable && intensityEnable) {
                highpassAmount *= GetHighpassAmountIntensity(intensity);
            }

            return Mathf.Clamp(highpassAmount, 0f, 1f);
        }

        public float GetHighpassAmountDistance(float distance) {
            return highpassDistanceAmountCurve.Evaluate(GetRolloff(distance, highpassDistanceAmountRolloff));
        }

        public float GetHighpassAmountIntensity(float intensity) {
            return (highpassIntensityAmountCurve.Evaluate(GetRolloff(intensity, highpassIntensityAmountRolloff)) - 1f) * highpassIntensityAmountStrength + 1f;
        }
    }
}