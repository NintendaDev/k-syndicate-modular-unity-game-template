// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using UnityEngine.Audio;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// The <see cref="SoundManagerBase">SoundManager</see> is the master object which is used to play sounds and manage global settings.
    /// An instance of this object is required in the scene in order to play <see cref="SoundEventBase">SoundEvents</see>.
    /// You can add the pre-made prefab called “SoundManager” found in “Assets\Plugins\Sonity\Prefabs” to your scene.
    /// Or you can add the “Sonity - Sound Manager” component to an empty <see cref="GameObject"/> in the scene, it works just as well.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity 🔊/Sonity - Sound Manager")]
    public class SoundManager : SoundManagerBase {

        /// <summary>
        /// The static instance of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        new public static SoundManager Instance { get { return (SoundManager)SoundManagerBase.Instance; } }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner) {
            Internals.Play(soundEvent, owner);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, SoundTag localSoundTag) {
            Internals.Play(soundEvent, owner, localSoundTag);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, params SoundParameterInternals[] soundParameters) {
            Internals.Play(soundEvent, owner, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/> (can follow positon)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play(SoundEvent soundEvent, Transform owner, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            Internals.Play(soundEvent, owner, localSoundTag, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position) {
            Internals.PlayAtPosition(soundEvent, owner, position);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, SoundTag localSoundTag) {
            Internals.PlayAtPosition(soundEvent, owner, position, localSoundTag);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, params SoundParameterInternals[] soundParameters) {
            Internals.PlayAtPosition(soundEvent, owner, position, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Transform"/> position with another <see cref="Transform"/> owner with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
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
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Transform position, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            Internals.PlayAtPosition(soundEvent, owner, position, localSoundTag, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position) {
            Internals.PlayAtPosition(soundEvent, owner, position);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, SoundTag localSoundTag) {
            Internals.PlayAtPosition(soundEvent, owner, position, localSoundTag);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, params SoundParameterInternals[] soundParameters) {
            Internals.PlayAtPosition(soundEvent, owner, position, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> at the <see cref="Vector3"/> position with the Local <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
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
        public void PlayAtPosition(SoundEvent soundEvent, Transform owner, Vector3 position, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            Internals.PlayAtPosition(soundEvent, owner, position, localSoundTag, soundParameters);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop(SoundEvent soundEvent, Transform owner, bool allowFadeOut = true) {
            Internals.Stop(soundEvent, owner, allowFadeOut);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> played at the position <see cref="Transform"/>.
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='position'>
        /// The position <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAtPosition(SoundEvent soundEvent, Transform position, bool allowFadeOut = true) {
            Internals.StopAtPosition(soundEvent, position, allowFadeOut);
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> with the owner <see cref="Transform"/>
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllAtOwner(Transform owner, bool allowFadeOut = true) {
            Internals.StopAllAtOwner(owner, allowFadeOut);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> everywhere
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopEverywhere(SoundEvent soundEvent, bool allowFadeOut = true) {
            Internals.StopEverywhere(soundEvent, allowFadeOut);
        }

        /// <summary>
        /// Stops all the <see cref="SoundEventBase">SoundEvents</see>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvents</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopEverything(bool allowFadeOut = true) {
            Internals.StopEverything(allowFadeOut);
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to pause
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void Pause(SoundEvent soundEvent, Transform owner, bool forcePause = false) {
            Internals.Pause(soundEvent, owner, forcePause);
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to unpause
        /// </param>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        public void Unpause(SoundEvent soundEvent, Transform owner) {
            Internals.Unpause(soundEvent, owner);
        }

        /// <summary>
        /// Pauses all <see cref="SoundEventBase">SoundEvents</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseAllAtOwner(Transform owner, bool forcePause = false) {
            Internals.PauseAllAtOwner(owner, forcePause);
        }

        /// <summary>
        /// Unpauses all <see cref="SoundEventBase">SoundEvents</see> with the owner <see cref="Transform"/> locally
        /// </summary>
        /// <param name='owner'>
        /// The owner <see cref="Transform"/>
        /// </param>
        public void UnpauseAllAtOwner(Transform owner) {
            Internals.UnpauseAllAtOwner(owner);
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> everywhere locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to pause
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseEverywhere(SoundEvent soundEvent, bool forcePause = false) {
            Internals.PauseEverywhere(soundEvent, forcePause);
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> everywhere locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to unpause
        /// </param>
        public void UnpauseEverywhere(SoundEvent soundEvent) {
            Internals.UnpauseEverywhere(soundEvent);
        }

        /// <summary>
        /// Pauses the all <see cref="SoundEventBase">SoundEvents</see> locally
        /// </summary>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseEverything(bool forcePause = false) {
            Internals.PauseEverything(forcePause);
        }

        /// <summary>
        /// Unpauses the all <see cref="SoundEventBase">SoundEvents</see> locally
        /// </summary>
        public void UnpauseEverything() {
            Internals.UnpauseEverything();
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        public void Play2D(SoundEvent soundEvent) {
            Internals.Play2D(soundEvent);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        public void Play2D(SoundEvent soundEvent, SoundTag localSoundTag) {
            Internals.Play2D(soundEvent, localSoundTag);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(SoundEvent soundEvent, params SoundParameterInternals[] soundParameters) {
            Internals.Play2D(soundEvent, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> with <see cref="SoundParameterInternals"/> with the Local <see cref="SoundTagBase">SoundTag</see> with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name='localSoundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> which will determine the Local <see cref="SoundTagBase">SoundTag</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </param>
        /// <param name='soundParameters'>
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void Play2D(SoundEvent soundEvent, SoundTag localSoundTag, params SoundParameterInternals[] soundParameters) {
            Internals.Play2D(soundEvent, localSoundTag, soundParameters);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the position with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// Useful for <see cref="UnityEngine.Events.UnityEvent"/>s because it only has one parameter
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Vector3"/> (can't follow position)
        /// </param>
        public void Play2DAtPosition(SoundEvent soundEvent, Vector3 position) {
            Internals.Play2DAtPosition(soundEvent, position);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the position with the 2D <see cref="Transform"/> as owner
        /// Useful to play e.g. UI or other 2D sounds without having to pass a <see cref="Transform"/>
        /// To make the sound 2D you still need to disable distance and set spatial blend to 0 in the <see cref="SoundContainerBase">SoundContainer</see>
        /// Useful for <see cref="UnityEngine.Events.UnityEvent"/>s because it only has one parameter
        /// </summary>
        /// <param name='position'>
        /// The position <see cref="Transform"/> (can follow position)
        /// </param>
        public void Play2DAtPosition(SoundEvent soundEvent, Transform position) {
            Internals.Play2DAtPosition(soundEvent, position);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void Stop2D(SoundEvent soundEvent, bool allowFadeOut = true) {
            Internals.Stop2D(soundEvent, allowFadeOut);
        }

        /// <summary>
        /// Stops all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAll2D(bool allowFadeOut = true) {
            Internals.StopAll2D(allowFadeOut);
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> at the 2D <see cref="Transform"/> locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to pause
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void Pause2D(SoundEvent soundEvent, bool forcePause = false) {
            Internals.Pause2D(soundEvent, forcePause);
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> at the 2D <see cref="Transform"/> locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to pause
        /// </param>
        public void Unpause2D(SoundEvent soundEvent) {
            Internals.Unpause2D(soundEvent);
        }

        /// <summary>
        /// Pauses all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/> locally
        /// </summary>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseAll2D(bool forcePause = false) {
            Internals.PauseAll2D(forcePause);
        }

        /// <summary>
        /// Unpauses all <see cref="SoundEventBase">SoundEvents</see> at the 2D <see cref="Transform"/> locally
        /// </summary>
        public void UnpauseAll2D() {
            Internals.UnpauseAll2D();
        }

        /// <summary>
        /// <para> Uses the 2D owner Transform </para>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If paused either locally or globally it returns <see cref="SoundEventState.Paused"/> </para>
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the <see cref="SoundEventState"/> from </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState Get2DSoundEventState(SoundEvent soundEvent) {
            return Internals.Get2DSoundEventState(soundEvent);
        }

        /// <summary>
        /// <para> Uses the 2D owner Transform </para>
        /// <para> Returns the length (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float Get2DLastPlayedClipLength(SoundEvent soundEvent, bool pitchSpeed) {
            return Internals.Get2DLastPlayedClipLength(soundEvent, pitchSpeed);
        }

        /// <summary>
        /// <para> Uses the 2D owner Transform </para>
        /// <para> Returns the current time (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float Get2DLastPlayedClipTimeSeconds(SoundEvent soundEvent, bool pitchSpeed) {
            return Internals.Get2DLastPlayedClipTimeSeconds(soundEvent, pitchSpeed);
        }

        /// <summary>
        /// <para> Uses the 2D owner Transform </para>
        /// <para> Returns the current time (in range 0 to 1) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <returns> Time ratio from 0 to 1 </returns>
        public float Get2DLastPlayedClipTimeRatio(SoundEvent soundEvent) {
            return Internals.Get2DLastPlayedClipTimeRatio(soundEvent);
        }

        /// <summary>
        /// <para> Uses the 2D owner Transform </para>
        /// <para> Returns the time (in seconds) since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Is calculated using the time scale selected in the <see cref="SoundManagerBase">SoundManager</see> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time played from </param>
        /// <returns> Time in seconds </returns>
        public float Get2DTimePlayed(SoundEvent soundEvent) {
            return Internals.Get2DTimePlayed(soundEvent);
        }

        /// <summary>
        /// Returns the owner <see cref="Transform"/> used by Play2D() etc
        /// </summary>
        /// <returns> The <see cref="Transform"/> used by Play2D() etc </returns>
        public Transform Get2DTransform() {
            return Internals.Get2DTransform();
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name="soundEvent">
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void PlayMusic(SoundEvent soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true) {
            Internals.PlayMusic(soundEvent, stopAllOtherMusic, allowFadeOut);
        }

        /// <summary>
        /// Plays the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/>
        /// </summary>
        /// <param name="soundEvent">
        /// The <see cref="SoundEventBase">SoundEvent</see> to play
        /// </param>
        /// <param name="stopAllOtherMusic">
        /// If all other <see cref="SoundEventBase">SoundEvents</see> played at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> should be stopped
        /// </param>
        /// <param name="allowFadeOut">
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        /// <param name="soundParameters">
        /// For example <see cref="SoundParameterVolumeDecibel"/> is used to modify how the <see cref="SoundEventBase">SoundEvent</see> is played
        /// </param>
        public void PlayMusic(SoundEvent soundEvent, bool stopAllOtherMusic = true, bool allowFadeOut = true, params SoundParameterInternals[] soundParameters) {
            Internals.PlayMusic(soundEvent, stopAllOtherMusic, allowFadeOut, soundParameters);
        }

        /// <summary>
        /// Stops the <see cref="SoundEventBase">SoundEvent</see> playing at the Music <see cref="Transform"/>
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to stop
        /// </param>
        /// <param name='allowFadeOut'>
        /// If the other stopped <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise they are going to be stopped immediately
        /// </param>
        public void StopMusic(SoundEvent soundEvent, bool allowFadeOut = true) {
            Internals.StopMusic(soundEvent, allowFadeOut);
        }

        /// <summary>
        /// Stops all the <see cref="SoundEventBase">SoundEvents</see> playing at the Music <see cref="Transform"/>
        /// </summary>
        /// <param name='allowFadeOut'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be allowed to fade out. Otherwise it is going to be stopped immediately
        /// </param>
        public void StopAllMusic(bool allowFadeOut = true) {
            Internals.StopAllMusic(allowFadeOut);
        }

        /// <summary>
        /// Pauses the <see cref="SoundEventBase">SoundEvent</see> playing at the Music <see cref="Transform"/> locally
        /// </summary>
        /// <param name='soundEvent'>
        /// The <see cref="SoundEventBase">SoundEvent</see> to pause
        /// </param>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseMusic(SoundEvent soundEvent, bool forcePause = false) {
            Internals.PauseMusic(soundEvent, forcePause);
        }

        /// <summary>
        /// Unpauses the <see cref="SoundEventBase">SoundEvent</see> at the <see cref="SoundManagerBase">SoundManagers</see> music <see cref="Transform"/> locally
        /// </summary>
        /// <param name="soundEvent">
        /// The <see cref="SoundEventBase">SoundEvent</see> to unpause
        /// </param>
        public void UnpauseMusic(SoundEvent soundEvent) {
            Internals.UnpauseMusic(soundEvent);
        }

        /// <summary>
        /// Pauses all the <see cref="SoundEventBase">SoundEvents</see> playing at the Music <see cref="Transform"/> locally
        /// </summary>
        /// <param name='forcePause'>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should be paused even if it is set to "Ignore Local Pause"
        /// </param>
        public void PauseAllMusic(bool forcePause = false) {
            Internals.PauseAllMusic(forcePause);
        }

        /// <summary>
        /// Unpauses all the <see cref="SoundEventBase">SoundEvents</see> playing at the Music <see cref="Transform"/> locally
        /// </summary>
        public void UnpauseAllMusic() {
            Internals.UnpauseAllMusic();
        }

        /// <summary>
        /// <para> Uses the Music owner Transform </para>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If paused either locally or globally it returns <see cref="SoundEventState.Paused"/> </para>
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the <see cref="SoundEventState"/> from </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState GetMusicSoundEventState(SoundEvent soundEvent) {
            return Internals.GetMusicSoundEventState(soundEvent);
        }

        /// <summary>
        /// <para> Uses the Music owner Transform </para>
        /// <para> Returns the length (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float GetMusicLastPlayedClipLength(SoundEvent soundEvent, bool pitchSpeed) {
            return Internals.GetMusicLastPlayedClipLength(soundEvent, pitchSpeed);
        }

        /// <summary>
        /// <para> Uses the Music owner Transform </para>
        /// <para> Returns the current time (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float GetMusicLastPlayedClipTimeSeconds(SoundEvent soundEvent, bool pitchSpeed) {
            return Internals.GetMusicLastPlayedClipTimeSeconds(soundEvent, pitchSpeed);
        }

        /// <summary>
        /// <para> Uses the Music owner Transform </para>
        /// <para> Returns the current time (in range 0 to 1) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <returns> Time ratio from 0 to 1 </returns>
        public float GetMusicLastPlayedClipTimeRatio(SoundEvent soundEvent) {
            return Internals.GetMusicLastPlayedClipTimeRatio(soundEvent);
        }

        /// <summary>
        /// <para> Uses the Music owner Transform </para>
        /// <para> Returns the time (in seconds) since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Is calculated using the time scale selected in the <see cref="SoundManagerBase">SoundManager</see> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time played from </param>
        /// <returns> Time in seconds </returns>
        public float GetMusicTimePlayed(SoundEvent soundEvent) {
            return Internals.GetMusicTimePlayed(soundEvent);
        }

        /// <summary>
        /// Returns the owner <see cref="Transform"/> used by PlayMusic() etc
        /// </summary>
        /// <returns> The <see cref="Transform"/> used by PlayMusic() etc </returns>
        public Transform GetMusicTransform() {
            return Internals.GetMusicTransform();
        }

        /// <summary>
        /// <para> If playing it returns <see cref="SoundEventState.Playing"/> </para> 
        /// <para> If paused either locally or globally it returns <see cref="SoundEventState.Paused"/> </para>
        /// <para> If not playing, but it is delayed it returns <see cref="SoundEventState.Delayed"/> </para> 
        /// <para> If not playing and it is not delayed it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// <para> If the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null it returns <see cref="SoundEventState.NotPlaying"/> </para> 
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the <see cref="SoundEventState"/> from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Returns <see cref="SoundEventState"/> of the <see cref="SoundEventBase">SoundEvents</see> <see cref="SoundEventInstance"/> </returns>
        public SoundEventState GetSoundEventState(SoundEvent soundEvent, Transform owner) {
            return Internals.GetSoundEventState(soundEvent, owner);
        }

        /// <summary>
        /// <para> Returns the length (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Length in seconds </returns>
        public float GetLastPlayedClipLength(SoundEvent soundEvent, Transform owner, bool pitchSpeed) {
            return Internals.GetLastPlayedClipLength(soundEvent, owner, pitchSpeed);
        }

        /// <summary>
        /// <para> Returns the current time (in seconds) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// <para> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="pitchSpeed"> If it should be scaled by pitch. E.g. -12 semitones will be twice as long </param>
        /// <returns> Time in seconds </returns>
        public float GetLastPlayedClipTimeSeconds(SoundEvent soundEvent, Transform owner, bool pitchSpeed) {
            return Internals.GetLastPlayedClipTimeSeconds(soundEvent, owner, pitchSpeed);
        }

        /// <summary>
        /// <para> Returns the current time (in range 0 to 1) of the <see cref="AudioClip"/> in the last played <see cref="AudioSource"/> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Time ratio from 0 to 1 </returns>
        public float GetLastPlayedClipTimeRatio(SoundEvent soundEvent, Transform owner) {
            return Internals.GetLastPlayedClipTimeRatio(soundEvent, owner);
        }

        /// <summary>
        /// <para> Provides a block of spectrum data from <see cref="AudioSource"/>s </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> to get the spectrum data from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <param name="samples"> The array to populate with audio samples. Its length must be a power of 2 </param>
        /// <param name="channel"> The channel to sample from </param>
        /// <param name="window"> The <see cref="FFTWindow"/> type to use when sampling </param>
        /// <param name="spectrumDataFrom"> Where to get the spectrum data from </param>
        public void GetSpectrumData(SoundEvent soundEvent, Transform owner, ref float[] samples, int channel, FFTWindow window, SpectrumDataFrom spectrumDataFrom) {
            Internals.GetSpectrumData(soundEvent, owner, ref samples, channel, window, spectrumDataFrom);
        }

        /// <summary>
        /// <para> Returns the last played <see cref="AudioSource"/> </para>
        /// <para> Note that the <see cref="AudioSource"/> might be stolen or reused for different Voices over time </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> to get the <see cref="AudioSource"/> from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        public AudioSource GetLastPlayedAudioSource(SoundEvent soundEvent, Transform owner) {
            return Internals.GetLastPlayedAudioSource(soundEvent, owner);
        }

        /// <summary>
        /// <para> Returns the max length (in seconds) of the <see cref="SoundEventBase">SoundEvent</see> (calculated from the longest audioClip) </para>
        /// <para> Is scaled by the pitch of the <see cref="SoundEventBase">SoundEvent</see> and <see cref="SoundContainerBase">SoundContainer</see> </para>
        /// <para> Does not take into account random, intensity or parameter pitch </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the length from </param>
        /// <returns> The max length in seconds </returns>
        public float GetMaxLength(SoundEvent soundEvent) {
            return Internals.GetMaxLength(soundEvent);
        }

        /// <summary>
        /// <para> Returns the time (in seconds) since the <see cref="SoundEventBase">SoundEvent</see> was played </para>
        /// <para> Is calculated using the time scale selected in the <see cref="SoundManagerBase">SoundManager</see> </para>
        /// <para> Returns 0 if the <see cref="SoundEventInstance"/> is not playing </para>
        /// <para> Returns 0 if the <see cref="SoundEventBase">SoundEvent</see> or <see cref="Transform"/> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> get the time played from </param>
        /// <param name="owner"> The owner <see cref="Transform"/> </param>
        /// <returns> Time in seconds </returns>
        public float GetTimePlayed(SoundEvent soundEvent, Transform owner) {
            return Internals.GetTimePlayed(soundEvent, owner);
        }

        /// <summary
        /// <para> Returns if any <see cref="SoundContainerBase">SoundContainers</see> in the <see cref="SoundEventBase">SoundEvent</see> is set to looping </para>
        /// <para> Returns false if the <see cref="SoundEventBase">SoundEvent</see> is null </para>
        /// </summary>
        /// <param name="soundEvent"> The <see cref="SoundEventBase">SoundEvent</see> to check if its looping </param>
        /// <returns> If the <see cref="SoundEventBase">SoundEvent</see> contains a loop </returns>
        public bool GetContainsLoop(SoundEventBase soundEvent) {
            return Internals.GetContainsLoop(soundEvent);
        }

        /// <summary>
        /// Returns the <see cref="Transform"/> of the <see cref="SoundManagerBase">SoundManager</see>
        /// </summary>
        /// <returns> The <see cref="Transform"/> of the <see cref="SoundManagerBase">SoundManager</see> </returns>
        public Transform GetSoundManagerTransform() {
            return Internals.GetSoundManagerTransform();
        }

        /// <summary>
        /// Loads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        public void LoadAudioData(SoundEventBase soundEvent) {
            Internals.LoadAudioData(soundEvent);
        }

        /// <summary>
        /// Unloads the audio data of any <see cref="AudioClip"/>s assigned to the <see cref="SoundContainerBase">SoundContainers</see> of the <see cref="SoundEventBase">SoundEvent</see>
        /// </summary>
        public void UnloadAudioData(SoundEventBase soundEvent) {
            Internals.UnloadAudioData(soundEvent);
        }

        /// <summary>
        /// Pauses the all <see cref="SoundEventBase">SoundEvents</see> except those who are set to "Ignore Global Pause"
        /// </summary>
        public void SetGlobalPause() {
            Internals.SetGlobalPause();
        }

        /// <summary>
        /// Unpauses the all <see cref="SoundEventBase">SoundEvents</see> except those who are set to "Ignore Global Pause"
        /// </summary>
        public void SetGlobalUnpause() {
            Internals.SetGlobalUnpause();
        }

        /// <summary>
        /// Returns if Global Pause is enabled
        /// </summary>
        public bool GetGlobalPaused() {
            return Internals.GetGlobalPaused();
        }

        /// <summary>
        /// Sets a global volume of all sounds in decibels using <see cref="AudioListener"/>.volume, ranging from <see cref="Mathf.NegativeInfinity"/> dB to 0 dB
        /// </summary>
        /// <param name="volumeDecibel"> The volume in decibels, ranging from <see cref="Mathf.NegativeInfinity"/> dB to 0 dB</param>
        public void SetGlobalVolumeDecibel(float volumeDecibel) {
            Internals.SetGlobalVolumeDecibel(volumeDecibel);
        }

        /// <summary>
        /// Returns the global volume in decibels, ranging from <see cref="Mathf.NegativeInfinity"/> dB to 0 dB
        /// </summary>
        /// <returns> The volume in decibels, ranging from <see cref="Mathf.NegativeInfinity"/> dB to 0 dB</returns>
        public float GetGlobalVolumeDecibel() {
            return Internals.GetGlobalVolumeDecibel();
        }

        /// <summary>
        /// Sets a global volume of all sounds in linear scale using <see cref="AudioListener"/>.volume, ranging from 0 to 1
        /// </summary>
        /// <param name="volumeRatio"> The volume in linear scale, ranging from 0 to 1</param>
        public void SetGlobalVolumeRatio(float volumeRatio) {
            Internals.SetGlobalVolumeRatio(volumeRatio);
        }

        /// <summary>
        /// Returns the global volume in linear scale, ranging from 0 to 1
        /// </summary>
        /// <returns> The volume in linear scale, ranging from 0 to 1 </returns>
        public float GetGlobalVolumeRatio() {
            return Internals.GetGlobalVolumeRatio();
        }

        /// <summary>
        /// Sets the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <param name='soundTag'>
        /// The <see cref="SoundTagBase">SoundTag</see> to change to
        /// </param>
        public void SetGlobalSoundTag(SoundTag soundTag) {
            Internals.SetGlobalSoundTag(soundTag);
        }

        /// <summary>
        /// Returns the global <see cref="SoundTagBase">SoundTag</see>
        /// </summary>
        /// <returns> The global <see cref="SoundTagBase">SoundTag</see> </returns>
        public SoundTag GetGlobalSoundTag() {
            return (SoundTag)Internals.GetGlobalSoundTag();
        }

        /// <summary>
        /// Sets the global distance scale (default is a scale of 100 units)
        /// </summary>
        /// <param name='distanceScale'>
        /// The new range scale
        /// </param>
        public void SetGlobalDistanceScale(float distanceScale) {
            Internals.SetGlobalDistanceScale(distanceScale);
        }

        /// <summary>
        /// Returns the global distance scale
        /// </summary>
        public float GetGlobalDistanceScale() {
            return Internals.GetGlobalDistanceScale();
        }

        /// <summary>
        /// Set if speed of sound should be enabled
        /// </summary>
        /// <param name='speedOfSoundEnabled'>
        /// Should speed of Sound be active
        /// </param>
        public void SetSpeedOfSoundEnabled(bool speedOfSoundEnabled) {
            Internals.SetSpeedOfSoundEnabled(speedOfSoundEnabled);
        }

        /// <summary>
        /// <para> Set the speed of sound scale </para>
        /// <para> The default is a multiplier of 1 (by the base value of 340 unity units per second) </para>
        /// </summary>
        /// <param name='speedOfSoundScale'>
        /// The scale of speed of Sound (default is a multipler of 1)
        /// </param>
        public void SetSpeedOfSoundScale(float speedOfSoundScale) {
            Internals.SetSpeedOfSoundScale(speedOfSoundScale);
        }

        /// <summary>
        /// Returns the speed of sound scale
        /// </summary>
        public float GetSpeedOfSoundScale() {
            return Internals.GetSpeedOfSoundScale();
        }

        /// <summary>
        /// Sets the <see cref="Voice"/> limit
        /// </summary>
        /// <param name='voiceLimit'>
        /// The maximum number of <see cref="Voice"/>s which can be played at the same time
        /// </param>
        public void SetVoiceLimit(int voiceLimit) {
            Internals.SetVoiceLimit(voiceLimit);
        }

        /// <summary>
        /// Returns the <see cref="Voice"/> limit
        /// </summary>
        public int GetVoiceLimit() {
            return Internals.GetVoiceLimit();
        }

        /// <summary>
        /// Sets the <see cref="VoiceEffect"/> limit
        /// </summary>
        public void SetVoiceEffectLimit(int voiceEffectLimit) {
            Internals.SetVoiceEffectLimit(voiceEffectLimit);
        }

        /// <summary>
        /// Returns the <see cref="VoiceEffect"/> limit
        /// </summary>
        public int GetVoiceEffectLimit() {
            return Internals.GetVoiceEffectLimit();
        }

        /// <summary>
        /// Disables/enables all the Play functionality
        /// </summary>
        /// <param name="disablePlayingSounds"></param>
        public void SetDisablePlayingSounds(bool disablePlayingSounds) {
            Internals.SetDisablePlayingSounds(disablePlayingSounds);
        }

        /// <summary>
        /// If the Play functionality is disabled
        /// </summary>
        public bool GetDisablePlayingSounds() {
            return Internals.GetDisablePlayingSounds();
        }

        /// <summary>
        /// If you have Adressable AudioMixer enabled in the SoundManager you need to use this AudioMixer instance for e.g. AudioMixer.SetFloat().
        /// This is to fix problems when the AudioMixer is an adressable asset because it might create both a normal and an adressable instance with different IDs.
        /// For Adressable AudioMixer to work, you need to add the package "com.unity.addressables".
        /// You also need to define the Script Define Symbol "SONITY_ENABLE_ADRESSABLE_AUDIOMIXER".
        /// Asmdef_Sonity.Internal.Runtime references Unity.Addressables and Unity.ResourceManager.
        /// Asmdef_Sonity.Internal.Editor references Unity.Addressables.
        /// </summary>
        public AudioMixer GetAdressableAudioMixer() {
            return Internals.GetAdressableAudioMixer();
        }
    }
}