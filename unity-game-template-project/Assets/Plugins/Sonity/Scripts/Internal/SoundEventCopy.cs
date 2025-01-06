// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public static class SoundEventCopy {

        public static void CopyTo(SoundEventBase to, SoundEventBase from, 
            bool copySoundContainers = false, bool copyTimeline = false, bool copyIntensity = false, 
            bool copyTriggerOnSoundEvents = false, bool copySoundTagSoundEvents = false
            ) {

            // Skip this
            //to.internals.notes = from.internals.notes;

            if (copySoundContainers) {
                to.internals.soundContainers = from.internals.soundContainers;
            }

#if UNITY_EDITOR
            to.internals.data.previewAudioMixerGroup = from.internals.data.previewAudioMixerGroup;
#endif
            to.internals.data.expandSoundContainers = from.internals.data.expandSoundContainers;
            to.internals.data.expandTimeline = from.internals.data.expandTimeline;
            to.internals.data.expandPreview = from.internals.data.expandPreview;

            to.internals.data.expandAllSoundTag = from.internals.data.expandAllSoundTag;

            if (copyTimeline) {
                to.internals.data.timelineSoundContainerData = new SoundEventTimelineData[from.internals.data.timelineSoundContainerData.Length];
                for (int i = 0; i < to.internals.data.timelineSoundContainerData.Length; i++) {
                    to.internals.data.timelineSoundContainerData[i].volumeDecibel = from.internals.data.timelineSoundContainerData[i].volumeDecibel;
                    to.internals.data.timelineSoundContainerData[i].volumeRatio = from.internals.data.timelineSoundContainerData[i].volumeDecibel;
                    to.internals.data.timelineSoundContainerData[i].delay = from.internals.data.timelineSoundContainerData[i].delay;
                }
            }

//#if UNITY_EDITOR
              // Would be copied by reference
//            to.internals.data.foundReferences = from.internals.data.foundReferences;
//#endif

            to.internals.data.disableEnable = from.internals.data.disableEnable;
#if UNITY_EDITOR
            to.internals.data.muteEnable = from.internals.data.muteEnable;
            //to.internals.data.soloEnable = from.internals.data.soloEnable;
#endif
            // Base
            to.internals.data.baseExpand = from.internals.data.baseExpand;
            to.internals.data.volumeDecibel = from.internals.data.volumeDecibel;
            to.internals.data.volumeRatio = from.internals.data.volumeRatio;
            to.internals.data.polyphony = from.internals.data.polyphony;

            to.internals.data.soundEventModifier.CloneTo(from.internals.data.soundEventModifier);

            // Settings
            to.internals.data.settingsExpandBase = from.internals.data.settingsExpandBase;
            to.internals.data.settingsExpandAdvanced = from.internals.data.settingsExpandAdvanced;
            to.internals.data.polyphonyMode = from.internals.data.polyphonyMode;
            to.internals.data.audioMixerGroup = from.internals.data.audioMixerGroup;
            to.internals.data.soundVolumeGroup = from.internals.data.soundVolumeGroup;
            to.internals.data.soundMix = from.internals.data.soundMix;
            to.internals.data.soundPolyGroup = from.internals.data.soundPolyGroup;
            to.internals.data.soundPolyGroupPriority = from.internals.data.soundPolyGroupPriority;
            to.internals.data.cooldownTime = from.internals.data.cooldownTime;
            to.internals.data.probability = from.internals.data.probability;
            to.internals.data.passParameters = from.internals.data.passParameters;

            to.internals.data.ignoreLocalPause = from.internals.data.ignoreLocalPause;
            to.internals.data.ignoreGlobalPause = from.internals.data.ignoreGlobalPause;

#if UNITY_EDITOR
            // Debug
            to.internals.data.debugExpand = from.internals.data.debugExpand;
            to.internals.data.debugDrawShow = from.internals.data.debugDrawShow;
            to.internals.data.debugLogShow = from.internals.data.debugLogShow;
            to.internals.data.debugDrawSoundEventStyleOverride = from.internals.data.debugDrawSoundEventStyleOverride;
            to.internals.data.debugDrawSoundEventFontSizeMultiplier = from.internals.data.debugDrawSoundEventFontSizeMultiplier;
            to.internals.data.debugDrawSoundEventOpacityMultiplier = from.internals.data.debugDrawSoundEventOpacityMultiplier;
            to.internals.data.debugDrawSoundEventColorStart = from.internals.data.debugDrawSoundEventColorStart;
            to.internals.data.debugDrawSoundEventColorEnd = from.internals.data.debugDrawSoundEventColorEnd;
            to.internals.data.debugDrawSoundEventColorOutline = from.internals.data.debugDrawSoundEventColorOutline;
#endif
            // Intensity
            to.internals.data.expandIntensity = from.internals.data.expandIntensity;
            to.internals.data.intensityAdd = from.internals.data.intensityAdd;
            to.internals.data.intensityMultiplier = from.internals.data.intensityMultiplier;
            to.internals.data.intensityRolloff = from.internals.data.intensityRolloff;
            to.internals.data.intensitySeekTime = from.internals.data.intensitySeekTime;
            to.internals.data.intensityCurve = from.internals.data.intensityCurve;
            to.internals.data.intensityThresholdEnable = from.internals.data.intensityThresholdEnable;
            to.internals.data.intensityThreshold = from.internals.data.intensityThreshold;

            if (copyIntensity) {
                to.internals.data.intensityScaleAdd = from.internals.data.intensityScaleAdd;
                to.internals.data.intensityScaleMultiplier = from.internals.data.intensityScaleMultiplier;
            }

#if UNITY_EDITOR
            to.internals.data.intensityRecord = from.internals.data.intensityRecord;
            to.internals.data.intensityDebugResolution = from.internals.data.intensityDebugResolution;
            to.internals.data.intensityDebugZoom = from.internals.data.intensityDebugZoom;
            // Would be copied by reference
            //to.internals.data.intensityDebugValueList = from.internals.data.intensityDebugValueList;
#endif
            // Trigger On
            to.internals.data.triggerOnExpand = from.internals.data.triggerOnExpand;

            // TriggerOnPlay
            to.internals.data.triggerOnPlayEnable = from.internals.data.triggerOnPlayEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnPlaySoundEvents = from.internals.data.triggerOnPlaySoundEvents;
            }
            to.internals.data.triggerOnPlayWhichToPlay = from.internals.data.triggerOnPlayWhichToPlay;

            // TriggerOnStop
            to.internals.data.triggerOnStopEnable = from.internals.data.triggerOnStopEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnStopSoundEvents = from.internals.data.triggerOnStopSoundEvents;
            }
            to.internals.data.triggerOnStopWhichToPlay = from.internals.data.triggerOnStopWhichToPlay;

            // TriggerOnTail
            to.internals.data.triggerOnTailEnable = from.internals.data.triggerOnTailEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnTailSoundEvents = from.internals.data.triggerOnTailSoundEvents;
            }
            to.internals.data.triggerOnTailWhichToPlay = from.internals.data.triggerOnTailWhichToPlay;
            to.internals.data.triggerOnTailLength = from.internals.data.triggerOnTailLength;
            to.internals.data.triggerOnTailBpm = from.internals.data.triggerOnTailBpm;
            to.internals.data.triggerOnTailBeatLength = from.internals.data.triggerOnTailBeatLength;

            // SoundTag
            to.internals.data.soundTagEnable = from.internals.data.soundTagEnable;
            to.internals.data.soundTagMode = from.internals.data.soundTagMode;
            to.internals.data.soundTagGroups = new SoundEventSoundTagGroup[from.internals.data.soundTagGroups.Length];
            for (int i = 0; i < to.internals.data.soundTagGroups.Length; i++) {
                if (to.internals.data.soundTagGroups[i] != null) {
                    to.internals.data.soundTagGroups[i].soundTag = from.internals.data.soundTagGroups[i].soundTag;
                    to.internals.data.soundTagGroups[i].soundEventModifierBase.CloneTo(from.internals.data.soundTagGroups[i].soundEventModifierBase);
                    to.internals.data.soundTagGroups[i].soundEventModifierSoundTag.CloneTo(from.internals.data.soundTagGroups[i].soundEventModifierSoundTag);
                    if (copySoundTagSoundEvents) {
                        to.internals.data.soundTagGroups[i].soundEvent = from.internals.data.soundTagGroups[i].soundEvent;
                    }
                }
            }
        }
    }
}