// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPickerPart {

        public SoundEventBase soundEvent;
        public SoundEventModifier soundEventModifier = new SoundEventModifier();
        public bool expandModifier = true;

        private bool SoundManagerIsNull() {
            if (SoundManagerBase.Instance == null) {
                if (!ApplicationQuitting.GetApplicationIsQuitting()) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
                }
                return true;
            }
            return false;
        }

        public void Play(Transform owner, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.Play, owner, null, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void PlayAtPosition(Transform owner, Transform position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtTransform, owner, null, position, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void PlayAtPosition(Transform owner, Vector3 position, SoundParameterInternals[] soundParameters, SoundParameterInternals soundParameterDistanceScale, SoundTagBase localSoundTag) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.InternalPlay(soundEvent, SoundEventPlayType.PlayAtVector, owner, position, null, soundEventModifier, null, soundParameters, soundParameterDistanceScale, localSoundTag);
            }
        }

        public void Stop(Transform owner, bool allowFadeOut) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.Stop(soundEvent, owner, allowFadeOut);
            }
        }

        public void StopAtPosition(Transform position, bool allowFadeOut) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.StopAtPosition(soundEvent, position, allowFadeOut);
            }
        }

        public void Pause(Transform owner, bool forcePause = false) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.Pause(soundEvent, owner, forcePause);
            }
        }

        public void Unpause(Transform owner) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.Unpause(soundEvent, owner);
            }
        }

        public void PauseEverywhere(bool forcePause = false) {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.PauseEverywhere(soundEvent, forcePause);
            }
        }

        public void UnpauseEverywhere() {
            if (!SoundManagerIsNull()) {
                SoundManagerBase.Instance.Internals.UnpauseEverywhere(soundEvent);
            }
        }

        public void LoadAudioData() {
            if (soundEvent != null) {
                soundEvent.LoadAudioData();
            }
        }

        public void UnloadAudioData() {
            if (soundEvent != null) {
                soundEvent.UnloadAudioData();
            }
        }
    }
}