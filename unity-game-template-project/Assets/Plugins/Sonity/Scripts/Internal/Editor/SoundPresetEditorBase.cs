// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundPresetEditorBase : Editor {

        public SoundPresetBase mTarget;
        public SoundPresetBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public SerializedProperty internals;
        public SerializedProperty notes;
        public SerializedProperty disableAll;
        public SerializedProperty automaticLoop;
        public SerializedProperty soundPresetGroup;

        public void OnEnable() {
            FindProperties();
        }

        public void FindProperties() {
            internals = serializedObject.FindProperty(nameof(SoundPresetBase.internals));
            notes = internals.FindPropertyRelative(nameof(SoundPresetInternals.notes));
            disableAll = internals.FindPropertyRelative(nameof(SoundPresetInternals.disableAll));
            automaticLoop = internals.FindPropertyRelative(nameof(SoundPresetInternals.automaticLoop));
            soundPresetGroup = internals.FindPropertyRelative(nameof(SoundPresetInternals.soundPresetGroup));
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

        Color backgroundColorHue;
        float backgroundColorHueShift = -0.05f;

        private void BackgroundColorHueOffset() {
            StartBackgroundColor(backgroundColorHue);
            backgroundColorHue = EditorColor.ChangeHue(backgroundColorHue, backgroundColorHueShift);
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

        public override void OnInspectorGUI() {

            mTarget = (SoundPresetBase)target;

            mTargets = new SoundPresetBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundPresetBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            backgroundColorHue = EditorColor.GetPreset(EditorColorProSkin.GetCustomEditorBackgroundAlpha());

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundPreset, EditorTextSoundPreset.soundPresetTooltip, true);
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

            BeginChange();
            EditorGUILayout.PropertyField(disableAll, new GUIContent(EditorTextSoundPreset.disableAllLabel, EditorTextSoundPreset.disableAllTooltip));
            EndChange();

            EditorGUI.BeginDisabledGroup(disableAll.boolValue);

            BeginChange();
            EditorGUILayout.PropertyField(automaticLoop, new GUIContent(EditorTextSoundPreset.automaticLoopLabel, EditorTextSoundPreset.automaticLoopTooltip));
            EndChange();
            EditorGUILayout.Separator();

            for (int i = 0; i < soundPresetGroup.arraySize; i++) {
                SerializedProperty group = soundPresetGroup.GetArrayElementAtIndex(i);

                SerializedProperty soundContainerPreset = group.FindPropertyRelative(nameof(SoundPresetGroup.soundContainerPreset));
                SerializedProperty soundEventPreset = group.FindPropertyRelative(nameof(SoundPresetGroup.soundEventPreset));

                bool foundSC = false;
                string nameSC = "";
                if (soundContainerPreset.objectReferenceValue != null) {
                    foundSC = true;
                    nameSC = soundContainerPreset.objectReferenceValue.name;
                    nameSC = nameSC.Remove(nameSC.Length - GetNameToRemoveWithRightSeparatorChar(nameSC, "SE").Length);
                }

                bool foundSE = false;
                string nameSE = "";
                if (soundEventPreset.objectReferenceValue != null) {
                    foundSE = true;
                    nameSE = soundEventPreset.objectReferenceValue.name;
                    nameSE = nameSE.Remove(nameSE.Length - GetNameToRemoveWithRightSeparatorChar(nameSE, "SE").Length);
                }

                string nameToDrawPrefix = "Preset - ";
                string nameToDraw = "";

                if (foundSC || foundSE) {
                    if (foundSC && foundSE) {
                        if (nameSC == nameSE) {
                            nameToDraw = nameToDrawPrefix + nameSC;
                        } else {
                            nameToDraw = nameToDrawPrefix + nameSC + " and " + nameSE;
                        }
                    } else if (foundSC) {
                        nameToDraw = nameToDrawPrefix + nameSC;
                    } else if (foundSE) {
                        nameToDraw = nameToDrawPrefix + nameSE;
                    }
                } else {
                    nameToDraw = nameToDrawPrefix + "Empty";
                }

                EditorGuiFunction.DrawLabel(nameToDraw, "", false, false);

                BackgroundColorHueOffset();
                SerializedProperty disable = group.FindPropertyRelative(nameof(SoundPresetGroup.disable));

                BeginChange();
                EditorGUILayout.PropertyField(disable, new GUIContent(EditorTextSoundPreset.disableLabel, EditorTextSoundPreset.disableTooltip));
                EndChange();

                if (!disableAll.boolValue) {
                    EditorGUI.BeginDisabledGroup(disable.boolValue);
                }

                BeginChange();
                EditorGUILayout.PropertyField(soundContainerPreset, new GUIContent(EditorTextSoundPreset.fromSoundContainerLabel, EditorTextSoundPreset.fromSoundContainerTooltip));
                EndChange();

                BeginChange();
                EditorGUILayout.PropertyField(soundEventPreset, new GUIContent(EditorTextSoundPreset.fromSoundEventLabel, EditorTextSoundPreset.fromSoundEventTooltip));
                EndChange();

                SerializedProperty matchExpand = group.FindPropertyRelative(nameof(SoundPresetGroup.matchExpand));
                BeginChange();
                EditorGuiFunction.DrawFoldout(matchExpand, "Match", "");
                EndChange();
                EditorGUI.indentLevel++;

                if (matchExpand.boolValue) {

                    SerializedProperty matchUse = group.FindPropertyRelative(nameof(SoundPresetGroup.matchUse));
                    BeginChange();
                    EditorGUILayout.PropertyField(matchUse, new GUIContent(EditorTextSoundPreset.matchNameLabel, EditorTextSoundPreset.matchNameTooltip));
                    EndChange();

                    if (matchUse.boolValue) {

                        SerializedProperty applyOnAll = group.FindPropertyRelative(nameof(SoundPresetGroup.applyOnAll));
                        BeginChange();
                        EditorGUILayout.PropertyField(applyOnAll, new GUIContent(EditorTextSoundPreset.applyOnAllLabel, EditorTextSoundPreset.applyOnAllTooltip));
                        EndChange();

                        if (!applyOnAll.boolValue) {
                            SerializedProperty isPrefixUse = group.FindPropertyRelative(nameof(SoundPresetGroup.isPrefixUse));
                            BeginChange();
                            EditorGUILayout.PropertyField(isPrefixUse, new GUIContent(EditorTextSoundPreset.isPrefixLabel, EditorTextSoundPreset.isPrefixTooltip));
                            EndChange();

                            if (isPrefixUse.boolValue) {
                                EditorGUI.indentLevel++;
                                SerializedProperty isPrefixString = group.FindPropertyRelative(nameof(SoundPresetGroup.isPrefixString));
                                int lowestArrayLength = int.MaxValue;
                                for (int n = 0; n < mTargets.Length; n++) {
                                    if (lowestArrayLength > mTargets[n].internals.soundPresetGroup[i].isPrefixString.Length) {
                                        lowestArrayLength = mTargets[n].internals.soundPresetGroup[i].isPrefixString.Length;
                                    }
                                }
                                EditorGuiFunction.DrawReordableArray(isPrefixString, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                                EditorGUI.indentLevel--;
                            }

                            SerializedProperty isNotPrefixUse = group.FindPropertyRelative(nameof(SoundPresetGroup.isNotPrefixUse));
                            BeginChange();
                            EditorGUILayout.PropertyField(isNotPrefixUse, new GUIContent(EditorTextSoundPreset.isNotPrefixLabel, EditorTextSoundPreset.isNotPrefixTooltip));
                            EndChange();

                            if (isNotPrefixUse.boolValue) {
                                EditorGUI.indentLevel++;
                                SerializedProperty isNotPrefixString = group.FindPropertyRelative(nameof(SoundPresetGroup.isNotPrefixString));
                                int lowestArrayLength = int.MaxValue;
                                for (int n = 0; n < mTargets.Length; n++) {
                                    if (lowestArrayLength > mTargets[n].internals.soundPresetGroup[i].isNotPrefixString.Length) {
                                        lowestArrayLength = mTargets[n].internals.soundPresetGroup[i].isNotPrefixString.Length;
                                    }
                                }
                                EditorGuiFunction.DrawReordableArray(isNotPrefixString, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                                EditorGUI.indentLevel--;
                            }

                            SerializedProperty containsUse = group.FindPropertyRelative(nameof(SoundPresetGroup.containsUse));
                            BeginChange();
                            EditorGUILayout.PropertyField(containsUse, new GUIContent(EditorTextSoundPreset.containsLabel, EditorTextSoundPreset.containsTooltip));
                            EndChange();

                            if (containsUse.boolValue) {
                                EditorGUI.indentLevel++;
                                SerializedProperty containsString = group.FindPropertyRelative(nameof(SoundPresetGroup.containsString));
                                int lowestArrayLength = int.MaxValue;
                                for (int n = 0; n < mTargets.Length; n++) {
                                    if (lowestArrayLength > mTargets[n].internals.soundPresetGroup[i].containsString.Length) {
                                        lowestArrayLength = mTargets[n].internals.soundPresetGroup[i].containsString.Length;
                                    }
                                }
                                EditorGuiFunction.DrawReordableArray(containsString, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                                EditorGUI.indentLevel--;
                            }

                            SerializedProperty notContainsUse = group.FindPropertyRelative(nameof(SoundPresetGroup.notContainsUse));
                            BeginChange();
                            EditorGUILayout.PropertyField(notContainsUse, new GUIContent(EditorTextSoundPreset.notContainsLabel, EditorTextSoundPreset.notContainsTooltip));
                            EndChange();

                            if (notContainsUse.boolValue) {
                                EditorGUI.indentLevel++;
                                SerializedProperty notContainsString = group.FindPropertyRelative(nameof(SoundPresetGroup.notContainsString));
                                int lowestArrayLength = int.MaxValue;
                                for (int n = 0; n < mTargets.Length; n++) {
                                    if (lowestArrayLength > mTargets[n].internals.soundPresetGroup[i].notContainsString.Length) {
                                        lowestArrayLength = mTargets[n].internals.soundPresetGroup[i].notContainsString.Length;
                                    }
                                }
                                EditorGuiFunction.DrawReordableArray(notContainsString, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                }
                EditorGUI.indentLevel--;

                StopBackgroundColor();

                if (!disableAll.boolValue && disable.boolValue) {
                    EditorGUI.EndDisabledGroup();
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth)); // For offsetting the buttons to the right
                BeginChange();
                if (GUILayout.Button(new GUIContent("+", ""))) {
                    soundPresetGroup.InsertArrayElementAtIndex(i);
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent("-", ""))) {
                    if (soundPresetGroup.arraySize > 1) {
                        soundPresetGroup.DeleteArrayElementAtIndex(i);
                    }
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent("↑", ""))) {
                    soundPresetGroup.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, soundPresetGroup.arraySize));
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent("↓", ""))) {
                    soundPresetGroup.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, soundPresetGroup.arraySize));
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button("Reset")) {
                    for (int ii = 0; ii < mTargets.Length; ii++) {
                        Undo.RecordObject(mTargets[ii], $"Reset {nameof(NameOf.SoundPhysics)}");
                        if (i < mTargets[ii].internals.soundPresetGroup.Length && mTargets[ii].internals.soundPresetGroup[i] != null) {
                            mTargets[ii].internals.soundPresetGroup[i] = new SoundPresetGroup();
                        }
                        EditorUtility.SetDirty(mTargets[ii]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
            }

            if (disableAll.boolValue) {
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();


            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));

            // Reset All
            BeginChange();
            if (GUILayout.Button(new GUIContent("Reset All", ""))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset");
                    mTargets[i].internals = new SoundPresetInternals();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }
    }
}
#endif