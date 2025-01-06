// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public static class EditorColorProSkin {

        private static float customEditorBackgroundAlphaDarkSkin = 0.8f;
        private static float customEditorBackgroundAlphaLightSkin = 0.2f;

        public static float GetCustomEditorBackgroundAlpha() {
            if (EditorGUIUtility.isProSkin) {
                return customEditorBackgroundAlphaDarkSkin;
            } else {
                return customEditorBackgroundAlphaLightSkin;
            }
        }

        private static float customPropertyDrawerBackgroundAlphaDarkSkin = 1f;
        private static float customPropertyDrawerBackgroundAlphaLightSkin = 0.3f;

        public static float GetCustomPropertyDrawerBackgroundAlpha() {
            if (EditorGUIUtility.isProSkin) {
                return customPropertyDrawerBackgroundAlphaDarkSkin;
            } else {
                return customPropertyDrawerBackgroundAlphaLightSkin;
            }
        }

        // Black
        private static Color lightSkinTextColor = new Color(0f, 0f, 0f);
        public static Color GetLightSkinTextColor() {
            return lightSkinTextColor;
        }

        // Light grey
        private static Color darkSkinTextColor = new Color(0.706f, 0.706f, 0.706f);
        public static Color GetDarkSkinTextColor() {
            return darkSkinTextColor;
        }

        // Green
        public static Color GetTextGreen() {
            if (EditorGUIUtility.isProSkin) {
                return new Color(0f, 1f, 0f);
            } else {
                return EditorColor.ChangeValue(new Color(0f, 1f, 0f), -0.4f);
            }
        }

        // Red
        public static Color GetTextRed() {
            if (EditorGUIUtility.isProSkin) {
                return new Color(1f, 0f, 0f);
            } else {
                return EditorColor.ChangeValue(new Color(1f, 0f, 0f), -0.2f);
            }
        }

        // Orange
        public static Color GetTextOrange() {
            if (EditorGUIUtility.isProSkin) {
                return new Color(1f, 0.5f, 0f);
            } else {
                return EditorColor.ChangeValue(new Color(1f, 0.5f, 0f), -0.1f);
            }
        }
    }
}
#endif