// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundEventTimelineData {

        public float delay = 0f;
        public float volumeRatio = 1f;
        public float volumeDecibel = 0f;

        public float GetVolumeRatio() {
            return volumeRatio;
        }

        public float EditorGetVolumeLinear() {
            return Mathf.Clamp(-(volumeDecibel / VolumeScale.lowestVolumeDecibel) + 1f, 0f, 1f);
        }
    }
}