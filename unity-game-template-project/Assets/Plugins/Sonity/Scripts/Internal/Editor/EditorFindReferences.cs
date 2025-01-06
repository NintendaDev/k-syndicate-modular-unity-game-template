// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;

namespace Sonity.Internal {

    public static class EditorFindReferences {

        public static UnityEngine.Object[] GetObjects(UnityEngine.Object targetObject) {

            AssetDatabase.SaveAssets();

            string targetPath = AssetDatabase.GetAssetPath(targetObject);
            string[] allAssets = AssetDatabase.GetAllAssetPaths();
            List<string> foundPaths = new List<string>();

            // Progress Bar Initialize
            int progressCurrent = 0;
            int progressTarget = allAssets.Length;
            bool cancel = false;

            // Find dependencies
            foreach (string prefab in allAssets) {
                progressCurrent++;
                string[] dependencies = AssetDatabase.GetDependencies(prefab, false);
                progressTarget += dependencies.Length;
                foreach (string dependedPath in dependencies) {
                    progressCurrent++;
                    if (dependedPath == targetPath) {
                        foundPaths.Add(prefab);
                    }

                    // Progress Bar Display
                    float progressBar = (progressCurrent + 1f) / progressTarget;
                    if (EditorUtility.DisplayCancelableProgressBar($"Finding References", $"{dependedPath}", progressBar)) {
                        cancel = true;
                        break;
                    }
                }
                if (cancel) {
                    break;
                }
            }

            EditorUtility.ClearProgressBar();

            // Sort based on path
            foundPaths.Sort();

            // Get object references
            List<UnityEngine.Object> foundObjects = new List<UnityEngine.Object>();
            for (int i = 0; i < foundPaths.Count; i++) {
                foundObjects.Add(AssetDatabase.LoadAssetAtPath(foundPaths[i], typeof(UnityEngine.Object)));
            }

            return foundObjects.ToArray();
        }
    }
}
#endif