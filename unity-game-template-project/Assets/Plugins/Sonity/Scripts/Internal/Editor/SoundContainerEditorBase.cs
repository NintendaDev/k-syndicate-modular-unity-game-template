// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    public abstract class SoundContainerEditorBase : Editor {

        public SoundContainerBase mTarget;
        public SoundContainerBase[] mTargets;

        public EditorPreviewControls previewEditorSetting = new EditorPreviewControls();

        public SoundContainerEditorCurveDraw curveDraw;
        public SoundContainerEditorPreview preview;
        public SoundContainerEditorFindAssets updateAudioClips;

        public SerializedProperty previewAudioMixerGroup;

        public SerializedProperty expandPreview;
        public SerializedProperty expandAudioClips;
        public SerializedProperty settingsExpandBase;
        public SerializedProperty settingsExpandAdvanced;
        public SerializedProperty previewCurves;

        public SerializedProperty audioClips;

        public SerializedProperty assetGuid;

        public SerializedProperty internals;
        public SerializedProperty data;

        public SerializedProperty notes;

        public SerializedProperty foundReferences;

        public SerializedProperty neverStealVoice;
        public SerializedProperty neverStealVoiceEffects;

        public SerializedProperty startPosition;

        public SerializedProperty reverse;

        public SerializedProperty dopplerAmount;

        public SerializedProperty reverbZoneMixExpand;
        public SerializedProperty reverbZoneMixDecibel;
        public SerializedProperty reverbZoneMixRatio;

        public SerializedProperty reverbZoneMixDistanceIncrease;
        public SerializedProperty reverbZoneMixDistanceRolloff;
        public SerializedProperty reverbZoneMixDistanceCurve;

        public SerializedProperty reverbZoneMixIntensityEnable;
        public SerializedProperty reverbZoneMixIntensityRolloff;
        public SerializedProperty reverbZoneMixIntensityAmount;
        public SerializedProperty reverbZoneMixIntensityCurve;

        public SerializedProperty stereoPanExpand;
        public SerializedProperty stereoPanOffset;
        public SerializedProperty stereoPanAngleUse;
        public SerializedProperty stereoPanAngleAmount;
        public SerializedProperty stereoPanAngleRolloff;

        public SerializedProperty bypassReverbZones;
        public SerializedProperty bypassVoiceEffects;
        public SerializedProperty bypassListenerEffects;

        public SerializedProperty hrtfPluginSpatialize;
        public SerializedProperty hrtfPluginSpatializePostEffects;

        public SerializedProperty audioMixerGroup;
        public SerializedProperty preventEndClicks;
        public SerializedProperty loopEnabled;
        public SerializedProperty followPosition;
        public SerializedProperty randomStartPosition;
        public SerializedProperty randomStartPositionMin;
        public SerializedProperty randomStartPositionMax;
        public SerializedProperty stopIfTransformIsNull;
        //public SerializedProperty virtualize; // Virtualize Todo

        public SerializedProperty priority;

        public SerializedProperty lockAxisEnable;
        public SerializedProperty lockAxis;
        public SerializedProperty lockAxisPosition;
        public SerializedProperty playOrder;

        public SerializedProperty distanceEnabled;
        public SerializedProperty distanceScale;

        public SerializedProperty volumeExpand;

        public SerializedProperty volumeDecibel;
        public SerializedProperty volumeRatio;

        public SerializedProperty volumeRandomEnable;
        public SerializedProperty volumeRandomRangeDecibel;

        public SerializedProperty volumeDistanceRolloff;
        public SerializedProperty volumeDistanceCurve;

        public SerializedProperty volumeIntensityEnable;
        public SerializedProperty volumeIntensityRolloff;
        public SerializedProperty volumeIntensityStrength;
        public SerializedProperty volumeIntensityCurve;

        public SerializedProperty volumeDistanceCrossfadeEnable;
        public SerializedProperty volumeDistanceCrossfadeTotalLayersOneBased;
        public SerializedProperty volumeDistanceCrossfadeLayerOneBased;
        public SerializedProperty volumeDistanceCrossfadeTotalLayers;
        public SerializedProperty volumeDistanceCrossfadeLayer;
        public SerializedProperty volumeDistanceCrossfadeRolloff;
        public SerializedProperty volumeDistanceCrossfadeCurve;

        public SerializedProperty volumeIntensityCrossfadeEnable;
        public SerializedProperty volumeIntensityCrossfadeTotalLayersOneBased;
        public SerializedProperty volumeIntensityCrossfadeLayerOneBased;
        public SerializedProperty volumeIntensityCrossfadeTotalLayers;
        public SerializedProperty volumeIntensityCrossfadeLayer;
        public SerializedProperty volumeIntensityCrossfadeRolloff;
        public SerializedProperty volumeIntensityCrossfadeCurve;

        public SerializedProperty spatialBlendExpand;
        public SerializedProperty spatialBlend;

        public SerializedProperty spatialBlendDistanceRolloff;
        public SerializedProperty spatialBlendDistance3DIncrease;
        public SerializedProperty spatialBlendDistanceCurve;

        public SerializedProperty spatialBlendIntensityEnable;
        public SerializedProperty spatialBlendIntensityRolloff;
        public SerializedProperty spatialBlendIntensityStrength;
        public SerializedProperty spatialBlendIntensityCurve;

        public SerializedProperty spatialSpreadExpand;
        public SerializedProperty spatialSpreadDegrees;
        public SerializedProperty spatialSpreadRatio;

        public SerializedProperty spatialSpreadDistanceRolloff;
        public SerializedProperty spatialSpreadDistanceCurve;

        public SerializedProperty spatialSpreadIntensityEnable;
        public SerializedProperty spatialSpreadIntensityRolloff;
        public SerializedProperty spatialSpreadIntensityStrength;
        public SerializedProperty spatialSpreadIntensityCurve;

        public SerializedProperty distortionExpand;
        public SerializedProperty distortionEnabled;
        public SerializedProperty distortionAmount;

        public SerializedProperty distortionDistanceEnable;
        public SerializedProperty distortionDistanceRolloff;
        public SerializedProperty distortionDistanceCurve;

        public SerializedProperty distortionIntensityEnable;
        public SerializedProperty distortionIntensityRolloff;
        public SerializedProperty distortionIntensityStrength;
        public SerializedProperty distortionIntensityCurve;

        public SerializedProperty lowpassExpand;
        public SerializedProperty lowpassEnabled;
        public SerializedProperty lowpassFrequencyEditor;
        public SerializedProperty lowpassFrequencyEngine;
        public SerializedProperty lowpassAmountEditor;
        public SerializedProperty lowpassAmountEngine;

        public SerializedProperty lowpassDistanceEnable;
        public SerializedProperty lowpassDistanceFrequencyRolloff;
        public SerializedProperty lowpassDistanceFrequencyCurve;
        public SerializedProperty lowpassDistanceAmountRolloff;
        public SerializedProperty lowpassDistanceAmountCurve;

        public SerializedProperty lowpassIntensityEnable;
        public SerializedProperty lowpassIntensityFrequencyRolloff;
        public SerializedProperty lowpassIntensityFrequencyStrength;
        public SerializedProperty lowpassIntensityFrequencyCurve;
        public SerializedProperty lowpassIntensityAmountRolloff;
        public SerializedProperty lowpassIntensityAmountStrength;
        public SerializedProperty lowpassIntensityAmountCurve;

        public SerializedProperty highpassExpand;
        public SerializedProperty highpassEnabled;
        public SerializedProperty highpassFrequencyEditor;
        public SerializedProperty highpassFrequencyEngine;
        public SerializedProperty highpassAmountEditor;
        public SerializedProperty highpassAmountEngine;

        public SerializedProperty highpassDistanceEnable;
        public SerializedProperty highpassDistanceFrequencyRolloff;
        public SerializedProperty highpassDistanceFrequencyCurve;
        public SerializedProperty highpassDistanceAmountRolloff;
        public SerializedProperty highpassDistanceAmountCurve;

        public SerializedProperty highpassIntensityEnable;
        public SerializedProperty highpassIntensityFrequencyRolloff;
        public SerializedProperty highpassIntensityFrequencyStrength;
        public SerializedProperty highpassIntensityFrequencyCurve;
        public SerializedProperty highpassIntensityAmountRolloff;
        public SerializedProperty highpassIntensityAmountStrength;
        public SerializedProperty highpassIntensityAmountCurve;

        public SerializedProperty pitchExpand;
        public SerializedProperty pitchSemitoneEditor;
        public SerializedProperty pitchRatio;
        public SerializedProperty pitchRandomEnable;
        public SerializedProperty pitchRandomRangeSemitone;

        public SerializedProperty pitchIntensityEnable;
        public SerializedProperty pitchIntensityLowSemitone;
        public SerializedProperty pitchIntensityLowRatio;
        public SerializedProperty pitchIntensityHighSemitone;
        public SerializedProperty pitchIntensityHighRatio;
        public SerializedProperty pitchIntensityRangeSemitone;
        public SerializedProperty pitchIntensityRangeRatio;
        public SerializedProperty pitchIntensityBaseSemitone;
        public SerializedProperty pitchIntensityBaseRatio;
        public SerializedProperty pitchIntensityRolloff;
        public SerializedProperty pitchIntensityCurve;

        private float guiCurveHeight = 25f;

        [NonSerialized]
        private bool initialized;

        // The material to use when drawing with OpenGL
        public Material cachedMaterial;

        private void OnEnable() {
            // Cache the "Hidden/Internal-Colored" shader
            cachedMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        private void FindProperties() {

            assetGuid = serializedObject.FindProperty(nameof(SoundContainerBase.assetGuid));

            internals = serializedObject.FindProperty(nameof(SoundContainerBase.internals));

            audioClips = internals.FindPropertyRelative(nameof(SoundContainerBase.internals.audioClips));

            data = internals.FindPropertyRelative(nameof(SoundContainerBase.internals.data));

            notes = internals.FindPropertyRelative(nameof(SoundContainerBase.internals.notes));

            foundReferences = data.FindPropertyRelative(nameof(SoundContainerInternalsData.foundReferences));

            previewAudioMixerGroup = data.FindPropertyRelative(nameof(SoundContainerInternalsData.previewAudioMixerGroup));

            expandPreview = data.FindPropertyRelative(nameof(SoundContainerInternalsData.expandPreview));
            expandAudioClips = data.FindPropertyRelative(nameof(SoundContainerInternalsData.expandAudioClips));
            settingsExpandBase = data.FindPropertyRelative(nameof(SoundContainerInternalsData.settingsExpandBase)); ;
            settingsExpandAdvanced = data.FindPropertyRelative(nameof(SoundContainerInternalsData.settingsExpandAdvanced)); 

            previewCurves = data.FindPropertyRelative(nameof(SoundContainerInternalsData.previewCurves));

            neverStealVoice = data.FindPropertyRelative(nameof(SoundContainerInternalsData.neverStealVoice));
            neverStealVoiceEffects = data.FindPropertyRelative(nameof(SoundContainerInternalsData.neverStealVoiceEffects));

            startPosition = data.FindPropertyRelative(nameof(SoundContainerInternalsData.startPosition));

            reverse = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverse));

            dopplerAmount = data.FindPropertyRelative(nameof(SoundContainerInternalsData.dopplerAmount));

            reverbZoneMixExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixExpand));
            reverbZoneMixDecibel = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixDecibel));
            reverbZoneMixRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixRatio));

            reverbZoneMixDistanceIncrease = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixDistanceIncrease));
            reverbZoneMixDistanceRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixDistanceRolloff));
            reverbZoneMixDistanceCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixDistanceCurve));

            reverbZoneMixIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixIntensityEnable));
            reverbZoneMixIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixIntensityRolloff));
            reverbZoneMixIntensityAmount = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixIntensityAmount));
            reverbZoneMixIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.reverbZoneMixIntensityCurve));

            stereoPanExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stereoPanExpand));
            stereoPanOffset = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stereoPanOffset));
            stereoPanAngleUse = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stereoPanAngleUse));
            stereoPanAngleAmount = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stereoPanAngleAmount));
            stereoPanAngleRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stereoPanAngleRolloff));

            bypassReverbZones = data.FindPropertyRelative(nameof(SoundContainerInternalsData.bypassReverbZones));
            bypassVoiceEffects = data.FindPropertyRelative(nameof(SoundContainerInternalsData.bypassVoiceEffects));
            bypassListenerEffects = data.FindPropertyRelative(nameof(SoundContainerInternalsData.bypassListenerEffects));

            hrtfPluginSpatialize = data.FindPropertyRelative(nameof(SoundContainerInternalsData.hrtfPluginSpatialize));
            hrtfPluginSpatializePostEffects = data.FindPropertyRelative(nameof(SoundContainerInternalsData.hrtfPluginSpatializePostEffects));

            preventEndClicks = data.FindPropertyRelative(nameof(SoundContainerInternalsData.preventEndClicks));
            loopEnabled = data.FindPropertyRelative(nameof(SoundContainerInternalsData.loopEnabled));
            followPosition = data.FindPropertyRelative(nameof(SoundContainerInternalsData.followPosition));
            stopIfTransformIsNull = data.FindPropertyRelative(nameof(SoundContainerInternalsData.stopIfTransformIsNull));
            //virtualize = data.FindPropertyRelative(nameof(SoundContainerInternalsData.virtualize)); // Virtualize Todo
            randomStartPosition = data.FindPropertyRelative(nameof(SoundContainerInternalsData.randomStartPosition));
            randomStartPositionMin = data.FindPropertyRelative(nameof(SoundContainerInternalsData.randomStartPositionMin));
            randomStartPositionMax = data.FindPropertyRelative(nameof(SoundContainerInternalsData.randomStartPositionMax));
            priority = data.FindPropertyRelative(nameof(SoundContainerInternalsData.priority));
            lockAxisEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lockAxisEnable));
            lockAxis = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lockAxis));
            lockAxisPosition = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lockAxisPosition));
            playOrder = data.FindPropertyRelative(nameof(SoundContainerInternalsData.playOrder));

            distanceEnabled = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distanceEnabled));
            distanceScale = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distanceScale));

            volumeExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeExpand));
            volumeDecibel = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDecibel));
            volumeRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeRatio));

            volumeRandomEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeRandomEnable));
            volumeRandomRangeDecibel = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeRandomRangeDecibel));

            volumeDistanceRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceRolloff));
            volumeDistanceCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCurve));

            volumeIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityEnable));
            volumeIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityRolloff));
            volumeIntensityStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityStrength));
            volumeIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCurve));

            volumeDistanceCrossfadeEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeEnable));
            volumeDistanceCrossfadeTotalLayersOneBased = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeTotalLayersOneBased));
            volumeDistanceCrossfadeLayerOneBased = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeLayerOneBased));
            volumeDistanceCrossfadeTotalLayers = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeTotalLayers));
            volumeDistanceCrossfadeLayer = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeLayer));
            volumeDistanceCrossfadeRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeRolloff));
            volumeDistanceCrossfadeCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeDistanceCrossfadeCurve));

            volumeIntensityCrossfadeEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeEnable));
            volumeIntensityCrossfadeTotalLayersOneBased = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeTotalLayersOneBased));
            volumeIntensityCrossfadeLayerOneBased = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeLayerOneBased));
            volumeIntensityCrossfadeTotalLayers = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeTotalLayers));
            volumeIntensityCrossfadeLayer = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeLayer));
            volumeIntensityCrossfadeRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeRolloff));
            volumeIntensityCrossfadeCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.volumeIntensityCrossfadeCurve));

            spatialBlendExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendExpand));
            spatialBlend = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlend));

            spatialBlendDistanceRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendDistanceRolloff));
            spatialBlendDistance3DIncrease = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendDistance3DIncrease));
            spatialBlendDistanceCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendDistanceCurve));

            spatialBlendIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendIntensityEnable));
            spatialBlendIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendIntensityRolloff));
            spatialBlendIntensityStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendIntensityStrength));
            spatialBlendIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialBlendIntensityCurve));

            spatialSpreadExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadExpand));
            spatialSpreadDegrees = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadDegrees));
            spatialSpreadRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadRatio));

            spatialSpreadDistanceRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadDistanceRolloff));
            spatialSpreadDistanceCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadDistanceCurve));

            spatialSpreadIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadIntensityEnable));
            spatialSpreadIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadIntensityRolloff));
            spatialSpreadIntensityStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadIntensityStrength));
            spatialSpreadIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.spatialSpreadIntensityCurve));

            distortionExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionExpand));
            distortionEnabled = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionEnabled));
            distortionAmount = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionAmount));

            distortionDistanceEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionDistanceEnable));
            distortionDistanceRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionDistanceRolloff));
            distortionDistanceCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionDistanceCurve));

            distortionIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionIntensityEnable));
            distortionIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionIntensityRolloff));
            distortionIntensityStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionIntensityStrength));
            distortionIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.distortionIntensityCurve));

            lowpassExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassExpand));
            lowpassEnabled = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassEnabled));
            lowpassFrequencyEditor = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassFrequencyEditor));
            lowpassFrequencyEngine = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassFrequencyEngine));
            lowpassAmountEditor = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassAmountEditor));
            lowpassAmountEngine = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassAmountEngine));

            lowpassDistanceEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassDistanceEnable));
            lowpassDistanceFrequencyRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassDistanceFrequencyRolloff));
            lowpassDistanceFrequencyCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassDistanceFrequencyCurve));
            lowpassDistanceAmountRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassDistanceAmountRolloff));
            lowpassDistanceAmountCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassDistanceAmountCurve));

            lowpassIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityEnable));
            lowpassIntensityFrequencyRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityFrequencyRolloff));
            lowpassIntensityFrequencyStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityFrequencyStrength));
            lowpassIntensityFrequencyCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityFrequencyCurve));
            lowpassIntensityAmountRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityAmountRolloff));
            lowpassIntensityAmountStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityAmountStrength));
            lowpassIntensityAmountCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.lowpassIntensityAmountCurve));

            highpassExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassExpand));
            highpassEnabled = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassEnabled));
            highpassFrequencyEditor = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassFrequencyEditor));
            highpassFrequencyEngine = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassFrequencyEngine));
            highpassAmountEditor = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassAmountEditor));
            highpassAmountEngine = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassAmountEngine));

            highpassDistanceEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassDistanceEnable));
            highpassDistanceFrequencyRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassDistanceFrequencyRolloff));
            highpassDistanceFrequencyCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassDistanceFrequencyCurve));
            highpassDistanceAmountRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassDistanceAmountRolloff));
            highpassDistanceAmountCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassDistanceAmountCurve));

            highpassIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityEnable));
            highpassIntensityFrequencyRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityFrequencyRolloff));
            highpassIntensityFrequencyStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityFrequencyStrength));
            highpassIntensityFrequencyCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityFrequencyCurve));
            highpassIntensityAmountRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityAmountRolloff));
            highpassIntensityAmountStrength = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityAmountStrength));
            highpassIntensityAmountCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.highpassIntensityAmountCurve));

            pitchExpand = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchExpand));
            pitchSemitoneEditor = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchSemitone));
            pitchRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchRatio));
            pitchRandomEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchRandomEnable));
            pitchRandomRangeSemitone = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchRandomRangeSemitoneBipolar));

            pitchIntensityEnable = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityEnable));
            pitchIntensityLowSemitone = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityLowSemitone));
            pitchIntensityLowRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityLowRatio));
            pitchIntensityHighSemitone = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityHighSemitone));
            pitchIntensityHighRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityHighRatio));
            pitchIntensityRangeSemitone = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityRangeSemitone));
            pitchIntensityRangeRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityRangeRatio));
            pitchIntensityBaseSemitone = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityBaseSemitone));
            pitchIntensityBaseRatio = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityBaseRatio));
            pitchIntensityRolloff = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityRolloff));
            pitchIntensityCurve = data.FindPropertyRelative(nameof(SoundContainerInternalsData.pitchIntensityCurve));
        }

        public void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        public void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private GUIStyle guiStyleBoldCenter = new GUIStyle();
        private Color defaultGuiColor;

        // To get rid of errors when multi-selecting and moving items up/down, creating a duplicate
        private bool verticalBegun = false;

        public void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
            verticalBegun = true;
        }

        public void StopBackgroundColor() {
            if (verticalBegun) {
                EditorGUILayout.EndVertical();
            }
            verticalBegun = false;
        }

        public void InitializeEditors() {
            curveDraw = ScriptableObject.CreateInstance<SoundContainerEditorCurveDraw>();
            curveDraw.Initialize(this);
            preview = ScriptableObject.CreateInstance<SoundContainerEditorPreview>();
            preview.Initialize(this);
            updateAudioClips = ScriptableObject.CreateInstance<SoundContainerEditorFindAssets>();
            updateAudioClips.Initialize(this);
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundContainerBase)target;

            mTargets = new SoundContainerBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundContainerBase)targets[i];
            }

            if (!initialized) {
                initialized = true;
                FindProperties();
                InitializeEditors();
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundContainer, EditorTextSoundContainer.soundContainerTooltip, true);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            GuiNotes();
            GuiPresetsMenu();

            preview.Preview();
            EditorGUILayout.Separator();

            // If properties are not found, then find properties
            try {
                if (loopEnabled.boolValue == true) {
                }
            } catch {
                FindProperties();
            }

            EditorGUI.indentLevel = 1;

            GuiAudioClips();
            GuiSettingsBase();
            GuiVolume();
            GuiPitch();
            GuiSpatialBlend();
            GuiSpatialSpread();
            GuiStereoPan();
            GuiReverbZoneMix();
            GuiDistortion();
            GuiLowpass();
            GuiHighpass();
            GuiReset();
            GuiFindReferences();

            // Asset GUID
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            BeginChange();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(assetGuid, new GUIContent(EditorTextAssetGuid.assetGuidLabel, EditorTextAssetGuid.assetGuidTooltip));
            for (int i = 0; i < mTargets.Length; i++) {
                string assetGuidTemp = EditorAssetGuid.GetAssetGuid(mTargets[i]);
                long assetGuidHashTemp = EditorAssetGuid.GetInt64HashFromString(assetGuidTemp);
                if (mTargets[i].assetGuid != assetGuidTemp || mTargets[i].assetGuidHash != assetGuidHashTemp) {
                    mTargets[i].assetGuid = assetGuidTemp;
                    mTargets[i].assetGuidHash = assetGuidHashTemp;
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EditorGUI.EndDisabledGroup();
            EndChange();
            StopBackgroundColor();
        }

        private void GuiNotes() {
            // Notes
            EditorGUI.indentLevel = 0;
            Color previousColor = GUI.color;
            if (string.IsNullOrEmpty(notes.stringValue) || notes.stringValue == "Notes") {
                // Make less transparent if empty or default text
                GUI.color = new Color(1f, 1f, 1f, 0.4f);
            }
            BeginChange();
            notes.stringValue = EditorGUILayout.TextArea(notes.stringValue);
            EndChange();
            GUI.color = previousColor;
        }

        private void DragAndDropCallback<T>(T[] draggedObjects) where T : UnityEngine.Object {
            AudioClip[] newObjects = draggedObjects as AudioClip[];
            // If there are any objects of the right type dragged
            if (newObjects.Length > 0) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], $"Drag and Dropped {nameof(AudioClip)}");
                    mTargets[i].internals.audioClips = new AudioClip[newObjects.Length];
                    for (int ii = 0; ii < newObjects.Length; ii++) {
                        mTargets[i].internals.audioClips[ii] = newObjects[ii];
                    }
                    // Expands the audioClip array
                    audioClips.isExpanded = true;
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
        }

        private void GuiAudioClips() {

            EditorGUI.indentLevel = 1;
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(expandAudioClips, "AudioClips");
            EndChange();

            if (expandAudioClips.boolValue) {

                // Menu for updating AudioClips
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.updateAudioClipsLabel, EditorTextSoundContainer.updateAudioClipsTooltip))) {
                    updateAudioClips.MenuFindAsset();
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // Audio Clip
                int lowestArrayLength = int.MaxValue;
                for (int n = 0; n < mTargets.Length; n++) {
                    if (lowestArrayLength > mTargets[n].internals.audioClips.Length) {
                        lowestArrayLength = mTargets[n].internals.audioClips.Length;
                    }
                }
                EditorGUI.indentLevel = 1;
                EditorGuiFunction.DrawReordableArray(audioClips, serializedObject, lowestArrayLength, false);

                EditorDragAndDropArea.DrawDragAndDropAreaCustomEditor<AudioClip>(new EditorDragAndDropArea.DragAndDropAreaInfo($"{nameof(AudioClip)}"), DragAndDropCallback);
            }

            if (ShouldDebug.GuiWarnings()) {
                // Waring if null/empty AudioClips
                if (mTarget.internals.audioClips.Length == 0) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.HelpBox(EditorTextSoundContainer.audioClipWarningEmpty, MessageType.Warning);
                    EditorGUILayout.Separator();
                } else {
                    bool audioClipsNull = false;
                    for (int i = 0; i < mTarget.internals.audioClips.Length; i++) {
                        if (mTarget.internals.audioClips[i] == null) {
                            audioClipsNull = true;
                            break;
                        }
                    }
                    if (audioClipsNull) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundContainer.audioClipWarningNull, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }
        
        private void GuiPresetsMenu() {
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.presetsLabel, EditorTextSoundContainer.presetsTooltip))) {
                PresetsMenuDraw();
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private enum PresetType {
            SFX3D = 0,
            SFX2D = 1,
            Music = 2,
            Looping = 3,
            Crossfades = 4,
            PresetSame = 5,
            PresetMatchName = 6,
        }

        private class PresetsMenuObject {
            public PresetType presetType;
            public SoundContainerBase soundContainerPreset;

            public PresetsMenuObject(PresetType presetType, SoundContainerBase soundContainerPreset = null) {
                this.presetType = presetType;
                this.soundContainerPreset = soundContainerPreset;
            }
        }

        private void PresetsMenuDraw() {
            GenericMenu menu = new GenericMenu();

            // Tooltips dont work for menu
            menu.AddItem(new GUIContent("Apply SFX 3D Settings"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.SFX3D));
            menu.AddItem(new GUIContent("Apply SFX 2D Settings"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.SFX2D));
            menu.AddItem(new GUIContent("Apply Music Settings"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.Music));
            menu.AddItem(new GUIContent("Automatic Looping"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.Looping));
            menu.AddItem(new GUIContent("Automatic Crossfades"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.Crossfades));

            menu.AddSeparator("");

            bool anyMenuItemAdded = false;

            SoundPresetFind.FindAllSoundPresets();
            if (SoundPresetFind.soundPresets != null && SoundPresetFind.soundPresets.Length > 0) {
                for (int i = 0; i < SoundPresetFind.soundPresets.Length; i++) {
                    SoundPresetBase soundPreset = SoundPresetFind.soundPresets[i];
                    if (soundPreset != null && !soundPreset.internals.disableAll) {
                        for (int ii = 0; ii < SoundPresetFind.soundPresets[i].internals.soundPresetGroup.Length; ii++) {
                            SoundPresetGroup soundPresetGroup = SoundPresetFind.soundPresets[i].internals.soundPresetGroup[ii];
                            if (soundPresetGroup != null && !soundPresetGroup.disable && soundPresetGroup.soundContainerPreset != null) {
                                anyMenuItemAdded = true;
                                string parentName = "Preset - " + soundPresetGroup.soundContainerPreset.name;
                                menu.AddItem(new GUIContent(parentName), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.PresetSame, soundPresetGroup.soundContainerPreset));
                            }
                        }
                    }
                }
                if (anyMenuItemAdded) {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Preset - Auto Match"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.PresetMatchName));
                } else {
                    menu.AddItem(new GUIContent($"All {NameOf.SoundPreset}s are disabled or empty"), false, PresetsMenuCallback, null);
                }
            } else {
                menu.AddItem(new GUIContent($"Make custom presets using the {NameOf.SoundPreset} object"), false, PresetsMenuCallback, null);
            }

            menu.ShowAsContext();
        }

        private void PresetsMenuCallback(object obj) {
            try {
                PresetsMenuObject menuObject = (PresetsMenuObject)obj;
                if (menuObject == null) {
                    return;
                }
                // If updating this, update the tooltip and documentation also
                if (menuObject.presetType == PresetType.SFX3D) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Set to SFX 3D");
                        mTargets[i].internals.data.distanceEnabled = true;
                        mTargets[i].internals.data.spatialBlend = 1f;
                        mTargets[i].internals.data.neverStealVoice = false;
                        mTargets[i].internals.data.neverStealVoiceEffects = false;
                        mTargets[i].internals.data.pitchRandomEnable = true;
                        mTargets[i].internals.data.priority = 0.5f;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                else if (menuObject.presetType == PresetType.SFX2D) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Set to SFX 2D");
                        mTargets[i].internals.data.distanceEnabled = false;
                        mTargets[i].internals.data.spatialBlend = 0f;
                        mTargets[i].internals.data.neverStealVoice = false;
                        mTargets[i].internals.data.neverStealVoiceEffects = false;
                        mTargets[i].internals.data.pitchRandomEnable = true;
                        mTargets[i].internals.data.priority = 0.5f;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                else if (menuObject.presetType == PresetType.Music) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Set to Music Settings");
                        mTargets[i].internals.data.distanceEnabled = false;
                        mTargets[i].internals.data.spatialBlend = 0f;
                        mTargets[i].internals.data.neverStealVoice = true;
                        mTargets[i].internals.data.neverStealVoiceEffects = true;
                        mTargets[i].internals.data.pitchRandomEnable = false;
                        mTargets[i].internals.data.volumeRandomEnable = false;
                        mTargets[i].internals.data.priority = 1;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                } 
                else if (menuObject.presetType == PresetType.Looping) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Automatic Looping");
                        if (mTargets[i].name.ToLowerInvariant().Contains("loop")) {
                            mTargets[i].internals.data.loopEnabled = true;
                            mTargets[i].internals.data.followPosition = true;
                            mTargets[i].internals.data.randomStartPosition = true;
                            mTargets[i].internals.data.stopIfTransformIsNull = true;
                            //mTargets[i].internals.data.virtualize = true; // Virtualize Todo
                        }
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                } 
                else if (menuObject.presetType == PresetType.Crossfades) {

                    if (mTargets.Length <= 1) {
                        return;
                    }

                    List<CrossfadeGroup> crossfadeGroups = new List<CrossfadeGroup>();

                    for (int i = 0; i < targets.Length; i++) {
                        string tempName = mTargets[i].name;
                        CrossfadePartType tempCrossfadeType = NameIsCrossfade(ref tempName);
                        if (tempCrossfadeType != CrossfadePartType.None) {
                            CrossfadePart newPart = new CrossfadePart();
                            newPart.crossfadePartType = tempCrossfadeType;
                            newPart.soundContainer = mTargets[i];

                            bool found = false;
                            for (int ii = 0; ii < crossfadeGroups.Count; ii++) {
                                if (crossfadeGroups[ii].name == tempName) {
                                    found = true;
                                    crossfadeGroups[ii].crossfadeParts.Add(newPart);
                                }
                            }
                            if (!found) {
                                CrossfadeGroup newGroup = new CrossfadeGroup();
                                newGroup.name = tempName;
                                newGroup.crossfadeParts.Add(newPart);
                                crossfadeGroups.Add(newGroup);
                            }
                        }
                    }

                    for (int i = 0; i < crossfadeGroups.Count; i++) {
                        bool containsDistanceClose = false;
                        bool containsDistanceDistant = false;
                        bool containsDistanceFar = false;
                        bool containsIntensitySoft = false;
                        bool containsIntensityMedium = false;
                        bool containsIntensityHard = false;

                        for (int ii = 0; ii < crossfadeGroups[i].crossfadeParts.Count; ii++) {
                            switch (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType) {
                                case CrossfadePartType.Close:
                                    containsDistanceClose = true;
                                    break;
                                case CrossfadePartType.Distant:
                                    containsDistanceDistant = true;
                                    break;
                                case CrossfadePartType.Far:
                                    containsDistanceFar = true;
                                    break;
                                case CrossfadePartType.Soft:
                                    containsIntensitySoft = true;
                                    break;
                                case CrossfadePartType.Medium:
                                    containsIntensityMedium = true;
                                    break;
                                case CrossfadePartType.Hard:
                                    containsIntensityHard = true;
                                    break;
                            }
                        }
                        for (int ii = 0; ii < crossfadeGroups[i].crossfadeParts.Count; ii++) {
                            if (containsDistanceClose && containsDistanceDistant && containsDistanceFar) {
                                if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Close) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 1);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Distant) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 2);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Far) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 3);
                                }
                            }
                            else if (containsDistanceClose && containsDistanceDistant) {
                                if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Close) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 1);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Distant) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 2);
                                }
                            } else if (containsDistanceClose && containsDistanceFar) {
                                if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Close) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 1);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Far) {
                                    SetCrossfadeDistance(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 2);
                                }
                            } else if (containsIntensitySoft && containsIntensityMedium && containsIntensityHard) {
                                if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Soft) {
                                    SetCrossfadeIntensity(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 1);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Medium) {
                                    SetCrossfadeIntensity(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 2);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Hard) {
                                    SetCrossfadeIntensity(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 3, 3);
                                }
                            } else if (containsIntensitySoft && containsIntensityHard) {
                                if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Soft) {
                                    SetCrossfadeIntensity(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 1);
                                } else if (crossfadeGroups[i].crossfadeParts[ii].crossfadePartType == CrossfadePartType.Hard) {
                                    SetCrossfadeIntensity(crossfadeGroups[i].crossfadeParts[ii].soundContainer, 2, 2);
                                }
                            }
                        }
                    }
                } else if (menuObject.presetType == PresetType.PresetSame) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Set to Preset Settings");
                        SoundContainerCopy.CopyTo(mTargets[i], menuObject.soundContainerPreset);
                        if (mTargets[i].name.ToLowerInvariant().Contains("loop")) {
                            mTargets[i].internals.data.loopEnabled = true;
                            mTargets[i].internals.data.followPosition = true;
                            mTargets[i].internals.data.randomStartPosition = true;
                            mTargets[i].internals.data.stopIfTransformIsNull = true;
                        }
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                } else if (menuObject.presetType == PresetType.PresetMatchName) {
                    SoundPresetFind.FindAllSoundPresets();
                    if (SoundPresetFind.soundPresets != null) {
                        for (int i = 0; i < SoundPresetFind.soundPresets.Length; i++) {
                            SoundPresetBase soundPreset = SoundPresetFind.soundPresets[i];
                            if (soundPreset != null && !soundPreset.internals.disableAll) {
                                for (int ii = 0; ii < soundPreset.internals.soundPresetGroup.Length; ii++) {
                                    SoundPresetGroup soundPresetGroup = soundPreset.internals.soundPresetGroup[ii];
                                    if (soundPresetGroup != null && !soundPresetGroup.ShouldUseMatch(true)) {
                                        for (int iii = 0; iii < mTargets.Length; iii++) {
                                            if (soundPresetGroup.GetNameMatches(mTargets[iii].name, true)) {
                                                Undo.RecordObject(mTargets[iii], "Set to Preset Settings");
                                                SoundContainerCopy.CopyTo(mTargets[iii], soundPresetGroup.soundContainerPreset);
                                                if (soundPreset.internals.automaticLoop && mTargets[iii].name.ToLowerInvariant().Contains("loop")) {
                                                    mTargets[iii].internals.data.loopEnabled = true;
                                                    mTargets[iii].internals.data.followPosition = true;
                                                    mTargets[iii].internals.data.randomStartPosition = true;
                                                    mTargets[iii].internals.data.stopIfTransformIsNull = true;
                                                }
                                                Debug.Log($"Sonity: Preset \"" + soundPresetGroup.soundContainerPreset.name + $"\" is applied to \"" + mTargets[iii].name + "\"", mTargets[iii]);
                                                EditorUtility.SetDirty(mTargets[iii]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch {
                return;
            }
        }

        private static string GetNameToRemoveWithRightSeparatorChar(string fileName, string nameToRemove) {
            if (fileName.EndsWith("_" + nameToRemove)) {
                return "_" + nameToRemove;
            } else if (fileName.EndsWith(" " + nameToRemove)) {
                return " " + nameToRemove;
            } else if (fileName.EndsWith("-" + nameToRemove)) {
                return "-" + nameToRemove;
            }
            return "_" + nameToRemove;
        }

        private CrossfadePartType NameIsCrossfade(ref string name) {

            name = name.ToLowerInvariant();
            name = RemoveEndString(name, "sc");
            name = RemoveTrailingJunk(name, false);
            name = RemoveEndString(name, "loop");
            name = RemoveTrailingJunk(name, false);

            CrossfadePartType tempCrossfadeType = CrossfadePartType.None;

            if (name.EndsWith("close")) {
                tempCrossfadeType = CrossfadePartType.Close;
            } else if (name.EndsWith("distant")) {
                tempCrossfadeType = CrossfadePartType.Distant;
            } else if (name.EndsWith("far")) {
                tempCrossfadeType = CrossfadePartType.Far;
            } else if (name.EndsWith("soft")) {
                tempCrossfadeType = CrossfadePartType.Soft;
            } else if (name.EndsWith("medium")) {
                tempCrossfadeType = CrossfadePartType.Medium;
            } else if (name.EndsWith("hard")) {
                tempCrossfadeType = CrossfadePartType.Hard;
            }

            name = RemoveEndString(name, tempCrossfadeType.ToString().ToLowerInvariant());
            name = RemoveTrailingJunk(name, false);

            return tempCrossfadeType;
        }

        enum CrossfadePartType {
            None = 0,
            Close = 1,
            Distant = 2,
            Far = 3,
            Soft = 4,
            Medium = 5,
            Hard = 6,
        }

        class CrossfadePart {
            public CrossfadePartType crossfadePartType = CrossfadePartType.None;
            public SoundContainerBase soundContainer;
        }

        class CrossfadeGroup {
            public string name = "";
            public List<CrossfadePart> crossfadeParts = new List<CrossfadePart>();
        }

        private void SetCrossfadeDistance(SoundContainerBase soundContainer, int layersOneIndexed, int thisIsOneIndexed) {
            Undo.RecordObject(soundContainer, "Automatic Crossfades");
            soundContainer.internals.data.SetVolumeDistanceCrossfade(layersOneIndexed, thisIsOneIndexed, true);
            EditorUtility.SetDirty(soundContainer);
        }

        private void SetCrossfadeIntensity(SoundContainerBase soundContainer, int layersOneIndexed, int thisIsOneIndexed) {
            Undo.RecordObject(soundContainer, "Automatic Crossfades");
            soundContainer.internals.data.SetVolumeIntensityCrossfade(layersOneIndexed, thisIsOneIndexed, true);
            EditorUtility.SetDirty(soundContainer);
        }

        private bool EndsWithCrossfade(string input) {
            if (input.EndsWith("close")) {
                return true;
            } else if (input.EndsWith("distant")) {
                return true;
            } else if (input.EndsWith("far")) {
                return true;
            } else if (input.EndsWith("soft")) {
                return true;
            } else if (input.EndsWith("medium")) {
                return true;
            } else if (input.EndsWith("hard")) {
                return true;
            }
            return false;
        }

        private string RemoveTrailingJunk(string input, bool removeNumbers) {
            char[] charsToRemove;
            if (removeNumbers) {
                charsToRemove = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };
            } else {
                charsToRemove = new char[] { ' ', '_', '-' };
            }
            return input.TrimEnd(charsToRemove);
        }

        private string RemoveEndString(string input, string toRemove) {
            if (input.EndsWith(toRemove)) {
                input = input.Remove(input.Length - toRemove.Length);
                
            }
            return input;
        }

        private void GuiSettingsBase() {

            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;

            BeginChange();
            EditorGuiFunction.DrawFoldout(settingsExpandBase, "Settings");
            EndChange();

            if (settingsExpandBase.boolValue) {

                // Distance Enabled
                BeginChange();
                EditorGUILayout.PropertyField(distanceEnabled, new GUIContent(EditorTextSoundContainer.enableDistanceLabel, EditorTextSoundContainer.enableDistanceTooltip));
                EndChange();

                // Distance Scale
                if (distanceEnabled.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.PropertyField(distanceScale, new GUIContent(EditorTextSoundContainer.distanceScaleLabel, EditorTextSoundContainer.distanceScaleTooltip));
                    if (distanceScale.floatValue < 0) {
                        distanceScale.floatValue = 0f;
                    }
                    EndChange();
                    if (distanceScale.floatValue == 0 && distanceEnabled.boolValue) {
                        EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.distanceScaleWarningLabel), EditorStyles.helpBox);
                    }
                    EditorGUI.indentLevel--;
                }

                // Loop
                BeginChange();
                bool tempLoopEnabled = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundContainer.loopLabel, EditorTextSoundContainer.loopTooltip), loopEnabled.boolValue);
                // Automatically change random start position and stop if transform is null
                if (loopEnabled.boolValue != tempLoopEnabled) {
                    loopEnabled.boolValue = tempLoopEnabled;
                    if (tempLoopEnabled) {
                        followPosition.boolValue = true;
                        stopIfTransformIsNull.boolValue = true;
                        randomStartPosition.boolValue = true;
                        //virtualize.boolValue = true; // Virtualize Todo
                    } else {
                        followPosition.boolValue = false;
                        stopIfTransformIsNull.boolValue = false;
                        randomStartPosition.boolValue = false;
                        //virtualize.boolValue = false; // Virtualize Todo
                    }
                }
                EndChange();

                // Follow Position
                BeginChange();
                EditorGUILayout.PropertyField(followPosition, new GUIContent(EditorTextSoundContainer.followPositionLabel, EditorTextSoundContainer.followPositionTooltip));
                EndChange();

                // Stop if Transform is null
                BeginChange();
                EditorGUILayout.PropertyField(stopIfTransformIsNull, new GUIContent(EditorTextSoundContainer.stopIfTransformIsNullLabel, EditorTextSoundContainer.stopIfTransformIsNullTooltip));
                EndChange();

                // Virtualize Todo
                //// Virtualize
                //BeginChange();
                //EditorGUILayout.PropertyField(virtualize, new GUIContent(EditorTextSoundContainer.virtualizeLabel, EditorTextSoundContainer.virtualizeTooltip));
                //EndChange();

                // Random Start Position
                BeginChange();
                EditorGUILayout.PropertyField(randomStartPosition, new GUIContent(EditorTextSoundContainer.randomStartPositionLabel, EditorTextSoundContainer.randomStartPositionTooltip));
                EndChange();

                if (randomStartPosition.boolValue) {
                    EditorGUI.indentLevel++;
                    // Random Start Position Min Max
                    float min = randomStartPositionMin.floatValue;
                    float max = randomStartPositionMax.floatValue;
                    BeginChange();
                    EditorGUILayout.MinMaxSlider(
                        new GUIContent(
                            EditorTextSoundContainer.randomStartPositionMinMaxLabel, 
                            EditorTextSoundContainer.randomStartPositionMinMaxTooltip), 
                        ref min, ref max, 0f, 1f);
                    randomStartPositionMin.floatValue = min;
                    randomStartPositionMax.floatValue = max;
                    EndChange();
                    EditorGUI.indentLevel--;
                } else {
                    // Start Offset
                    BeginChange();
                    EditorGUILayout.Slider(startPosition, 0f, 1f, new GUIContent(EditorTextSoundContainer.startPositionLabel, EditorTextSoundContainer.startPositionTooltip));
                    EndChange();
                }

                BeginChange();
                EditorGuiFunction.DrawFoldout(settingsExpandAdvanced, "Advanced", "", 0, false, false, true);
                EndChange();

                if (settingsExpandAdvanced.boolValue) {

                    // Reverse
                    BeginChange();
                    bool tempReverse = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundContainer.reverseLabel, EditorTextSoundContainer.reverseTooltip), reverse.boolValue);
                    if (reverse.boolValue != tempReverse) {
                        reverse.boolValue = tempReverse;
                        startPosition.floatValue = 1f - startPosition.floatValue;
                    }
                    EndChange();

                    // Play Order
                    BeginChange();
                    EditorGUILayout.PropertyField(playOrder, new GUIContent(EditorTextSoundContainer.playOrderLabel, EditorTextSoundContainer.playOrderTooltip));
                    EndChange();

                    // Priority
                    BeginChange();
                    EditorGUILayout.Slider(priority, 0f, 1f, new GUIContent(EditorTextSoundContainer.priorityLabel, EditorTextSoundContainer.priorityTooltip));
                    EndChange();

                    // Lock Axis Enable
                    BeginChange();
                    EditorGUILayout.PropertyField(lockAxisEnable, new GUIContent(EditorTextSoundContainer.lockAxisEnableLabel, EditorTextSoundContainer.lockAxisEnableTooltip));
                    EndChange();

                    if (lockAxisEnable.boolValue) {
                        EditorGUI.indentLevel++;
                        // Lock Axis
                        BeginChange();
                        EditorGUILayout.PropertyField(lockAxis, new GUIContent(EditorTextSoundContainer.lockAxisLabel, EditorTextSoundContainer.lockAxisTooltip));
                        EndChange();
                        // Lock Axis Value
                        BeginChange();
                        EditorGUILayout.PropertyField(lockAxisPosition, new GUIContent(EditorTextSoundContainer.lockAxisPositionLabel, EditorTextSoundContainer.lockAxisPositionTooltip));
                        EndChange();
                        EditorGUI.indentLevel--;
                    }

                    // Prevent End Clicks
                    BeginChange();
                    EditorGUILayout.PropertyField(preventEndClicks, new GUIContent(EditorTextSoundContainer.preventEndClicksLabel, EditorTextSoundContainer.preventEndClicksTooltip));
                    EndChange();

                    // Never Steal Voice
                    BeginChange();
                    EditorGUILayout.PropertyField(neverStealVoice, new GUIContent(EditorTextSoundContainer.neverStealVoiceLabel, EditorTextSoundContainer.neverStealVoiceTooltip));
                    EndChange();

                    // Never Steal Voice Effects
                    BeginChange();
                    EditorGUILayout.PropertyField(neverStealVoiceEffects, new GUIContent(EditorTextSoundContainer.neverStealVoiceEffectsLabel, EditorTextSoundContainer.neverStealVoiceEffectsTooltip));
                    EndChange();

                    // Doppler
                    BeginChange();
                    EditorGUILayout.Slider(dopplerAmount, 0f, 5f, new GUIContent(EditorTextSoundContainer.dopplerAmountLabel, EditorTextSoundContainer.dopplerAmountTooltip));
                    EndChange();

                    // Bypass Reverb Zones
                    BeginChange();
                    EditorGUILayout.PropertyField(bypassReverbZones, new GUIContent(EditorTextSoundContainer.bypassReverbZonesLabel, EditorTextSoundContainer.bypassReverbZonesTooltip));
                    EndChange();

                    // Bypass AudioSource Effects
                    BeginChange();
                    EditorGUILayout.PropertyField(bypassVoiceEffects, new GUIContent(EditorTextSoundContainer.bypassVoiceEffectsLabel, EditorTextSoundContainer.bypassVoiceEffectsTooltip));
                    EndChange();

                    // Bypass Listener Effects
                    BeginChange();
                    EditorGUILayout.PropertyField(bypassListenerEffects, new GUIContent(EditorTextSoundContainer.bypassListenerEffectsLabel, EditorTextSoundContainer.bypassListenerEffectsTooltip));
                    EndChange();

                    // HRTF Plugin Spatialize
                    BeginChange();
                    EditorGUILayout.PropertyField(hrtfPluginSpatialize, new GUIContent(EditorTextSoundContainer.hrtfPluginSpatializeLabel, EditorTextSoundContainer.hrtfPluginSpatializeTooltip));
                    EndChange();

                    // HRTF Plugin Spatialize Post Effects
                    BeginChange();
                    EditorGUILayout.PropertyField(hrtfPluginSpatializePostEffects, new GUIContent(EditorTextSoundContainer.hrtfPluginSpatializePostEffectsLabel, EditorTextSoundContainer.hrtfPluginSpatializePostEffectsTooltip));
                    EndChange();
                }

            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiVolume() {

            // Volume
            StartBackgroundColor(EditorColor.GetVolumeMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            
            BeginChange();
            EditorGuiFunction.DrawFoldout(volumeExpand, "Volume");
            EndChange();

            if (volumeExpand.boolValue) {

                BeginChange();
                EditorGUILayout.Slider(volumeDecibel, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease24dbMaxDecibel, new GUIContent(EditorTextSoundContainer.volumeLabel, EditorTextSoundContainer.volumeTooltip));
                if (volumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                    volumeDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (volumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue)) {
                    volumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue);
                }
                EndChange();

                // Lower volume 1 dB
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                
                string minVolumeName = "";
                if (mTargets.Length > 1) {
                    // Finds the lowest volume in dB
                    float minVolumeDecibel = Mathf.Infinity;
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (minVolumeDecibel > mTargets[i].internals.data.volumeDecibel) {
                            minVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
                        }
                    }
                    if (minVolumeDecibel < VolumeScale.lowestVolumeDecibel) {
                        minVolumeName = " (Min " + "-Infinity" + ")";
                    } else {
                        minVolumeName = " (Min " + Mathf.FloorToInt(minVolumeDecibel) + ")";
                    }
                }
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.volumeRelativeLowerLabel + minVolumeName, EditorTextSoundContainer.volumeRelativeLowerTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Volume -1 dB");
                        // Don't clamp max when lowering volume so you always can lower it
                        mTargets[i].internals.data.volumeDecibel = Mathf.Clamp(mTargets[i].internals.data.volumeDecibel - 1f, VolumeScale.lowestVolumeDecibel, Mathf.Infinity);
                        mTargets[i].internals.data.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.data.volumeDecibel);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();

                string maxVolumeName = "";
                if (mTargets.Length > 1) {
                    // Finds the highest volume in dB
                    float maxVolumeDecibel = Mathf.NegativeInfinity;
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (maxVolumeDecibel < mTargets[i].internals.data.volumeDecibel) {
                            maxVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
                        }
                    }
                    if (maxVolumeDecibel < VolumeScale.lowestVolumeDecibel) {
                        maxVolumeName = " (Max " + "-Infinity" + ")";
                    } else {
                        maxVolumeName = " (Max " + Mathf.FloorToInt(maxVolumeDecibel) + ")";
                    }
                }
                // Increase volume 1 dB
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.volumeRelativeIncreaseLabel + maxVolumeName, EditorTextSoundContainer.volumeRelativeIncreaseTooltip))) {
                    // Finds the highest volume in dB
                    float maxVolumeDecibel = Mathf.NegativeInfinity;
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (maxVolumeDecibel < mTargets[i].internals.data.volumeDecibel) {
                            maxVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
                        }
                    }
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Volume +1 dB");
                        mTargets[i].internals.data.volumeDecibel = Mathf.Clamp(mTargets[i].internals.data.volumeDecibel + 1f, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease24dbMaxDecibel);
                        mTargets[i].internals.data.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.data.volumeDecibel);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // Warning if volume is over 0
                for (int i = 0; i < mTargets.Length; i++) {
                    if (mTargets[i].internals.data.volumeRatio > (VolumeScale.volumeIncrease24dbMaxRatio + 0.00001)) {
                        EditorGUILayout.HelpBox(EditorTextSoundContainer.volumeOverLimitWarning, MessageType.Warning);
                        break;
                    }
                }

                // Random Volume
                BeginChange();
                EditorGUILayout.PropertyField(volumeRandomEnable, new GUIContent(EditorTextSoundContainer.volumeRandomLabel, EditorTextSoundContainer.volumeRandomTooltip));
                EndChange();

                if (volumeRandomEnable.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(volumeRandomRangeDecibel, -36f, 0f, new GUIContent(EditorTextSoundContainer.volumeRandomRangeLabel, EditorTextSoundContainer.volumeRandomRangeTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                if (distanceEnabled.boolValue) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.volumeDistanceLabel, EditorTextSoundContainer.volumeDistanceTooltip));

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(volumeDistanceRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.volumeDistanceRolloffLabel, EditorTextSoundContainer.volumeDistanceRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(volumeDistanceCurve, EditorColor.GetVolumeMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.volumeDistanceCurveLabel, EditorTextSoundContainer.volumeDistanceCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Separator();
                    // Distance Volume Crossfade
                    GuiVolumeDistanceCrossfade();

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.Volume, EditorSoundContainerCurveValue.Distance);
                } 

                EditorGUILayout.Separator();

                BeginChange();
                EditorGUILayout.PropertyField(volumeIntensityEnable, new GUIContent(EditorTextSoundContainer.volumeIntensityEnableLabel, EditorTextSoundContainer.volumeIntensityEnableTooltip));
                EndChange();

                if (volumeIntensityEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(volumeIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.volumeIntensityRolloffLabel, EditorTextSoundContainer.volumeIntensityRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(volumeIntensityStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.volumeIntensityStrengthLabel, EditorTextSoundContainer.volumeIntensityStrengthTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(volumeIntensityCurve, EditorColor.GetVolumeMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.volumeIntensityCurveLabel, EditorTextSoundContainer.volumeIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Separator();
                    // Intensity Volume Crossfade
                    GuiVolumeIntensityCrossfade();

                    curveDraw.Draw(EditorSoundContainerCurveType.Volume, EditorSoundContainerCurveValue.Intensity);
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiVolumeDistanceCrossfade() {
            // Distance Crossfade
            if (distanceEnabled.boolValue) {

                BeginChange();
                EditorGUILayout.PropertyField(volumeDistanceCrossfadeEnable, new GUIContent(EditorTextSoundContainer.volumeDistanceCrossfadeEnabledLabel, EditorTextSoundContainer.volumeDistanceCrossfadeEnabledTooltip));
                EndChange();

                if (volumeDistanceCrossfadeEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    // Total Number of Layers 
                    BeginChange();
                    EditorGUILayout.PropertyField(volumeDistanceCrossfadeTotalLayersOneBased, new GUIContent(EditorTextSoundContainer.volumeDistanceCrossfadeLayersLabel, EditorTextSoundContainer.volumeDistanceCrossfadeLayersTooltip));
                    volumeDistanceCrossfadeTotalLayersOneBased.intValue = Mathf.Clamp(volumeDistanceCrossfadeTotalLayersOneBased.intValue, 2, int.MaxValue);
                    volumeDistanceCrossfadeTotalLayers.intValue = volumeDistanceCrossfadeTotalLayersOneBased.intValue - 1;
                    volumeDistanceCrossfadeTotalLayers.intValue = Mathf.Clamp(volumeDistanceCrossfadeTotalLayers.intValue, 1, int.MaxValue);
                    // So that the current layer is not more than total number of layers
                    volumeDistanceCrossfadeLayerOneBased.intValue = Mathf.Clamp(volumeDistanceCrossfadeLayerOneBased.intValue, 0, volumeDistanceCrossfadeTotalLayersOneBased.intValue);
                    volumeDistanceCrossfadeLayer.intValue = volumeDistanceCrossfadeLayerOneBased.intValue - 1;
                    EndChange();

                    // Which layer this is
                    BeginChange();
                    volumeDistanceCrossfadeLayerOneBased.intValue = EditorGUILayout.IntSlider(new GUIContent(EditorTextSoundContainer.volumeDistanceCrossfadeThisIsLabel, EditorTextSoundContainer.volumeDistanceCrossfadeThisIsTooltip), volumeDistanceCrossfadeLayerOneBased.intValue, 1, volumeDistanceCrossfadeTotalLayersOneBased.intValue);
                    volumeDistanceCrossfadeLayerOneBased.intValue = Mathf.Clamp(volumeDistanceCrossfadeLayerOneBased.intValue, 0, volumeDistanceCrossfadeTotalLayersOneBased.intValue);
                    volumeDistanceCrossfadeLayer.intValue = volumeDistanceCrossfadeLayerOneBased.intValue - 1;
                    volumeDistanceCrossfadeLayer.intValue = Mathf.Clamp(volumeDistanceCrossfadeLayer.intValue, 0, volumeDistanceCrossfadeTotalLayers.intValue);
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(volumeDistanceCrossfadeRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.volumeDistanceCrossfadeRolloffLabel, EditorTextSoundContainer.volumeDistanceCrossfadeRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(volumeDistanceCrossfadeCurve, EditorColor.GetVolumeMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.volumeDistanceCrossfadeCurveLabel, EditorTextSoundContainer.volumeDistanceCrossfadeCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Separator();
            }
        }

        private void GuiVolumeIntensityCrossfade() {

            BeginChange();
            EditorGUILayout.PropertyField(volumeIntensityCrossfadeEnable, new GUIContent(EditorTextSoundContainer.volumeIntensityCrossfadeEnabledLabel, EditorTextSoundContainer.volumeIntensityCrossfadeEnabledTooltip));
            EndChange();

            if (volumeIntensityCrossfadeEnable.boolValue) {

                EditorGUI.indentLevel++;
                // Total Number of Layers 
                BeginChange();
                EditorGUILayout.PropertyField(volumeIntensityCrossfadeTotalLayersOneBased, new GUIContent(EditorTextSoundContainer.volumeIntensityCrossfadeLayersLabel, EditorTextSoundContainer.volumeIntensityCrossfadeLayersTooltip));
                volumeIntensityCrossfadeTotalLayersOneBased.intValue = Mathf.Clamp(volumeIntensityCrossfadeTotalLayersOneBased.intValue, 2, int.MaxValue);
                volumeIntensityCrossfadeTotalLayers.intValue = volumeIntensityCrossfadeTotalLayersOneBased.intValue - 1;
                volumeIntensityCrossfadeTotalLayers.intValue = Mathf.Clamp(volumeIntensityCrossfadeTotalLayers.intValue, 1, int.MaxValue);
                // So that the current layer is not more than total number of layers
                volumeIntensityCrossfadeLayerOneBased.intValue = Mathf.Clamp(volumeIntensityCrossfadeLayerOneBased.intValue, 0, volumeIntensityCrossfadeTotalLayersOneBased.intValue);
                volumeIntensityCrossfadeLayer.intValue = volumeIntensityCrossfadeLayerOneBased.intValue - 1;
                EndChange();

                // Which layer this is
                BeginChange();
                volumeIntensityCrossfadeLayerOneBased.intValue = EditorGUILayout.IntSlider(new GUIContent(EditorTextSoundContainer.volumeIntensityCrossfadeThisIsLabel, EditorTextSoundContainer.volumeIntensityCrossfadeThisIsTooltip), volumeIntensityCrossfadeLayerOneBased.intValue, 1, volumeIntensityCrossfadeTotalLayersOneBased.intValue);
                volumeIntensityCrossfadeLayerOneBased.intValue = Mathf.Clamp(volumeIntensityCrossfadeLayerOneBased.intValue, 0, volumeIntensityCrossfadeTotalLayersOneBased.intValue);
                volumeIntensityCrossfadeLayer.intValue = volumeIntensityCrossfadeLayerOneBased.intValue - 1;
                volumeIntensityCrossfadeLayer.intValue = Mathf.Clamp(volumeIntensityCrossfadeLayer.intValue, 0, volumeIntensityCrossfadeTotalLayers.intValue);
                EndChange();

                BeginChange();
                EditorGUILayout.Slider(volumeIntensityCrossfadeRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.volumeIntensityCrossfadeRolloffLabel, EditorTextSoundContainer.volumeIntensityCrossfadeRolloffTooltip));
                EndChange();

                BeginChange();
                EditorGUILayout.CurveField(volumeIntensityCrossfadeCurve, EditorColor.GetVolumeMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.intensityCrossfadeCurveLabel, EditorTextSoundContainer.intensityCrossfadeCurveTooltip), GUILayout.Height(guiCurveHeight));
                EndChange();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Separator();
        }

        private void GuiPitch() {

            // Pitch
            StartBackgroundColor(EditorColor.GetPitchMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(pitchExpand, "Pitch");
            EndChange();

            if (pitchExpand.boolValue) {
                BeginChange();
                EditorGUILayout.Slider(pitchSemitoneEditor, -24f, 24f, new GUIContent(EditorTextSoundContainer.pitchLabel, EditorTextSoundContainer.pitchTooltip));
                if (pitchRatio.floatValue != PitchScale.SemitonesToRatio(pitchSemitoneEditor.floatValue)) {
                    pitchRatio.floatValue = PitchScale.SemitonesToRatio(pitchSemitoneEditor.floatValue);
                }
                EndChange();

                BeginChange();
                EditorGUILayout.PropertyField(pitchRandomEnable, new GUIContent(EditorTextSoundContainer.pitchRandomLabel, EditorTextSoundContainer.pitchRandomTooltip));
                EndChange();

                if (pitchRandomEnable.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(pitchRandomRangeSemitone, 0f, 24f, new GUIContent(EditorTextSoundContainer.pitchRandomRangeLabel, EditorTextSoundContainer.pitchRandomRangeTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Separator();

                // Pitch Intensity
                BeginChange();
                EditorGUILayout.PropertyField(pitchIntensityEnable, new GUIContent(EditorTextSoundContainer.pitchIntensityEnableLabel, EditorTextSoundContainer.pitchIntensityEnableTooltip));
                EndChange();

                if (pitchIntensityEnable.boolValue) {
                    EditorGUI.indentLevel++;

                    // Pitch Highest
                    BeginChange();
                    EditorGUILayout.PropertyField(pitchIntensityHighSemitone, new GUIContent(EditorTextSoundContainer.pitchIntensityRangeLabel, EditorTextSoundContainer.pitchIntensityRangeTooltip));
                    pitchIntensityHighSemitone.floatValue = Mathf.Clamp(pitchIntensityHighSemitone.floatValue, -128, 128);
                    if (pitchIntensityHighRatio.floatValue != PitchScale.SemitonesToRatio(pitchIntensityHighSemitone.floatValue)) {
                        pitchIntensityHighRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityHighSemitone.floatValue);
                        // Converting from high/low to base/range
                        pitchIntensityBaseSemitone.floatValue = pitchIntensityLowSemitone.floatValue;
                        pitchIntensityBaseRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityBaseSemitone.floatValue);
                        pitchIntensityRangeSemitone.floatValue = -(pitchIntensityLowSemitone.floatValue - pitchIntensityHighSemitone.floatValue);
                        pitchIntensityRangeRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityRangeSemitone.floatValue);
                    }
                    EndChange();

                    // Pitch Lowest
                    BeginChange();
                    EditorGUILayout.PropertyField(pitchIntensityLowSemitone, new GUIContent(EditorTextSoundContainer.pitchIntensityBaseLabel, EditorTextSoundContainer.pitchIntensityBaseTooltip));
                    pitchIntensityLowSemitone.floatValue = Mathf.Clamp(pitchIntensityLowSemitone.floatValue, -128, 128);
                    if (pitchIntensityLowRatio.floatValue != PitchScale.SemitonesToRatio(pitchIntensityLowSemitone.floatValue)) {
                        pitchIntensityLowRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityLowSemitone.floatValue);
                        // Converting from high/low to base/range
                        pitchIntensityBaseSemitone.floatValue = pitchIntensityLowSemitone.floatValue;
                        pitchIntensityBaseRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityBaseSemitone.floatValue);
                        pitchIntensityRangeSemitone.floatValue = -(pitchIntensityLowSemitone.floatValue - pitchIntensityHighSemitone.floatValue);
                        pitchIntensityRangeRatio.floatValue = PitchScale.SemitonesToRatio(pitchIntensityRangeSemitone.floatValue);
                    }
                    EndChange();

                    // Pitch Intensity Rolloff
                    BeginChange();
                    EditorGUILayout.Slider(pitchIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.pitchIntensityRolloffLabel, EditorTextSoundContainer.pitchIntensityRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(pitchIntensityCurve, EditorColor.GetVolumeMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.pitchIntensityCurveLabel, EditorTextSoundContainer.pitchIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    curveDraw.Draw(EditorSoundContainerCurveType.Pitch, EditorSoundContainerCurveValue.Intensity);
                }
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiSpatialBlend() {
            // Spatial Blend
            StartBackgroundColor(EditorColor.GetSpatialBlendMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(spatialBlendExpand, "Spatial Blend");
            EndChange();

            if (spatialBlendExpand.boolValue) {

                BeginChange();
                EditorGUILayout.Slider(spatialBlend, 0f, 1f, new GUIContent(EditorTextSoundContainer.spatialBlendBaseLabel, EditorTextSoundContainer.spatialBlendBaseTooltip));
                EndChange();

                if (distanceEnabled.boolValue) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.spatialBlendDistanceLabel, EditorTextSoundContainer.spatialBlendDistanceTooltip));

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(spatialBlendDistanceRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.spatialBlendDistanceRolloffLabel, EditorTextSoundContainer.spatialBlendDistanceRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(spatialBlendDistance3DIncrease, 0f, 1f, new GUIContent(EditorTextSoundContainer.spatialBlendDistance3DIncreaseLabel, EditorTextSoundContainer.spatialBlendDistance3DIncreaseTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(spatialBlendDistanceCurve, EditorColor.GetSpatialBlendMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.spatialBlendDistanceCurveLabel, EditorTextSoundContainer.spatialBlendDistanceCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.SpatialBlend, EditorSoundContainerCurveValue.Distance);
                } 

                EditorGUILayout.Separator();
                BeginChange();
                EditorGUILayout.PropertyField(spatialBlendIntensityEnable, new GUIContent(EditorTextSoundContainer.spatialBlendIntensityEnableLabel, EditorTextSoundContainer.spatialBlendIntensityEnableTooltip));
                EndChange();

                if (spatialBlendIntensityEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(spatialBlendIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.spatialBlendIntensityRolloffLabel, EditorTextSoundContainer.spatialBlendIntensityRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(spatialBlendIntensityStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.spatialBlendIntensityStrengthLabel, EditorTextSoundContainer.spatialBlendIntensityStrengthTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(spatialBlendIntensityCurve, EditorColor.GetSpatialBlendMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.spatialBlendIntensityCurveLabel, EditorTextSoundContainer.spatialBlendIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.SpatialBlend, EditorSoundContainerCurveValue.Intensity);
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiSpatialSpread() {
            // Spatial Spread
            StartBackgroundColor(EditorColor.GetSpatialSpreadMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(spatialSpreadExpand, "Spatial Spread");
            EndChange();

            if (spatialSpreadExpand.boolValue) {

                BeginChange();
                EditorGUILayout.Slider(spatialSpreadDegrees, 0f, 360f, new GUIContent(EditorTextSoundContainer.spatialSpreadBaseLabel, EditorTextSoundContainer.spatialSpreadBaseTooltip));
                if (spatialSpreadRatio.floatValue != spatialSpreadDegrees.floatValue / 360f) {
                    spatialSpreadRatio.floatValue = spatialSpreadDegrees.floatValue / 360f;
                }
                EndChange();

                if (distanceEnabled.boolValue) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.spatialSpreadDistanceLabel, EditorTextSoundContainer.spatialSpreadDistanceTooltip));

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(spatialSpreadDistanceRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.spatialSpreadDistanceRolloffLabel, EditorTextSoundContainer.spatialSpreadDistanceRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(spatialSpreadDistanceCurve, EditorColor.GetSpatialSpreadMax(1), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.spatialSpreadDistanceCurveLabel, EditorTextSoundContainer.spatialSpreadDistanceCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.SpatialSpread, EditorSoundContainerCurveValue.Distance);
                } 
                EditorGUILayout.Separator();

                BeginChange();
                EditorGUILayout.PropertyField(spatialSpreadIntensityEnable, new GUIContent(EditorTextSoundContainer.spatialSpreadIntensityEnableLabel, EditorTextSoundContainer.spatialSpreadIntensityEnableTooltip));
                EndChange();

                if (spatialSpreadIntensityEnable.boolValue) {
                    EditorGUI.indentLevel++;

                    BeginChange();
                    EditorGUILayout.Slider(spatialSpreadIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.spatialSpreadIntensityRolloffLabel, EditorTextSoundContainer.spatialSpreadIntensityRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(spatialSpreadIntensityStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.spatialSpreadIntensityStrengthLabel, EditorTextSoundContainer.spatialSpreadIntensityStrengthTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(spatialSpreadIntensityCurve, EditorColor.GetSpatialSpreadMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.spatialSpreadIntensityCurveLabel, EditorTextSoundContainer.spatialSpreadIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.SpatialSpread, EditorSoundContainerCurveValue.Intensity);
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiStereoPan() {

            // StereoPan
            StartBackgroundColor(EditorColor.GetStereoPanMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(stereoPanExpand, "Stereo Pan");
            EndChange();

            if (stereoPanExpand.boolValue) {
                // StereoPan Offset
                BeginChange();
                EditorGUILayout.Slider(stereoPanOffset, -1f, 1f, new GUIContent(EditorTextSoundContainer.stereoPanOffsetLabel, EditorTextSoundContainer.stereoPanOffsetTooltip));
                EndChange();

                // StereoPan Angle Use
                BeginChange();
                stereoPanAngleUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundContainer.stereoPanAngleToSteroPanUseLabel, EditorTextSoundContainer.stereoPanAngleToSteroPanUseTooltip), stereoPanAngleUse.boolValue);
                EndChange();

                if (stereoPanAngleUse.boolValue) {
                    EditorGUI.indentLevel++;

                    // Stereo Pan Automatic Angle Amount
                    BeginChange();
                    EditorGUILayout.Slider(stereoPanAngleAmount, 0f, 1f, new GUIContent(EditorTextSoundContainer.stereoPanAngleToSteroPanStrengthLabel, EditorTextSoundContainer.stereoPanAngleToSteroPanStrengthTooltip));
                    EndChange();

                    // Stereo Pan Angle Rolloff
                    BeginChange();
                    EditorGUILayout.Slider(stereoPanAngleRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.stereoPanAngleToSteroPanRolloffLabel, EditorTextSoundContainer.stereoPanAngleToSteroPanRolloffTooltip));
                    EndChange();

                    curveDraw.Draw(EditorSoundContainerCurveType.StereoPan, EditorSoundContainerCurveValue.Angle);

                    EditorGUI.indentLevel--;
                }
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiReverbZoneMix() {

            // ReverbZoneMix
            StartBackgroundColor(EditorColor.GetReverbZoneMixColorMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(reverbZoneMixExpand, "Reverb Zone Mix");
            EndChange();

            if (reverbZoneMixExpand.boolValue) {

                // Reverb Zone Mix
                BeginChange();
                EditorGUILayout.Slider(reverbZoneMixDecibel, VolumeScale.lowestReverbMixDecibel, 10f, new GUIContent(EditorTextSoundContainer.reverbZoneMixDecibelLabel, EditorTextSoundContainer.reverbZoneMixDecibelTooltip));
                if (reverbZoneMixDecibel.floatValue <= VolumeScale.lowestReverbMixDecibel) {
                    reverbZoneMixDecibel.floatValue = Mathf.NegativeInfinity;
                }
                // Max value is +10 dB (3.1622776601683795)
                // Will be scaled later to where 1.1 is +10dB
                if (reverbZoneMixRatio.floatValue != VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue)) {
                    reverbZoneMixRatio.floatValue = VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue);
                }
                EndChange();

                if (distanceEnabled.boolValue) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.reverbZoneMixDistanceLabel, EditorTextSoundContainer.reverbZoneMixDistanceTooltip));

                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.Slider(reverbZoneMixDistanceRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.reverbZoneMixDistanceRolloffLabel, EditorTextSoundContainer.reverbZoneMixDistanceRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(reverbZoneMixDistanceIncrease, 0f, 1f, new GUIContent(EditorTextSoundContainer.reverbZoneMixDistanceIncreaseLabel, EditorTextSoundContainer.reverbZoneMixDistanceIncreaseTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(reverbZoneMixDistanceCurve, EditorColor.GetReverbZoneMixColorMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.reverbZoneMixDistanceCurveLabel, EditorTextSoundContainer.reverbZoneMixDistanceCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Separator();

                    // Preview Curve
                    curveDraw.Draw(EditorSoundContainerCurveType.ReverbZoneMix, EditorSoundContainerCurveValue.Distance);
                } 
                EditorGUILayout.Separator();

                BeginChange();
                EditorGUILayout.PropertyField(reverbZoneMixIntensityEnable, new GUIContent(EditorTextSoundContainer.reverbZoneMixIntensityEnableLabel, EditorTextSoundContainer.reverbZoneMixIntensityEnableTooltip));
                EndChange();

                if (reverbZoneMixIntensityEnable.boolValue) {

                    EditorGUI.indentLevel++;

                    BeginChange();
                    EditorGUILayout.Slider(reverbZoneMixIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.reverbZoneMixIntensityRolloffLabel, EditorTextSoundContainer.reverbZoneMixIntensityRolloffTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(reverbZoneMixIntensityAmount, 0f, 1f, new GUIContent(EditorTextSoundContainer.reverbZoneMixIntensityStrengthLabel, EditorTextSoundContainer.reverbZoneMixIntensityStrengthTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.CurveField(reverbZoneMixIntensityCurve, EditorColor.GetReverbZoneMixColorMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.reverbZoneMixIntensityCurveLabel, EditorTextSoundContainer.reverbZoneMixIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                    EndChange();

                    EditorGUI.indentLevel--;

                    EditorGUILayout.Separator();

                    curveDraw.Draw(EditorSoundContainerCurveType.ReverbZoneMix, EditorSoundContainerCurveValue.Intensity);
                }
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiDistortion() {

            // Distortion
            StartBackgroundColor(EditorColor.GetDistortionMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(distortionExpand, "Distortion");
            EndChange();

            if (distortionExpand.boolValue) {

                BeginChange();
                EditorGUILayout.PropertyField(distortionEnabled, new GUIContent(EditorTextSoundContainer.distortionEnableLabel, EditorTextSoundContainer.distortionEnableTooltip));
                EndChange();

                if (distortionEnabled.boolValue) {
                
                    BeginChange();
                    EditorGUILayout.Slider(distortionAmount, 0f, 1f, new GUIContent(EditorTextSoundContainer.distortionAmountLabel, EditorTextSoundContainer.distortionAmountTooltip));
                    EndChange();

                    if (distanceEnabled.boolValue) {

                        EditorGUILayout.Separator();
                        BeginChange();
                        EditorGUILayout.PropertyField(distortionDistanceEnable, new GUIContent(EditorTextSoundContainer.distortionDistanceLabel, EditorTextSoundContainer.distortionDistanceTooltip));
                        EndChange();

                        if (distortionDistanceEnable.boolValue) {
                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.Slider(distortionDistanceRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.distortionDistanceRolloffLabel, EditorTextSoundContainer.distortionDistanceRolloffTooltip));
                            EndChange();

                            BeginChange();
                            EditorGUILayout.CurveField(distortionDistanceCurve, EditorColor.GetDistortionMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.distortionDistanceCurveLabel, EditorTextSoundContainer.distortionDistanceCurveTooltip), GUILayout.Height(guiCurveHeight));
                            EndChange();
                            EditorGUI.indentLevel--;

                            // Preview Curve
                            curveDraw.Draw(EditorSoundContainerCurveType.Distortion, EditorSoundContainerCurveValue.Distance);
                        }
                    } 
                    EditorGUILayout.Separator();

                    BeginChange();
                    EditorGUILayout.PropertyField(distortionIntensityEnable, new GUIContent(EditorTextSoundContainer.distortionIntensityEnableLabel, EditorTextSoundContainer.distortionIntensityEnableTooltip));
                    EndChange();

                    if (distortionIntensityEnable.boolValue) {

                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.Slider(distortionIntensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.distortionIntensityRolloffLabel, EditorTextSoundContainer.distortionIntensityRolloffTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.Slider(distortionIntensityStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.distortionIntensityStrengthLabel, EditorTextSoundContainer.distortionIntensityStrengthTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.CurveField(distortionIntensityCurve, EditorColor.GetDistortionMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.distortionIntensityCurveLabel, EditorTextSoundContainer.distortionIntensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                        EndChange();
                        EditorGUI.indentLevel--;

                        // Preview Curve
                        curveDraw.Draw(EditorSoundContainerCurveType.Distortion, EditorSoundContainerCurveValue.Intensity);
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiLowpass() {

            // Lowpass
            StartBackgroundColor(EditorColor.GetLowpassAmountMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(lowpassExpand, "Lowpass Filter");
            EndChange();

            if (lowpassExpand.boolValue) {

                BeginChange();
                EditorGUILayout.PropertyField(lowpassEnabled, new GUIContent(EditorTextSoundContainer.lowpassEnableLabel, EditorTextSoundContainer.lowpassEnableTooltip));
                EndChange();

                if (lowpassEnabled.boolValue) {

                    BeginChange();
                    EditorGUILayout.Slider(lowpassFrequencyEditor, 20f, 20000f, new GUIContent(EditorTextSoundContainer.lowpassFrequencyLabel, EditorTextSoundContainer.lowpassFrequencyTooltip));
                    // Convert values for engine
                    lowpassFrequencyEngine.floatValue = (lowpassFrequencyEditor.floatValue - 20f) / 19980f;
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(lowpassAmountEditor, 0f, 6f, new GUIContent(EditorTextSoundContainer.lowpassAmountLabel, EditorTextSoundContainer.lowpassAmountTooltip));
                    // Convert values for engine
                    lowpassAmountEngine.floatValue = lowpassAmountEditor.floatValue / 6f;
                    EndChange();

                    if (distanceEnabled.boolValue) {

                        EditorGUILayout.Separator();
                        BeginChange();
                        EditorGUILayout.PropertyField(lowpassDistanceEnable, new GUIContent(EditorTextSoundContainer.lowpassDistanceLabel, EditorTextSoundContainer.lowpassDistanceTooltip));
                        EndChange();
                        
                        if (lowpassDistanceEnable.boolValue) {

                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.Slider(lowpassDistanceFrequencyRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.lowpassDistanceFrequencyRolloffLabel, EditorTextSoundContainer.lowpassDistanceFrequencyRolloffTooltip));
                            EndChange();

                            BeginChange();
                            EditorGUILayout.CurveField(lowpassDistanceFrequencyCurve, EditorColor.GetLowpassFrequencyMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.lowpassDistanceFrequencyCurveLabel, EditorTextSoundContainer.lowpassDistanceFrequencyCurveTooltip), GUILayout.Height(guiCurveHeight));
                            EndChange();
                            EditorGUI.indentLevel--;

                            // Preview Curve
                            curveDraw.Draw(EditorSoundContainerCurveType.LowpassFrequency, EditorSoundContainerCurveValue.Distance);
                            EditorGUILayout.Separator();

                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.Slider(lowpassDistanceAmountRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.lowpassDistanceAmountRolloffLabel, EditorTextSoundContainer.lowpassDistanceAmountRolloffTooltip));
                            EndChange();

                            BeginChange();
                            EditorGUILayout.CurveField(lowpassDistanceAmountCurve, EditorColor.GetLowpassAmountMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.lowpassDistanceAmountCurveLabel, EditorTextSoundContainer.lowpassDistanceAmountCurveTooltip), GUILayout.Height(guiCurveHeight));
                            EndChange();
                            EditorGUI.indentLevel--;

                            // Preview Curve
                            curveDraw.Draw(EditorSoundContainerCurveType.LowpassAmount, EditorSoundContainerCurveValue.Distance);
                        }
                    } 

                    EditorGUILayout.Separator();
                    BeginChange();
                    EditorGUILayout.PropertyField(lowpassIntensityEnable, new GUIContent(EditorTextSoundContainer.lowpassIntensityEnableLabel, EditorTextSoundContainer.lowpassIntensityEnableTooltip));
                    EndChange();

                    if (lowpassIntensityEnable.boolValue) {
                        EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.lowpassIntensityFrequencyLabel, EditorTextSoundContainer.lowpassIntensityFrequencyTooltip));

                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.Slider(lowpassIntensityFrequencyRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.lowpassIntensityFrequencyRolloffLabel, EditorTextSoundContainer.lowpassIntensityFrequencyRolloffTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.Slider(lowpassIntensityFrequencyStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.lowpassIntensityFrequencyStrengthLabel, EditorTextSoundContainer.lowpassIntensityFrequencyStrengthTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.CurveField(lowpassIntensityFrequencyCurve, EditorColor.GetLowpassFrequencyMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.lowpassIntensityFrequencyCurveLabel, EditorTextSoundContainer.lowpassIntensityFrequencyCurveTooltip), GUILayout.Height(guiCurveHeight));
                        EndChange();
                        EditorGUI.indentLevel--;

                        // Preview Curve
                        curveDraw.Draw(EditorSoundContainerCurveType.LowpassFrequency, EditorSoundContainerCurveValue.Intensity);

                        EditorGUILayout.Separator();

                        EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.lowpassIntensityAmountLabel, EditorTextSoundContainer.lowpassIntensityAmountTooltip));

                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.Slider(lowpassIntensityAmountRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.lowpassIntensityAmountRolloffLabel, EditorTextSoundContainer.lowpassIntensityAmountRolloffTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.Slider(lowpassIntensityAmountStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.lowpassIntensityAmountStrengthLabel, EditorTextSoundContainer.lowpassIntensityAmountStrengthTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.CurveField(lowpassIntensityAmountCurve, EditorColor.GetLowpassAmountMax(1f), new Rect(0f, 0f, 1f, 1f),  new GUIContent(EditorTextSoundContainer.lowpassIntensityAmountCurveLabel, EditorTextSoundContainer.lowpassIntensityAmountCurveTooltip), GUILayout.Height(guiCurveHeight));
                        EndChange();
                        EditorGUI.indentLevel--;

                        // Preview Curve
                        curveDraw.Draw(EditorSoundContainerCurveType.LowpassAmount, EditorSoundContainerCurveValue.Intensity);
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiHighpass() {

            // Highpass
            StartBackgroundColor(EditorColor.GetHighpassAmountMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            BeginChange();
            EditorGuiFunction.DrawFoldout(highpassExpand, "Highpass Filter");
            EndChange();

            if (highpassExpand.boolValue) {

                BeginChange();
                EditorGUILayout.PropertyField(highpassEnabled, new GUIContent(EditorTextSoundContainer.highpassEnableLabel, EditorTextSoundContainer.highpassEnableTooltip));
                EndChange();

                if (highpassEnabled.boolValue) {

                    BeginChange();
                    EditorGUILayout.Slider(highpassFrequencyEditor, 20f, 20000f, new GUIContent(EditorTextSoundContainer.highpassFrequencyLabel, EditorTextSoundContainer.highpassFrequencyTooltip));
                    // Convert values for engine
                    highpassFrequencyEngine.floatValue = (highpassFrequencyEditor.floatValue - 20f) / 19980f;
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(highpassAmountEditor, 0f, 6f, new GUIContent(EditorTextSoundContainer.highpassAmountLabel, EditorTextSoundContainer.highpassAmountTooltip));
                    // Convert values for engine
                    highpassAmountEngine.floatValue = highpassAmountEditor.floatValue / 6f;
                    EndChange();

                    if (distanceEnabled.boolValue) {

                        EditorGUILayout.Separator();
                        BeginChange();
                        EditorGUILayout.PropertyField(highpassDistanceEnable, new GUIContent(EditorTextSoundContainer.highpassDistanceLabel, EditorTextSoundContainer.highpassDistanceTooltip));
                        EndChange();

                        if (highpassDistanceEnable.boolValue) {
                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.Slider(highpassDistanceFrequencyRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.highpassDistanceFrequencyRolloffLabel, EditorTextSoundContainer.highpassDistanceFrequencyRolloffTooltip));
                            EndChange();

                            BeginChange();
                            EditorGUILayout.CurveField(highpassDistanceFrequencyCurve, EditorColor.GetHighpassFrequencyMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.highpassDistanceFrequencyCurveLabel, EditorTextSoundContainer.highpassDistanceFrequencyCurveTooltip), GUILayout.Height(guiCurveHeight));
                            EndChange();
                            EditorGUI.indentLevel--;

                            // Preview Curve
                            curveDraw.Draw(EditorSoundContainerCurveType.HighpassFrequency, EditorSoundContainerCurveValue.Distance);
                            EditorGUILayout.Separator();

                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.Slider(highpassDistanceAmountRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.highpassDistanceAmountRolloffLabel, EditorTextSoundContainer.highpassDistanceAmountRolloffTooltip));
                            EndChange();

                            BeginChange();
                            EditorGUILayout.CurveField(highpassDistanceAmountCurve, EditorColor.GetHighpassAmountMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.highpassDistanceAmountCurveLabel, EditorTextSoundContainer.highpassDistanceAmountCurveTooltip), GUILayout.Height(guiCurveHeight));
                            EndChange();
                            EditorGUI.indentLevel--;

                            // Preview Curve
                            curveDraw.Draw(EditorSoundContainerCurveType.HighpassAmount, EditorSoundContainerCurveValue.Distance);
                        }
                    }

                    EditorGUILayout.Separator();
                    BeginChange();
                    EditorGUILayout.PropertyField(highpassIntensityEnable, new GUIContent(EditorTextSoundContainer.highpassIntensityEnableLabel, EditorTextSoundContainer.highpassIntensityEnableTooltip));
                    EndChange();

                    if (highpassIntensityEnable.boolValue) {
                        EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.highpassIntensityFrequencyLabel, EditorTextSoundContainer.highpassIntensityFrequencyTooltip));

                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.Slider(highpassIntensityFrequencyRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.highpassIntensityFrequencyRolloffLabel, EditorTextSoundContainer.highpassIntensityFrequencyRolloffTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.Slider(highpassIntensityFrequencyStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.highpassIntensityFrequencyStrengthLabel, EditorTextSoundContainer.highpassIntensityFrequencyStrengthTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.CurveField(highpassIntensityFrequencyCurve, EditorColor.GetHighpassFrequencyMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.highpassIntensityFrequencyCurveLabel, EditorTextSoundContainer.highpassIntensityFrequencyCurveTooltip), GUILayout.Height(guiCurveHeight));
                        EndChange();
                        EditorGUI.indentLevel--;

                        // Preview Curve
                        curveDraw.Draw(EditorSoundContainerCurveType.HighpassFrequency, EditorSoundContainerCurveValue.Intensity);

                        EditorGUILayout.Separator();
                        EditorGUILayout.LabelField(new GUIContent(EditorTextSoundContainer.highpassIntensityAmountLabel, EditorTextSoundContainer.highpassIntensityAmountTooltip));

                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.Slider(highpassIntensityAmountRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundContainer.highpassIntensityAmountRolloffLabel, EditorTextSoundContainer.highpassIntensityAmountRolloffTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.Slider(highpassIntensityAmountStrength, 0f, 1f, new GUIContent(EditorTextSoundContainer.highpassIntensityAmountStrengthLabel, EditorTextSoundContainer.highpassIntensityAmountStrengthTooltip));
                        EndChange();

                        BeginChange();
                        EditorGUILayout.CurveField(highpassIntensityAmountCurve, EditorColor.GetHighpassAmountMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundContainer.highpassIntensityAmountCurveLabel, EditorTextSoundContainer.highpassIntensityAmountCurveTooltip), GUILayout.Height(guiCurveHeight));
                        EndChange();
                        EditorGUI.indentLevel--;

                        // Preview Curve
                        curveDraw.Draw(EditorSoundContainerCurveType.HighpassAmount, EditorSoundContainerCurveValue.Intensity);
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiReset() {

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            BeginChange();
            EditorGUILayout.PropertyField(previewCurves, new GUIContent(EditorTextSoundContainer.showPreviewCurvesLabel, EditorTextSoundContainer.showPreviewCurvesTooltip));
            EndChange();
            StopBackgroundColor();

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));

            // Reset
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.resetSettingsLabel, EditorTextSoundContainer.resetSettingsTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset Settings");
                    mTargets[i].internals.data = new SoundContainerInternalsData();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.resetAllLabel, EditorTextSoundContainer.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].internals.data = new SoundContainerInternalsData();
                    mTargets[i].internals.audioClips = new AudioClip[1];
                    mTargets[i].internals.notes = "Notes";
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private void GuiFindReferences() {

            EditorGUI.indentLevel = 0;
            StartBackgroundColor(Color.grey);
            EditorGUILayout.BeginHorizontal();
            if (mTarget.internals.data.foundReferences == null || mTarget.internals.data.foundReferences.Length == 0) {
                EditorGUILayout.LabelField(new GUIContent("No Search"), GUILayout.Width(EditorGUIUtility.labelWidth));
            } else {
                EditorGUILayout.LabelField(new GUIContent(mTarget.internals.data.foundReferences.Length + " References Found"), GUILayout.Width(EditorGUIUtility.labelWidth));
            }
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.findReferencesLabel, EditorTextSoundContainer.findReferencesTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    mTargets[i].internals.data.foundReferences = EditorFindReferences.GetObjects(mTargets[i]);
                    EditorUtility.SetDirty(mTargets[i]);
                }
                GUIUtility.ExitGUI();
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.findReferencesSelectAllLabel, EditorTextSoundContainer.findReferencesSelectAllTooltip))) {
                List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>();
                for (int i = 0; i < mTargets.Length; i++) {
                    if (mTargets[i].internals.data.foundReferences != null) {
                        newSelection.AddRange(mTargets[i].internals.data.foundReferences);
                    }
                }
                // Only select if something is found
                if (newSelection != null && newSelection.Count > 0) {
                    Selection.objects = newSelection.ToArray();
                }
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundContainer.findReferencesClearLabel, EditorTextSoundContainer.findReferencesClearTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    mTargets[i].internals.data.foundReferences = new UnityEngine.Object[0];
                    EditorUtility.SetDirty(mTargets[i]);
                }
                GUIUtility.ExitGUI();
            }
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Showing the references
            for (int i = 0; i < foundReferences.arraySize; i++) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(foundReferences.GetArrayElementAtIndex(i), new GUIContent(foundReferences.GetArrayElementAtIndex(i).objectReferenceValue.name));
                EditorGUILayout.EndHorizontal();
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

    }
}
#endif