// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Sonity.Internal {

    public static class EditorAssetCreateMenu {

        // Shortcut key modifiers
        // % -> Ctrl on Windows, Linux, CMD on MacOS
        // ^ -> Ctrl on Windows, Linux, MacOS
        // # -> Shift
        // & -> Alt

        // If priority is more than 10 away from the previous priority a divider will be drawn
        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SC+SE from AudioClip Group Multiple SE ^#Q", false, 0)]
        static void SoundContainerAndSoundEventFromAudioClipCombineMultiple(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<AudioClip, SoundContainerBase>(AssetGrouping.Combine);
            if (isCancelled) {
                ClearProgressBar(true);
            }
            CreateAssets<SoundContainerBase, SoundEventBase>(AssetGrouping.Multiple);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SC+SE from AudioClip Group Single SE ^#W", false, 1)]
        static void SoundContainerAndSoundEventFromAudioClipCombineSingle(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<AudioClip, SoundContainerBase>(AssetGrouping.Combine);
            if (isCancelled) {
                ClearProgressBar(true);
            }
            CreateAssets<SoundContainerBase, SoundEventBase>(AssetGrouping.Single);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SoundPolyGroup for SE Multiple", false, 20)]
        static void SoundPolyGroupForSoundEventMultiple(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<SoundEventBase, SoundPolyGroupBase>(AssetGrouping.Multiple);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SoundPolyGroup for SE Single", false, 21)]
        static void SoundPolyGroupForSoundEventSingle(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<SoundEventBase, SoundPolyGroupBase>(AssetGrouping.Single);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SC from AudioClips Multiple", false, 40)]
        static void SoundContainerFromAudioClipMultiple(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<AudioClip, SoundContainerBase>(AssetGrouping.Multiple);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SC from AudioClips Single", false, 41)]
        static void SoundContainerFromAudioClipSingle(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<AudioClip, SoundContainerBase>(AssetGrouping.Single);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SE from SC Multiple", false, 60)]
        static void SoundEventFromSoundContainerMultiple(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<SoundContainerBase, SoundEventBase>(AssetGrouping.Multiple);
        }

        [MenuItem("Assets/Create/Sonity 🔊/Create Assets From Selection/SE from SC Single", false, 61)]
        static void SoundEventFromSoundContainerSingle(MenuCommand menuCommand) {
            isCancelled = false;
            soundPresetsFound = false;
            CreateAssets<SoundContainerBase, SoundEventBase>(AssetGrouping.Single);
        }

        private enum AssetGrouping {
            Single,
            Multiple,
            Combine,
        }

        private static void AddRightTypeGuidList<T>(ref List<string> rightTypeGuidList, string[] guids) {
            for (int i = 0; i < guids.Length; i++) {
                if (AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(T)) != null) {
                    if (!rightTypeGuidList.Contains(guids[i])) {
                        rightTypeGuidList.Add(guids[i]);
                    }
                }
            }
        }

        private static List<string> SortNameGuidList(List<string> unsortedGuidList) {
            List<string> pathList = new List<string>();
            for (int i = 0; i < unsortedGuidList.Count; i++) {
                // Convert to path so it can be sorted
                pathList.Add(AssetDatabase.GUIDToAssetPath(unsortedGuidList[i]));
            }
            pathList.Sort();
            List<string> sortedGuidList = new List<string>();
            for (int i = 0; i < pathList.Count; i++) {
                // Convert path back to guid
                sortedGuidList.Add(AssetDatabase.AssetPathToGUID(pathList[i]));
            }
            return sortedGuidList;
        }

        private static void ClearProgressBar(bool isCancelledByClick) {
            EditorUtility.ClearProgressBar();
            if (isCancelledByClick && !isCancelled) {
                isCancelled = true;
                GUIUtility.ExitGUI();
            }
        }

        private static bool soundPresetsFound = false;

        private static void CreateAssets<typeFrom, typeTo>(AssetGrouping assetGrouping) {

            AssetDatabase.SaveAssets();

            newAssetsList.Clear();
            List<string> rightTypeGuidList = new List<string>();

            // Add Selected Assets
            AddRightTypeGuidList<typeFrom>(ref rightTypeGuidList, Selection.assetGUIDs);

            // Find assets from selected folders
            List<string> assetsInFoldersGuids = new List<string>();
            for (int i = 0; i < Selection.assetGUIDs.Length; i++) {

                // Progress Bar
                float progress = (i + 1f) / Mathf.Clamp(Selection.assetGUIDs.Length, 1, int.MaxValue);
                if (EditorUtility.DisplayCancelableProgressBar($"Creating Assets - Finding Assets", EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i])), progress)) {
                    ClearProgressBar(true);
                    return;
                }

                string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
                string[] foundGuids = AssetDatabase.FindAssets($"t:" + typeof(typeFrom).Name, new[] { path });
                for (int ii = 0; ii < foundGuids.Length; ii++) {
                    if (!assetsInFoldersGuids.Contains(foundGuids[ii])) {
                        assetsInFoldersGuids.Add(foundGuids[ii]);
                    }
                }
            }

            // Add assets from selected folders
            AddRightTypeGuidList<typeFrom>(ref rightTypeGuidList, assetsInFoldersGuids.ToArray());

            // If no assets of the right type are selected
            if (rightTypeGuidList.Count == 0) {
                ClearProgressBar(false);
                return;
            }

            // Sort them
            rightTypeGuidList = SortNameGuidList(rightTypeGuidList);

            // Single asset
            if (assetGrouping == AssetGrouping.Single) {
                MakeFiles<typeFrom, typeTo>(rightTypeGuidList, false, true, true);
                if (isCancelled) {
                    ClearProgressBar(false);
                    return;
                }
            }
            // Multiple assets
            else if (assetGrouping == AssetGrouping.Multiple) {
                MakeFiles<typeFrom, typeTo>(rightTypeGuidList, true, true, true);
                if (isCancelled) {
                    ClearProgressBar(false);
                    return;
                }
            }
            // Combine same name
            else if (assetGrouping == AssetGrouping.Combine) {
                List<string> sameNameGuidList = new List<string>();

                int progressCountStart = rightTypeGuidList.Count;

                while (rightTypeGuidList.Count > 0) {

                    sameNameGuidList.Clear();
                    sameNameGuidList.Add(rightTypeGuidList[0]);
                    rightTypeGuidList.RemoveAt(0);
                    for (int i = rightTypeGuidList.Count - 1; i >= 0; i--) {

                        // Progress Bar
                        float progress = (rightTypeGuidList.Count + 1f) / Mathf.Clamp(progressCountStart, 1, int.MaxValue);
                        if (EditorUtility.DisplayCancelableProgressBar($"Creating Assets - Checking Name", EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i])), progress)) {
                            ClearProgressBar(true);
                            return;
                        }

                        if (AssetsAreSameName(sameNameGuidList[0], rightTypeGuidList[i])) {
                            sameNameGuidList.Add(rightTypeGuidList[i]);
                            rightTypeGuidList.RemoveAt(i);
                        }
                    }
                    // Sort them again
                    sameNameGuidList = SortNameGuidList(sameNameGuidList);
                    MakeFiles<typeFrom, typeTo>(sameNameGuidList, false, true, false);
                    if (isCancelled) {
                        ClearProgressBar(false);
                        return;
                    }
                }
                // Select new assets
                Selection.objects = newAssetsList.ToArray();
            }

            // Sound Preset
            if (!soundPresetsFound) {
                soundPresetsFound = true;
                SoundPresetFind.FindAllSoundPresets();
            }
            if (SoundPresetFind.soundPresets != null) {
                for (int i = 0; i < SoundPresetFind.soundPresets.Length; i++) {
                    SoundPresetBase soundPreset = SoundPresetFind.soundPresets[i];
                    if (soundPreset != null && !soundPreset.internals.disableAll) {
                        for (int ii = 0; ii < soundPreset.internals.soundPresetGroup.Length; ii++) {
                            SoundPresetGroup soundPresetGroup = soundPreset.internals.soundPresetGroup[ii];
                            if (soundPresetGroup != null) {
                                // SoundContainer
                                if (soundPresetGroup.ShouldUseMatch(true) && typeof(typeTo) == typeof(SoundContainerBase)) {
                                    for (int iii = 0; iii < newAssetsList.Count; iii++) {
                                        UnityEngine.Object newAsset = newAssetsList[iii];
                                        if (newAsset != null) {
                                            SoundContainerBase soundContainer = (SoundContainerBase)newAsset;
                                            if (soundContainer != null) {
                                                if (soundPresetGroup.GetNameMatches(soundContainer.name, true)) {
                                                    SoundContainerCopy.CopyTo(soundContainer, soundPresetGroup.soundContainerPreset);
                                                    if (SoundPresetFind.soundPresets[i].internals.automaticLoop && soundContainer.name.ToLowerInvariant().Contains("loop")) {
                                                        soundContainer.internals.data.loopEnabled = true;
                                                        soundContainer.internals.data.followPosition = true;
                                                        soundContainer.internals.data.randomStartPosition = true;
                                                        soundContainer.internals.data.stopIfTransformIsNull = true;
                                                    }
                                                    Debug.Log($"Sonity: Preset \"" + soundPresetGroup.soundContainerPreset.name + $"\" is applied to \"" + soundContainer.name + "\"", soundContainer);
                                                }
                                            }
                                        }
                                    }
                                }
                                // SoundEvent
                                if (soundPresetGroup.ShouldUseMatch(false) && typeof(typeTo) == typeof(SoundEventBase)) {
                                    for (int iii = 0; iii < newAssetsList.Count; iii++) {
                                        UnityEngine.Object newAsset = newAssetsList[iii];
                                        if (newAsset != null) {
                                            SoundEventBase soundEvent = (SoundEventBase)newAsset;
                                            if (soundEvent != null) {
                                                if (soundPresetGroup.GetNameMatches(soundEvent.name, false)) {
                                                    SoundEventCopy.CopyTo(soundEvent, soundPresetGroup.soundEventPreset);
                                                    Debug.Log($"Sonity: Preset \"" + soundPresetGroup.soundEventPreset.name + $"\" is applied to \"" + soundEvent.name + "\"", soundEvent);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();

            ClearProgressBar(false);
        }

        private static string RemoveTrailingJunk(string input, bool removeNumbers = true) {
            char[] charsToRemove;
            if (removeNumbers) {
                charsToRemove = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };
            } else {
                charsToRemove = new char[] { ' ', '_', '-' };
            }
            return input.TrimEnd(charsToRemove);
        }

        private static bool AssetNameContainsLoop(string guid) {
            guid = EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guid));
            if (guid.ToLowerInvariant().Contains("loop")) {
                return true;
            } else {
                return false;
            }
        }

        private static bool AssetsAreSameName(string guidA, string guidB) {
            guidA = EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guidA));
            guidB = EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guidB));
            guidA = RemoveTrailingJunk(guidA);
            guidB = RemoveTrailingJunk(guidB);
            if (guidA == guidB) {
                return true;
            } else {
                return false;
            }
        }

        private static string GetNameToRemoveWithRightSeparatorChar(string fileName, string nameToRemove) {
            if (fileName.EndsWith("_" + nameToRemove)) {
                return "_" + nameToRemove;
            } else if (fileName.EndsWith(" " + nameToRemove)) {
                return " " + nameToRemove;
            } else if (fileName.EndsWith("-" + nameToRemove)) {
                return "-" + nameToRemove;
            }
            return "_" + nameToRemove;
        }

        private static List<UnityEngine.Object> newAssetsList = new List<UnityEngine.Object>();
        private static bool isCancelled = false;

        private static void MakeFiles<typeFrom, typeTo>(List<string> rightTypeGuidList, bool makeMultipleAssets, bool removeTrailingNumbers, bool selectNewAssets) {
            
            for (int i = 0; i < rightTypeGuidList.Count; i++) {

                // Progress Bar
                float progress = (i + 1f) / Mathf.Clamp(rightTypeGuidList.Count, 1, int.MaxValue);
                if (EditorUtility.DisplayCancelableProgressBar($"Creating Assets - Making Files", EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i])), progress)) {
                    ClearProgressBar(true);
                    if (selectNewAssets) {
                        // Multiple assets
                        Selection.objects = newAssetsList.ToArray();
                    }
                    return;
                }

                // Get currently selected path
                string selectedPath = AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]);

                // Remove Filename from Path
                selectedPath = selectedPath.Replace(EditorPath.GetFileName(selectedPath), "");

                // Get selected filename
                string selectedFilename = EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]));

                // Remove trailing numbers, space, underscore, dash
                if (removeTrailingNumbers) {
                    selectedFilename = RemoveTrailingJunk(selectedFilename);
                }

                // Get add separator character
                char addSeparatorChar = '_';
                for (int ii = 0; ii < selectedFilename.Length; ii++) {
                    if (selectedFilename[ii] == ' ') {
                        addSeparatorChar = ' ';
                    } else if (selectedFilename[ii] == '_') {
                        addSeparatorChar = '_';
                    } else if (selectedFilename[ii] == '-') {
                        addSeparatorChar = '-';
                    }
                }

                string nameToRemove = "";
                string nameToAdd = "";

                // From AudioClip to SoundContainer
                if (typeof(typeFrom) == typeof(AudioClip) && typeof(typeTo) == typeof(SoundContainerBase)) {
                    nameToRemove = "";
                    nameToAdd = addSeparatorChar + "SC";
                }
                // From SoundContainer to SoundEvent
                else if (typeof(typeFrom) == typeof(SoundContainerBase) && typeof(typeTo) == typeof(SoundEventBase)) {
                    nameToRemove = GetNameToRemoveWithRightSeparatorChar(selectedFilename, "SC");
                    nameToAdd = addSeparatorChar + "SE";
                }
                // From SoundEvent to SoundContainer
                else if (typeof(typeFrom) == typeof(SoundEventBase) && typeof(typeTo) == typeof(SoundContainerBase)) {
                    nameToRemove = GetNameToRemoveWithRightSeparatorChar(selectedFilename, "SE");
                    nameToAdd = addSeparatorChar + "SC";
                }
                // From SoundEvent to SoundPolyGroup
                else if (typeof(typeFrom) == typeof(SoundEventBase) && typeof(typeTo) == typeof(SoundPolyGroupBase)) {
                    nameToRemove = GetNameToRemoveWithRightSeparatorChar(selectedFilename, "SE");
                    nameToAdd = addSeparatorChar + "SPG";
                }

                // Replace if ends with
                if (nameToRemove != "" && selectedFilename.EndsWith(nameToRemove)) {
                    selectedFilename = selectedFilename.Remove(selectedFilename.Length - nameToRemove.Length);
                }

                // Add name to the end
                selectedFilename = selectedFilename + nameToAdd;

                // From AudioClip to SoundContainer
                if (typeof(typeFrom) == typeof(AudioClip) && typeof(typeTo) == typeof(SoundContainerBase)) {
                    // Create new instance
                    SoundContainerBase newAsset = ScriptableObject.CreateInstance<SoundContainer>();
                    // Make it looping if the name contains loop
                    if (AssetNameContainsLoop(rightTypeGuidList[i])) {
                        newAsset.internals.data.loopEnabled = true;
                        newAsset.internals.data.followPosition = true;
                        newAsset.internals.data.randomStartPosition = true;
                        newAsset.internals.data.stopIfTransformIsNull = true;
                    }
                    newAssetsList.Add(newAsset);
                    // Make multiple assets
                    if (makeMultipleAssets) {
                        Array.Resize(ref newAsset.internals.audioClips, 1);
                        newAsset.internals.audioClips[0] = (AudioClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]), typeof(AudioClip));
                    }
                    // Make single asset
                    else {
                        // Adds the selected assets
                        Array.Resize(ref newAsset.internals.audioClips, rightTypeGuidList.Count);
                        for (int ii = 0; ii < rightTypeGuidList.Count; ii++) {
                            newAsset.internals.audioClips[ii] = (AudioClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[ii]), typeof(AudioClip));
                        }
                    }
                    // Creates the asset
                    string newAssetPath = selectedPath + "/" + selectedFilename + ".asset";
                    AssetDatabase.CreateAsset(newAsset, AssetDatabase.GenerateUniqueAssetPath(newAssetPath));
                }
                // From SoundContainer to SoundEvent
                else if (typeof(typeFrom) == typeof(SoundContainerBase) && typeof(typeTo) == typeof(SoundEventBase)) {
                    // Create new instance
                    SoundEventBase newAsset = ScriptableObject.CreateInstance<SoundEvent>();
                    newAssetsList.Add(newAsset);
                    // Make multiple assets
                    if (makeMultipleAssets) {
                        Array.Resize(ref newAsset.internals.soundContainers, 1);
                        newAsset.internals.soundContainers[0] = (SoundContainerBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]), typeof(SoundContainerBase));
                        newAsset.internals.data.InitializeTimelineSoundContainerSetting(newAsset.internals.soundContainers.Length);
                    }
                    // Make single asset
                    else {
                        // Need to save the created SoundContainers before editing them
                        // Otherwise distance crossfade settings might not be applied
                        AssetDatabase.SaveAssets();

                        // Adds the selected assets
                        Array.Resize(ref newAsset.internals.soundContainers, rightTypeGuidList.Count);
                        for (int ii = 0; ii < rightTypeGuidList.Count; ii++) {
                            newAsset.internals.soundContainers[ii] = (SoundContainerBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[ii]), typeof(SoundContainerBase));
                        }
                        newAsset.internals.data.InitializeTimelineSoundContainerSetting(newAsset.internals.soundContainers.Length);
                        
                        // Checking if the names have any common denominator
                        if (rightTypeGuidList.Count > 1) {

                            int charsEndToRemove = 0;
                            while (charsEndToRemove <= selectedFilename.Length) {

                                // If not finding anything tries to remove chars from the end of the filename to find a match
                                string tempFilename = selectedFilename.Substring(0, selectedFilename.Length - charsEndToRemove);
                                int foundMatches = 0;

                                for (int ii = 0; ii < rightTypeGuidList.Count; ii++) {
                                    if (EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[ii])).ToLowerInvariant().Contains(tempFilename.ToLowerInvariant())) {
                                        foundMatches++;
                                    }
                                }

                                // If all are matching then use that name
                                if (foundMatches == rightTypeGuidList.Count) {
                                    selectedFilename = RemoveTrailingJunk(tempFilename, false) + addSeparatorChar + "SE";
                                    break;
                                }

                                if (charsEndToRemove > selectedFilename.Length) {
                                    break;
                                } else {
                                    // If no assets are found, try to remove one more character at the end of the filename
                                    charsEndToRemove++;
                                }
                            }
                        }
                    }
                    // Creates the asset
                    string newAssetPath = selectedPath + "/" + selectedFilename + ".asset";
                    AssetDatabase.CreateAsset(newAsset, AssetDatabase.GenerateUniqueAssetPath(newAssetPath));
                }
                // From SoundEvent to SoundContainer
                else if (typeof(typeFrom) == typeof(SoundEventBase) && typeof(typeTo) == typeof(SoundContainerBase)) {
                    // Create new instance
                    SoundContainerBase newAsset = ScriptableObject.CreateInstance<SoundContainer>();
                    // Make it looping if the name contains loop
                    if (AssetNameContainsLoop(rightTypeGuidList[i])) {
                        newAsset.internals.data.loopEnabled = true;
                        newAsset.internals.data.followPosition = true;
                        newAsset.internals.data.randomStartPosition = true;
                        newAsset.internals.data.stopIfTransformIsNull = true;
                    }
                    newAssetsList.Add(newAsset);
                    // Creates the asset
                    string newAssetPath = selectedPath + "/" + selectedFilename + ".asset";
                    AssetDatabase.CreateAsset(newAsset, AssetDatabase.GenerateUniqueAssetPath(newAssetPath));
                    // Sets multiple assets
                    if (makeMultipleAssets) {
                        SoundEventBase fromAsset = (SoundEventBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]), typeof(SoundEventBase));
                        Array.Resize(ref fromAsset.internals.soundContainers, 1);
                        fromAsset.internals.soundContainers[0] = newAsset; 
                    }
                    // Sets single asset
                    else {
                        for (int ii = 0; ii < rightTypeGuidList.Count; ii++) {
                            SoundEventBase fromAsset = (SoundEventBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[ii]), typeof(SoundEventBase));
                            Array.Resize(ref fromAsset.internals.soundContainers, 1);
                            fromAsset.internals.soundContainers[0] = newAsset;
                        }
                    }
                }
                // From SoundEvent to SoundPolyGroup
                else if (typeof(typeFrom) == typeof(SoundEventBase) && typeof(typeTo) == typeof(SoundPolyGroupBase)) {
                    // Create new instance
                    SoundPolyGroupBase newAsset = ScriptableObject.CreateInstance<SoundPolyGroup>();
                    newAssetsList.Add(newAsset);
                    // Creates the asset
                    string newAssetPath = selectedPath + "/" + selectedFilename + ".asset";
                    AssetDatabase.CreateAsset(newAsset, AssetDatabase.GenerateUniqueAssetPath(newAssetPath));
                    // Sets multiple assets
                    if (makeMultipleAssets) {
                        SoundEventBase fromAsset = (SoundEventBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[i]), typeof(SoundEventBase));
                        fromAsset.internals.data.soundPolyGroup = newAsset;
                    }
                    // Sets single asset
                    else {
                        for (int ii = 0; ii < rightTypeGuidList.Count; ii++) {
                            SoundEventBase fromAsset = (SoundEventBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(rightTypeGuidList[ii]), typeof(SoundEventBase));
                            fromAsset.internals.data.soundPolyGroup = newAsset;
                        }
                    }
                }

                if (!makeMultipleAssets) {
                    if (selectNewAssets) {
                        Selection.objects = newAssetsList.ToArray();
                    }
                    return;
                }
            }

            if (selectNewAssets) {
                // Multiple assets
                Selection.objects = newAssetsList.ToArray();
            }
        }
    }
}
#endif