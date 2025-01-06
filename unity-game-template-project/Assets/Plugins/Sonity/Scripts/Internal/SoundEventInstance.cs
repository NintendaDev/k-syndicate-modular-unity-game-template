// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [AddComponentMenu("")]
    public class SoundEventInstance : MonoBehaviour {

        [HideInInspector]
        public SoundEventBase soundEvent;

        private Transform cachedTransform;

        public Transform GetTransform() {
            return cachedTransform;
        }

        // Starting at a negative value so it wont be unable to play at start
        private float cooldownTimeCurrent = Mathf.NegativeInfinity;

        [HideInInspector]
        public bool managedUpdateWaitingToPlayOnTail = false;

        private int voicesNotPlaying;

        [HideInInspector]
        public bool waitingForPooling;

        [HideInInspector]
        public int ownerTransformInstanceID;

        private SoundEventInstanceSoundContainerHolder[] instanceSoundContainerHolder = new SoundEventInstanceSoundContainerHolder[0];

        private bool foundVoice;

        private VoiceParameterInstance latestVoiceParameterInstance = new VoiceParameterInstance();

        private SoundMixBase tempSoundMix;

        public void Initialize(SoundEventBase soundEvent) {
            this.soundEvent = soundEvent;
            gameObject.name = this.soundEvent.name;
            cachedTransform = transform;
            instanceSoundContainerHolder = new SoundEventInstanceSoundContainerHolder[this.soundEvent.internals.soundContainers.Length];
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                instanceSoundContainerHolder[s] = new SoundEventInstanceSoundContainerHolder();
                instanceSoundContainerHolder[s].soundEvent = this.soundEvent;
                instanceSoundContainerHolder[s].soundContainer = this.soundEvent.internals.soundContainers[s];
                instanceSoundContainerHolder[s].randomClipLast = new int[this.soundEvent.internals.soundContainers[s].internals.audioClips.Length / 2];
                instanceSoundContainerHolder[s].timelineSoundContainerSetting = soundEvent.internals.GetTimelineSoundContainerSetting(s);
            }
        }

        public int GetPolyphonyLimit() {
            if (soundEvent.internals.data.soundPolyGroup != null) {
                // Is forced to the lower polyphony
                if (latestVoiceParameterInstance.currentModifier.polyphonyUse) {
                    // SoundPolyGroup or Modifier Polyphony
                    return Mathf.Min(soundEvent.internals.data.soundPolyGroup.internals.polyphonyLimit, latestVoiceParameterInstance.currentModifier.polyphony);
                } else {
                    // SoundPolyGroup or SoundEvent Polyphony
                    return Mathf.Min(soundEvent.internals.data.soundPolyGroup.internals.polyphonyLimit, soundEvent.internals.data.polyphony);
                }
            } else {
                if (latestVoiceParameterInstance.currentModifier.polyphonyUse) {
                    // Modifier Polyphony
                    return latestVoiceParameterInstance.currentModifier.polyphony;
                } else {
                    // SoundEvent Polyphony
                    return soundEvent.internals.data.polyphony;
                }
            }
        }

        public int StatisticsGetNumberOfUsedVoices() {
            int voices = 0;
            for (int i = 0; i < instanceSoundContainerHolder.Length; i++) {
                for (int ii = 0; ii < instanceSoundContainerHolder[i].voiceHolder.Length; ii++) {
                    if (instanceSoundContainerHolder[i].voiceHolder[ii].voice != null) {
                        voices++;
                    }
                }
            }
            return voices;
        }

        public float StatisticsGetAverageVolumeRatio() {
            float volume = 0f;
            int voices = 0;
            for (int i = 0; i < instanceSoundContainerHolder.Length; i++) {
                for (int ii = 0; ii < instanceSoundContainerHolder[i].voiceHolder.Length; ii++) {
                    if (instanceSoundContainerHolder[i].voiceHolder[ii].voice != null) {
                        voices++;
                        volume += instanceSoundContainerHolder[i].voiceHolder[ii].voice.GetVolumeRatioWithFade();
                    }
                }
            }
            // Avoid divide by zero
            if (voices > 0) {
                return volume / voices;
            } else {
                return volume;
            }
        }

        private SoundEventInstancePlayValues playValuesLast = new SoundEventInstancePlayValues();

        public class SoundEventInstancePlayValues {
            public SoundEventPlayType playType;
            public Transform instanceIDTransform;
            public Vector3? positionVector3;
            public Transform positionTransform;
            public SoundEventModifier soundEventModifierTrigger;
            public SoundEventModifier soundEventModifierSoundTag;
            public SoundParameterInternals[] soundParameters;
            public SoundParameterInternals soundParameterDistanceScale;
            public SoundTagBase localSoundTag;

            public void SetValues(
                SoundEventPlayType playType, Transform instanceIDTransform, Vector3? positionVector3, Transform positionTransform,
                SoundEventModifier soundEventModifierTrigger, SoundEventModifier soundEventModifierSoundTag,
                SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {

                this.playType = playType;
                this.instanceIDTransform = instanceIDTransform;
                this.positionVector3 = positionVector3;
                this.positionTransform = positionTransform;
                this.soundEventModifierTrigger = soundEventModifierTrigger;
                this.soundEventModifierSoundTag = soundEventModifierSoundTag;
                this.soundParameters = soundParameters;
                this.soundParameterDistanceScale = soundParameterDistanceScale;
                this.localSoundTag = localSoundTag;
            }
        }

#if UNITY_EDITOR
        private bool mutedWarned = false;
        private bool disabledWarned = false;
        private bool intensityRecordWarned = false;
#endif

        public void Play(
            SoundEventPlayType playType, Transform instanceIDTransform, Vector3? positionVector3, Transform positionTransform,
            SoundEventModifier soundEventModifierSoundPicker, SoundEventModifier soundEventModifierSoundTag,
            SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {

#if UNITY_EDITOR
            if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsPlayEnabled(soundEvent)) {
                SoundManagerBase.Instance.Internals.debug.LogSoundEventsPlay(soundEvent, playType, instanceIDTransform, positionTransform, positionVector3);
            }
#endif

            ownerTransformInstanceID = instanceIDTransform.GetInstanceID();
            if (soundEvent.internals.data.disableEnable) {
#if UNITY_EDITOR
                // Warning once if disabled
                if (!disabledWarned && ShouldDebug.Warnings()) {
                    disabledWarned = true;
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)}: The " + soundEvent.name + " is disabled.", soundEvent);
                }
#endif
                return;
            }

            // Probability range 0 to 100 %
            if (soundEvent.internals.data.probability == 100f || soundEvent.internals.data.probability > UnityEngine.Random.Range(0f, 100f)) {
                
                // Cooldown
                if (soundEvent.internals.data.cooldownTime == 0f || SoundTimeScale.GetTimeRuntime() - cooldownTimeCurrent > soundEvent.internals.data.cooldownTime) {
                    cooldownTimeCurrent = SoundTimeScale.GetTimeRuntime();
#if UNITY_EDITOR
                    // Warning once if muted
                    if (soundEvent.internals.data.muteEnable && !mutedWarned) {
                        mutedWarned = true;
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)}: The " + soundEvent.name + " is muted.", soundEvent);
                        }
                    }
#endif
                    // Nullcheck SoundParameters
                    if (soundParameters != null) {
                        for (int i = 0; i < soundParameters.Length; i++) {
                            if (soundParameters[i] == null) {
                                if (ShouldDebug.Warnings()) {
                                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)}: The " + soundEvent.name + $" has null {nameof(NameOf.SoundParameter)}s.", soundEvent);
                                    break;
                                }
                            }
                        }
                    }

                    // Save Values for TriggerOn other SoundEvents
                    if (soundEvent.internals.data.triggerOnPlayEnable || soundEvent.internals.data.triggerOnStopEnable || soundEvent.internals.data.triggerOnTailEnable) {
                        playValuesLast.SetValues(
                            playType, instanceIDTransform, positionVector3, positionTransform, soundEventModifierSoundPicker,
                            soundEventModifierSoundTag, soundParameters, soundParameterDistanceScale, localSoundTag);
                    }

#if UNITY_EDITOR
                    // Warning if volume is over 0 SoundEvent
                    if (soundEvent.internals.data.volumeRatio > (VolumeScale.volumeIncrease24dbMaxRatio + 0.00001)) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)}: " + soundEvent.name + " volume is over 0dB, please add scripting define symbol: \"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume.", soundEvent);
                    }
#endif

#if UNITY_EDITOR
                    // Warning if volume is over 0 SoundVolumeGroup
                    if (soundEvent.internals.data.soundVolumeGroup != null && soundEvent.internals.data.soundVolumeGroup.internals.volumeRatio > (VolumeScale.volumeIncrease12dbMaxRatio + 0.00001)) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundVolumeGroup)}: " + soundEvent.internals.data.soundVolumeGroup.name + " volume is over 0dB, please add scripting define symbol: \"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume.", soundEvent.internals.data.soundVolumeGroup);
                    }
#endif

                    // If there are no SoundContainers
                    if (soundEvent.internals.soundContainers.Length > 0) {

#if UNITY_EDITOR
                        // Warning if volume is over 0 SoundContainer
                        for (int i = 0; i < soundEvent.internals.soundContainers.Length; i++) {
                            if (soundEvent.internals.soundContainers[i] != null) {
                                if (soundEvent.internals.soundContainers[i].internals.data.volumeRatio > (VolumeScale.volumeIncrease24dbMaxRatio + 0.00001)) {
                                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundContainer)}: " + soundEvent.internals.soundContainers[i].name + " volume is over 0dB, please add scripting define symbol: \"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume.", soundEvent.internals.soundContainers[i]);
                                }
                            }
                        }
#endif

#if UNITY_EDITOR
                        // For Statistics
                        SoundManagerBase.Instance.Internals.InternalStatisticsNumberOfPlays(soundEvent, true);
#endif
                        // Reset on Play
                        voicesNotPlaying = 0;
                        waitingForPooling = false;

                        // Used to calculate the time played
                        lastStartTime = SoundTimeScale.GetTimeRuntime();

                        // Modifiers Update
                        latestVoiceParameterInstance.ResetModifiers();

                        // Add SoundEvent Modifier
                        latestVoiceParameterInstance.ModifiersAddValuesToOffset(soundEvent.internals.data.soundEventModifier);
                        
                        // Tag Modifier
                        if (soundEvent.internals.data.soundTagEnable) {
                            if (soundEvent.internals.data.soundTagMode == SoundTagMode.Local && localSoundTag != null) {
                                for (int i = 0; i < soundEvent.internals.data.soundTagGroups.Length; i++) {
                                    if (soundEvent.internals.data.soundTagGroups[i].soundTag == localSoundTag) {
                                        latestVoiceParameterInstance.ModifiersAddValuesToOffset(soundEvent.internals.data.soundTagGroups[i].soundEventModifierBase);
                                    }
                                }
                            } else if (soundEvent.internals.data.soundTagMode == SoundTagMode.Global && SoundManagerBase.Instance.Internals.settings.globalSoundTag != null) {
                                for (int i = 0; i < soundEvent.internals.data.soundTagGroups.Length; i++) {
                                    if (soundEvent.internals.data.soundTagGroups[i].soundTag == SoundManagerBase.Instance.Internals.settings.globalSoundTag) {
                                        latestVoiceParameterInstance.ModifiersAddValuesToOffset(soundEvent.internals.data.soundTagGroups[i].soundEventModifierBase);
                                    }
                                }
                            }
                        }

                        // If this instance is triggered from a SoundTrigger or SoundPicker
                        latestVoiceParameterInstance.ModifiersAddValuesToOffset(soundEventModifierSoundPicker);
                        
                        // If this instance is a sub event in an SoundTag
                        latestVoiceParameterInstance.ModifiersAddValuesToOffset(soundEventModifierSoundTag);

                        // Adding SoundMix and their parents
                        if (soundEvent.internals.data.soundMix != null && !soundEvent.internals.data.soundMix.internals.CheckIsInfiniteLoop(soundEvent.internals.data.soundMix, false)) {
                            tempSoundMix = soundEvent.internals.data.soundMix;
                            while (tempSoundMix != null) {
                                latestVoiceParameterInstance.ModifiersAddValuesToOffset(tempSoundMix.internals.soundEventModifier);
                                tempSoundMix = tempSoundMix.internals.soundMixParent;
                            }
                        }

                        // Radius Handle SoundParameter
                        if (soundParameterDistanceScale != null) {
                            latestVoiceParameterInstance.offsetModifier.distanceScale *= soundParameterDistanceScale.internals.valueFloat;
                        }

                        // SoundParameter Update
                        latestVoiceParameterInstance.SetSoundParameters(soundParameters);
                        latestVoiceParameterInstance.SoundParameterUpdateOnce();
                        latestVoiceParameterInstance.SoundParameterUpdateContinuous();
#if UNITY_EDITOR
                        if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsSoundParametersOnceEnabled(soundEvent)) {
                            SoundManagerBase.Instance.Internals.debug.LogSoundEventsSoundParameters(soundEvent, latestVoiceParameterInstance.soundParameters);
                        }
#endif

#if UNITY_EDITOR
                        // Intensity Debug
                        if (latestVoiceParameterInstance.currentModifier.intensityUse) {
                            if (soundEvent.internals.data.GetIntensityRecord()) {
                                soundEvent.internals.data.intensityDebugValueList.Add(latestVoiceParameterInstance.currentModifier.intensity);
                                // Warning once if intensity record
                                if (!intensityRecordWarned) {
                                    intensityRecordWarned = true;
                                    if (ShouldDebug.Warnings()) {
                                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundEvent)}: The " + soundEvent.name + " has Intensity Record enabled.", soundEvent);
                                    }
                                }
                            }
                        }
#endif
                        // If its not under the threshold
                        if (!(latestVoiceParameterInstance.currentModifier.intensityUse 
                            && soundEvent.internals.data.intensityThresholdEnable 
                            && soundEvent.internals.data.GetScaledIntensity(latestVoiceParameterInstance.currentModifier.intensity) < soundEvent.internals.data.intensityThreshold
                            )) {
                            // Assign Voice
                            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                                foundVoice = false;
                                for (int n = 0; n < instanceSoundContainerHolder[s].nextVoices.Count; n++) {
                                    // Found Voice
                                    if (!instanceSoundContainerHolder[s].nextVoices[n].assinged) {
                                        instanceSoundContainerHolder[s].nextVoices[n].Assign(
                                            latestVoiceParameterInstance, playType, instanceIDTransform, positionVector3, positionTransform, instanceSoundContainerHolder[s].soundContainer, soundEvent);
                                        instanceSoundContainerHolder[s].NextVoiceSetMaxRange(n);
                                        instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance.SetCachedDistancesAndAngle(instanceSoundContainerHolder[s].nextVoices[n].maxRange, instanceSoundContainerHolder[s].nextVoices[n].voiceParameter, true);
                                        // Delay
                                        instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay = SoundTimeScale.GetTimeRuntime() + soundEvent.internals.GetTimelineSoundContainerSetting(s).delay;
                                        if (latestVoiceParameterInstance.currentModifier.delayUse) {
                                            instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay += latestVoiceParameterInstance.currentModifier.delay;
                                        }
                                        foundVoice = true;
                                        break;
                                    }
                                }
                                // Not Found Voice
                                if (!foundVoice) {
                                    instanceSoundContainerHolder[s].nextVoices.Add(new SoundEventInstanceNextVoice());
                                    int n = instanceSoundContainerHolder[s].nextVoices.Count - 1;
                                    instanceSoundContainerHolder[s].nextVoices[n].Assign(
                                        latestVoiceParameterInstance, playType, instanceIDTransform, positionVector3, positionTransform, instanceSoundContainerHolder[s].soundContainer, soundEvent);
                                    instanceSoundContainerHolder[s].NextVoiceSetMaxRange(n);
                                    instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance.SetCachedDistancesAndAngle(instanceSoundContainerHolder[s].nextVoices[n].maxRange, instanceSoundContainerHolder[s].nextVoices[n].voiceParameter, true);
                                    // Delay
                                    instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay = SoundTimeScale.GetTimeRuntime() + soundEvent.internals.GetTimelineSoundContainerSetting(s).delay;
                                    if (latestVoiceParameterInstance.currentModifier.delayUse) {
                                        instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay += latestVoiceParameterInstance.currentModifier.delay;
                                    }
                                }
                            }
                        }

                        // Remove or add voice containers
                        for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                            if (instanceSoundContainerHolder[s].voiceHolder.Length != GetPolyphonyLimit()) {
                                if (instanceSoundContainerHolder[s].voiceHolder.Length < GetPolyphonyLimit()) {
                                    // If there are too few voice containers
                                    Array.Resize(ref instanceSoundContainerHolder[s].voiceHolder, GetPolyphonyLimit());
                                    for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                                        if (instanceSoundContainerHolder[s].voiceHolder[v] == null) {
                                            instanceSoundContainerHolder[s].voiceHolder[v] = new SoundEventInstanceVoiceHolder();
                                        }
                                    }
                                } else if (instanceSoundContainerHolder[s].voiceHolder.Length > GetPolyphonyLimit()) {
                                    // If there are too many voice containers
                                    for (int v = 0; v < GetPolyphonyLimit() - instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                                        instanceSoundContainerHolder[s].voiceHolder[GetPolyphonyLimit() + v].PoolSingleVoice(false, false);
                                    }
                                    Array.Resize(ref instanceSoundContainerHolder[s].voiceHolder, GetPolyphonyLimit());
                                    if (instanceSoundContainerHolder[s].nextVoiceIndex >= GetPolyphonyLimit()) {
                                        instanceSoundContainerHolder[s].nextVoiceIndex = 0;
                                    }
                                }
                            }
                        }

                        // Check if any should play
                        for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                            for (int n = 0; n < instanceSoundContainerHolder[s].nextVoices.Count; n++) {
                                if (instanceSoundContainerHolder[s].nextVoices[n].assinged) {
                                    // Check if delayed
                                    if (instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay <= SoundTimeScale.GetTimeRuntime()
                                        && instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance.GetSpeedOfSoundDistance() <= 0
                                        ) {
                                        // Checking if the SoundContainer should play
                                        if (instanceSoundContainerHolder[s].NextVoiceShouldPlay(n)) {
                                            instanceSoundContainerHolder[s].voiceHolder[instanceSoundContainerHolder[s].nextVoiceIndex].voiceIsToPlay = true;
                                            // Preparing Voice
                                            instanceSoundContainerHolder[s].VoicePrepare(s, instanceSoundContainerHolder[s].nextVoiceIndex,
                                                true, instanceSoundContainerHolder[s].nextVoices[n].voiceParameter, GetTransform(),
                                                instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance, false);
                                            instanceSoundContainerHolder[s].nextVoices[n].ResetAssigned();
                                            break;
                                        } else {
                                            // Shouldn't play
                                            instanceSoundContainerHolder[s].nextVoices[n].ResetAssigned();
                                        }
                                    }
                                }
                            }
                        }

                        // Playing the SoundContainer
                        for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                            if (instanceSoundContainerHolder[s].voiceHolder[instanceSoundContainerHolder[s].nextVoiceIndex].voiceIsToPlay) {
                                instanceSoundContainerHolder[s].voiceHolder[instanceSoundContainerHolder[s].nextVoiceIndex].voiceIsToPlay = false;
                                instanceSoundContainerHolder[s].VoicePlay(instanceSoundContainerHolder[s].nextVoiceIndex, GetPolyphonyLimit(), lastStartTime);
                                // TriggerOnTail
                                if (soundEvent.internals.data.triggerOnTailEnable) {
                                    // Only SC 0 should reset TriggerOnTail
                                    if (s == 0) {
                                        instanceSoundContainerHolder[s].voiceHolder[instanceSoundContainerHolder[s].lastPlayedVoiceIndex].triggerOnTailHasPlayed = false;
                                        triggerOnTailClipFound = false;
                                        triggerOnTailIsStopped = false;
                                    }
                                }
                                // Last played SoundContainer index to keep track of which was the last SoundContainer to play
                                lastPlayedSoundContainerIndex = s;
                            }
                        }
                    }

                    // SoundTag play other SoundEvent
                    if (soundEvent.internals.data.soundTagEnable) {
                        // Local SoundTag
                        if (soundEvent.internals.data.soundTagMode == SoundTagMode.Local && localSoundTag != null) {
                            for (int i = 0; i < soundEvent.internals.data.soundTagGroups.Length; i++) {
                                if (soundEvent.internals.data.soundTagGroups[i].soundTag.GetAssetID() == localSoundTag.GetAssetID()) {
                                    for (int ii = 0; ii < soundEvent.internals.data.soundTagGroups[i].soundEvent.Length; ii++) {
                                        if (soundEvent.internals.data.soundTagGroups[i].soundEvent[ii] != null) {
                                            // Does not send SoundTag, so it can not repeat infinitely
                                            SoundManagerBase.Instance.Internals.InternalPlay(
                                                soundEvent.internals.data.soundTagGroups[i].soundEvent[ii],
                                                playType,
                                                instanceIDTransform,
                                                positionVector3,
                                                positionTransform,
                                                soundEventModifierSoundPicker,
                                                soundEvent.internals.data.soundTagGroups[i].soundEventModifierSoundTag,
                                                soundEvent.internals.data.passParameters ? soundParameters : null,
                                                soundParameterDistanceScale,
                                                null
                                                );
                                        }
                                    }
                                }
                            }
                        }
                        // Global SoundTag
                        else if (soundEvent.internals.data.soundTagMode == SoundTagMode.Global && SoundManagerBase.Instance.Internals.settings.globalSoundTag != null) {
                            for (int i = 0; i < soundEvent.internals.data.soundTagGroups.Length; i++) {
                                if (soundEvent.internals.data.soundTagGroups[i].soundTag.GetAssetID() == SoundManagerBase.Instance.Internals.settings.globalSoundTag.GetAssetID()) {
                                    for (int ii = 0; ii < soundEvent.internals.data.soundTagGroups[i].soundEvent.Length; ii++) {
                                        if (soundEvent.internals.data.soundTagGroups[i].soundEvent[ii] != null) {
                                            // Does not send SoundTag, so it can not repeat infinitely
                                            SoundManagerBase.Instance.Internals.InternalPlay(
                                                soundEvent.internals.data.soundTagGroups[i].soundEvent[ii],
                                                playType,
                                                instanceIDTransform,
                                                positionVector3,
                                                positionTransform,
                                                soundEventModifierSoundPicker,
                                                soundEvent.internals.data.soundTagGroups[i].soundEventModifierSoundTag,
                                                soundEvent.internals.data.passParameters ? soundParameters : null,
                                                soundParameterDistanceScale,
                                                null
                                                );
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // TriggerOnPlay
                    if (soundEvent.internals.data.triggerOnPlayEnable) {
                        if (!soundEvent.internals.data.CheckTriggerOnPlayIsInfiniteLoop(soundEvent, false)) {
                            TriggerOtherSoundEvent(soundEvent.internals.data.triggerOnPlaySoundEvents, soundEvent.internals.data.triggerOnPlayWhichToPlay, SoundEventTriggerOnType.TriggerOnPlay);
                        }
                    }
                }
            }

            if (isPausedLocal) {
                PauseUnpauseNormal(true, true);
            }
            if (AudioListener.pause) {
                PauseUnpauseGlobal(true);
            }
        }

        public SoundEventState GetSoundEventState() {
            if (isPausedGlobal || isPausedLocal) {
                return SoundEventState.Paused;
            }
            // Check if any over the voices are playing
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                if (instanceSoundContainerHolder[s].GetIsPlaying()) {
                    return SoundEventState.Playing;
                }
            }
            // Check if any of the voice are going to play
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                if (instanceSoundContainerHolder[s].GetNextVoiceListAnyAssigned()) {
                    return SoundEventState.Delayed;
                }
            }
            // If nothing is playing
            return SoundEventState.NotPlaying;
        }

        private int lastPlayedSoundContainerIndex;

        public float GetLastPlayedAudioSourceClipLength(bool pitchSpeed) {
            if (instanceSoundContainerHolder.Length == 0 || instanceSoundContainerHolder[lastPlayedSoundContainerIndex] == null) {
                return 0f;
            } else {
                return instanceSoundContainerHolder[lastPlayedSoundContainerIndex].GetLastPlayedClipLength(pitchSpeed);
            }
        }

        public float GetLastPlayedAudioSourceTimeSeconds(bool pitchSpeed) {
            if (instanceSoundContainerHolder.Length == 0 || instanceSoundContainerHolder[lastPlayedSoundContainerIndex] == null) {
                return 0f;
            } else {
                return instanceSoundContainerHolder[lastPlayedSoundContainerIndex].GetLastPlayedClipTimeSeconds(pitchSpeed);
            }
        }

        public float GetLastPlayedAudioSourceTimeRatio() {
            if (instanceSoundContainerHolder.Length == 0 || instanceSoundContainerHolder[lastPlayedSoundContainerIndex] == null) {
                return 0f;
            } else {
                return instanceSoundContainerHolder[lastPlayedSoundContainerIndex].GetLastPlayedClipTimeRatio();
            }
        }

        public void GetSpectrumData(ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            if (spectrumDataFrom == SpectrumDataFrom.LastPlayedAudioSource) {
                if (instanceSoundContainerHolder.Length == 0 || instanceSoundContainerHolder[lastPlayedSoundContainerIndex] == null) {
                    return;
                } else {
                    instanceSoundContainerHolder[lastPlayedSoundContainerIndex].GetLastPlayedAudioSourceSpectrumData(ref samples, channel, window);
                }
            }
        }

        public AudioSource GetLastPlayedAudioSource() {
            if (instanceSoundContainerHolder.Length == 0 || instanceSoundContainerHolder[lastPlayedSoundContainerIndex] == null) {
                return null;
            } else {
                return instanceSoundContainerHolder[lastPlayedSoundContainerIndex].GetLastPlayedAudioSource();
            }
        }

        private bool triggerOnTailClipFound = false;
        private float triggerOnTailClipLength = 0f;
        private bool triggerOnTailIsStopped = false;

        private void TriggerOnTailSetClip() {
            triggerOnTailClipFound = true;
            triggerOnTailClipLength = instanceSoundContainerHolder[0].GetLastPlayedClipLength(false);
        }

        private float TriggerOnTailGetClipLength() {
            return triggerOnTailClipLength;
        }

        private float TriggerOnTailGetClipTime() {
            return instanceSoundContainerHolder[0].GetLastPlayedClipTimeSeconds(false);
        }

        private bool TriggerOnTailGetHasPlayed(bool useLastPlayedVoiceIndex, int voiceIndex) {
            if (useLastPlayedVoiceIndex) {
                return instanceSoundContainerHolder[0].voiceHolder[instanceSoundContainerHolder[0].lastPlayedVoiceIndex].triggerOnTailHasPlayed;
            } else {
                return instanceSoundContainerHolder[0].voiceHolder[voiceIndex].triggerOnTailHasPlayed;
            }
        }

        private void TriggerOnTailSetHasPlayed(bool useLastPlayedVoiceIndex, int voiceIndex) {
            if (useLastPlayedVoiceIndex) {
                instanceSoundContainerHolder[0].voiceHolder[instanceSoundContainerHolder[0].lastPlayedVoiceIndex].triggerOnTailHasPlayed = true;
            } else {
                instanceSoundContainerHolder[0].voiceHolder[voiceIndex].triggerOnTailHasPlayed = true;
            }
        }

        [NonSerialized]
        private bool triggerOnTailNullChecked = false;
        [NonSerialized]
        private bool triggerOnTailIsNull = false;

        private void TriggerOnTailUpdate(bool isPooledForcePlay, int pooledVoiceIndex = 0) {
            if (soundEvent.internals.data.triggerOnTailEnable) {
                if (!triggerOnTailNullChecked) {
                    triggerOnTailNullChecked = true;
                    if (soundEvent.internals.soundContainers.Length == 0 || soundEvent.internals.soundContainers[0] == null) {
                        triggerOnTailIsNull = true;
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity: \"" + soundEvent.name + $"\" TriggerOnTail: No {nameof(AudioClip)} found on the first {nameof(NameOf.SoundContainer)}.", soundEvent);
                        }
                    }
                }
                if (!triggerOnTailIsNull && !TriggerOnTailGetHasPlayed(!isPooledForcePlay, pooledVoiceIndex) && !managedUpdateWaitingToPlayOnTail && !triggerOnTailIsStopped) {
                    if (!triggerOnTailClipFound) {
                        TriggerOnTailSetClip();
                    }
                    if (isPooledForcePlay || (triggerOnTailClipFound && TriggerOnTailGetClipTime() >= TriggerOnTailGetClipLength() - soundEvent.internals.data.triggerOnTailLength)) {
                        TriggerOnTailSetHasPlayed(!isPooledForcePlay, pooledVoiceIndex);
                        managedUpdateWaitingToPlayOnTail = true;
                        // Cant play directly because ManagedUpdate is iterating over a dictionary
                        SoundManagerBase.Instance.Internals.AddManagedUpdateToPlayOnTail(this);
                    }
                }
            }
        }

        private float lastStartTime;

        public float GetTimePlayed() {
            return SoundTimeScale.GetTimeRuntime() - lastStartTime;
        }

        public void PoolSingleVoice(int s, int v, bool shouldRestartIfLoop, bool allowFadeOut, bool isCalledByStop, bool isCalledByOnDestroy) {
            // Trigger on tail
            if (instanceSoundContainerHolder[0].soundEvent.internals.data.triggerOnTailEnable && s == 0 && !isCalledByStop && !isCalledByOnDestroy) {
                // Only the last played voice index should trigger on tail
                if (v == instanceSoundContainerHolder[0].lastPlayedVoiceIndex) {
                    TriggerOnTailUpdate(true, v);
                }
            }
            // Pooling
            if (allowFadeOut) {
                instanceSoundContainerHolder[s].voiceHolder[v].voiceFade.SetToFadeOut(instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier);
                instanceSoundContainerHolder[s].voiceHolder[v].shouldRestartIfLoop = isCalledByStop ? false : shouldRestartIfLoop;
            } else {
                instanceSoundContainerHolder[s].voiceHolder[v].PoolSingleVoice(isCalledByStop ? false : shouldRestartIfLoop, isCalledByOnDestroy);
            }
        }

        public void PoolAllVoices(bool allowFadeOut, bool isCalledByStop, bool isCalledByOnDestroy) {
            // TriggerOnTail dont play after stop
            // TriggerOnStop
            if (isCalledByStop && !isCalledByOnDestroy) {
#if UNITY_EDITOR
                if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsStopEnabled(soundEvent)) {
                    SoundManagerBase.Instance.Internals.debug.LogSoundEventsStop(soundEvent, allowFadeOut);
                }
#endif
                triggerOnTailIsStopped = true;
                if (soundEvent.internals.data.triggerOnStopEnable) {
                    if (!soundEvent.internals.data.CheckTriggerOnStopIsInfiniteLoop(soundEvent, false)) {
                        TriggerOtherSoundEvent(soundEvent.internals.data.triggerOnStopSoundEvents, soundEvent.internals.data.triggerOnStopWhichToPlay, SoundEventTriggerOnType.TriggerOnStop);
                    }
                }
            }

            // Pooling
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    PoolSingleVoice(s, v, false, allowFadeOut, isCalledByStop, isCalledByOnDestroy);
                }
                if (isCalledByStop) {
                    // Removing all delayed voices
                    for (int n = 0; n < instanceSoundContainerHolder[s].nextVoices.Count; n++) {
                        instanceSoundContainerHolder[s].nextVoices[n].ResetAssigned();
                    }
                }
            }
        }

        public void PoolVoicesWithPositionTransform(Transform positionTransform, bool allowFadeOut) {
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.playType == SoundEventPlayType.PlayAtTransform
                        && instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.positionTransform == positionTransform) {
                        PoolSingleVoice(s, v, false, allowFadeOut, true, false);
                    }
                }
            }
        }

        public void TriggerOnTail() {
            if (soundEvent.internals.data.triggerOnTailEnable && !soundEvent.internals.data.CheckTriggerOnTailLengthTooShort(soundEvent, false)) {
                TriggerOtherSoundEvent(soundEvent.internals.data.triggerOnTailSoundEvents, soundEvent.internals.data.triggerOnTailWhichToPlay, SoundEventTriggerOnType.TriggerOnTail);
            }
        }

        public void TriggerOtherSoundEvent(SoundEventBase[] soundEvents, WhichToPlay whichToPlay, SoundEventTriggerOnType triggerType) {
            if (soundEvents.Length == 0) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity: \"" + soundEvent.name + $"\": The " + triggerType.ToString() + $" has no {nameof(NameOf.SoundEvent)}s.", soundEvent);
                }
            } else {
                if (whichToPlay == WhichToPlay.PlayAll) {
                    for (int i = 0; i < soundEvents.Length; i++) {
                        if (soundEvents[i] == null) {
                            if (ShouldDebug.Warnings()) {
                                Debug.LogWarning($"Sonity: \"" + soundEvent.name + $"\": The " + triggerType.ToString() + $" has null {nameof(NameOf.SoundEvent)}s.", soundEvent);
                            }
                        } else {
                            SoundManagerBase.Instance.Internals.InternalPlay(
                                soundEvents[i],
                                playValuesLast.playType,
                                playValuesLast.instanceIDTransform,
                                playValuesLast.positionVector3,
                                playValuesLast.positionTransform,
                                playValuesLast.soundEventModifierTrigger,
                                null,
                                soundEvent.internals.data.passParameters ? playValuesLast.soundParameters : null,
                                playValuesLast.soundParameterDistanceScale,
                                playValuesLast.localSoundTag);
                        }
                    }
                } else if (whichToPlay == WhichToPlay.PlayOneRandom) {
                    // Pseudo random function remembering which clips it last played to avoid repetition
                    int randomSoundEvent = 0;
                    if (triggerType == SoundEventTriggerOnType.TriggerOnPlay) {
                        randomSoundEvent = soundEvent.internals.data.GetTriggerOnPlayRandomSoundEvent();
                    } else if (triggerType == SoundEventTriggerOnType.TriggerOnStop) {
                        randomSoundEvent = soundEvent.internals.data.GetTriggerOnStopRandomSoundEvent();
                    } else if (triggerType == SoundEventTriggerOnType.TriggerOnTail) {
                        randomSoundEvent = soundEvent.internals.data.GetTriggerOnTailRandomSoundEvent();
                    }
                    if (soundEvents[randomSoundEvent] == null) {
                        if (ShouldDebug.Warnings()) {
                            Debug.LogWarning($"Sonity: \"" + soundEvent.name + $"\": The " + triggerType.ToString() + $" has null {nameof(NameOf.SoundEvent)}s.", soundEvent);
                        }
                    } else {
                        SoundManagerBase.Instance.Internals.InternalPlay(
                            soundEvents[randomSoundEvent],
                            playValuesLast.playType,
                            playValuesLast.instanceIDTransform,
                            playValuesLast.positionVector3,
                            playValuesLast.positionTransform,
                            playValuesLast.soundEventModifierTrigger,
                            null,
                            soundEvent.internals.data.passParameters ? playValuesLast.soundParameters : null,
                            playValuesLast.soundParameterDistanceScale,
                            playValuesLast.localSoundTag);
                    }
                }
            }
        }

        private bool isPausedLocal = false;
        private bool isPausedGlobal = false;

        public void PauseUnpauseNormal(bool pause, bool forcePause = false) {
            if (pause && (soundEvent.internals.data.ignoreLocalPause && !forcePause)) {
                return;
            }
            if (!isPausedGlobal && (isPausedLocal != pause || forcePause)) {
                isPausedLocal = pause;
#if UNITY_EDITOR
                if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsPauseUnpauseEnabled(soundEvent)) {
                    SoundManagerBase.Instance.Internals.debug.LogSoundEventsPauseUnpause(soundEvent, pause);
                }
#endif
                for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                    for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                        if (instanceSoundContainerHolder[s].voiceHolder[v].voice != null) {
                            if (instanceSoundContainerHolder[s].voiceHolder[v].voice.cachedAudioSource != null) {
                                if (pause) {
                                    instanceSoundContainerHolder[s].voiceHolder[v].voice.cachedAudioSource.Pause();
                                } else {
                                    instanceSoundContainerHolder[s].voiceHolder[v].voice.cachedAudioSource.UnPause();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PauseUnpauseGlobal(bool pause) {
            if (soundEvent.internals.data.ignoreGlobalPause) {
                return;
            }
#if UNITY_EDITOR
            if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsGlobalPauseUnpauseEnabled(soundEvent)) {
                SoundManagerBase.Instance.Internals.debug.LogSoundEventsGlobalPauseUnpause(soundEvent, pause);
            }
#endif
            isPausedGlobal = pause;
        }

        public void ManagedUpdate() {

            if (waitingForPooling) {
                return;
            }

            if (soundEvent.internals.data.disableEnable) {
                return;
            }

            if (isPausedLocal || isPausedGlobal) {
                return;
            }

#if UNITY_EDITOR
            // Intensity Continious Debug
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (soundEvent.internals.data.GetIntensityRecord()
                        && instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier.intensityUse
                        && instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.SoundParametersHasContinuousIntensity()
                        ) {
                        // Intensity Debug
                        if (soundEvent.internals.data.GetIntensityRecord()) {
                            soundEvent.internals.data.intensityDebugValueList.Add(
                                instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier.intensity
                                );
                        }
                    }
                }
            }
#endif
            TriggerOnTailUpdate(false);

            // Update Continuous Parameters
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter != null) {
                        instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.SoundParameterUpdateContinuous();
#if UNITY_EDITOR
                        if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsSoundParametersContiniousEnabled(soundEvent)) {
                            SoundManagerBase.Instance.Internals.debug.LogSoundEventsSoundParameters(soundEvent, instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.soundParameters);
                        }
#endif
                    }
                }
            }

            // Updates Distances
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].voiceHolder[v].voice != null) {
                        // This needs to happen before positions are updated
                        instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.SetCachedDistancesAndAngle(instanceSoundContainerHolder[s].voiceHolder[v].maxRange, instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter, false);
                    }
                }
            }

            // Update Positions
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    // Stop if stopIfTransformIsNull
                    // Fade check it doesn't retrigger all the time
                    if (instanceSoundContainerHolder[s].soundContainer.internals.data.stopIfTransformIsNull
                        && instanceSoundContainerHolder[s].voiceHolder[v].voiceFade.state != VoiceFadeState.FadePool
                        && instanceSoundContainerHolder[s].voiceHolder[v].voiceFade.state != VoiceFadeState.FadeOut
                        ) {
                        if (instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.instanceIDTransform == null) {
                            PoolSingleVoice(s, v, false, true, false, false);
                        } else if (instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.playType == SoundEventPlayType.PlayAtTransform
                            && instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.positionTransform == null
                            ) {
                            PoolSingleVoice(s, v, false, true, false, false);
                        }
                    }

                    // Update Positions
                    if (instanceSoundContainerHolder[s].voiceHolder[v].voice != null) {
                        if ((!instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier.followPositionUse
                            && instanceSoundContainerHolder[s].soundContainer.internals.data.followPosition)
                            || (instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier.followPositionUse
                            && instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter.currentModifier.followPosition)
                            ) {
                            // PlayType.playAtVector doesnt need to follow position
                            if (instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.playType != SoundEventPlayType.PlayAtVector) {
                                if (instanceSoundContainerHolder[s].soundContainer.internals.data.lockAxisEnable) {
                                    instanceSoundContainerHolder[s].voiceHolder[v].voice.cachedTransform.position =
                                        AxisLock.Lock(instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.GetPosition(),
                                        instanceSoundContainerHolder[s].soundContainer.internals.data.lockAxis,
                                        instanceSoundContainerHolder[s].soundContainer.internals.data.lockAxisPosition
                                        );
                                } else {
                                    instanceSoundContainerHolder[s].voiceHolder[v].voice.cachedTransform.position
                                        = instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.GetPosition();
                                }
                            }
                        }
                    }
                }
            }

            // Checks if the SoundContainer should play delayed
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int n = 0; n < instanceSoundContainerHolder[s].nextVoices.Count; n++) {
                    if (instanceSoundContainerHolder[s].nextVoices[n].assinged) {
                        instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance.SetCachedDistancesAndAngle(instanceSoundContainerHolder[s].nextVoices[n].maxRange, instanceSoundContainerHolder[s].nextVoices[n].voiceParameter, false);
                        if (instanceSoundContainerHolder[s].nextVoices[n].startTimeAndDelay 
                            + instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance.GetSpeedOfSoundDistance() <= SoundTimeScale.GetTimeRuntime()) {
                            if (instanceSoundContainerHolder[s].NextVoiceShouldPlay(n)) {
                                instanceSoundContainerHolder[s].voiceHolder[instanceSoundContainerHolder[s].nextVoiceIndex].voiceIsToPlay = true;
                                instanceSoundContainerHolder[s].VoicePrepare(
                                    s, instanceSoundContainerHolder[s].nextVoiceIndex, true, instanceSoundContainerHolder[s].nextVoices[n].voiceParameter, 
                                    GetTransform(), instanceSoundContainerHolder[s].nextVoices[n].playTypeInstance, false
                                    );
                            }
                            instanceSoundContainerHolder[s].nextVoices[n].ResetAssigned();
                        } 
                    }
                }
            }

            // Restart looping SoundContainer if they were stopped by being out of range or too low volume or stolen
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].soundContainer.internals.data.loopEnabled
                        && instanceSoundContainerHolder[s].voiceHolder[v].shouldRestartIfLoop
                        && instanceSoundContainerHolder[s].voiceHolder[v].voice == null
                        && instanceSoundContainerHolder[s].voiceHolder[v].voiceFade.state != VoiceFadeState.FadePool
                        ) {
                        instanceSoundContainerHolder[s].voiceHolder[v].playTypeInstance.SetCachedDistancesAndAngle(
                            instanceSoundContainerHolder[s].voiceHolder[v].maxRange,
                            instanceSoundContainerHolder[s].voiceHolder[v].voiceParameter, false
                            );
                        if (instanceSoundContainerHolder[s].ShouldBePlaying(v, false)) {
                            instanceSoundContainerHolder[s].voiceHolder[v].voiceIsToPlay = true;
                            instanceSoundContainerHolder[s].VoicePrepare(s, v, false, null, GetTransform(), null, true);
                        }
                    }
                }
            }

            // Play delayed and loop restart
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].voiceHolder[v].voiceIsToPlay) {
                        instanceSoundContainerHolder[s].voiceHolder[v].voiceIsToPlay = false;
                        instanceSoundContainerHolder[s].VoicePlay(v, GetPolyphonyLimit(), lastStartTime);
                        // TriggerOnTail
                        if (soundEvent.internals.data.triggerOnTailEnable) {
                            // Only SC 0 should reset TriggerOnTail
                            if (s == 0) {
                                instanceSoundContainerHolder[s].voiceHolder[v].triggerOnTailHasPlayed = false;
                                triggerOnTailClipFound = false;
                                triggerOnTailIsStopped = false;
                            }
                        }
                        lastPlayedSoundContainerIndex = s;
                    }
                }
            }

            // Reset
            voicesNotPlaying = 0;

            // Update curves or pool
            for (int s = 0; s < instanceSoundContainerHolder.Length; s++) {
                for (int v = 0; v < instanceSoundContainerHolder[s].voiceHolder.Length; v++) {
                    if (instanceSoundContainerHolder[s].voiceHolder[v].voice == null) {
                        // Checks if there are no delayed SoundContainer to play
                        if (!instanceSoundContainerHolder[s].GetNextVoiceListAnyAssigned()) {
                            // If loop shouldn't play again
                            if (!(instanceSoundContainerHolder[s].soundContainer.internals.data.loopEnabled && instanceSoundContainerHolder[s].voiceHolder[v].shouldRestartIfLoop)) {
                                voicesNotPlaying++;
                            }
                        }
                    } else {
                        if (instanceSoundContainerHolder[s].voiceHolder[v].voice.GetVoiceIsPlaying()) {
                            if (instanceSoundContainerHolder[s].ShouldBePlaying(v, true)) {
                                instanceSoundContainerHolder[s].VoiceUpdate(v);
                            } else {
                                PoolSingleVoice(s, v, true, false,false, false);
                            }
                        } else {
                            // If its not waiting for delay
                            if (!instanceSoundContainerHolder[s].GetNextVoiceListAnyAssigned()) {
                                PoolSingleVoice(s, v, true, false, false, false);
                            }
                        }
                    }
                }
            }

            // If no voices are playing
            if (voicesNotPlaying >= instanceSoundContainerHolder.Length * GetPolyphonyLimit()) {
                voicesNotPlaying = 0;

                if (!waitingForPooling) {
                    waitingForPooling = true;
#if UNITY_EDITOR
                    if (SoundManagerBase.Instance.Internals.debug.LogSoundEventsPoolEnabled(soundEvent)) {
                        SoundManagerBase.Instance.Internals.debug.LogSoundEventsPool(soundEvent);
                    }
#endif
                    PoolAllVoices(false, false, false);
                    // Cant pool directly because ManagedUpdate is iterating over a dictionary
                    SoundManagerBase.Instance.Internals.AddManagedUpdateToPool(this);
                }
            }
        }
    }
}