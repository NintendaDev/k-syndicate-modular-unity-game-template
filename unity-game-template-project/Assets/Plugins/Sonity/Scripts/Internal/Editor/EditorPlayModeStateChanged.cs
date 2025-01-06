// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Sonity.Internal {

    [InitializeOnLoad]
    public static class EditorPlayModeStateChanged {

        static EditorPlayModeStateChanged() {
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        private static void PlayModeStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.EnteredPlayMode) {
                if (EditorUtility.audioMasterMute && ShouldDebug.Warnings()) {
                    Debug.LogWarning($"Sonity sounds will not be heard because \"Mute Audio\" is enabled in the Unity editor");
                }
            }
        }
    }
}
#endif