// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public static class EditorTrial {

        // Don't fill in version
        public static string versionText = "";

        // Full Version
        public static string trialTooltip = "";
        public static bool isEducational = false;
        public static bool isTrial = false;

        //// Educational Version
        //public static string trialTooltip = "\n\nThis educational version is for non-commercial use only.\nPlease buy the full version to get all features and to support developement.";
        //public static bool isEducational = true;
        //public static bool isTrial = false;

        //// Free Trial
        //public static string trialTooltip = "\n\nThis free trial is for testing only.\nIt doesn't output sounds in build.\nPlease buy the full version to get sound in builds and to support developement.";
        //public static bool isEducational = false;
        //public static bool isTrial = true;

        public static void InfoText() {
            // Free Trial
            if (isTrial || isEducational) {
                GUIStyle guiStyleTrial = new GUIStyle();
                guiStyleTrial.fontSize = 16;
                guiStyleTrial.fontStyle = FontStyle.Bold;
                guiStyleTrial.alignment = TextAnchor.MiddleCenter;
                guiStyleTrial.normal.textColor = EditorColorProSkin.GetTextOrange();
                if (isTrial) {
                    if (GUILayout.Button(new GUIContent($"Free Trial - Buy the full version to get all features",
                        "This free trial is for testing only.\nIt doesn't output sounds in build\nPlease buy the full version to get sound in builds and to support developement.")
                        , guiStyleTrial, GUILayout.ExpandWidth(true), GUILayout.Height(40)
#if !UNITY_2019_1_OR_NEWER
                    // Unity 2018 or older has wrong offset
                    , GUILayout.Width(EditorGUIUtility.currentViewWidth - 55)
#endif
                        )) {
                        Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857");
                    }
                }
                // Educational Version
                if (isEducational) {
                    if (GUILayout.Button(new GUIContent($"Educational Version - Only for non-commercial use",
                        "This educational version is for non-commercial use only.\nPlease buy the full version to get all features and to support developement.")
                        , guiStyleTrial, GUILayout.ExpandWidth(true), GUILayout.Height(40)
#if !UNITY_2019_1_OR_NEWER
                    // Unity 2018 or older has wrong offset
                    , GUILayout.Width(EditorGUIUtility.currentViewWidth - 55)
#endif
                        )) {
                        Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/sonity-audio-middleware-229857");
                    }
                }
            }
        }
    }
}
#endif