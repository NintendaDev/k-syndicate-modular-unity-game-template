// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundPickerBase">SoundPicker</see> is a serializable class for easily selecting multiple <see cref="SoundEventBase">SoundEvents</see> and modifiers.
    /// Add a serialized or public <see cref="SoundPickerBase">SoundPicker</see> to a C# script and edit it in the inspector.
    /// The <see cref="SoundPickerBase">SoundPicker</see> is not nestable in an array or custom class because it is built upon CustomPropertyDrawer.
    /// This is because custom serializable classes do not support inheritance and polymorphism for serialization.
    /// <see cref="SoundPickerBase">SoundPicker</see> are multi-object editable.
    /// <code>
    /// <para/> // Example use
    /// <para/> public Sonity.SoundPicker soundPicker;
    /// <para/> private void Start() {
    /// <para/>     soundPicker.Play(transform);
    /// <para/> } 
    /// </code>
    /// </summary>
    [Serializable]
    public class SoundPicker : SoundPickerBase {

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The <see cref="Transform"/> where is should play at
        /// </param>
        public void Play(Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Play(owner, null, null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The <see cref="Transform"/> where is should play at
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(Transform owner, SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Play(owner, null, null, localSoundTag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The <see cref="Transform"/> where is should play at
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(Transform owner, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Play(owner, soundParameters, null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The <see cref="Transform"/> where is should play at
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(Transform owner, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Play(owner, soundParameters, null, localSoundTag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The owner {nameof(Transform)} is null.", position);
                    }
                } else {
                    if (internals.isEnabled) {
                        for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                            if (internals.soundPickerPart[i].soundEvent != null) {
                                internals.soundPickerPart[i].PlayAtPosition(position, owner, null, null, null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, SoundTagBase localSoundTag) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The owner {nameof(Transform)} is null.", position);
                    }
                } else {
                    if (internals.isEnabled) {
                        for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                            if (internals.soundPickerPart[i].soundEvent != null) {
                                internals.soundPickerPart[i].PlayAtPosition(owner, position, null, null, localSoundTag);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The owner {nameof(Transform)} is null.", position);
                    }
                } else {
                    if (internals.isEnabled) {
                        for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                            if (internals.soundPickerPart[i].soundEvent != null) {
                                internals.soundPickerPart[i].PlayAtPosition(owner, position, soundParameters, null, null);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Transform position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The position {nameof(Transform)} is null.", owner);
                }
            } else {
                if (owner == null) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The owner {nameof(Transform)} is null.", position);
                    }
                } else {
                    if (internals.isEnabled) {
                        for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                            if (internals.soundPickerPart[i].soundEvent != null) {
                                internals.soundPickerPart[i].PlayAtPosition(owner, position, soundParameters, null, localSoundTag);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].PlayAtPosition(owner, position, null, null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position,  SoundTagBase localSoundTag) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].PlayAtPosition(owner, position, null, null, localSoundTag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].PlayAtPosition(owner, position, soundParameters, null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(Transform owner, Vector3 position, SoundTagBase localSoundTag, params SoundParameterInternals[] soundParameters) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].PlayAtPosition(owner, position, soundParameters, null, localSoundTag);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop(Transform owner, bool allowFadeOut = true) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Stop(owner, allowFadeOut);
                        }
                    }
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
            if (position == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].StopAtPosition(position, allowFadeOut);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void Pause(Transform owner, bool forcePause = false) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Pause(owner, forcePause);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        public void Unpause(Transform owner) {
            if (owner == null) {
                if (ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPicker)}: The {nameof(Transform)} is null.");
                }
            } else {
                if (internals.isEnabled) {
                    for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                        if (internals.soundPickerPart[i].soundEvent != null) {
                            internals.soundPickerPart[i].Unpause(owner);
                        }
                    }
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
            if (internals.isEnabled) {
                for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                    if (internals.soundPickerPart[i].soundEvent != null) {
                        internals.soundPickerPart[i].PauseEverywhere(forcePause);
                    }
                }
            }
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> everywhere locally
        /// </summary>
        public void UnpauseEverywhere() {
            if (internals.isEnabled) {
                for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                    if (internals.soundPickerPart[i].soundEvent != null) {
                        internals.soundPickerPart[i].UnpauseEverywhere();
                    }
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
        /// <param name="owner"> 
        /// The owner <see cref="Transform"/> 
        /// </param>
        /// <returns> 
        /// Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> 
        /// </returns>
        public SoundEventState GetSoundEventState(Transform owner) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
                return SoundEventState.NotPlaying;
            }
            // Custom code to get the combined state of all SoundEvents in the SoundPicker
            SoundEventState soundEventState = SoundEventState.NotPlaying;
            bool soundEventStateDelayed = false;
            for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                if (internals.soundPickerPart[i].soundEvent != null) {
                    soundEventState = SoundManagerBase.Instance.Internals.GetSoundEventState(internals.soundPickerPart[i].soundEvent, owner);
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
            for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                internals.soundPickerPart[i].LoadAudioData();
            }
        }

        /// <summary>
        /// Unloads the Audio Data of the <see cref="AudioClip"/>(s) of the <see cref="SoundContainerBase">SoundContainer</see>(s) from RAM
        /// </summary>
        public void UnloadAudioData() {
            for (int i = 0; i < internals.soundPickerPart.Length; i++) {
                internals.soundPickerPart[i].UnloadAudioData();
            }
        }
    }
}