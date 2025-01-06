// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Sonity.Internal {

    public class SoundEventEditorFindAssets : Editor {

        SoundEventEditorBase parent;

        public void Initialize(SoundEventEditorBase soundEventEditor) {
            parent = soundEventEditor;
        }

        public void MenuFindAsset() {

            GenericMenu menu = new GenericMenu();

            // Tooltips dont work for menu
            menu.AddItem(new GUIContent($"Find SoundEvent Group in This Folder"), false, FindMenuCallback, SearchType.FindAssetThisFolder);
            menu.AddItem(new GUIContent($"Find SoundEvent Group in All Folders"), false, FindMenuCallback, SearchType.FindAssetAllFolders);
            menu.ShowAsContext();
        }

        private enum SearchType {
            FindAssetThisFolder,
            FindAssetAllFolders,
        }

        public void FindMenuCallback(object obj) {
            SearchType searchType;
            try {
                searchType = (SearchType)obj;
            } catch {
                return;
            }

            for (int i = 0; i < parent.mTargets.Length; i++) {
                if (EditorUtility.DisplayCancelableProgressBar($"Finding SoundContainers", $"{parent.mTargets[i].name}", (i + 1) / parent.mTargets.Length)) {
                    EditorUtility.ClearProgressBar();
                    return;
                }
                Undo.RecordObject(parent.mTargets[i], "Finding SoundContainers");
                if (searchType == SearchType.FindAssetThisFolder) {
                    FindAssets(ref parent.mTargets[i].internals.soundContainers, parent.mTargets[i], false, false);
                } else if (searchType == SearchType.FindAssetAllFolders) {
                    FindAssets(ref parent.mTargets[i].internals.soundContainers, parent.mTargets[i], false, false, true);
                }
                EditorUtility.SetDirty(parent.mTargets[i]);
            }
            EditorUtility.ClearProgressBar();
        }

        private string RemoveTrailingJunk(string input) {
            // Remove trailing numbers, space, underscore, dash
            char[] charsToRemove = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };
            return input.TrimEnd(charsToRemove);
        }

        public void FindAssets(ref SoundContainerBase[] soundContainer, SoundEventBase soundEvent, bool removeSubfolders, bool refreshElseFind, bool searchAllFolders = false) {

            string selectedPath;
            string selectedFilename;

            if (refreshElseFind) {
                SoundContainerBase firstSoundContainer = null;

                // Find first non null asset
                for (int i = 0; i < soundContainer.Length; i++) {
                    if (soundContainer[i] != null) {
                        firstSoundContainer = soundContainer[i];
                        break;
                    }
                }

                // If no asset was found
                if (firstSoundContainer == null) {
                    return;
                }

                // Get current selected path
                selectedPath = AssetDatabase.GetAssetPath(firstSoundContainer);
                selectedFilename = EditorPath.GetFileNameWithoutExtension(selectedPath);

            } else {
                // Get current selected path
                selectedPath = AssetDatabase.GetAssetPath(soundEvent);
                selectedFilename = EditorPath.GetFileNameWithoutExtension(selectedPath);
            }

            // Split into filename and path
            int index = selectedPath.LastIndexOf("/");
            if (index > 0) {
                // Remove filename
                selectedPath = selectedPath.Substring(0, index);
            }

            // Remove trailing numbers, slash, space, underscore, dash
            selectedFilename = RemoveTrailingJunk(selectedFilename).ToLowerInvariant();

            // Remove SoundEvent name if it exists
            if (selectedFilename.EndsWith("_se")) {
                selectedFilename = selectedFilename.Remove(selectedFilename.Length - "_se".Length);
            } else if (selectedFilename.EndsWith(" se")) {
                selectedFilename = selectedFilename.Remove(selectedFilename.Length - " se".Length);
            } else if (selectedFilename.EndsWith("-se")) {
                selectedFilename = selectedFilename.Remove(selectedFilename.Length - "-se".Length);
            }

            int charsEndToRemove = 0;
            List<string> foundGuids = new List<string>();
            List<string> allAssetPaths = new List<string>();
            List<string> foundPaths = new List<string>();

            if (searchAllFolders) {
                allAssetPaths.AddRange(AssetDatabase.GetAllAssetPaths());
                // Removing all assets of wrong type
                for (int i = allAssetPaths.Count - 1; i >= 0; i--) {
                    if (AssetDatabase.LoadAssetAtPath(allAssetPaths[i], typeof(SoundContainerBase)) == null) {
                        allAssetPaths.RemoveAt(i);
                    }
                }
            }

            while (charsEndToRemove <= selectedFilename.Length) {

                // If not finding anything tries to remove chars from the end of the filename to find a match
                selectedFilename = selectedFilename.Substring(0, selectedFilename.Length - charsEndToRemove).ToLowerInvariant();

                // Find all SoundContainers in the folder with the same name
                if (searchAllFolders) {
                    foundPaths.Clear();
                    foundPaths.AddRange(allAssetPaths);

                    for (int i = foundPaths.Count - 1; i >= 0; i--) {
                        if (!EditorPath.GetFileNameWithoutExtension(foundPaths[i]).ToLowerInvariant().Contains(selectedFilename)) {
                            foundPaths.RemoveAt(i);
                        }
                    }

                    // If no assets were found
                    if (foundPaths.Count == 0) {
                        if (charsEndToRemove > selectedFilename.Length) {
                            // Removes all the old SoundContainers
                            soundContainer = new SoundContainerBase[0];
                            return;
                        } else {
                            // If no assets are found, try to remove one more character at the end of the filename
                            charsEndToRemove++;
                        }
                    } else {
                        // If any are found
                        break;
                    }
                } else {
                    foundGuids.AddRange(AssetDatabase.FindAssets(selectedFilename + $" t:{nameof(NameOf.SoundContainer)}", new[] { selectedPath }));

                    // If no assets were found
                    if (foundGuids.Count == 0) {
                        if (charsEndToRemove > selectedFilename.Length) {
                            // Removes all the old SoundContainers
                            soundContainer = new SoundContainerBase[0];
                            return;
                        } else {
                            // If no assets are found, try to remove one more character at the end of the filename
                            charsEndToRemove++;
                        }
                    } else {
                        // If any are found
                        break;
                    }
                }
            }

            // The guids to add
            List<string> guidToAddList = new List<string>();
            List<string> guidToAddListCheckRemove = new List<string>();

            // Adds all hits even if they are in subfolders
            if (searchAllFolders) {
                for (int i = 0; i < foundPaths.Count; i++) {
                    // Convert from paths to guids
                    guidToAddList.Add(AssetDatabase.AssetPathToGUID(foundPaths[i]));
                }
            } else {
                guidToAddList.AddRange(foundGuids);
            }
            guidToAddListCheckRemove.AddRange(foundGuids);

            // Remove all hits which is more than the orig name
            // Eg. If orig name is Impact_Soft then remove Impact_Soft_Ground
            for (int i = guidToAddListCheckRemove.Count - 1; i >= 0; i--) {
                string tempString = EditorPath.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(guidToAddListCheckRemove[i]));

                // Remove trailing numbers, slash, space, underscore, dash
                tempString = RemoveTrailingJunk(tempString).ToLowerInvariant();

                // Remove if not same name
                if (tempString != selectedFilename) {
                    guidToAddListCheckRemove.RemoveAt(i);
                }
            }

            // If none are still left then use unfiltered version
            if (guidToAddListCheckRemove.Count > 0) {
                guidToAddList = guidToAddListCheckRemove;
            }

            // Removing all hits which are in subfolders
            if (removeSubfolders) {
                for (int i = guidToAddList.Count - 1; i >= 0; i--) {
                    string tempString = AssetDatabase.GUIDToAssetPath(guidToAddList[i]);
                    tempString = tempString.Replace(selectedPath, "");

                    // Remove start slash
                    char[] charSlash = new char[] { '/' };

                    tempString = tempString.TrimStart(charSlash);

                    // Remove if it is a subfolder
                    if (tempString.Contains("/")) {
                        guidToAddList.RemoveAt(i);
                    }
                }
            }

            // Removes all the old SoundContainers
            soundContainer = new SoundContainerBase[guidToAddList.Count];

            // Adds the assets to the SoundContainer array
            for (int i = 0; i < soundContainer.Length; i++) {
                soundContainer[i] = (SoundContainerBase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guidToAddList[i]), typeof(SoundContainerBase));
            }

            // Sort by name
            soundContainer = soundContainer.OrderBy(parameter => parameter.name).ToArray();
        }
    }
}
#endif