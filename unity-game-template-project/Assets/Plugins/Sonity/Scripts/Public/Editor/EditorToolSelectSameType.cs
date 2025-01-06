// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR
#if SONITY_ENABLE_EDITOR_TOOL_SELECT_SAME_TYPE

using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace Sonity.Internal.EditorToolSelectSameType {

    // Toolbar Actions
    // Tools/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder
    // Tools/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders
    // Assets/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder
    // Assets/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders

    // Default Shortcuts
    // Ctrl+Alt+A - Select Same Type 🤏/In Same Folder
    // Ctrl+Alt+Shift+A - Select Same Type 🤏/In Subfolders

    // Version 1.0
    // First Release

    public static class EditorToolSelectSameType {

        // Shortcut key modifiers
        // % -> Ctrl on Windows, Linux, CMD on MacOS
        // ^ -> Ctrl on Windows, Linux, MacOS
        // # -> Shift
        // & -> Alt

        const string toolsMenuPathInSameFolder = "Tools/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder";
        const string toolsMenuPathInSubfolders = "Tools/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders";
        const string assetsMenuPathInSameFolder = "Assets/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder ^&A";
        const string assetsMenuPathInSubfolders = "Assets/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders ^&#A";

        const int toolsMenuPriority = 101;
        const int assetsMenuPriority = 101;

        [MenuItem(assetsMenuPathInSameFolder, false, assetsMenuPriority)]
        private static void AssetsSelectObjectsInSameFolder() {
            SelectObjectsOfSameType(false);
        }

        [MenuItem(assetsMenuPathInSubfolders, false, assetsMenuPriority)]
        private static void AssetsSelectObjectsInSubFolders() {
            SelectObjectsOfSameType(true);
        }

        [MenuItem(toolsMenuPathInSameFolder, false, toolsMenuPriority)]
        private static void ToolsSelectObjectsInSameFolder() {
            SelectObjectsOfSameType(false);
        }

        [MenuItem(toolsMenuPathInSubfolders, false, toolsMenuPriority)]
        private static void ToolsSelectObjectsInSubFolders() {
            SelectObjectsOfSameType(true);
        }

        private static void SelectObjectsOfSameType(bool subFolders) {

            AssetDatabase.SaveAssets();

            if (Selection.objects.Length > 0) {

                UnityEngine.Object selectedObject = Selection.objects[0];

                if (selectedObject == null) {
                    return;
                }

                string selectedPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

                // Remove Filename from Path
                selectedPath = selectedPath.Replace(EditorPath.GetFileName(selectedPath), "");

                // Finding all guids of right type
                List<string> foundGuids = AssetDatabase.FindAssets($"t:" + selectedObject.GetType().Name, new[] { selectedPath }).ToList<string>();

                // Removing files in subfolders
                if (!subFolders) {
                    for (int i = foundGuids.Count - 1; i >= 0; i--) {
                        // Getting path
                        string tempString = AssetDatabase.GUIDToAssetPath(foundGuids[i]);
                        // Removing parent path
                        tempString = tempString.Replace(selectedPath, "");
                        // If contains subpath
                        if (tempString.Contains("/")){
                            // Remove index with subpath
                            foundGuids.RemoveAt(i);
                        }
                    }
                }

                List<UnityEngine.Object> foundObjects = new List<UnityEngine.Object>();

                // Load found objects
                for (int i = 0; i < foundGuids.Count; i++) {
                    UnityEngine.Object tempObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGuids[i]), typeof(UnityEngine.Object));
                    if (tempObject != null) {
                        if (!foundObjects.Contains(tempObject)) {
                            foundObjects.Add(tempObject);
                        }
                    }
                }

                // Set selelection to found objects
                Selection.objects = foundObjects.ToArray();

                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
#endif