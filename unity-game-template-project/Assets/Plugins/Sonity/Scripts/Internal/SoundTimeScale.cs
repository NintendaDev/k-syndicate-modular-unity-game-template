// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class SoundTimeScale {

        private static float time = 0f;

        public static float GetTimeEditor() {
            // The other time scales doesnt work in the editor
            return Time.realtimeSinceStartup;
        }

        public static float GetTimeRuntime() {
            return time;
        }

        public static void UpdateTime(SoundTimeScaleMode soundTimeScale) {
            if (soundTimeScale == SoundTimeScaleMode.UnscaledTime) {
                time = Time.unscaledTime;
            } else if (soundTimeScale == SoundTimeScaleMode.Time) {
                time = Time.time;
            } else if (soundTimeScale == SoundTimeScaleMode.FixedUnscaledTime) {
                time = Time.fixedUnscaledTime;
            } else if (soundTimeScale == SoundTimeScaleMode.RealtimeSinceStartup) {
                time = Time.realtimeSinceStartup;
            }
        }
    }
}