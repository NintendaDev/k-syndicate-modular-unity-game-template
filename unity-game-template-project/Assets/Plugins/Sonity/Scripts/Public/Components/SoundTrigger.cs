// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundTriggerBase">SoundTrigger</see> is a component used for easily playing/stopping <see cref="SoundEventBase">SoundEvents</see> on callbacks built into Unity like Enable, Disable, OnCollisionEnter etc.
    /// They contain <see cref="SoundEventBase">SoundEvents</see> with modifiers and triggers which decide when it should play or stop.
    /// <see cref="SoundTriggerBase">SoundTriggers</see> also have a radius handle, which is visually editable in the scene viewport for easy adjustment of how far <see cref="SoundEventBase">SoundEvents</see> should be heard.
    /// All <see cref="SoundTriggerBase">SoundTrigger</see> components are multi-object editable.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity 🔊/Sonity - Sound Trigger")]
	public class SoundTrigger : SoundTriggerBase {

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        public void Play() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Play(internals.cachedTransform, null, internals.soundParameterDistanceScale, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(SoundTagBase localSoundTag) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Play(internals.cachedTransform, null, internals.soundParameterDistanceScale, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(params SoundParameterInternals[] soundParameters) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Play(internals.cachedTransform, soundParameters, internals.soundParameterDistanceScale, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Play(internals.cachedTransform, soundParameters, internals.soundParameterDistanceScale, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void PlayAtPosition(Transform position) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", internals.cachedTransform);
                }
            } else {
                for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                    if (internals.soundTriggerPart[i].soundEvent != null) {
                        internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, null, internals.soundParameterDistanceScale, null);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform position, SoundTagBase localSoundTag) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", internals.cachedTransform);
                }
            } else {
                for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                    if (internals.soundTriggerPart[i].soundEvent != null) {
                        internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, null, internals.soundParameterDistanceScale, localSoundTag);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Transform position, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", internals.cachedTransform);
                }
            } else {
                for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                    if (internals.soundTriggerPart[i].soundEvent != null) {
                        internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, soundParameters, internals.soundParameterDistanceScale, null);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", internals.cachedTransform);
                }
            } else {
                for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                    if (internals.soundTriggerPart[i].soundEvent != null) {
                        internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, soundParameters, internals.soundParameterDistanceScale, localSoundTag);
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void PlayAtPosition(Vector3 position) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, null, internals.soundParameterDistanceScale, null);
                }
            }
        }
        
        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Vector3 position, SoundTagBase localSoundTag) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, null, internals.soundParameterDistanceScale, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Vector3 position, params SoundParameterInternals[] soundParameters) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, soundParameters, internals.soundParameterDistanceScale, null);
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Vector3 position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].PlayAtPosition(internals.cachedTransform, position, soundParameters, internals.soundParameterDistanceScale, localSoundTag);
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop(bool allowFadeOut = true) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Stop(internals.cachedTransform, allowFadeOut);
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> played at the position <see cref="Transform"/>
        /// </summary>
        /// <param name='position'>
        /// The <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAtPosition(Transform position, bool allowFadeOut = true) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].StopAtPosition(position, allowFadeOut);
                }
            }
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void Pause(bool forcePause = false) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Pause(internals.cachedTransform, forcePause);
                }
            }
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        public void Unpause() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].Unpause(internals.cachedTransform);
                }
            }
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> everywhere locally
        /// </summary>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseEverywhere(bool forcePause = false) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].PauseEverywhere(forcePause);
                }
            }
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> everywhere locally
        /// </summary>
        public void UnpauseEverywhere() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    internals.soundTriggerPart[i].UnpauseEverywhere();
                }
            }
        }

        /// <summary>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If paused either locally or globally it returns <see cref="SoundEventState.Paused"/> </para>
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <returns>
        /// Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> 
        /// </returns>
        public SoundEventState GetSoundEventState() {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
                return SoundEventState.NotPlaying;
            }
            SoundEventState soundEventState = SoundEventState.NotPlaying;
            bool soundEventStateDelayed = false;
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundEvent != null) {
                    soundEventState = SoundManagerBase.Instance.Internals.GetSoundEventState(internals.soundTriggerPart[i].soundEvent, internals.cachedTransform);
                    if (soundEventState == SoundEventState.Playing) {
                        return SoundEventState.Playing;
                    } else if (soundEventState == SoundEventState.Delayed) {
                        soundEventStateDelayed = true;
                    }
                }
            }
            // If no SoundEventInstance is playing
            if (soundEventStateDelayed) {
                return SoundEventState.Delayed;
            } else {
                return SoundEventState.NotPlaying;
            }
        }

        /// <summary>
        /// Loads the Audio Data of the <see cref="AudioClip"/>(s) of the <see cref="SoundContainerBase">SoundContainer</see>(s) to RAM
        /// </summary>
        public void LoadAudioData() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                internals.soundTriggerPart[i].LoadAudioData();
            }
        }

        /// <summary>
        /// Unloads the Audio Data of the <see cref="AudioClip"/>(s) of the <see cref="SoundContainerBase">SoundContainer</see>(s) from RAM
        /// </summary>
        public void UnloadAudioData() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                internals.soundTriggerPart[i].UnloadAudioData();
            }
        }
    }
}