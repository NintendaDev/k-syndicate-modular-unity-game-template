// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class VolumeScale {

#if SONITY_ENABLE_VOLUME_INCREASE
        // AudioListener volume
        // 1000 ratio is +60 dB
        public static float volumeIncrease60dbAudioListenerRatioMax = 1000f;

        // 0.001 ratio is -60dB
        public static float volumeIncrease60dbAudioListenerRatioLowerBack = 0.001f;

        // Volume increase 24 dB (for SoundContainer and SoundEvent)
        public static float volumeIncrease24dbMaxDecibel = 24f;
        // 15.84893 ratio is 24 decibels
        public static float volumeIncrease24dbMaxRatio = 15.84893f;
        // 0.06309573 ratio is -24 decibels
        public static float volumeIncrease24dbMaxToOneRatio = 0.06309573f;

        // Volume increase 12 dB (for SoundVolumeGroup)
        public static float volumeIncrease12dbMaxDecibel = 12f;
        // 3.981072 ratio is 12 decibels
        public static float volumeIncrease12dbMaxRatio = 3.981072f;
        // 0.2511886 ratio is -12 decibels
        public static float volumeIncrease12dbMaxToOneRatio = 0.2511886f;

#else
        // AudioListener volume no effect
        public static float volumeIncrease60dbAudioListenerRatioMax = 1f;
        public static float volumeIncrease60dbAudioListenerRatioLowerBack = 1f;

        // Volume increase no effect 24 dB (for SoundContainer and SoundEvent)
        public static float volumeIncrease24dbMaxDecibel = 0f;
        public static float volumeIncrease24dbMaxRatio = 1f;
        public static float volumeIncrease24dbMaxToOneRatio = 1f;

        // Volume increase no effect 12 dB (for SoundVolumeGroup)
        public static float volumeIncrease12dbMaxDecibel = 0f;
        public static float volumeIncrease12dbMaxRatio = 1f;
        public static float volumeIncrease12dbMaxToOneRatio = 1f;
#endif

        // The lowest volume in decibels before snapping to -infinity
        public static float lowestVolumeDecibel = -60f;

        // The lowest reverb mix in decibels before snapping to -infinity
        public static float lowestReverbMixDecibel = -80f;

        public static float ConvertRatioToDecibel(float volumeRatio) {
            return 6.0206f * Mathf.Log(volumeRatio, 2f);
        }

        public static float ConvertDecibelToRatio(float volumeDecibel) {
            return Mathf.Pow(2f, volumeDecibel / 6.0206f);
        }
    }
}
