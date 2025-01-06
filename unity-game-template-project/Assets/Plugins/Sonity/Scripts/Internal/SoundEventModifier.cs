// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    /// <summary>
    /// Holds variables of how to play the <see cref="SoundEventBase">SoundEvent</see>
    /// </summary>
    [Serializable]
    public class SoundEventModifier {

        // Used by the editor. Range -60dB to -0 dB
        public float volumeDecibel = 0f;
        public float volumeRatio = 1f;
        // Used By the editor range -24 to +24 semitones
        public float pitchSemitone = 0f;
        public float pitchRatio = 1f;
        // In Seconds
        public float delay = 0f;
        public float increase2D = 0f;
        public float intensity = 1f;

        // Used by the editor. Range -80dB to -0 dB
        public float reverbZoneMixDecibel = 0f;
        public float reverbZoneMixRatio = 1f;
        public float startPosition = 0f;
        public bool reverse = false;
        public float stereoPan = 0f;

        public int polyphony = 1;
        public float distanceScale = 1f;
        public float distortionIncrease = 0f;

        public float fadeInLength = 0f;
        public float fadeInShape = 2f;

        public float fadeOutLength = 0f;
        public float fadeOutShape = -2f;

        public bool followPosition = true;

        public bool bypassReverbZones = false;
        public bool bypassVoiceEffects = false;
        public bool bypassListenerEffects = false;

        // Use

        public bool volumeUse = false;
        public bool pitchUse = false;
        public bool delayUse = false;
        public bool increase2DUse = false;
        public bool intensityUse = false;

        public bool reverbZoneMixUse = false;
        public bool startPositionUse = false;
        public bool reverseUse = false;
        public bool stereoPanUse = false;

        public bool polyphonyUse = false;
        public bool distanceScaleUse = false;
        public bool distortionIncreaseUse = false;

        public bool fadeInLengthUse = false;
        public bool fadeInShapeUse = false;

        public bool fadeOutLengthUse = false;
        public bool fadeOutShapeUse = false;

        public bool followPositionUse = false;

        public bool bypassReverbZonesUse = false;
        public bool bypassVoiceEffectsUse = false;
        public bool bypassListenerEffectsUse = false;

        public void Reset() {
            volumeDecibel = 0f;
            volumeRatio = 1;
            pitchSemitone = 0f;
            pitchRatio = 1f;
            delay = 0f;
            increase2D = 0f;
            intensity = 1f;

            reverbZoneMixRatio = 1f;
            startPosition = 0f;
            reverse = false;
            stereoPan = 0f;

            polyphony = 1;
            distanceScale = 1f;
            distortionIncrease = 0f;

            fadeInLength = 0f;
            fadeInShape = 2f;

            fadeOutLength = 0f;
            fadeOutShape = -2f;

            followPosition = true;

            bypassReverbZones = false;
            bypassVoiceEffects = false;
            bypassListenerEffects = false;

            // Use

            volumeUse = false;
            pitchUse = false;
            delayUse = false;
            increase2DUse = false;
            intensityUse = false;

            reverbZoneMixUse = false;
            startPositionUse = false;
            reverseUse = false;
            stereoPanUse = false;

            polyphonyUse = false;
            distanceScaleUse = false;
            distortionIncreaseUse = false;

            fadeInLengthUse = false;
            fadeInShapeUse = false;

            fadeOutLengthUse = false;
            fadeOutShapeUse = false;

            followPositionUse = false;

            bypassReverbZonesUse = false;
            bypassVoiceEffectsUse = false;
            bypassListenerEffectsUse = false;
        }

        public void CloneTo(SoundEventModifier from) {
            // Volume decibel only editor
            volumeRatio = from.volumeRatio;
            // Pitch semitones only editor
            pitchRatio = from.pitchRatio;
            delay = from.delay;
            increase2D = from.increase2D;
            intensity = from.intensity;

            reverbZoneMixRatio = from.reverbZoneMixRatio;
            startPosition = from.startPosition;
            reverse = from.reverse;
            stereoPan = from.stereoPan;

            polyphony = from.polyphony;
            distanceScale = from.distanceScale;
            distortionIncrease = from.distortionIncrease;

            fadeInLength = from.fadeInLength;
            fadeInShape = from.fadeInShape;

            fadeOutLength = from.fadeOutLength;
            fadeOutShape = from.fadeOutShape;

            followPosition = from.followPosition;

            bypassReverbZones = from.bypassReverbZones;
            bypassVoiceEffects = from.bypassVoiceEffects;
            bypassListenerEffects = from.bypassListenerEffects;

            // Use

            volumeUse = from.volumeUse;
            pitchUse = from.pitchUse;
            delayUse = from.delayUse;
            increase2DUse = from.increase2DUse;
            intensityUse = from.intensityUse;

            reverbZoneMixUse = from.reverbZoneMixUse;
            startPositionUse = from.startPositionUse;
            reverseUse = from.reverseUse;
            stereoPanUse = from.stereoPanUse;

            polyphonyUse = from.polyphonyUse;
            distanceScaleUse = from.distanceScaleUse;
            distortionIncreaseUse = from.distortionIncreaseUse;

            fadeInLengthUse = from.fadeInLengthUse;
            fadeInShapeUse = from.fadeInShapeUse;

            fadeOutLengthUse = from.fadeOutLengthUse;
            fadeOutShapeUse = from.fadeOutShapeUse;

            followPositionUse = from.followPositionUse;

            bypassReverbZonesUse = from.bypassReverbZonesUse;
            bypassVoiceEffectsUse = from.bypassVoiceEffectsUse;
            bypassListenerEffectsUse = from.bypassListenerEffectsUse;
        }

        public void AddValuesTo(SoundEventModifier from) {

            if (from.volumeUse) {
                volumeRatio *= from.volumeRatio;
                volumeUse = true;
            }
            if (from.pitchUse) {
                pitchRatio *= from.pitchRatio;
                pitchUse = true;
            }
            if (from.delayUse) {
                delay += from.delay;
                delayUse = true;
            }
            if (from.increase2DUse) {
                increase2D = (increase2D - 1f) * (1f - from.increase2D) + 1f;
                increase2DUse = true;
            }
            if (from.intensityUse) {
                intensity *= from.intensity;
                intensityUse = true;
            }

            if (from.reverbZoneMixUse) {
                reverbZoneMixRatio *= from.reverbZoneMixRatio;
                reverbZoneMixUse = true;
            }
            if (from.startPositionUse) {
                startPosition = from.startPosition;
                startPositionUse = true;
            }
            if (from.reverseUse) {
                reverse = from.reverse;
                reverseUse = true;
            }
            if (from.stereoPanUse) {
                stereoPan += from.stereoPan;
                stereoPanUse = true;
            }

            if (from.polyphonyUse) {
                polyphony = from.polyphony;
                polyphonyUse = true;
            }
            if (from.distanceScaleUse) {
                distanceScale *= from.distanceScale;
                distanceScaleUse = true;
            }
            if (from.distortionIncreaseUse) {
                distortionIncrease = (distortionIncrease - 1f) * (1f - from.distortionIncrease) + 1f;
                distortionIncreaseUse = true;
            }

            if (from.fadeInLengthUse) {
                fadeInLength = from.fadeInLength;
                fadeInLengthUse = true;
            }
            if (from.fadeInShapeUse) {
                fadeInShape = from.fadeInShape;
                fadeInShapeUse = true;
            }

            if (from.fadeOutLengthUse) {
                fadeOutLength = from.fadeOutLength;
                fadeOutLengthUse = true;
            }
            if (from.fadeOutShapeUse) {
                fadeOutShape = from.fadeOutShape;
                fadeOutShapeUse = true;
            }

            if (from.followPositionUse) {
                followPosition = from.followPosition;
                followPositionUse = true;
            }

            if (from.bypassReverbZonesUse) {
                bypassReverbZones = from.bypassReverbZones;
                bypassReverbZonesUse = true;
            }
            if (from.bypassVoiceEffectsUse) {
                bypassVoiceEffects = from.bypassVoiceEffects;
                bypassVoiceEffectsUse = true;
            }
            if (from.bypassListenerEffectsUse) {
                bypassListenerEffects = from.bypassListenerEffects;
                bypassListenerEffectsUse = true;
            }
        }
    }
}