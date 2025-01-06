// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundMixInternals {

#if UNITY_EDITOR
        public string notes = "Notes";
#endif
        public SoundEventModifier soundEventModifier = new SoundEventModifier();
        public SoundMixBase soundMixParent;

        /// <summary>
        /// Sets the volume based on decibel.
        /// The volume will be saved in the scriptable object.
        /// </summary>
        /// <param name="volumeDecibel"> Range NegativeInfinity to 0 </param>
        public void SetVolumeDecibel(float volumeDecibel) {
            Mathf.Clamp(volumeDecibel, Mathf.NegativeInfinity, 0);
            soundEventModifier.volumeDecibel = volumeDecibel;
            soundEventModifier.volumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
        }

        /// <summary>
        /// Sets the pitch based on semitones.
        /// The pitch will be saved in the scriptable object.
        /// </summary>
        /// <param name="pitchSemitone"> No range limit </param>
        public void SetPitchSemitone(float pitchSemitone) {
            soundEventModifier.pitchSemitone = pitchSemitone;
            soundEventModifier.pitchRatio = PitchScale.SemitonesToRatio(pitchSemitone);
        }

        public bool CheckIsInfiniteLoop(SoundMixBase soundMix, bool isEditor) {
            if (isEditor) {
                bool isInfiniteLoop = GetIfInfiniteLoop(soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious);
                if (isInfiniteLoop) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundMix)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
                return isInfiniteLoop;
            } else {
                return SoundManagerBase.Instance.Internals.InternalCheckSoundMixIsInfiniteLoop(soundMix);
            }
        }

        public bool GetIfInfiniteLoop(SoundMixBase soundMix, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious) {

            infiniteInstigator = null;
            infinitePrevious = null;

            if (soundMix == null) {
                return false;
            }

            List<SoundMixBase> toCheck = new List<SoundMixBase>();
            List<SoundMixBase> isChecked = new List<SoundMixBase>();

            toCheck.Add(soundMix);

            while (toCheck.Count > 0) {
                SoundMixBase soundMixChild = toCheck[0];
                toCheck.RemoveAt(0);
                if (soundMixChild != null) {
                    for (int i = 0; i < isChecked.Count; i++) {
                        if (isChecked[i] == soundMixChild) {
                            infiniteInstigator = isChecked[i];
                            return true;
                        }
                    }

                    if (soundMixChild.internals.soundMixParent != null) {
                        toCheck.Add(soundMixChild.internals.soundMixParent);
                        infinitePrevious = soundMixChild;
                    }

                    isChecked.Add(soundMixChild);
                }
            }
            return false;
        }
    }
}