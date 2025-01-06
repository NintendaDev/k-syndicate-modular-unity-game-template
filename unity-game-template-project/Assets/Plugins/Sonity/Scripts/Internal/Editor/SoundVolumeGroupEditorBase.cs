// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundVolumeGroupEditorBase : Editor {

        public SoundVolumeGroupBase mTarget;
        public SoundVolumeGroupBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public SerializedProperty assetGuid;
        public SerializedProperty internals;
        public SerializedProperty notes;
        public SerializedProperty volumeRatio;
        public SerializedProperty volumeDecibel;

        public void OnEnable() {
            FindProperties();
        }

        public void FindProperties() {
            assetGuid = serializedObject.FindProperty(nameof(SoundVolumeGroupBase.assetGuid));
            internals = serializedObject.FindProperty(nameof(SoundVolumeGroupBase.internals));
            notes = internals.FindPropertyRelative(nameof(SoundVolumeGroupInternals.notes));
            volumeRatio = internals.FindPropertyRelative(nameof(SoundVolumeGroupInternals.volumeRatio));
            volumeDecibel = internals.FindPropertyRelative(nameof(SoundVolumeGroupInternals.volumeDecibel));
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

            mTarget = (SoundVolumeGroupBase)target;

            mTargets = new SoundVolumeGroupBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundVolumeGroupBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundVolumeGroup, EditorTextSoundVolumeGroup.soundVolumeGroupTooltip, true);
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

            // Volume
            StartBackgroundColor(EditorColor.GetVolumeMax(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            BeginChange();
            EditorGUILayout.Slider(volumeDecibel, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease12dbMaxDecibel, new GUIContent(EditorTextSoundVolumeGroup.volumeLabel, EditorTextSoundVolumeGroup.volumeTooltip));
            if (volumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                volumeDecibel.floatValue = Mathf.NegativeInfinity;
            }
            if (volumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue)) {
                volumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue);
            }
            EndChange();

            // Lower volume 1 dB
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));

            string minVolumeName = "";
            if (mTargets.Length > 1) {
                // Finds the lowest volume in dB
                float minVolumeDecibel = Mathf.Infinity;
                for (int i = 0; i < mTargets.Length; i++) {
                    if (minVolumeDecibel > mTargets[i].internals.volumeDecibel) {
                        minVolumeDecibel = mTargets[i].internals.volumeDecibel;
                    }
                }
                if (minVolumeDecibel < VolumeScale.lowestVolumeDecibel) {
                    minVolumeName = " (Min " + "-Infinity" + ")";
                } else {
                    minVolumeName = " (Min " + Mathf.FloorToInt(minVolumeDecibel) + ")";
                }
            }
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundVolumeGroup.volumeRelativeLowerLabel + minVolumeName, EditorTextSoundVolumeGroup.volumeRelativeLowerTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Volume -1 dB");
                    // Don't clamp max when lowering volume so you always can lower it
                    mTargets[i].internals.volumeDecibel = Mathf.Clamp(mTargets[i].internals.volumeDecibel - 1f, VolumeScale.lowestVolumeDecibel, Mathf.Infinity);
                    mTargets[i].internals.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.volumeDecibel);
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();

            string maxVolumeName = "";
            if (mTargets.Length > 1) {
                // Finds the highest volume in dB
                float maxVolumeDecibel = Mathf.NegativeInfinity;
                for (int i = 0; i < mTargets.Length; i++) {
                    if (maxVolumeDecibel < mTargets[i].internals.volumeDecibel) {
                        maxVolumeDecibel = mTargets[i].internals.volumeDecibel;
                    }
                }
                if (maxVolumeDecibel < VolumeScale.lowestVolumeDecibel) {
                    maxVolumeName = " (Max " + "-Infinity" + ")";
                } else {
                    maxVolumeName = " (Max " + Mathf.FloorToInt(maxVolumeDecibel) + ")";
                }
            }
            // Increase volume 1 dB
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundVolumeGroup.volumeRelativeIncreaseLabel + maxVolumeName, EditorTextSoundVolumeGroup.volumeRelativeIncreaseTooltip))) {
                // Finds the highest volume in dB
                float maxVolumeDecibel = Mathf.NegativeInfinity;
                for (int i = 0; i < mTargets.Length; i++) {
                    if (maxVolumeDecibel < mTargets[i].internals.volumeDecibel) {
                        maxVolumeDecibel = mTargets[i].internals.volumeDecibel;
                    }
                }
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Volume +1 dB");
                    mTargets[i].internals.volumeDecibel = Mathf.Clamp(mTargets[i].internals.volumeDecibel + 1f, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease12dbMaxDecibel);
                    mTargets[i].internals.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.volumeDecibel);
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Warning if volume is over 0
            for (int i = 0; i < mTargets.Length; i++) {
                if (mTargets[i].internals.volumeRatio > (VolumeScale.volumeIncrease12dbMaxRatio + 0.00001)) {
                    EditorGUILayout.HelpBox(EditorTextSoundVolumeGroup.volumeOverLimitWarning, MessageType.Warning);
                    break;
                }
            }
            StopBackgroundColor();
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