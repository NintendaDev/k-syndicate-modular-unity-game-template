// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundVolumeGroupInternals {

#if UNITY_EDITOR
        public string notes = "Notes";
#endif
        // Decibel only used in the editor
        public float volumeDecibel = 0;
        public float volumeRatio = 1f;

        public float GetVolumeRatioClamped() {
            return Mathf.Clamp(volumeRatio, 0f, VolumeScale.volumeIncrease12dbMaxRatio);
        }

        /// <summary>
        /// Sets the volume in decibel scale.
        /// Updates the volume of the <see cref="SoundEventBase">SoundEvents</see> in realtime.
        /// The volume will be saved in the scriptable object.
        /// </summary>
        /// <param name="volumeDecibel"> Range from NegativeInfinity to +0 dB or +12 dB if "Enable Volume Increase" is active.</param>
        public void SetVolumeDecibel(float volumeDecibel) {
            volumeDecibel = Mathf.Clamp(volumeDecibel, Mathf.NegativeInfinity, VolumeScale.volumeIncrease12dbMaxDecibel);
            this.volumeDecibel = volumeDecibel;
            volumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
        }

        /// <summary>
        /// Returns the volume in decibel scale.
        /// </summary>
        /// <returns> Range from NegativeInfinity to +0 dB or +12 dB if "Enable Volume Increase" is active.</returns>
        public float GetVolumeDecibel() {
            return volumeDecibel;
        }

        /// <summary>
        /// Sets the volume in linear scale.
        /// Updates the volume of the <see cref="SoundEventBase">SoundEvents</see> in realtime.
        /// The volume will be saved in the scriptable object.
        /// </summary>
        /// <param name="volumeRatio"> Range from 0 to 1 or 3.981072 (+12dB) if "Enable Volume Increase" is active.</param>
        public void SetVolumeRatio(float volumeRatio) {
            volumeRatio = Mathf.Clamp(volumeRatio, 0f, VolumeScale.volumeIncrease12dbMaxRatio);
            this.volumeRatio = volumeRatio;
            volumeDecibel = VolumeScale.ConvertRatioToDecibel(volumeRatio);
        }

        /// <summary>
        /// Returns the volume in linear scale.
        /// </summary>
        /// <returns> Range from 0 to 1 or 3.981072 (+12dB) if "Enable Volume Increase" is active.</returns>
        public float GetVolumeRatio() {
            return volumeRatio;
        }
    }
}