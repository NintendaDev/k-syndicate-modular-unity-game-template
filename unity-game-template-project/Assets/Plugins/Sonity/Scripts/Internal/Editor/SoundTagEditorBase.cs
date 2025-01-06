// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundTagEditorBase : Editor {

        public SoundTagBase mTarget;
        public SoundTagBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public SerializedProperty internals;
        public SerializedProperty notes;

        public SerializedProperty assetGuid;

        public void OnEnable() {
            FindProperties();
        }

        public void FindProperties() {
            assetGuid = serializedObject.FindProperty(nameof(SoundTagBase.assetGuid));
            internals = serializedObject.FindProperty(nameof(SoundTagBase.internals));
            notes = internals.FindPropertyRelative(nameof(SoundTagInternals.notes));
        }

        public void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        public void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public Color defaultGuiColor;
        public GUIStyle guiStyleBoldCenter = new GUIStyle();

        public void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        public void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundTagBase)target;

            mTargets = new SoundTagBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundTagBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundTag, EditorTextSoundTag.soundTagTooltip, true);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            // Notes
            EditorGUI.indentLevel = 0;
            Color previousColor = GUI.color;
            if (string.IsNullOrEmpty(notes.stringValue) || notes.stringValue == "Notes") {
                // Make less transparent if empty or default text
                GUI.color = new Color(1f, 1f, 1f, 0.4f);
            }
            BeginChange();
            notes.stringValue = EditorGUILayout.TextArea(notes.stringValue);
            EndChange();
            GUI.color = previousColor;
            EditorGUILayout.Separator();

            // Asset GUID
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            BeginChange();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(assetGuid, new GUIContent(EditorTextAssetGuid.assetGuidLabel, EditorTextAssetGuid.assetGuidTooltip));
            for (int i = 0; i < mTargets.Length; i++) {
                string assetGuidTemp = EditorAssetGuid.GetAssetGuid(mTargets[i]);
                long assetGuidHashTemp = EditorAssetGuid.GetInt64HashFromString(assetGuidTemp);
                if (mTargets[i].assetGuid != assetGuidTemp || mTargets[i].assetGuidHash != assetGuidHashTemp) {
                    mTargets[i].assetGuid = assetGuidTemp;
                    mTargets[i].assetGuidHash = assetGuidHashTemp;
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EditorGUI.EndDisabledGroup();
            EndChange();
            StopBackgroundColor();
        }
    }
}
#endif