// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    // So it works for preview
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    public class VoiceEffect : MonoBehaviour {

        private bool isEnabled = false;

        [HideInInspector]
        public bool checkVoiceEffectLimit = false;

        public void SetEnabled(bool enabled) {
            if (enabled && !isEnabled) {
                checkVoiceEffectLimit = true;
            } else if (!enabled && isEnabled) {
                checkVoiceEffectLimit = false;
            }
            isEnabled = enabled;
        }

        public bool GetEnabled() {
            return isEnabled;
        }

        public bool GetAnyEnabledInternal() {
            if (distortionEnabledInternal || lowpassEnabledInternal || highpassEnabledInternal) {
                return true;
            } else {
                return false;
            }
        }

        // Waveshaper type distortion
        private bool distortionEnabled = false;
        private bool distortionEnabledInternal = false;
        private float distortionAmount;
        // -1 (expansion, not used), 0 (normal), 1 (distortion)
        private float distortionK;
        private float distortionVolume;

        // Enables or disables the distortion
        public void DistortionSetEnabled(bool enabled) {
            distortionEnabled = enabled;
            if (!enabled) {
                distortionEnabledInternal = false;
            }
        }

        public bool DistortionGetEnabled() {
            return distortionEnabled;
        }

        /// <summary>
        /// Amount Range from 0 (normal) to 1 (distortion)
        /// </summary>
        public void DistortionSetValue(float amount) {
            if (amount > 0f) {
                distortionEnabledInternal = true;
                amount = Mathf.Clamp(amount, 0f, 1f);
                if (distortionAmount != amount) {
                    distortionAmount = amount;
                    // Capping distortion amount (value 1 is too much)
                    amount = LogLinExp.Get(distortionAmount, -2f) * 0.999f;
                    distortionK = 2 * amount / (1f - amount);
                    // Compensating the distortion by lowering the volume a bit
                    distortionVolume = 1f - amount * 0.8f;
                }
            } else {
                distortionEnabledInternal = false;
            }
        }

        private bool lowpassEnabled = false;
        private bool lowpassEnabledInternal = false;
        private float lowpassFrequency;
        private float lowpassSlope;
        private ShelvingFilter shelvingLowpass = new ShelvingFilter();

        public void LowpassSetEnabled(bool enabled) {
            lowpassEnabled = enabled;
            if (!enabled) {
                lowpassEnabledInternal = false;
            }
        }

        public bool LowpassGetEnabled() {
            return lowpassEnabled;
        }

        /// <summary>
        /// Frequency Range from 0 (20Hz) to 1 (20000Hz)
        /// Slope Range from 0 (0dB/oct) to 1 (6dB/oct)
        /// </summary>
        public void LowpassSetValue(float frequency, float slope) {
            frequency = Mathf.Clamp(frequency, 0f, 1f);
            slope = Mathf.Clamp(slope, 0f, 1f);
            if (frequency < 1f && slope > 0f) {
                lowpassEnabledInternal = true;
                if (lowpassFrequency != frequency || lowpassSlope != slope) {
                    lowpassFrequency = frequency;
                    lowpassSlope = slope;
                    // Scale frequency from 0 to 1 to 20Hz to 20000Hz
                    shelvingLowpass.frequency = frequency * 19980f + 20f;
                    // Scale gain from 0 to 1 to -0dB to -6dB
                    shelvingLowpass.gain = slope * 6f;
                    ShelvingFilterUpdate(ref shelvingLowpass);
                }
            } else {
                lowpassEnabledInternal = false;
            }
        }

        private bool highpassEnabled = false;
        private bool highpassEnabledInternal = false;
        private float highpassFrequency;
        private float highpassSlope;
        private ShelvingFilter shelvingHighpass = new ShelvingFilter();

        public void HighpassSetEnabled(bool enabled) {
            highpassEnabled = enabled;
            if (!enabled) {
                highpassEnabledInternal = false;
            }
        }

        public bool HighpassGetEnabled() {
            return highpassEnabled;
        }

        /// <summary>
        /// Frequency Range from 0 (20Hz) to 1 (20000Hz)
        /// Slope Range from 0 (0dB/oct) to 1 (6dB/oct)
        /// </summary>
        public void HighpassSetValue(float frequency, float slope) {
            frequency = Mathf.Clamp(frequency, 0f, 1f);
            slope = Mathf.Clamp(slope, 0f, 1f);
            if (frequency > 0f && slope > 0f) {
                highpassEnabledInternal = true;
                if (highpassFrequency != frequency || highpassSlope != slope) {
                    highpassFrequency = frequency;
                    highpassSlope = slope;
                    // Scale frequency from 0 to 1 to 20Hz to 20000Hz
                    shelvingHighpass.frequency = frequency * 19980f + 20f;
                    // Scale gain from 0 to 1 to -0dB to -6dB
                    shelvingHighpass.gain = slope * 6f;
                    ShelvingFilterUpdate(ref shelvingHighpass);
                }
            } else {
                highpassEnabledInternal = false;
            }
        }

        private class ShelvingFilter {
            public float frequency;
            public float gain;
            public float[] lpOut = { 0.0f, 0.0f };
            public float g;
            public float n;
            public float a0;
            public float b1;
            public float omega;
            public float sampleRate3;
            public float gainFactor = 5;
            public float amp = 6 / Mathf.Log(2);
        }

        private void ShelvingFilterUpdate(ref ShelvingFilter shelvingFilter) {
            shelvingFilter.sampleRate3 = AudioSettings.outputSampleRate * 3;
            shelvingFilter.g = Mathf.Exp(-shelvingFilter.gainFactor * shelvingFilter.gain / shelvingFilter.amp) - 1;
            shelvingFilter.omega = 2f * Mathf.PI * shelvingFilter.frequency;
            shelvingFilter.n = 1f / (shelvingFilter.sampleRate3 + shelvingFilter.omega);
            shelvingFilter.a0 = 2f * shelvingFilter.omega * shelvingFilter.n;
            shelvingFilter.b1 = (shelvingFilter.sampleRate3 - shelvingFilter.omega) * shelvingFilter.n;
        }

        private void OnAudioFilterRead(float[] data, int channels) {
            if (!isEnabled) {
                return;
            }
            if (!distortionEnabledInternal && !lowpassEnabledInternal && !highpassEnabledInternal) {
                return;
            }
            for (int i = 0; i < data.Length; i += channels) {
                for (int ii = 0; ii < channels; ii++) {

                    // Distortion
                    if (distortionEnabledInternal) {
                        data[i + ii] = (1f + distortionK) * data[i + ii] / (1f + distortionK * Mathf.Abs(data[i + ii])) * distortionVolume;
                    }

                    // Lowpass shelving filter
                    if (lowpassEnabledInternal) {
                        shelvingLowpass.lpOut[ii] = shelvingLowpass.a0 * data[i + ii] + shelvingLowpass.b1 * shelvingLowpass.lpOut[ii];
                        data[i + ii] = data[i + ii] + shelvingLowpass.g * (data[i + ii] - shelvingLowpass.lpOut[ii]);
                    }

                    // Highpass shelving filter
                    if (highpassEnabledInternal) {
                        shelvingHighpass.lpOut[ii] = shelvingHighpass.a0 * data[i + ii] + shelvingHighpass.b1 * shelvingHighpass.lpOut[ii];
                        data[i + ii] = data[i + ii] + shelvingHighpass.g * shelvingHighpass.lpOut[ii];
                    }
                }
            }
        }
    }
}