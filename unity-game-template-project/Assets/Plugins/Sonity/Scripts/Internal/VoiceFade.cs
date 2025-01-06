// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public class VoiceFade {
        public VoiceFadeState state = VoiceFadeState.FadePool;
        private float inVolume = 0f;
        private float outVolume = 0f;
        private float timeStart = 0f;

        public void Reset() {
            state = VoiceFadeState.FadePool;
            inVolume = 0f;
            outVolume = 0f;
            timeStart = 0f;
        }

        private float GetTime() {
#if UNITY_EDITOR
            if (Application.isPlaying) {
#endif
                return SoundTimeScale.GetTimeRuntime();
#if UNITY_EDITOR
            } else {
                return SoundTimeScale.GetTimeEditor();
            }
#endif
        }

        public void SetToFadeIn(SoundEventModifier soundEventModifier) {
            if (soundEventModifier.fadeInLength > 0f) {
                state = VoiceFadeState.FadeIn;
                timeStart = GetTime();
            } else {
                SetToFadeHold();
            }
        }

        public void SetToFadeHold() {
            state = VoiceFadeState.FadeHold;
            inVolume = 1f;
            outVolume = 0f;
            timeStart = 0f;
        }

        public void SetToFadeOut(SoundEventModifier soundEventModifier) {
            if (state == VoiceFadeState.FadeIn || state == VoiceFadeState.FadeHold) {
                if (soundEventModifier.fadeOutLength > 0f) {
                    state = VoiceFadeState.FadeOut;
                    timeStart = GetTime();
                } else {
                    SetToFadePool();
                }
            }
        }

        public void SetToFadePool() {
            state = VoiceFadeState.FadePool;
            inVolume = 0f;
            outVolume = 1f;
            timeStart = 0f;
        }

        public float GetVolume(SoundEventModifier soundEventModifier) {
            // Hold
            if (state == VoiceFadeState.FadeHold) {
                return 1f;
            }
            // In
            else if (state == VoiceFadeState.FadeIn) {
                inVolume = LogLinExp.Get(Mathf.Clamp((GetTime() - timeStart) / soundEventModifier.fadeInLength, 0f, 1f), -soundEventModifier.fadeInShape) * (1f - outVolume) + outVolume;
                if (inVolume >= 1f) {
                    SetToFadeHold();
                }
                return inVolume;
            }
            // Out
            else if (state == VoiceFadeState.FadeOut) {
                outVolume = LogLinExp.Get(Mathf.Clamp((soundEventModifier.fadeOutLength - (GetTime() - timeStart)) / soundEventModifier.fadeOutLength, 0f, 1f), -soundEventModifier.fadeOutShape) * inVolume;
                if (outVolume <= 0f) {
                    SetToFadePool();
                }
                return outVolume;
            }
            // Pool
            return 0f;
        }
    }
}
