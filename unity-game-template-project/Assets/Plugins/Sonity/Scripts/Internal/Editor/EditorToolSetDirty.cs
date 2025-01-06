// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    public static class ToolsSetDirty {

        [MenuItem("Tools/Sonity 🔊/Set Selected Assets as Dirty (Force Reserialize for Resave)", false, 110)] // Sonity
        private static void SetDirty() {
            foreach (UnityEngine.Object unityEngineObject in Selection.objects) {
                EditorUtility.SetDirty(unityEngineObject);
            }
        }
    }
}
#endif