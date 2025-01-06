// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    public static class SoundPresetFind {

        public static SoundPresetBase[] soundPresets;

        public static void FindAllSoundPresets() {
            string[] soundPresetGuids = AssetDatabase.FindAssets("t:SoundPreset", null);
            soundPresets = new SoundPresetBase[soundPresetGuids.Length];

            for (int i = 0; i < soundPresetGuids.Length; i++) {
                string guid = soundPresetGuids[i];
                soundPresets[i] = (SoundPresetBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(SoundPresetBase));
            }
        }
    }
}
#endif