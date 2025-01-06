// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sonity.Internal {

    public class SoundEventInstanceSoundContainerHolder {

        public List<SoundEventInstanceNextVoice> nextVoices = new List<SoundEventInstanceNextVoice>();

        // Virtualize Todo
        //public List<SoundEventInstanceVirtualVoice> virtualVoices = new List<SoundEventInstanceVirtualVoice>();

        public float GetVolume(int v) {
            return soundContainer.internals.data.GetVolume(
                voiceHolder[v].voiceParameter.currentModifier,
                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                )
                * timelineSoundContainerSetting.GetVolumeRatio()
                * soundEvent.internals.data.GetVolumeRatioClamped()
                * soundEvent.internals.data.GetSoundVolumeGroupRatio();
        }

        public bool ShouldBePlaying(int v, bool useCachedVolume) {
            // Stopping sounds under threshold
            if (soundEvent.internals.data.intensityThresholdEnable && voiceHolder[v].voiceParameter.SoundParametersHasContinuousIntensity()) {
                if (voiceHolder[v].playTypeInstance.GetScaledIntensity() < soundEvent.internals.data.intensityThreshold) {
                    // If intensity is under threshold, then intensity is not updated anymore bcs the sound is not playing
                    if (voiceHolder[v].voiceParameter.currentModifier.intensityUse) {
                        // Update intensity
                        voiceHolder[v].playTypeInstance.SetIntensity(voiceHolder[v].voiceParameter.currentModifier.intensity, false);
                        if (voiceHolder[v].playTypeInstance.GetScaledIntensity() < soundEvent.internals.data.intensityThreshold) {
                            // Stop if still under threshold
                            return false;
                        }
                    }
                }
            }
            // If SoundContainer not distance based or is within range
            if (!soundContainer.internals.data.distanceEnabled || voiceHolder[v].playTypeInstance.GetScaledDistance() < 1f) {
                if (useCachedVolume) {
                    if (voiceHolder[v].voice.GetVolumeRatioWithoutFade() > 0f) {
                        return true;
                    }
                } else {
                    if (GetVolume(v) > 0f) {
                        return true;
                    }
                }
            }
            // If SoundParameter has any volume or intensity parameters with update continious
            if (voiceHolder[v].voiceParameter.SoundParametersCanChangeVolume()) {
                return true;
            }
            return false;
        }

        public float NextVoiceGetVolume(int n) {
            return soundContainer.internals.data.GetVolume(
                nextVoices[n].voiceParameter.currentModifier,
                nextVoices[n].playTypeInstance.GetScaledDistance(),
                // Scaled Intensity if intensityUse
                nextVoices[n].voiceParameter.currentModifier.intensityUse ?
                soundEvent.internals.data.GetScaledIntensity(nextVoices[n].voiceParameter.currentModifier.intensity) : 1f
                )
                * timelineSoundContainerSetting.GetVolumeRatio()
                * soundEvent.internals.data.GetVolumeRatioClamped()
                * soundEvent.internals.data.GetSoundVolumeGroupRatio();
        }

        public bool NextVoiceShouldPlay(int n) {
            if (NextVoiceGetVolume(n) > 0f) {
                // If SoundContainer not distance based
                if (!soundContainer.internals.data.distanceEnabled) {
                    return true;
                }
                // If SoundContainer is within range
                if (nextVoices[n].playTypeInstance.GetScaledDistance() < 1f) {
                    return true;
                }
            }
            // Loops should play until stopped or transform null
            if (soundContainer.internals.data.loopEnabled) {
                return true;
            }
            // If SoundParameter has any volume or intensity parameters with update continious
            if (nextVoices[n].voiceParameter.SoundParametersCanChangeVolume()) {
                return true;
            }
            return false;
        }

        public int nextVoiceIndex;
        public void NextVoice(int polyphony) {
            nextVoiceIndex++;
            if (nextVoiceIndex >= polyphony) {
                nextVoiceIndex = 0;
            }
        }

        // Random Clip
        public int[] randomClipLast;
        public int randomClipListPosition;
        public int randomClip;

        public bool RandomClipLastContains() {
            for (int i = 0; i < randomClipLast.Length; i++) {
                if (randomClipLast[i] == randomClip) {
                    return true;
                }
            }
            return false;
        }

        // If any voices are waiting to play (eg delayed)
        public bool GetNextVoiceListAnyAssigned() {
            for (int n = 0; n < nextVoices.Count; n++) {
                if (nextVoices[n].assinged) {
                    return true;
                }
            }
            return false;
        }

        // Returns if any of the voices are playing
        public bool GetIsPlaying() {
            for (int i = 0; i < voiceHolder.Length; i++) {
                if (voiceHolder[i].voice != null) {
                    if (voiceHolder[i].voice.GetVoiceIsPlaying()) {
                        return true;
                    }
                }
            }
            return false;
        }

        // Last played voice index to keep track of which was the last voice to play
        public int lastPlayedVoiceIndex;

        public float GetLastPlayedClipLength(bool pitchSpeed) {
            if (voiceHolder[lastPlayedVoiceIndex].voice == null) {
                return 0f;
            } else {
                return voiceHolder[lastPlayedVoiceIndex].voice.GetAudioSourceClipLengthSeconds(pitchSpeed);
            }
        }

        public float GetLastPlayedClipTimeSeconds(bool pitchSpeed) {
            if (voiceHolder[lastPlayedVoiceIndex].voice == null) {
                return 0f;
            } else {
                return voiceHolder[lastPlayedVoiceIndex].voice.GetAudioSourceTimeSeconds(pitchSpeed);
            }
        }

        public float GetLastPlayedClipTimeRatio() {
            if (voiceHolder[lastPlayedVoiceIndex].voice == null) {
                return 0f;
            } else {
                return voiceHolder[lastPlayedVoiceIndex].voice.GetAudioSourceTimeRatio();
            }
        }

        public void GetLastPlayedAudioSourceSpectrumData(ref float[] samples, int channel, FFTWindow window) {
            if (voiceHolder[lastPlayedVoiceIndex].voice == null) {
                return;
            } else {
                voiceHolder[lastPlayedVoiceIndex].voice.GetAudioSourceSpectrumData(ref samples, channel, window);
            }
        }

        public AudioSource GetLastPlayedAudioSource() {
            if (voiceHolder[lastPlayedVoiceIndex].voice == null) {
                return null;
            } else {
                return voiceHolder[lastPlayedVoiceIndex].voice.GetAudioSource();
            }
        }

        public SoundEventInstanceVoiceHolder[] voiceHolder = new SoundEventInstanceVoiceHolder[0];

        public SoundEventBase soundEvent;
        public SoundContainerBase soundContainer;

        public SoundEventTimelineData timelineSoundContainerSetting;

        // Sets the ranges of the latest created next voice
        public void NextVoiceSetMaxRange(int index) {
            if (soundContainer.internals.data.distanceEnabled) {
                nextVoices[index].maxRange = SoundManagerBase.Instance.Internals.settings.distanceScale
                    * soundContainer.internals.data.distanceScale
                    * nextVoices[index].voiceParameter.currentModifier.distanceScale;
            } else {
                nextVoices[index].maxRange = 0f;
            }
        }

        // Sets the ranges of the latest created next voice
        public void SetMaxRange(int v) {
            if (soundContainer.internals.data.distanceEnabled) {
                voiceHolder[v].maxRange =
                    SoundManagerBase.Instance.Internals.settings.distanceScale
                    * soundContainer.internals.data.distanceScale
                    * voiceHolder[v].voiceParameter.currentModifier.distanceScale;
            } else {
                voiceHolder[v].maxRange = 0f;
            }
        }

#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
        private bool adressableAudioMixerGroupCachedLoaded;
        private AudioMixerGroup adressableAudioMixerGroupCached;
#endif

        private AudioMixerGroup GetAudioMixerGroup(SoundEventBase soundEvent) {
#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
#if UNITY_EDITOR
            // Cache disabled for editor so you can change the AudioMixerGroup during play
            adressableAudioMixerGroupCachedLoaded = false;
#endif
            if (adressableAudioMixerGroupCachedLoaded) {
                return adressableAudioMixerGroupCached;
            } else {
                if (SoundManagerBase.Instance != null && SoundManagerBase.Instance.Internals.settings.adressableAudioMixerUse) {
                    if (soundEvent.internals.data.audioMixerGroup == null) {
                        // Null AudioMixerGroups will be cached to null
                        adressableAudioMixerGroupCached = SoundManagerBase.Instance.Internals.settings.AdressableAudioMixerGroupConvert(soundContainer.internals.data.audioMixerGroup);
                        adressableAudioMixerGroupCachedLoaded = true;
                        return adressableAudioMixerGroupCached;
                    } else {
                        // Null AudioMixerGroups will be cached to null
                        adressableAudioMixerGroupCached = SoundManagerBase.Instance.Internals.settings.AdressableAudioMixerGroupConvert(soundEvent.internals.data.audioMixerGroup);
                        adressableAudioMixerGroupCachedLoaded = true;
                        return adressableAudioMixerGroupCached;
                    }
                }
            }
#endif
            return soundEvent.internals.data.audioMixerGroup;
        }

        // Prepares the voice for playback
        public void VoicePrepare(
            int s, 
            int v, 
            bool replaceVoiceParameter, 
            VoiceParameterInstance voiceParameter, 
            Transform soundEventInstanceTransform, 
            SoundEventPlayTypeInstance playTypeInstance,
            bool isRestartingLoop
            ) {

            // Repace VoiceParameter
            if (replaceVoiceParameter) {
                voiceHolder[v].voiceParameter.CopyTo(voiceParameter);
            }

            // Set Voice Range (Needs to update voiceParameter first)
            SetMaxRange(v);
            // Doesnt need to copy from if playing again
            if (playTypeInstance != null) {
                voiceHolder[v].playTypeInstance.CopyTo(playTypeInstance);
            }
            voiceHolder[v].playTypeInstance.SetCachedDistancesAndAngle(voiceHolder[v].maxRange, voiceHolder[v].voiceParameter, true);

            // Fade in Start
            voiceHolder[v].voiceFade.SetToFadeIn(voiceHolder[v].voiceParameter.currentModifier);

            // Update intensity
            if (voiceHolder[v].voiceParameter.currentModifier.intensityUse) {
                voiceHolder[v].playTypeInstance.SetIntensity(voiceHolder[v].voiceParameter.currentModifier.intensity, true);
            } else {
                voiceHolder[v].playTypeInstance.ResetScaledIntensity();
            }

            // Gets a voice from the voice pool
            if (voiceHolder[v].voice == null) {
                if (ShouldBePlaying(v, false)) {
                    voiceHolder[v].voice = SoundManagerBase.Instance.Internals.voicePool.GetVoice(
                        soundEventInstanceTransform, GetVolume(v) * soundContainer.internals.data.priority, GetAudioMixerGroup(soundEvent), isRestartingLoop
                        );
                    // If the voice shouldn't play
                    if (voiceHolder[v].voice == null) {
                        voiceHolder[v].voiceIsToPlay = false;
                        return;
                    }

                    SoundManagerBase.Instance.Internals.voicePool.SoundPolyGroupLimitPolyphony(soundEvent.internals.data.soundPolyGroup, voiceHolder[v].voice);

                    // Sets variables so that the instance can be found from the Voice and SoundManager
                    voiceHolder[v].voice.instanceVoiceHolder = voiceHolder[v];
                    voiceHolder[v].voice.soundEvent = soundEvent;
#if UNITY_EDITOR
                    voiceHolder[v].voice.soundEventName = soundEvent.name;
#endif
                    voiceHolder[v].voice.soundContainer = soundContainer;

                    if (voiceHolder[v].voice.soundContainer.internals.data.distanceEnabled) {
                        voiceHolder[v].voice.cachedAudioSource.maxDistance = voiceHolder[v].maxRange;
                    } else {
                        voiceHolder[v].voice.cachedAudioSource.maxDistance = Mathf.Infinity;
                    }
                    voiceHolder[v].voice.audioMixerGroup = GetAudioMixerGroup(soundEvent);
                    voiceHolder[v].voice.cachedAudioSource.outputAudioMixerGroup = GetAudioMixerGroup(soundEvent);

                    voiceHolder[v].voice.cachedAudioSource.ignoreListenerPause = soundEvent.internals.data.ignoreGlobalPause;
                } else {
                    // Loops should not play, but rather start when in range
                    if (soundContainer.internals.data.loopEnabled) {
                        voiceHolder[v].shouldRestartIfLoop = true;
                    } else {
                        return;
                    }
                }
            }

            // Set Position
            if (voiceHolder[v].voice != null) {
                if (soundContainer.internals.data.lockAxisEnable) {
                    voiceHolder[v].voice.cachedTransform.position = AxisLock.Lock(voiceHolder[v].playTypeInstance.GetPosition(), soundContainer.internals.data.lockAxis, soundContainer.internals.data.lockAxisPosition);
                } else {
                    voiceHolder[v].voice.cachedTransform.position = voiceHolder[v].playTypeInstance.GetPosition();
                }

                // Volume (also volume for priority)
                // Gets and saves the random volume
                voiceHolder[v].voice.SetVolumeRandomRatio(soundContainer.internals.data.GetVolumeRatioRandom());
                // Needs to be updated so that voice stealing based on volume will work
                voiceHolder[v].voice.SetVolumeRatioFirst(GetVolume(v), voiceHolder[v].voiceFade.GetVolume(voiceHolder[v].voiceParameter.currentModifier));

                // Priority is later multiplied with volume when evaluated
                voiceHolder[v].voice.priority = soundContainer.internals.data.priority;

                // Gets and saves the random pitch
                voiceHolder[v].voice.SetPitchRatioStarting(soundContainer.internals.data.GetPitchBaseAndRandom());

                // Random Clip
                if (soundContainer.internals.data.playOrder == PlayOrder.GlobalRandom) {
                    // Global randomclip
                    randomClip = soundContainer.internals.GetRandomClip();
                } else {
                    // So that it wont break when changing the length in the editor at runtime
                    if (randomClipLast.Length != soundContainer.internals.audioClips.Length / 2) {
                        randomClipLast = new int[soundContainer.internals.audioClips.Length / 2];
                        randomClipListPosition = 0;
                        randomClip = 0;
                    }

                    if (randomClipLast.Length == 0) {
                        randomClip = 0;
                    } else {
                        // Pseudo random function remembering which clips it last played to avoid repetition
                        do {
                            randomClip = UnityEngine.Random.Range(0, soundContainer.internals.audioClips.Length);
                        } while (RandomClipLastContains());
                        randomClipLast[randomClipListPosition] = randomClip;

                        // Wrap index
                        randomClipListPosition++;
                        if (randomClipListPosition >= randomClipLast.Length) {
                            randomClipListPosition = 0;
                        }
                    }
                }

                // Prepares the voice
                voiceHolder[v].voice.cachedAudioSource.clip = soundContainer.internals.audioClips[randomClip];

                // Setting loop
                voiceHolder[v].voice.cachedAudioSource.loop = soundContainer.internals.data.loopEnabled;

                // Doppler Amount
                voiceHolder[v].voice.cachedAudioSource.dopplerLevel = soundContainer.internals.data.dopplerAmount;
            }

            // Updates the curves of the voice
            VoiceUpdate(v);
        }

        // Plays the voice
        public void VoicePlay(int v, int polyphony, float lastStartTime) {

            if (voiceHolder[v].voice != null) {

                voiceHolder[v].voice.lastStartTime = lastStartTime;

                // Setting the last played voice index to keep track of which was the last voice to play
                lastPlayedVoiceIndex = v;

                // Sets pitch before playing so its more responsive
                voiceHolder[v].voice.SetPitchRatioFirst(
                    voiceHolder[v].voiceParameter.currentModifier.pitchRatio 
                    * soundContainer.internals.data.GetPitchRatioIntensity(voiceHolder[v].playTypeInstance.GetScaledIntensity())
                    , soundContainer.internals.data.GetReverse(voiceHolder[v].voiceParameter.currentModifier)
                    );

                // Need to set time before Play
                // Otherwise can get "Error executing result (An invalid seek position was passed to this function)"
                // Random Start Position
                if (soundContainer.internals.data.randomStartPosition) {
                    // Random Min Max
                    int clipSamples = voiceHolder[v].voice.cachedAudioSource.clip.samples;
                    voiceHolder[v].voice.cachedAudioSource.timeSamples =
                        (int)Mathf.Clamp(
                            UnityEngine.Random.Range(
                                clipSamples * soundContainer.internals.data.randomStartPositionMin,
                                clipSamples * soundContainer.internals.data.randomStartPositionMax
                                ), 0f, clipSamples - 1
                        );
                } else {
                    // Start Offset
                    voiceHolder[v].voice.cachedAudioSource.timeSamples = 
                        (int)Mathf.Clamp(voiceHolder[v].voice.cachedAudioSource.clip.samples 
                        * soundContainer.internals.data.GetStartPosition(voiceHolder[v].voiceParameter.currentModifier), 
                        0f, 
                        voiceHolder[v].voice.cachedAudioSource.clip.samples - 1
                        );
                }

                // Plays the voice
                voiceHolder[v].voice.SetState(VoiceState.Play, true);

                // Increments the voice for polyphony
                NextVoice(polyphony);
            }
        }

        // Updates the voice
        public void VoiceUpdate(int v) {

            // Update intensity
            if (voiceHolder[v].voiceParameter.currentModifier.intensityUse) {
                voiceHolder[v].playTypeInstance.SetIntensity(voiceHolder[v].voiceParameter.currentModifier.intensity, false);
            }

            if (voiceHolder[v].voice != null) {
                // MaxRange & Cached Distances are updated before VoiceUpdate
                // Volume (also volume for priority)
                voiceHolder[v].voice.SetVolumeRatioUpdate(GetVolume(v), voiceHolder[v].voiceFade.GetVolume(voiceHolder[v].voiceParameter.currentModifier));

                // Sets the priority to the AudioSource
                voiceHolder[v].voice.SetAudioSourcePriority(voiceHolder[v].voice.GetVolumeRatioWithoutFadeWithPriority());

                // Spatial Blend
                voiceHolder[v].voice.SetSpatialBlend(
                    soundContainer.internals.data.GetSpatialBlend(
                        voiceHolder[v].voiceParameter.currentModifier,
                        voiceHolder[v].playTypeInstance.GetScaledDistance(),
                        voiceHolder[v].playTypeInstance.GetScaledIntensity()
                        )
                    );

                // Spatial Spread Ratio
                voiceHolder[v].voice.SetSpatialSpreadRatio(
                    soundContainer.internals.data.GetSpatialSpread(
                        voiceHolder[v].playTypeInstance.GetScaledDistance(),
                        voiceHolder[v].playTypeInstance.GetScaledIntensity()
                        )
                    );

                // Stereo Pan
                voiceHolder[v].voice.SetStereoPan(
                    soundContainer.internals.data.GetStereoPan(
                        voiceHolder[v].voiceParameter.currentModifier,
                        voiceHolder[v].playTypeInstance.GetAngle()
                        )
                    );

                // Sets pitch
                voiceHolder[v].voice.SetPitchRatioUpdate(
                    voiceHolder[v].voiceParameter.currentModifier.pitchRatio
                    * soundContainer.internals.data.GetPitchRatioIntensity(
                        voiceHolder[v].playTypeInstance.GetScaledIntensity()
                        ),
                    soundContainer.internals.data.GetReverse(
                        voiceHolder[v].voiceParameter.currentModifier)
                    );

                // Bypass Voice Effects Setting
                if (soundContainer.internals.data.GetVoiceEffectsEnabled() && !soundContainer.internals.data.GetBypassVoiceEffects(voiceHolder[v].voiceParameter.currentModifier)) {

                    // Distortion
                    voiceHolder[v].voice.cachedVoiceEffect.DistortionSetEnabled(soundContainer.internals.data.GetDistortionEnabled());
                    if (voiceHolder[v].voice.cachedVoiceEffect.DistortionGetEnabled()) {
                        // Apply distortion
                        // Distortion range 0 to 1
                        voiceHolder[v].voice.cachedVoiceEffect.DistortionSetValue(
                            soundContainer.internals.data.GetDistortion(
                                voiceHolder[v].voiceParameter.currentModifier,
                                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                                )
                            );
                    }

                    // Lowpass
                    voiceHolder[v].voice.cachedVoiceEffect.LowpassSetEnabled(soundContainer.internals.data.GetLowpassEnabled());
                    if (voiceHolder[v].voice.cachedVoiceEffect.LowpassGetEnabled()) {
                        // Apply lowpass
                        voiceHolder[v].voice.cachedVoiceEffect.LowpassSetValue(
                            // Frequency range 0 to 1
                            soundContainer.internals.data.GetLowpassFrequency(
                                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                                ),
                            // Slope range 0 to 1
                            soundContainer.internals.data.GetLowpassAmount(
                                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                                )
                        );
                    }

                    // Highpass
                    voiceHolder[v].voice.cachedVoiceEffect.HighpassSetEnabled(soundContainer.internals.data.GetHighpassEnabled());
                    if (voiceHolder[v].voice.cachedVoiceEffect.HighpassGetEnabled()) {
                        // Apply highpass
                        voiceHolder[v].voice.cachedVoiceEffect.HighpassSetValue(
                            // Frequency range 0 to 1
                            soundContainer.internals.data.GetHighpassFrequency(
                                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                                ),
                            // Slope range 0 to 1
                            soundContainer.internals.data.GetHighpassAmount(
                                voiceHolder[v].playTypeInstance.GetScaledDistance(),
                                voiceHolder[v].playTypeInstance.GetScaledIntensity()
                                )
                        );
                    }

                    // Checks if any effects is actually enabled internally
                    voiceHolder[v].voice.cachedVoiceEffect.SetEnabled(voiceHolder[v].voice.cachedVoiceEffect.GetAnyEnabledInternal());

                    // Checks if voice effect limit is reached
                    if (voiceHolder[v].voice.cachedVoiceEffect.GetEnabled()) {
                        if (voiceHolder[v].voice.cachedVoiceEffect.checkVoiceEffectLimit) {
                            voiceHolder[v].voice.cachedVoiceEffect.checkVoiceEffectLimit = false;
                            if (!SoundManagerBase.Instance.Internals.voiceEffectPool.VoiceEffectShouldPlay(voiceHolder[v].voice)) {
                                voiceHolder[v].voice.cachedVoiceEffect.SetEnabled(false);
                            }
                        }
                    }

                    // Bypasses voice effects if disabled
                    voiceHolder[v].voice.SetBypassVoiceEffects(!voiceHolder[v].voice.cachedVoiceEffect.GetEnabled());

                } else {
                    voiceHolder[v].voice.cachedVoiceEffect.SetEnabled(false);
                    voiceHolder[v].voice.SetBypassVoiceEffects(true);
                }

                // Bypass Reverb Zones
                voiceHolder[v].voice.SetBypassReverbZones(soundContainer.internals.data.GetBypassReverbZones(voiceHolder[v].voiceParameter.currentModifier));

                // Reverb Zone Mix if not bypassed
                if (!voiceHolder[v].voice.GetBypassReverbZones()) {
                    voiceHolder[v].voice.SetReverbZoneMixRatio(
                        soundContainer.internals.data.GetReverbZoneMixRatio(
                            voiceHolder[v].voiceParameter.currentModifier,
                            voiceHolder[v].playTypeInstance.GetScaledDistance(),
                            voiceHolder[v].voiceParameter.currentModifier.intensity)
                        );
                }

                // Bypass Listener Effects
                voiceHolder[v].voice.SetBypassListenerEffects(soundContainer.internals.data.GetBypassListenerEffects(voiceHolder[v].voiceParameter.currentModifier));

                // HRTF Spatialize
                voiceHolder[v].voice.SetHrtfPluginSpatialize(soundContainer.internals.data.hrtfPluginSpatialize);
                voiceHolder[v].voice.SetHrtfSpatializePostEffects(soundContainer.internals.data.hrtfPluginSpatializePostEffects);

                // Pool when fade out is finished
                if (voiceHolder[v].voiceFade.state == VoiceFadeState.FadePool) {
                    voiceHolder[v].voiceFade.Reset();
                    voiceHolder[v].PoolSingleVoice(false, false);
                }
            }
        }
    }
}