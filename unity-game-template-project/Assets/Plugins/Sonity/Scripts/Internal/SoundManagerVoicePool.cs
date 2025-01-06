// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundManagerVoicePool {

        // Added because you cant resize arrays in a dictionary
        private class SoundPolyGroupDictionaryValue {
            public Voice[] voices;

            public SoundPolyGroupDictionaryValue(int length) {
                voices = new Voice[length];
            }
        }

        // Dictionaries are not serializable and is filled at start
        [NonSerialized]
        private Dictionary<long, SoundPolyGroupDictionaryValue> soundPolyGroupDictionary = new Dictionary<long, SoundPolyGroupDictionaryValue>();

        [NonSerialized]
        private SoundPolyGroupDictionaryValue soundPolyGroupDictionaryValue;

#if UNITY_EDITOR
        [NonSerialized]
        public int statisticsVoicesStolen;
#endif

        public void SoundPolyGroupLimitPolyphony(SoundPolyGroupBase soundPolyGroup, Voice voice) {
            if (soundPolyGroup == null) {
                return;
            } else {
                // Add if not in dictionary
                if (!soundPolyGroupDictionary.ContainsKey(soundPolyGroup.GetAssetID())) {
                    soundPolyGroupDictionary.Add(soundPolyGroup.GetAssetID(), new SoundPolyGroupDictionaryValue(soundPolyGroup.internals.polyphonyLimit));
                }

                // Dictionary get value
                soundPolyGroupDictionary.TryGetValue(soundPolyGroup.GetAssetID(), out soundPolyGroupDictionaryValue);

                // Resize Array if new polyphony is changed
                if (soundPolyGroupDictionaryValue.voices.Length != soundPolyGroup.internals.polyphonyLimit) {
                    if (soundPolyGroupDictionaryValue.voices.Length < soundPolyGroup.internals.polyphonyLimit) {
                        // If there are too few
                        Array.Resize(ref soundPolyGroupDictionaryValue.voices, soundPolyGroup.internals.polyphonyLimit);
                    } else if (soundPolyGroupDictionaryValue.voices.Length > soundPolyGroup.internals.polyphonyLimit) {
                        // If there are too many
                        for (int i = 0; i < soundPolyGroup.internals.polyphonyLimit - soundPolyGroupDictionaryValue.voices.Length; i++) {
                            if (soundPolyGroupDictionaryValue.voices[soundPolyGroup.internals.polyphonyLimit + i] != null) {
                                PoolVoice(soundPolyGroupDictionaryValue.voices[soundPolyGroup.internals.polyphonyLimit + i], false);
                            }
                        }
                        Array.Resize(ref soundPolyGroupDictionaryValue.voices, soundPolyGroup.internals.polyphonyLimit);
                    }
                }

                for (int i = 0; i < soundPolyGroupDictionaryValue.voices.Length; i++) {
                    // If already contains itself
                    if (soundPolyGroupDictionaryValue.voices[i] == voice) {
                        return;
                    }
                }

                for (int i = 0; i < soundPolyGroupDictionaryValue.voices.Length; i++) {
                    // If voice is null
                    if (soundPolyGroupDictionaryValue.voices[i] == null) {
                        soundPolyGroupDictionaryValue.voices[i] = voice;
                        return;
                    }
                }

                for (int i = 0; i < soundPolyGroupDictionaryValue.voices.Length; i++) {
                    // If voice is not assigned
                    if (!soundPolyGroupDictionaryValue.voices[i].isAssigned) {
                        soundPolyGroupDictionaryValue.voices[i] = voice;
                        return;
                    }
                }

                for (int i = 0; i < soundPolyGroupDictionaryValue.voices.Length; i++) {
                    // If voice has another SoundPolyGroup
                    if (soundPolyGroupDictionaryValue.voices[i].soundEvent.internals.data.soundPolyGroup != soundPolyGroup) {
                        soundPolyGroupDictionaryValue.voices[i] = voice;
                        return;
                    }
                }

                float higestPriority = 1f;
                int index = 0;

                // Finds the Voice with the lowest priority
                for (int i = 0; i < soundPolyGroupDictionaryValue.voices.Length; i++) {
                    // Volume Without Fade
                    float savedPriority = soundPolyGroupDictionaryValue.voices[i].GetVolumeRatioWithoutFade() * soundPolyGroupDictionaryValue.voices[i].soundEvent.internals.data.soundPolyGroupPriority;
                    if (savedPriority < higestPriority) {
                        higestPriority = savedPriority;
                        index = i;
                    }
                }

                // Pool Voice with lower priority
                soundPolyGroupDictionaryValue.voices[index].PoolVoice(false, false);

                // Sets Voice to the new Voice
                soundPolyGroupDictionaryValue.voices[index] = voice;
                return;
            }
        }

        [NonSerialized]
        public Voice[] voicePool = new Voice[0];

        [NonSerialized]
        public Queue<int> voiceIndexStopQueue = new Queue<int>();

        public void CreateVoice(int numberOf, bool disableVoices) {
            Array.Resize(ref voicePool, voicePool.Length + numberOf);
            for (int i = 0; i < numberOf; i++) {
                voicePool[voicePool.Length - numberOf + i] = new Voice("Voice " + (voicePool.Length - numberOf + i + 1));
                voicePool[voicePool.Length - numberOf + i].cachedTransform.parent = SoundManagerBase.Instance.Internals.cachedVoicePoolTransform;
                voicePool[voicePool.Length - numberOf + i].voiceIndex = voicePool.Length - numberOf + i;
                if (disableVoices) {
                    voicePool[voicePool.Length - numberOf + i].cachedGameObject.SetActive(false);
                }
            }
        }

        public void PoolVoice(Voice voice, bool isCalledByOnDestroy) {
            if (isCalledByOnDestroy) {
                // If its called by OnDestroy then force stop instead of pause
                voice.ResetVoice();
                voice.cachedVoiceEffect.SetEnabled(false);
                voice.StopOnDestroy();
            } else {
                voice.ResetVoice();
                voice.cachedVoiceEffect.SetEnabled(false);
                voice.SetBypassVoiceEffects(true);
                voice.SetState(VoiceState.Pause, false);
                // Queues the voice for stopping
                voice.stopTime = SoundTimeScale.GetTimeRuntime() + SoundManagerBase.Instance.Internals.settings.voiceStopTime;
                voiceIndexStopQueue.Enqueue(voice.voiceIndex);
                // Is needed for deleting SoundEvents in runtime
                voice.cachedGameObject.transform.parent = SoundManagerBase.Instance.Internals.cachedVoicePoolTransform;
            }
        }

        int tempIndex;

        public Voice GetVoice(Transform instanceIDTransform, float priority, AudioMixerGroup mixerGroup, bool isRestartingLoop) {

            tempIndex = Mathf.Clamp(voicePool.Length, 0, SoundManagerBase.Instance.Internals.settings.voiceLimit);

            // Return an available Voice with the same audioMixerGroup
            if (mixerGroup != null) {
                for (int i = 0; i < tempIndex; i++) {
                    if (!voicePool[i].isAssigned && voicePool[i].audioMixerGroup == mixerGroup) {
                        voicePool[i].AssignVoice(instanceIDTransform);
                        return voicePool[i];
                    }
                }
            }

            // Return an available Voice
            for (int i = 0; i < tempIndex; i++) {
                if (!voicePool[i].isAssigned) {
                    voicePool[i].AssignVoice(instanceIDTransform);
                    return voicePool[i];
                }
            }

            // Check if Voice max polyphony is reached
            // Never removes voice indexes in the pool, just clamp to voice limit instead
            if (Mathf.Clamp(voicePool.Length, 0, SoundManagerBase.Instance.Internals.settings.voiceLimit) >= SoundManagerBase.Instance.Internals.settings.voiceLimit) {
                // Restarting loops shouldnt steal polyphony
                if (isRestartingLoop) {
                    return null;
                }
                float higestPriority = 1f;
                int index = 0;

                // Find the Voice with the lowest priority
                for (int i = 0; i < voicePool.Length; i++) {
                    if (!voicePool[i].soundContainer.internals.data.neverStealVoice) {
                        float savedPriority = voicePool[i].GetVolumeRatioWithoutFadeWithPriority();
                        if (savedPriority < higestPriority) {
                            higestPriority = savedPriority;
                            index = i;
                        }
                    }
                }

                voicePool[index].PoolVoice(true, false);
#if UNITY_EDITOR
                statisticsVoicesStolen++;
#endif
                voicePool[index].AssignVoice(instanceIDTransform);
                return voicePool[index];
            } else {
                // Create a new Voice
                CreateVoice(1, false);
                return GetVoice(instanceIDTransform, priority, mixerGroup, isRestartingLoop);
            }
        }
    }
}