// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundDataGroupInternals {

#if UNITY_EDITOR
        public string notes = "Notes";
#endif

        public SoundDataGroupBase[] soundDataGroupChildren = new SoundDataGroupBase[0];
        public SoundEventBase[] soundEvents = new SoundEventBase[1];

        public void LoadUnloadAudioData(bool load, bool includeChildren, SoundDataGroupBase parent) {
            if (SoundManagerBase.Instance == null) {
                Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
                return;
            } else {
                if (!SoundManagerBase.Instance.Internals.InternalCheckSoundDataGroupIsInfiniteLoop(parent)) {
                    if (soundEvents != null) {
                        for (int i = 0; i < soundEvents.Length; i++) {
                            if (soundEvents[i] != null) {
                                if (load) {
                                    soundEvents[i].LoadAudioData();
                                } else {
                                    soundEvents[i].UnloadAudioData();
                                }
                            }
                        }
                    }
                    if (includeChildren) {
                        for (int i = 0; i < soundDataGroupChildren.Length; i++) {
                            if (soundDataGroupChildren[i] != null) {
                                soundDataGroupChildren[i].internals.LoadUnloadAudioData(load, includeChildren, soundDataGroupChildren[i]);
                            }
                        }
                    }
                }
            }
        }

        public bool GetIfInfiniteLoop(SoundDataGroupBase soundDataGroup, out SoundDataGroupBase infiniteInstigator, out SoundDataGroupBase infinitePrevious) {

            infiniteInstigator = null;
            infinitePrevious = null;

            if (soundDataGroup == null) {
                return false;
            }

            List<SoundDataGroupBase> toCheck = new List<SoundDataGroupBase>();
            List<SoundDataGroupBase> isChecked = new List<SoundDataGroupBase>();

            toCheck.Add(soundDataGroup);

            while (toCheck.Count > 0) {
                SoundDataGroupBase soundDataGroupChild = toCheck[0];
                toCheck.RemoveAt(0);
                if (soundDataGroupChild != null) {
                    for (int i = 0; i < isChecked.Count; i++) {
                        if (isChecked[i] == soundDataGroupChild) {
                            infiniteInstigator = isChecked[i];
                            return true;
                        }
                    }

                    if (soundDataGroupChild.internals.soundDataGroupChildren != null && soundDataGroupChild.internals.soundDataGroupChildren.Length > 0) {
                        toCheck.AddRange(soundDataGroupChild.internals.soundDataGroupChildren);
                        infinitePrevious = soundDataGroupChild;
                    }
                    isChecked.Add(soundDataGroupChild);
                }
            }
            return false;
        }
    }
}