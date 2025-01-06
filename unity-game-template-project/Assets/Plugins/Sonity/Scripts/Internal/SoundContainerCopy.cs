// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class SoundContainerCopy {

        public static void CopyTo(SoundContainerBase to, SoundContainerBase from, bool copyAudioClips = false) {

            if (copyAudioClips){
                to.internals.audioClips = new AudioClip[from.internals.audioClips.Length];
                for (int i = 0; i < to.internals.audioClips.Length; i++) {
                    to.internals.audioClips[i] = from.internals.audioClips[i];
                }
            }

            // Skip this
            //to.internals.notes = from.internals.notes;

#if UNITY_EDITOR
            //to.internals.data.foundReferences = from.internals.data.foundReferences;
            to.internals.data.previewAudioMixerGroup = from.internals.data.previewAudioMixerGroup;
#endif

            to.internals.data.expandPreview = from.internals.data.expandPreview;
            to.internals.data.expandAudioClips = from.internals.data.expandAudioClips;
            to.internals.data.settingsExpandBase = from.internals.data.settingsExpandBase;
            to.internals.data.settingsExpandAdvanced = from.internals.data.settingsExpandAdvanced;

            to.internals.data.previewCurves = from.internals.data.previewCurves;

            // Settings
            to.internals.data.priority = from.internals.data.priority;
            to.internals.data.distanceEnabled = from.internals.data.distanceEnabled;
            to.internals.data.distanceScale = from.internals.data.distanceScale;
            to.internals.data.preventEndClicks = from.internals.data.preventEndClicks;

            to.internals.data.loopEnabled = from.internals.data.loopEnabled;
            to.internals.data.followPosition = from.internals.data.followPosition;
            to.internals.data.stopIfTransformIsNull = from.internals.data.stopIfTransformIsNull;
            //to.internals.data.virtualize = from.internals.data.virtualize; // Virtualize Todo
            to.internals.data.randomStartPosition = from.internals.data.randomStartPosition;

            to.internals.data.randomStartPositionMin = from.internals.data.randomStartPositionMin;
            to.internals.data.randomStartPositionMax = from.internals.data.randomStartPositionMax;

            to.internals.data.startPosition = from.internals.data.startPosition;
            to.internals.data.reverse = from.internals.data.reverse;

            to.internals.data.neverStealVoice = from.internals.data.neverStealVoice;
            to.internals.data.neverStealVoiceEffects = from.internals.data.neverStealVoiceEffects;

            // Advanced Settings
            to.internals.data.lockAxisEnable = from.internals.data.lockAxisEnable;
            to.internals.data.lockAxis = from.internals.data.lockAxis;
            to.internals.data.lockAxisPosition = from.internals.data.lockAxisPosition;

            to.internals.data.playOrder = from.internals.data.playOrder;

            // Range to 5
            to.internals.data.dopplerAmount = from.internals.data.dopplerAmount;

            to.internals.data.bypassReverbZones = from.internals.data.bypassReverbZones;
            to.internals.data.bypassVoiceEffects = from.internals.data.bypassVoiceEffects;
            to.internals.data.bypassListenerEffects = from.internals.data.bypassListenerEffects;

            to.internals.data.hrtfPluginSpatialize = from.internals.data.hrtfPluginSpatialize;
            to.internals.data.hrtfPluginSpatializePostEffects = from.internals.data.hrtfPluginSpatializePostEffects;

            // Volume
            to.internals.data.volumeExpand = from.internals.data.volumeExpand;
            // Used in the editor
            to.internals.data.volumeDecibel = from.internals.data.volumeDecibel;
            to.internals.data.volumeRatio = from.internals.data.volumeRatio;

            to.internals.data.volumeRandomEnable = from.internals.data.volumeRandomEnable;
            // Unipolar downwards
            to.internals.data.volumeRandomRangeDecibel = from.internals.data.volumeRandomRangeDecibel;

            to.internals.data.volumeDistanceRolloff = from.internals.data.volumeDistanceRolloff;
            to.internals.data.volumeDistanceCurve = from.internals.data.volumeDistanceCurve;

            to.internals.data.volumeIntensityEnable = from.internals.data.volumeIntensityEnable;
            to.internals.data.volumeIntensityRolloff = from.internals.data.volumeIntensityRolloff;
            to.internals.data.volumeIntensityStrength = from.internals.data.volumeIntensityStrength;
            to.internals.data.volumeIntensityCurve = from.internals.data.volumeIntensityCurve;

            to.internals.data.volumeDistanceCrossfadeEnable = from.internals.data.volumeDistanceCrossfadeEnable;

            to.internals.data.volumeDistanceCrossfadeTotalLayersOneBased = from.internals.data.volumeDistanceCrossfadeTotalLayersOneBased;
            to.internals.data.volumeDistanceCrossfadeTotalLayers = from.internals.data.volumeDistanceCrossfadeTotalLayers;
            to.internals.data.volumeDistanceCrossfadeLayerOneBased = from.internals.data.volumeDistanceCrossfadeLayerOneBased;
            to.internals.data.volumeDistanceCrossfadeLayer = from.internals.data.volumeDistanceCrossfadeLayer;
            to.internals.data.volumeDistanceCrossfadeRolloff = from.internals.data.volumeDistanceCrossfadeRolloff;
            to.internals.data.volumeDistanceCrossfadeCurve = from.internals.data.volumeDistanceCrossfadeCurve;

            to.internals.data.volumeIntensityCrossfadeEnable = from.internals.data.volumeIntensityCrossfadeEnable;

            to.internals.data.volumeIntensityCrossfadeTotalLayersOneBased = from.internals.data.volumeIntensityCrossfadeTotalLayersOneBased;
            to.internals.data.volumeIntensityCrossfadeTotalLayers = from.internals.data.volumeIntensityCrossfadeTotalLayers;
            to.internals.data.volumeIntensityCrossfadeLayerOneBased = from.internals.data.volumeIntensityCrossfadeLayerOneBased;
            to.internals.data.volumeIntensityCrossfadeLayer = from.internals.data.volumeIntensityCrossfadeLayer;

            to.internals.data.volumeIntensityCrossfadeRolloff = from.internals.data.volumeIntensityCrossfadeRolloff;
            to.internals.data.volumeIntensityCrossfadeCurve = from.internals.data.volumeIntensityCrossfadeCurve;

            // Pitch
            to.internals.data.pitchExpand = from.internals.data.pitchExpand;

            // Used in the editor
            to.internals.data.pitchSemitone = from.internals.data.pitchSemitone;
            to.internals.data.pitchRatio = from.internals.data.pitchRatio;

            to.internals.data.pitchRandomEnable = from.internals.data.pitchRandomEnable;
            to.internals.data.pitchRandomRangeSemitoneBipolar = from.internals.data.pitchRandomRangeSemitoneBipolar;

            to.internals.data.pitchIntensityEnable = from.internals.data.pitchIntensityEnable;

            // Used in the editor
            to.internals.data.pitchIntensityLowSemitone = from.internals.data.pitchIntensityLowSemitone;
            to.internals.data.pitchIntensityLowRatio = from.internals.data.pitchIntensityLowRatio;
            to.internals.data.pitchIntensityHighSemitone = from.internals.data.pitchIntensityHighSemitone;
            to.internals.data.pitchIntensityHighRatio = from.internals.data.pitchIntensityHighRatio;

            to.internals.data.pitchIntensityBaseRatio = from.internals.data.pitchIntensityBaseRatio;
            to.internals.data.pitchIntensityBaseSemitone = from.internals.data.pitchIntensityBaseSemitone;
            // Used in the editor
            to.internals.data.pitchIntensityRangeSemitone = from.internals.data.pitchIntensityRangeSemitone;
            to.internals.data.pitchIntensityRangeRatio = from.internals.data.pitchIntensityRangeRatio;
            to.internals.data.pitchIntensityRolloff = from.internals.data.pitchIntensityRolloff;
            to.internals.data.pitchIntensityCurve = from.internals.data.pitchIntensityCurve;

            // Spatial Blend
            to.internals.data.spatialBlendExpand = from.internals.data.spatialBlendExpand;

            to.internals.data.spatialBlend = from.internals.data.spatialBlend;

            to.internals.data.spatialBlendDistanceRolloff = from.internals.data.spatialBlendDistanceRolloff;
            to.internals.data.spatialBlendDistance3DIncrease = from.internals.data.spatialBlendDistance3DIncrease;
            to.internals.data.spatialBlendDistanceCurve = from.internals.data.spatialBlendDistanceCurve;

            to.internals.data.spatialBlendIntensityEnable = from.internals.data.spatialBlendIntensityEnable;
            to.internals.data.spatialBlendIntensityRolloff = from.internals.data.spatialBlendIntensityRolloff;
            to.internals.data.spatialBlendIntensityStrength = from.internals.data.spatialBlendIntensityStrength;
            to.internals.data.spatialBlendIntensityCurve = from.internals.data.spatialBlendIntensityCurve;

            // Spatial Spread
            to.internals.data.spatialSpreadExpand = from.internals.data.spatialSpreadExpand;
            // Range to 360
            to.internals.data.spatialSpreadDegrees = from.internals.data.spatialSpreadDegrees;
            // Range to 1
            to.internals.data.spatialSpreadRatio = from.internals.data.spatialSpreadRatio;

            to.internals.data.spatialSpreadDistanceRolloff = from.internals.data.spatialSpreadDistanceRolloff;
            to.internals.data.spatialSpreadDistanceCurve = from.internals.data.spatialSpreadDistanceCurve;

            to.internals.data.spatialSpreadIntensityEnable = from.internals.data.spatialSpreadIntensityEnable;
            to.internals.data.spatialSpreadIntensityRolloff = from.internals.data.spatialSpreadIntensityRolloff;
            to.internals.data.spatialSpreadIntensityStrength = from.internals.data.spatialSpreadIntensityStrength;
            to.internals.data.spatialSpreadIntensityCurve = from.internals.data.spatialSpreadIntensityCurve;

            // Stereo Pan
            to.internals.data.stereoPanExpand = from.internals.data.stereoPanExpand;
            to.internals.data.stereoPanOffset = from.internals.data.stereoPanOffset;
            to.internals.data.stereoPanAngleUse = from.internals.data.stereoPanAngleUse;
            to.internals.data.stereoPanAngleAmount = from.internals.data.stereoPanAngleAmount;
            to.internals.data.stereoPanAngleRolloff = from.internals.data.stereoPanAngleRolloff;

            // Reverb Zone Mix
            to.internals.data.reverbZoneMixExpand = from.internals.data.reverbZoneMixExpand;
            // Used in the editor
            to.internals.data.reverbZoneMixDecibel = from.internals.data.reverbZoneMixDecibel;
            // Max value is +10 dB (3.1622776601683795 ratio)
            // Is scaled later so 1.1 is +10dB
            to.internals.data.reverbZoneMixRatio = from.internals.data.reverbZoneMixRatio;

            to.internals.data.reverbZoneMixDistanceIncrease = from.internals.data.reverbZoneMixDistanceIncrease;
            to.internals.data.reverbZoneMixDistanceRolloff = from.internals.data.reverbZoneMixDistanceRolloff;
            to.internals.data.reverbZoneMixDistanceCurve = from.internals.data.reverbZoneMixDistanceCurve;

            to.internals.data.reverbZoneMixIntensityEnable = from.internals.data.reverbZoneMixIntensityEnable;
            to.internals.data.reverbZoneMixIntensityRolloff = from.internals.data.reverbZoneMixIntensityRolloff;
            to.internals.data.reverbZoneMixIntensityAmount = from.internals.data.reverbZoneMixIntensityAmount;
            to.internals.data.reverbZoneMixIntensityCurve = from.internals.data.reverbZoneMixIntensityCurve;

            // Distortion
            to.internals.data.distortionExpand = from.internals.data.distortionExpand;
            to.internals.data.distortionEnabled = from.internals.data.distortionEnabled;
            // Range to 1f
            to.internals.data.distortionAmount = from.internals.data.distortionAmount;

            to.internals.data.distortionDistanceEnable = from.internals.data.distortionDistanceEnable;
            to.internals.data.distortionDistanceRolloff = from.internals.data.distortionDistanceRolloff;
            to.internals.data.distortionDistanceCurve = from.internals.data.distortionDistanceCurve;

            to.internals.data.distortionIntensityEnable = from.internals.data.distortionIntensityEnable;
            to.internals.data.distortionIntensityRolloff = from.internals.data.distortionIntensityRolloff;
            to.internals.data.distortionIntensityStrength = from.internals.data.distortionIntensityStrength;
            to.internals.data.distortionIntensityCurve = from.internals.data.distortionIntensityCurve;

            // Lowpass
            to.internals.data.lowpassExpand = from.internals.data.lowpassExpand;
            to.internals.data.lowpassEnabled = from.internals.data.lowpassEnabled;
            // Range 20 to 20000 hz = engine * 19980f + 20f
            to.internals.data.lowpassFrequencyEditor = from.internals.data.lowpassFrequencyEditor;
            // Range to = (editor - 20f) / 19980f
            to.internals.data.lowpassFrequencyEngine = from.internals.data.lowpassFrequencyEngine;
            // Range to 6 dB = engine * 6f
            to.internals.data.lowpassAmountEditor = from.internals.data.lowpassAmountEditor;
            // Range to = -editor / 6f
            to.internals.data.lowpassAmountEngine = from.internals.data.lowpassAmountEngine;

            to.internals.data.lowpassDistanceEnable = from.internals.data.lowpassDistanceEnable;
            to.internals.data.lowpassDistanceFrequencyRolloff = from.internals.data.lowpassDistanceFrequencyRolloff;
            to.internals.data.lowpassDistanceFrequencyCurve = from.internals.data.lowpassDistanceFrequencyCurve;
            to.internals.data.lowpassDistanceAmountRolloff = from.internals.data.lowpassDistanceAmountRolloff;
            to.internals.data.lowpassDistanceAmountCurve = from.internals.data.lowpassDistanceAmountCurve;

            to.internals.data.lowpassIntensityEnable = from.internals.data.lowpassIntensityEnable;
            to.internals.data.lowpassIntensityFrequencyRolloff = from.internals.data.lowpassIntensityFrequencyRolloff;
            to.internals.data.lowpassIntensityFrequencyStrength = from.internals.data.lowpassIntensityFrequencyStrength;
            to.internals.data.lowpassIntensityFrequencyCurve = from.internals.data.lowpassIntensityFrequencyCurve;
            to.internals.data.lowpassIntensityAmountRolloff = from.internals.data.lowpassIntensityAmountRolloff;
            to.internals.data.lowpassIntensityAmountStrength = from.internals.data.lowpassIntensityAmountStrength;
            to.internals.data.lowpassIntensityAmountCurve = from.internals.data.lowpassIntensityAmountCurve;

            // Highpass
            to.internals.data.highpassExpand = from.internals.data.highpassExpand;
            to.internals.data.highpassEnabled = from.internals.data.highpassEnabled;
            // Range 20 to 20000 hz = engine * 19980f + 20f
            to.internals.data.highpassFrequencyEditor = from.internals.data.highpassFrequencyEditor;
            // Range to = (editor - 20f) / 19980f
            to.internals.data.highpassFrequencyEngine = from.internals.data.highpassFrequencyEngine;
            // Range to 6 dB = engine * 6f
            to.internals.data.highpassAmountEditor = from.internals.data.highpassAmountEditor;
            // Range to = -editor / 6f
            to.internals.data.highpassAmountEngine = from.internals.data.highpassAmountEngine;

            to.internals.data.highpassDistanceEnable = from.internals.data.highpassDistanceEnable;
            to.internals.data.highpassDistanceFrequencyRolloff = from.internals.data.highpassDistanceFrequencyRolloff;
            to.internals.data.highpassDistanceFrequencyCurve = from.internals.data.highpassDistanceFrequencyCurve;
            to.internals.data.highpassDistanceAmountRolloff = from.internals.data.highpassDistanceAmountRolloff;
            to.internals.data.highpassDistanceAmountCurve = from.internals.data.highpassDistanceAmountCurve;

            to.internals.data.highpassIntensityEnable = from.internals.data.highpassIntensityEnable;
            to.internals.data.highpassIntensityFrequencyRolloff = from.internals.data.highpassIntensityFrequencyRolloff;
            to.internals.data.highpassIntensityFrequencyStrength = from.internals.data.highpassIntensityFrequencyStrength;
            to.internals.data.highpassIntensityFrequencyCurve = from.internals.data.highpassIntensityFrequencyCurve;
            to.internals.data.highpassIntensityAmountRolloff = from.internals.data.highpassIntensityAmountRolloff;
            to.internals.data.highpassIntensityAmountStrength = from.internals.data.highpassIntensityAmountStrength;
            to.internals.data.highpassIntensityAmountCurve = from.internals.data.highpassIntensityAmountCurve;
        }
    }
}