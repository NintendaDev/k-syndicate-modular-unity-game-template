// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// If the SoundParameter should update once or continuously
    /// </summary>
    public enum UpdateMode {
        /// <summary>
        /// The (for example) <see cref="SoundParameterVolumeDecibel"/> can update continuously
        /// </summary>
        Continuous,
        /// <summary>
        /// The (for example) <see cref="SoundParameterDelay"/> can update once at start
        /// </summary>
        Once,
    }

    /// <summary>
    /// Volume offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
    /// </summary>
    public class SoundParameterVolumeDecibel : SoundParameterInternals {

        /// <summary>
        /// Volume offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
        /// </summary>
        /// <param name="volumeDecibel"> Volume offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0 </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterVolumeDecibel(float volumeDecibel = 0f, UpdateMode updateMode = UpdateMode.Once) {
            VolumeDecibel = volumeDecibel;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Volume;
        }

        /// <summary>
        /// Volume offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
        /// </summary>
        public float VolumeDecibel {
            get { return  VolumeScale.ConvertRatioToDecibel(internals.valueFloat); }
            set {
                internals.valueFloat = Mathf.Clamp(VolumeScale.ConvertDecibelToRatio(value), 0f, 1f);
            }
        }
    }

    /// <summary>
    /// Volume multiplier. Range 0 to 1
    /// </summary>
    public class SoundParameterVolumeRatio : SoundParameterInternals {

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="volumeRatio"> Volume multiplier. Range 0 to 1 </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterVolumeRatio(float volumeRatio = 1f, UpdateMode updateMode = UpdateMode.Once) {
            VolumeRatio = volumeRatio;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Volume;
        }

        /// <summary>
        /// Volume multiplier. Range 0 to 1
        /// </summary>
        public float VolumeRatio {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    /// <summary>
    /// Pitch offset in semitones
    /// </summary>
    public class SoundParameterPitchSemitone : SoundParameterInternals {

        /// <summary>
        /// Pitch offset in semitones
        /// </summary>
        /// <param name="pitchSemitone"> Pitch offset in semitones </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterPitchSemitone(float pitchSemitone = 0f, UpdateMode updateMode = UpdateMode.Once) {
            PitchSemitone = pitchSemitone;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Pitch;
        }

        /// <summary>
        /// Pitch offset in semitones
        /// </summary>
        public float PitchSemitone {
            get { return PitchScale.RatioToSemitones(internals.valueFloat); }
            set {
                internals.valueFloat = PitchScale.SemitonesToRatio(value);
            }
        }
    }

    /// <summary>
    /// Pitch offset ratio. Range 0 to <see cref="Mathf.Infinity"/>
    /// </summary>
    public class SoundParameterPitchRatio : SoundParameterInternals {

        /// <summary>
        /// Pitch offset ratio. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        /// <param name="pitchRatio"> Pitch offset ratio. Range 0 to <see cref="Mathf.Infinity"/> </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterPitchRatio(float pitchRatio = 1f, UpdateMode updateMode = UpdateMode.Once) {
            PitchRatio = pitchRatio;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Pitch;
        }

        /// <summary>
        /// Pitch offset ratio. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        public float PitchRatio {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, Mathf.Infinity);
            }
        }
    }

    /// <summary>
    /// Delay increase. Range 0 to <see cref="Mathf.Infinity"/>
    /// </summary>
    public class SoundParameterDelay : SoundParameterInternals {

        /// <summary>
        /// Initialization. <see cref="UpdateMode.Continuous"/> not available
        /// </summary>
        /// <param name="delay"> Delay increase. Range 0 to <see cref="Mathf.Infinity"/> </param>
        public SoundParameterDelay(float delay = 0f) {
            Delay = delay;
            // Update Mode Once
            internals.type = SoundParameterType.Delay;
        }

        /// <summary>
        /// Delay increase. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        public float Delay {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, Mathf.Infinity);
            }
        }
    }

    /// <summary>
    /// Makes the sound more 2D (less spatialized). Range 0 to 1
    /// </summary>
    public class SoundParameterIncrease2D : SoundParameterInternals {

        /// <summary>
        /// Makes the sound more 2D (less spatialized). Range 0 to 1
        /// </summary>
        /// <param name="increase2D"> Makes the sound more 2D. Range 0 to 1 </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterIncrease2D(float increase2D = 0f, UpdateMode updateMode = UpdateMode.Once) {
            Increase2D = increase2D;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Increase2D;
        }

        /// <summary>
        /// Makes the sound more 2D (less spatialized). Range 0 to 1
        /// </summary>
        public float Increase2D {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    /// <summary>
    /// Controls the intensity of the <see cref="SoundContainerBase">SoundContainer</see>
    /// Go to the <see cref="SoundContainerBase">SoundContainer</see> to change how intensity will affect the sound
    /// </summary>
    public class SoundParameterIntensity : SoundParameterInternals {

        /// <summary>
        /// Controls the intensity of the <see cref="SoundContainerBase">SoundContainer</see>
        /// Go to the <see cref="SoundContainerBase">SoundContainer</see> to change how intensity will affect the sound
        /// </summary>
        /// <param name="intensity"> Intensity multiplier </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterIntensity(float intensity = 1f, UpdateMode updateMode = UpdateMode.Once) {
            Intensity = intensity;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Intensity;
        }

        /// <summary>
        /// Controls the intensity of the <see cref="SoundContainerBase">SoundContainer</see>
        /// Go to the <see cref="SoundContainerBase">SoundContainer</see> to change how intensity will affect the sound
        /// </summary>
        public float Intensity {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = value;
            }
        }
    }

    /// <summary>
    /// Reverb zone mix offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
    /// </summary>
    public class SoundParameterReverbZoneMixDecibel : SoundParameterInternals {

        /// <summary>
        /// Reverb zone mix offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
        /// </summary>
        /// <param name="reverbZoneMixDecibel"> </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterReverbZoneMixDecibel(float reverbZoneMixDecibel = 0f, UpdateMode updateMode = UpdateMode.Once) {
            ReverbZoneMixDecibel = reverbZoneMixDecibel;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.ReverbZoneMix;
        }

        /// <summary>
        /// Reverb zone mix offset in decibel. Range <see cref="Mathf.NegativeInfinity"/> to 0
        /// </summary>
        public float ReverbZoneMixDecibel {
            get { return VolumeScale.ConvertRatioToDecibel(internals.valueFloat); }
            set {
                internals.valueFloat = VolumeScale.ConvertDecibelToRatio(Mathf.Clamp(value, Mathf.NegativeInfinity, 0f));
            }
        }
    }

    /// <summary>
    /// Reverb zone mix ratio multiplier. Range 0 to 1
    /// </summary>
    public class SoundParameterReverbZoneMixRatio : SoundParameterInternals {

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="reverbZoneMixRatio"> Reverb zone mix ratio multiplier. Range 0 to 1 </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterReverbZoneMixRatio(float reverbZoneMixRatio = 1f, UpdateMode updateMode = UpdateMode.Once) {
            ReverbZoneMixRatio = reverbZoneMixRatio;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.ReverbZoneMix;
        }

        /// <summary>
        /// Reverb zone mix ratio multiplier. Range 0 to 1
        /// </summary>
        public float ReverbZoneMixRatio {
            get { return internals.valueFloat; }
            set {
                value = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    /// <summary>
    /// Start position. Range 0 to 1
    /// </summary>
    public class SoundParameterStartPosition : SoundParameterInternals {

        /// <summary>
        /// Start position. Range 0 to 1
        /// </summary>
        /// <param name="startPosition"> Start position. Range 0 to 1 </param>
        public SoundParameterStartPosition(float startPosition = 0f) {
            StartPosition = startPosition;
            // Update Mode Once
            internals.type = SoundParameterType.StartPosition;
        }

        /// <summary>
        /// Start position. Range 0 to 1
        /// </summary>
        public float StartPosition {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    /// <summary>
    /// If the sound should be played backwards. If enabled, set the start position to the end.
    /// Reverse is only supported for AudioClips which are stored in an uncompressed format or will be decompressed at load time.
    /// </summary>
    public class SoundParameterReverse : SoundParameterInternals {

        /// <summary>
        /// If the sound should be played backwards. If enabled, set the start position to the end.
        /// Reverse is only supported for AudioClips which are stored in an uncompressed format or will be decompressed at load time.
        /// </summary>
        /// <param name="reverse"> If the sound should be played backwards </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterReverse(bool reverse = false, UpdateMode updateMode = UpdateMode.Once) {
            Reverse = reverse;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.Reverse;
        }

        /// <summary>
        /// If the sound should be played backwards. If enabled, set the start position to the end.
        /// Reverse is only supported for AudioClips which are stored in an uncompressed format or will be decompressed at load time.
        /// </summary>
        public bool Reverse {
            get { return internals.valueBool; }
            set {
                internals.valueBool = value;
            }
        }
    }

    /// <summary>
    /// Stereo pan offset. Range -1 to 1
    /// </summary>
    public class SoundParameterStereoPan : SoundParameterInternals {

        /// <summary>
        /// Stereo pan offset. Range -1 to 1
        /// </summary>
        /// <param name="stereoPanOffset"> Stereo pan offset. Range -1 to 1 </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterStereoPan(float stereoPanOffset = 0f, UpdateMode updateMode = UpdateMode.Once) {
            StereoPanOffset = stereoPanOffset;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.StereoPan;
        }

        /// <summary>
        /// Stereo pan offset. Range -1 to 1
        /// </summary>
        public float StereoPanOffset {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, -1f, 1f);
            }
        }
    }

    /// <summary>
    /// The polyphony of the <see cref="SoundEventBase">SoundEvent</see>. Range 1 to <see cref="int.MaxValue"/>
    /// </summary>
    public class SoundParameterPolyphony : SoundParameterInternals {

        /// <summary>
        /// The polyphony of the <see cref="SoundEventBase">SoundEvent</see>. Range 1 to <see cref="int.MaxValue"/>
        /// </summary>
        /// <param name="polyphony"> The polyphony of the <see cref="SoundEventBase">SoundEvent</see>. Range 1 to <see cref="int.MaxValue"/> </param>
        public SoundParameterPolyphony(int polyphony = 1) {
            Polyphony = polyphony;
            // Update Mode Once
            internals.type = SoundParameterType.Polyphony;
        }

        /// <summary>
        /// The polyphony of the <see cref="SoundEventBase">SoundEvent</see>. Range 1 to <see cref="int.MaxValue"/>
        /// </summary>
        public int Polyphony {
            get { return internals.valueInt; }
            set {
                internals.valueInt = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }
    }

    /// <summary>
    /// Distance scale multiplier. Range 0 to <see cref="Mathf.Infinity"/>
    /// </summary>
    public class SoundParameterDistanceScale : SoundParameterInternals {

        /// <summary>
        /// Distance scale multiplier. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        /// <param name="distanceScale"> Distance scale multiplier. Range 0 to <see cref="Mathf.Infinity"/>  </param>
        public SoundParameterDistanceScale(float distanceScale = 1f) {
            DistanceScale = distanceScale;
            // Update Mode Once
            internals.type = SoundParameterType.DistanceScale;
        }

        /// <summary>
        /// Distance scale multiplier. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        public float DistanceScale {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, Mathf.Infinity);
            }
        }
    }

    /// <summary>
    /// Increases the distortion.
    /// Range 0 to 1.
    /// Distortion needs to be enabled on the <see cref="SoundContainerBase">SoundContainer</see> for this to have any effect.
    /// </summary>
    public class SoundParameterDistortionIncrease : SoundParameterInternals {

        /// <summary>
        /// Increases the distortion.
        /// Range 0 to 1.
        /// Distortion needs to be enabled on the <see cref="SoundContainerBase">SoundContainer</see> for this to have any effect.
        /// </summary>
        /// <param name="distortionIncrease">
        /// Increases the distortion.
        /// Range 0 to 1.
        /// Distortion needs to be enabled on the <see cref="SoundContainerBase">SoundContainer</see> for this to have any effect.
        /// </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterDistortionIncrease(float distortionIncrease = 0f, UpdateMode updateMode = UpdateMode.Once) {
            DistortionIncrease = distortionIncrease;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.DistortionIncrease;
        }

        /// <summary>
        /// Increases the distortion.
        /// Range 0 to 1.
        /// Distortion needs to be enabled on the <see cref="SoundContainerBase">SoundContainer</see> for this to have any effect.
        /// </summary>
        public float DistortionIncrease {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    /// <summary>
    /// Fade in length. Range 0 to <see cref="Mathf.Infinity"/>
    /// </summary>
    public class SoundParameterFadeInLength : SoundParameterInternals {

        /// <summary>
        /// Fade in length. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        /// <param name="fadeInLength"> Fade in length. Range 0 to <see cref="Mathf.Infinity"/> </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterFadeInLength(float fadeInLength = 0f, UpdateMode updateMode = UpdateMode.Once) {
            FadeInLength = fadeInLength;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.FadeInLength;
        }

        /// <summary>
        /// Fade in length. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        public float FadeInLength {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, Mathf.Infinity);
            }
        }
    }

    /// <summary>
    /// Fade in shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
    /// </summary> 
    public class SoundParameterFadeInShape : SoundParameterInternals {

        /// <summary>
        /// Fade in shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
        /// </summary>
        /// <param name="fadeInShape"> Fade in shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic) </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterFadeInShape(float fadeInShape = 2f, UpdateMode updateMode = UpdateMode.Once) {
            FadeInShape = fadeInShape;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.FadeInShape;
        }

        /// <summary>
        /// Fade in shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
        /// </summary>
        public float FadeInShape {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, -16, 16f);
            }
        }
    }

    /// <summary>
    /// Fade out length. Range 0 to <see cref="Mathf.Infinity"/>
    /// </summary>
    public class SoundParameterFadeOutLength : SoundParameterInternals {

        /// <summary>
        /// Fade out length. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        /// <param name="fadeOutLength"> Fade out length. Range 0 to <see cref="Mathf.Infinity"/> </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterFadeOutLength(float fadeOutLength = 0f, UpdateMode updateMode = UpdateMode.Once) {
            FadeOutLength = fadeOutLength;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.FadeOutLength;
        }

        /// <summary>
        /// Fade out length. Range 0 to <see cref="Mathf.Infinity"/>
        /// </summary>
        public float FadeOutLength {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, 0f, Mathf.Infinity);
            }
        }
    }

    /// <summary>
    /// Fade out shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
    /// </summary>
    public class SoundParameterFadeOutShape : SoundParameterInternals {

        /// <summary>
        /// Fade out shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
        /// </summary>
        /// <param name="fadeOutShape"> Fade out shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic) </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterFadeOutShape(float fadeOutShape = -2f, UpdateMode updateMode = UpdateMode.Once) {
            FadeOutShape = fadeOutShape;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.FadeOutShape;
        }

        /// <summary>
        /// Fade out shape. Range -16 to 16 (negative is exponential, 0 is linear, positive is logarithmic)
        /// </summary>
        public float FadeOutShape {
            get { return internals.valueFloat; }
            set {
                internals.valueFloat = Mathf.Clamp(value, -16, 16f);
            }
        }
    }

    /// <summary>
    /// If the <see cref="SoundEventBase">SoundEvent</see> should follow the <see cref="Transform"/> position
    /// </summary>
    public class SoundParameterFollowPosition : SoundParameterInternals {

        /// <summary>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should follow the <see cref="Transform"/> position
        /// </summary>
        /// <param name="followPosition"> If the <see cref="SoundEventBase">SoundEvent</see> should follow the <see cref="Transform"/> position </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterFollowPosition(bool followPosition = true, UpdateMode updateMode = UpdateMode.Once) {
            FollowPosition = followPosition;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.FollowPosition;
        }

        /// <summary>
        /// If the <see cref="SoundEventBase">SoundEvent</see> should follow the <see cref="Transform"/>
        /// </summary>
        public bool FollowPosition {
            get { return internals.valueBool; }
            set {
                internals.valueBool = value;
            }
        }
    }

    /// <summary>
    /// If reverb zones should be bypassed
    /// </summary>
    public class SoundParameterBypassReverbZones : SoundParameterInternals {

        /// <summary>
        /// If reverb zones should be bypassed
        /// </summary>
        /// <param name="bypassReverbZones"> If reverb zones should be bypassed </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterBypassReverbZones(bool bypassReverbZones = false, UpdateMode updateMode = UpdateMode.Once) {
            BypassReverbZones = bypassReverbZones;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.BypassReverbZones;
        }

        /// <summary>
        /// If reverb zones should be bypassed
        /// </summary>
        public bool BypassReverbZones {
            get { return internals.valueBool; }
            set {
                internals.valueBool = value;
            }
        }
    }

    /// <summary>
    /// If voice effects (distortion/lowpass/highpass) should be bypassed
    /// </summary>
    public class SoundParameterBypassVoiceEffects : SoundParameterInternals {

        /// <summary>
        /// If voice effects (distortion/lowpass/highpass) should be bypassed
        /// </summary>
        /// <param name="bypassVoiceEffects"> If voice effects (distortion, low/highpass filters) should be bypassed </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterBypassVoiceEffects(bool bypassVoiceEffects = false, UpdateMode updateMode = UpdateMode.Once) {
            BypassVoiceEffects = bypassVoiceEffects;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.BypassVoiceEffects;
        }

        /// <summary>
        /// If voice effects (distortion/lowpass/highpass) should be bypassed
        /// </summary>
        public bool BypassVoiceEffects {
            get { return internals.valueBool; }
            set {
                internals.valueBool = value;
            }
        }
    }

    /// <summary>
    /// If listener effects should be bypassed
    /// </summary>
    public class SoundParameterBypassListenerEffects : SoundParameterInternals {

        /// <summary>
        /// If listener effects should be bypassed
        /// </summary>
        /// <param name="bypassListenerEffects"> If listener effects should be bypassed </param>
        /// <param name="updateMode"> The <see cref="UpdateMode"/> </param>
        public SoundParameterBypassListenerEffects(bool bypassListenerEffects = false, UpdateMode updateMode = UpdateMode.Once) {
            BypassListenerEffects = bypassListenerEffects;
            internals.updateMode = updateMode;
            internals.type = SoundParameterType.BypassListenerEffects;
        }

        /// <summary>
        /// If listener effects should be bypassed
        /// </summary>
        public bool BypassListenerEffects {
            get { return internals.valueBool; }
            set {
                internals.valueBool = value;
            }
        }
    }
}