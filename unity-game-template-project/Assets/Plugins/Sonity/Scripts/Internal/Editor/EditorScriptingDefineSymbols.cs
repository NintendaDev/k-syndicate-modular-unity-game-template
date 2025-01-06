// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Sonity.Internal {

    public static class EditorScriptingDefineSymbols {

        private static string editorToolSelectionHistoryDefineSymbol = "SONITY_ENABLE_EDITOR_TOOL_SELECTION_HISTORY";
        private static string editorToolReferenceFinderDefineSymbol = "SONITY_ENABLE_EDITOR_TOOL_REFERENCE_FINDER";
        private static string editorToolSelectSameTypeDefineSymbol = "SONITY_ENABLE_EDITOR_TOOL_SELECT_SAME_TYPE";
        private static string audioListenerVolumeIncreaseDefineSymbol = "SONITY_ENABLE_VOLUME_INCREASE";
        private static string adressableAudioMixerDefineSymbol = "SONITY_ENABLE_ADRESSABLE_AUDIOMIXER";

        // Enable Editor Tool Selection History
        public static bool EditorToolSelectionHistoryExists() {
            return DefineSymbolExists(editorToolSelectionHistoryDefineSymbol);
        }

        public static void EditorToolSelectionHistoryShouldExist(bool shouldExist) {
            DefineSymbolAddRemove(editorToolSelectionHistoryDefineSymbol, shouldExist);
        }

        // Enable Editor Tool Reference Finder
        public static bool EditorToolReferenceFinderExists() {
            return DefineSymbolExists(editorToolReferenceFinderDefineSymbol);
        }

        public static void EditorToolReferenceFinderShouldExist(bool shouldExist) {
            DefineSymbolAddRemove(editorToolReferenceFinderDefineSymbol, shouldExist);
        }


        // Enable Editor Tool Select Same Type
        public static bool EditorToolSelectSameTypeExists() {
            return DefineSymbolExists(editorToolSelectSameTypeDefineSymbol);
        }

        public static void EditorToolSelectSameTypeShouldExist(bool shouldExist) {
            DefineSymbolAddRemove(editorToolSelectSameTypeDefineSymbol, shouldExist);
        }

        // Enable Volume Increase
        public static bool AudioListenerVolumeIncreaseExists() {
            return DefineSymbolExists(audioListenerVolumeIncreaseDefineSymbol);
        }

        public static void AudioListenerVolumeIncreaseShouldExist(bool shouldExist) {
            DefineSymbolAddRemove(audioListenerVolumeIncreaseDefineSymbol, shouldExist);
        }

        // Enable Adressable AudioMixer
        public static bool AdressableAudioMixerExists() {
            return DefineSymbolExists(adressableAudioMixerDefineSymbol);
        }

        public static void AdressableAudioMixerShouldExist(bool shouldExist) {
            DefineSymbolAddRemove(adressableAudioMixerDefineSymbol, shouldExist);
        }

        // Checks if the Define Symbol Exists
        private static bool DefineSymbolExists(string defineSymbol) {
#if UNITY_2021_2_OR_NEWER
            string definesString = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
#else
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
            List<string> allDefines = definesString.Split(';').ToList();
            return allDefines.Contains(defineSymbol);
        }


        // Adds or removes the given define symbols to PlayerSettings define symbols
        private static void DefineSymbolAddRemove(string defineSymbol, bool shouldExist) {
#if UNITY_2021_2_OR_NEWER
            string definesString = PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup));
#else
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
            List<string> allDefines = definesString.Split(';').ToList();
            if (shouldExist) {
                // Adds a new define if it doesnt already exist
                if (!allDefines.Contains(defineSymbol)) {
                    allDefines.Add(defineSymbol);
                    Debug.Log($"Sonity.{nameof(NameOf.SoundManager)}: Added scripting define symbol \"" + defineSymbol + "\"");
                }
            } else {
                // Remove the define if it exists
                for (int i = allDefines.Count - 1; i >= 0; i--) {
                    if (allDefines[i] == defineSymbol) {
                        allDefines.RemoveAt(i);
                        Debug.Log($"Sonity.{nameof(NameOf.SoundManager)}: Removed scripting define symbol \"" + defineSymbol + "\"");
                    }
                }
            }

            // Merges and adds the defines
#if UNITY_2021_2_OR_NEWER
            PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup), string.Join(";", allDefines.ToArray()));
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
#endif
        }
    }
}
#endif