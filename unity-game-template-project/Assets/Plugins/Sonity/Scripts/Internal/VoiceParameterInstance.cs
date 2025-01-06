// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public class VoiceParameterInstance {

        public SoundParameterInternals[] soundParameters;

        public void CopyTo(VoiceParameterInstance from) {
            soundParameters = from.soundParameters;
            soundParametersCanChangeVolume = from.SoundParametersCanChangeVolume();
            soundParametersHasContinuousIntensity = from.SoundParametersHasContinuousIntensity();
            currentModifier.CloneTo(from.currentModifier);
            offsetModifier.CloneTo(from.offsetModifier);
        }

        public void ResetModifiers() {
            soundParameters = null;
            soundParametersCanChangeVolume = false;
            soundParametersHasContinuousIntensity = false;
            currentModifier.Reset();
            offsetModifier.Reset();
        }

        private bool soundParametersCanChangeVolume = false;
        public bool SoundParametersCanChangeVolume() {
            return soundParametersCanChangeVolume;
        }

        private bool soundParametersHasContinuousIntensity = false;
        public bool SoundParametersHasContinuousIntensity() {
            return soundParametersHasContinuousIntensity;
        }

        // Used by update mode continuous
        public SoundEventModifier currentModifier = new SoundEventModifier();

        // Used by update mode once
        public SoundEventModifier offsetModifier = new SoundEventModifier();

        public void ModifiersAddValuesToOffset(SoundEventModifier from) {
            if (from != null) {
                offsetModifier.AddValuesTo(from);
            }
        }

        public void SetSoundParameters(SoundParameterInternals[] soundParameters) {
            this.soundParameters = soundParameters;
        }

        public void CheckIfSoundParametersCanChangeVolume(SoundContainerBase soundContainer){
            soundParametersCanChangeVolume = false;
            if (soundParameters != null && soundParameters.Length > 0) {
                for (int i = 0; i < soundParameters.Length; i++) {
                    if (soundParameters[i] != null && soundParameters[i].internals.updateMode == UpdateMode.Continuous) {
                        if (soundParameters[i].internals.type == SoundParameterType.Volume 
                            || (soundContainer.internals.data.volumeIntensityEnable && soundParameters[i].internals.type == SoundParameterType.Intensity)) {
                            soundParametersCanChangeVolume = true;
                            return;
                        }
                    }
                }
            }
        }

        public void CheckIfSoundParameterHasContinuousIntensity() {
            soundParametersHasContinuousIntensity = false;
            if (soundParameters != null && soundParameters.Length > 0) {
                for (int i = 0; i < soundParameters.Length; i++) {
                    if (soundParameters[i] != null && soundParameters[i].internals.type == SoundParameterType.Intensity && soundParameters[i].internals.updateMode == UpdateMode.Continuous) {
                        soundParametersHasContinuousIntensity = true;
                        return;
                    }
                }
            }
        }

        public void SoundParameterUpdateOnce() {
            // If no update is required, then it is only done once
            if (soundParameters == null || soundParameters.Length == 0) {
                currentModifier.CloneTo(offsetModifier);
            } else {
                for (int i = 0; i < soundParameters.Length; i++) {
                    if (soundParameters[i] != null && soundParameters[i].internals.updateMode == UpdateMode.Once) {
                        // Volume
                        if (soundParameters[i].internals.type == SoundParameterType.Volume) {
                            offsetModifier.volumeRatio *= soundParameters[i].internals.valueFloat;
                            offsetModifier.volumeUse = true;
                        }
                        // Pitch
                        else if (soundParameters[i].internals.type == SoundParameterType.Pitch) {
                            offsetModifier.pitchRatio *= soundParameters[i].internals.valueFloat;
                            offsetModifier.pitchUse = true;
                        }
                        // Delay
                        else if (soundParameters[i].internals.type == SoundParameterType.Delay) {
                            offsetModifier.delay += soundParameters[i].internals.valueFloat;
                            offsetModifier.delayUse = true;
                        }
                        // Increase 2D
                        else if (soundParameters[i].internals.type == SoundParameterType.Increase2D) {
                            offsetModifier.increase2D = (offsetModifier.increase2D - 1f) * (1f - soundParameters[i].internals.valueFloat) + 1f;
                            offsetModifier.increase2DUse = true;
                        }
                        // Intensity
                        else if (soundParameters[i].internals.type == SoundParameterType.Intensity) {
                            offsetModifier.intensity *= soundParameters[i].internals.valueFloat;
                            offsetModifier.intensityUse = true;
                        }
                        // Reverb Zone Mix Ratio
                        else if (soundParameters[i].internals.type == SoundParameterType.ReverbZoneMix) {
                            offsetModifier.reverbZoneMixRatio *= soundParameters[i].internals.valueFloat;
                            offsetModifier.reverbZoneMixUse = true;
                        }
                        // Start Position
                        else if (soundParameters[i].internals.type == SoundParameterType.StartPosition) {
                            offsetModifier.startPosition = soundParameters[i].internals.valueFloat;
                            offsetModifier.startPositionUse = true;
                        }
                        // Reverse Enabled
                        else if (soundParameters[i].internals.type == SoundParameterType.Reverse) {
                            offsetModifier.reverse = soundParameters[i].internals.valueBool;
                            offsetModifier.reverseUse = true;
                        }
                        // Stereo Pan
                        else if (soundParameters[i].internals.type == SoundParameterType.StereoPan) {
                            offsetModifier.stereoPan += soundParameters[i].internals.valueFloat;
                            offsetModifier.stereoPanUse = true;
                        }
                        // Polyphony
                        else if (soundParameters[i].internals.type == SoundParameterType.Polyphony) {
                            offsetModifier.polyphony = soundParameters[i].internals.valueInt;
                            offsetModifier.polyphonyUse = true;
                        }
                        // Distance Scale
                        else if (soundParameters[i].internals.type == SoundParameterType.DistanceScale) {
                            offsetModifier.distanceScale *= soundParameters[i].internals.valueFloat;
                            offsetModifier.distanceScaleUse = true;
                        }
                        // Distortion Increase
                        else if (soundParameters[i].internals.type == SoundParameterType.DistortionIncrease) {
                            offsetModifier.distortionIncrease = (offsetModifier.distortionIncrease - 1f) * (1f - soundParameters[i].internals.valueFloat) + 1f;
                            offsetModifier.distortionIncreaseUse = true;
                        }
                        // Fade In Length
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeInLength) {
                            offsetModifier.fadeInLength = soundParameters[i].internals.valueFloat;
                            offsetModifier.fadeInLengthUse = true;
                        }
                        // Fade In Shape
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeInShape) {
                            offsetModifier.fadeInShape = soundParameters[i].internals.valueFloat;
                            offsetModifier.fadeInShapeUse = true;
                        }
                        // Fade Out Length
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeOutLength) {
                            offsetModifier.fadeOutLength = soundParameters[i].internals.valueFloat;
                            offsetModifier.fadeOutLengthUse = true;
                        }
                        // Fade Out Shape
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeOutShape) {
                            offsetModifier.fadeOutShape = soundParameters[i].internals.valueFloat;
                            offsetModifier.fadeOutShapeUse = true;
                        }
                        // Follow Position
                        else if (soundParameters[i].internals.type == SoundParameterType.FollowPosition) {
                            offsetModifier.followPosition = soundParameters[i].internals.valueBool;
                            offsetModifier.followPositionUse = true;
                        }
                        // Bypass Reverb Zones
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassReverbZones) {
                            offsetModifier.bypassReverbZones = soundParameters[i].internals.valueBool;
                            offsetModifier.bypassReverbZonesUse = true;
                        }
                        // Bypass Voice Effects
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassVoiceEffects) {
                            offsetModifier.bypassVoiceEffects = soundParameters[i].internals.valueBool;
                            offsetModifier.bypassVoiceEffectsUse = true;
                        }
                        // Bypass Listener Effects
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassListenerEffects) {
                            offsetModifier.bypassListenerEffects = soundParameters[i].internals.valueBool;
                            offsetModifier.bypassListenerEffectsUse = true;
                        }
                    }
                }
            }
        }

        public void SoundParameterUpdateContinuous() {
            // Clones the offset values to the current values (for continuous update)
            if (soundParameters != null && soundParameters.Length > 0) {
                currentModifier.CloneTo(offsetModifier);
                for (int i = 0; i < soundParameters.Length; i++) {
                    if (soundParameters[i] != null && soundParameters[i].internals.updateMode == UpdateMode.Continuous) {
                        // Volume
                        if (soundParameters[i].internals.type == SoundParameterType.Volume) {
                            currentModifier.volumeRatio *= soundParameters[i].internals.valueFloat;
                            currentModifier.volumeUse = true;
                        }
                        // Pitch
                        else if (soundParameters[i].internals.type == SoundParameterType.Pitch) {
                            currentModifier.pitchRatio *= soundParameters[i].internals.valueFloat;
                            currentModifier.pitchUse = true;
                        }
                        // Delay Once only
                        // Increase 2D
                        else if (soundParameters[i].internals.type == SoundParameterType.Increase2D) {
                            currentModifier.increase2D = (currentModifier.increase2D - 1f) * (1f - soundParameters[i].internals.valueFloat) + 1f;
                            currentModifier.increase2DUse = true;
                        }
                        // Intensity
                        else if (soundParameters[i].internals.type == SoundParameterType.Intensity) {
                            currentModifier.intensity *= soundParameters[i].internals.valueFloat;
                            currentModifier.intensityUse = true;
                        }
                        // Reverb Zone Mix Ratio
                        else if (soundParameters[i].internals.type == SoundParameterType.ReverbZoneMix) {
                            currentModifier.reverbZoneMixRatio = soundParameters[i].internals.valueFloat;
                            currentModifier.reverbZoneMixUse = true;
                        }
                        // Start Position Once only
                        // Reverse Enabled
                        else if (soundParameters[i].internals.type == SoundParameterType.Reverse) {
                            currentModifier.reverse = soundParameters[i].internals.valueBool;
                            currentModifier.reverseUse = true;
                        }
                        // Stereo Pan
                        else if (soundParameters[i].internals.type == SoundParameterType.StereoPan) {
                            currentModifier.stereoPan += soundParameters[i].internals.valueFloat;
                            currentModifier.stereoPanUse = true;
                        }
                        // Polyphony Once Only
                        // Distance Scale Once Only
                        // Distortion Increase
                        else if (soundParameters[i].internals.type == SoundParameterType.DistortionIncrease) {
                            currentModifier.distortionIncrease = (currentModifier.distortionIncrease - 1f) * (1f - soundParameters[i].internals.valueFloat) + 1f;
                            currentModifier.distortionIncreaseUse = true;
                        }
                        // Fade In Length
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeInLength) {
                            currentModifier.fadeInLength = soundParameters[i].internals.valueFloat;
                            currentModifier.fadeInLengthUse = true;
                        }
                        // Fade In Shape
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeInShape) {
                            currentModifier.fadeInShape = soundParameters[i].internals.valueFloat;
                            currentModifier.fadeInShapeUse = true;
                        }
                        // Fade Out Length
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeOutLength) {
                            currentModifier.fadeOutLength = soundParameters[i].internals.valueFloat;
                            currentModifier.fadeOutLengthUse = true;
                        }
                        // Fade Out Shape
                        else if (soundParameters[i].internals.type == SoundParameterType.FadeOutShape) {
                            currentModifier.fadeOutShape = soundParameters[i].internals.valueFloat;
                            currentModifier.fadeOutShapeUse = true;
                        }
                        // Follow Position
                        else if (soundParameters[i].internals.type == SoundParameterType.FollowPosition) {
                            offsetModifier.followPosition = soundParameters[i].internals.valueBool;
                            offsetModifier.followPositionUse = true;
                        }
                        // Bypass Reverb Zones
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassReverbZones) {
                            currentModifier.bypassReverbZones = soundParameters[i].internals.valueBool;
                            currentModifier.bypassReverbZonesUse = true;
                        }
                        // Bypass Voice Effects
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassVoiceEffects) {
                            currentModifier.bypassVoiceEffects = soundParameters[i].internals.valueBool;
                            currentModifier.bypassVoiceEffectsUse = true;
                        }
                        // Bypass Listener Effects
                        else if (soundParameters[i].internals.type == SoundParameterType.BypassListenerEffects) {
                            currentModifier.bypassListenerEffects = soundParameters[i].internals.valueBool;
                            currentModifier.bypassListenerEffectsUse = true;
                        }
                    }
                }
            }
        }
    }
}