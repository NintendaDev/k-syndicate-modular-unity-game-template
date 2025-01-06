// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundPhysicsConditionEditorBase : Editor {

        public SoundPhysicsConditionBase mTarget;
        public SoundPhysicsConditionBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public float backgroundHueShiftAmount = 0.07f;

        public SerializedProperty assetGuid;
        public SerializedProperty internals;

        public SerializedProperty notes;

        public SerializedProperty childConditions;

        public SerializedProperty soundTag;
        public SerializedProperty physicsPlayOn;
        public SerializedProperty playDisregardingConditions;

        public SerializedProperty tagExpand;
        public SerializedProperty layerExpand;
        public SerializedProperty terrainIndexExpand;
        public SerializedProperty terrainNameExpand;
        public SerializedProperty componentExpand;

        public SerializedProperty isTagUse;
        public SerializedProperty isNotTagUse;
        public SerializedProperty isLayerUse;
        public SerializedProperty isNotLayerUse;
        public SerializedProperty isTerrainIndexUse;
        public SerializedProperty isNotTerrainIndexUse;
        public SerializedProperty isTerrainNameUse;
        public SerializedProperty isNotTerrainNameUse;
        public SerializedProperty hasComponentUse;
        public SerializedProperty hasNotComponentUse;

        private void OnEnable() {
            FindProperties();
        }

        private void FindProperties() {
            assetGuid = serializedObject.FindProperty(nameof(SoundPhysicsConditionBase.assetGuid));
            internals = serializedObject.FindProperty(nameof(SoundPhysicsConditionBase.internals));
            notes = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.notes));

            childConditions = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.childConditions));

            soundTag = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.soundTag));
            physicsPlayOn = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.physicsPlayOn));
            playDisregardingConditions = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.playDisregardingConditions));
            
            tagExpand = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.tagExpand));
            layerExpand = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.layerExpand));
            terrainIndexExpand = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.terrainIndexExpand));
            terrainNameExpand = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.terrainNameExpand));
            componentExpand = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.componentExpand));

            isTagUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTagUse));
            isNotTagUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTagUse));
            isLayerUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isLayerUse));
            isNotLayerUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotLayerUse));
            isTerrainIndexUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainIndexUse));
            isNotTerrainIndexUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainIndexUse));
            isTerrainNameUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainNameUse));
            isNotTerrainNameUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainNameUse));
            hasComponentUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasComponentUse));
            hasNotComponentUse = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasNotComponentUse));
        }

        private GUIStyle guiStyleBoldCenter = new GUIStyle();
        private Color defaultGuiColor;

        private void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        private void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        Color colorPhysicsCondition;

        public override void OnInspectorGUI() {

            mTarget = (SoundPhysicsConditionBase)target;

            mTargets = new SoundPhysicsConditionBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundPhysicsConditionBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundPhysicsCondition, EditorTextSoundPhysicsCondition.soundPhysicsConditionTooltip, true);
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

            // Conditions Background Color
            colorPhysicsCondition = EditorColor.GetPhysicsConditionsColor(EditorColorProSkin.GetCustomEditorBackgroundAlpha());

            EditorGuiFunction.DrawLabel(EditorTextSoundPhysicsCondition.childrenLabel, EditorTextSoundPhysicsCondition.childrenTooltip, false, false);

            int lowestArrayLengthChildren = int.MaxValue;
            for (int n = 0; n < mTargets.Length; n++) {
                if (lowestArrayLengthChildren > mTargets[n].internals.childConditions.Length) {
                    lowestArrayLengthChildren = mTargets[n].internals.childConditions.Length;
                }
            }
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f)); // Transparent background so the offset will be right
            EditorGuiFunction.DrawReordableArray(childConditions, serializedObject, lowestArrayLengthChildren, false);
            StopBackgroundColor();

            // Check if infinite loop
            if (ShouldDebug.GuiWarnings()) {
                if (mTarget.internals.GetIfInfiniteLoop(mTarget, out SoundPhysicsConditionBase infiniteInstigator, out SoundPhysicsConditionBase infinitePrevious)) {
                    EditorGUILayout.Separator();
                    EditorGUILayout.HelpBox("\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", MessageType.Error);
                    EditorGUILayout.Separator();
                }
            }

            EditorGUILayout.Separator();

            EditorGuiFunction.DrawLabel(EditorTextSoundPhysicsCondition.parentLabel, EditorTextSoundPhysicsCondition.parentTooltip, false, false); ;

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));

            // SoundTag
            BeginChange();
            EditorGUILayout.ObjectField(soundTag, new GUIContent(EditorTextSoundPhysicsCondition.soundTagLabel, EditorTextSoundPhysicsCondition.soundTagTooltip));
            EndChange();

            // PhysicsPlayOn
            BeginChange();
            EditorGUILayout.PropertyField(physicsPlayOn, new GUIContent(EditorTextSoundPhysicsCondition.playOnLabel, EditorTextSoundPhysicsCondition.playOnTooltip));
            EndChange();

            // Play Disregarding Conditions
            BeginChange();
            playDisregardingConditions.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.playDisregardingConditionsLabel, EditorTextSoundPhysicsCondition.playDisregardingConditionsTooltip), playDisregardingConditions.boolValue);
            EndChange();

            StopBackgroundColor();
            EditorGUILayout.Separator();

            // Disabled Group
            EditorGUI.BeginDisabledGroup(playDisregardingConditions.boolValue);

            // Tag
            BeginChange();
            EditorGuiFunction.DrawFoldout(tagExpand, EditorTextSoundPhysicsCondition.tagHeaderLabel, EditorTextSoundPhysicsCondition.tagHeaderTooltip);
            EndChange();
            if (tagExpand.boolValue) {
                if (isTagUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Tag
                BeginChange();
                isTagUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.tagIsLabel, EditorTextSoundPhysicsCondition.tagIsTooltip), isTagUse.boolValue);
                EndChange();
                if (isTagUse.boolValue) {
                    SerializedProperty abortAllOnNoMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTagAbortAllOnNoMatch));
                    BeginChange();
                    abortAllOnNoMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnNoMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnNoMatchTooltip), abortAllOnNoMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTagArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isTagArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isTagArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Tag);
                }
                StopBackgroundColor();

                if (isNotTagUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Not Tag
                BeginChange();
                isNotTagUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.tagIsNotLabel, EditorTextSoundPhysicsCondition.tagIsNotTooltip), isNotTagUse.boolValue);
                EndChange();
                if (isNotTagUse.boolValue) {
                    SerializedProperty abortAllOnMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTagAbortAllOnMatch));
                    BeginChange();
                    abortAllOnMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnMatchTooltip), abortAllOnMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTagArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isNotTagArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isNotTagArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Tag);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            // Layer
            BeginChange();
            EditorGuiFunction.DrawFoldout(layerExpand, EditorTextSoundPhysicsCondition.layerHeaderLabel, EditorTextSoundPhysicsCondition.layerHeaderTooltip);
            EndChange();
            if (layerExpand.boolValue) {
                if (isLayerUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Layer
                BeginChange();
                isLayerUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.layerIsLabel, EditorTextSoundPhysicsCondition.layerIsTooltip), isLayerUse.boolValue);
                EndChange();
                if (isLayerUse.boolValue) {
                    SerializedProperty abortAllOnNoMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isLayerAbortAllOnNoMatch));
                    BeginChange();
                    abortAllOnNoMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnNoMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnNoMatchTooltip), abortAllOnNoMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isLayerArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isLayerArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isLayerArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Layer);
                }
                StopBackgroundColor();

                if (isNotLayerUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Not Layer
                BeginChange();
                isNotLayerUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.layerIsNotLabel, EditorTextSoundPhysicsCondition.layerIsNotTooltip), isNotLayerUse.boolValue);
                EndChange();
                if (isNotLayerUse.boolValue) {
                    SerializedProperty abortAllOnMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotLayerAbortAllOnMatch));
                    BeginChange();
                    abortAllOnMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnMatchTooltip), abortAllOnMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotLayerArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isNotLayerArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isNotLayerArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Layer);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            // Component
            BeginChange();
            EditorGuiFunction.DrawFoldout(componentExpand, EditorTextSoundPhysicsCondition.componentHeaderLabel, EditorTextSoundPhysicsCondition.componentHeaderTooltip);
            EndChange();
            if (componentExpand.boolValue) {
                if (hasComponentUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Has Component
                BeginChange();
                hasComponentUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.componentHasLabel, EditorTextSoundPhysicsCondition.componentHasTooltip), hasComponentUse.boolValue);
                EndChange();
                if (hasComponentUse.boolValue) {
                    SerializedProperty abortAllOnNoMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasComponentAbortAllOnNoMatch));
                    BeginChange();
                    abortAllOnNoMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnNoMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnNoMatchTooltip), abortAllOnNoMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasComponentArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.hasComponentArray.Length) {
                            lowestArrayLength = mTargets[n].internals.hasComponentArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();

                if (hasNotComponentUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Has Not Component
                BeginChange();
                hasNotComponentUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.componentHasNotLabel, EditorTextSoundPhysicsCondition.componentHasNotTooltip), hasNotComponentUse.boolValue);
                EndChange();
                if (hasNotComponentUse.boolValue) {
                    SerializedProperty abortAllOnMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasNotComponentAbortAllOnMatch));
                    BeginChange();
                    abortAllOnMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnMatchTooltip), abortAllOnMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.hasNotComponentArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.hasNotComponentArray.Length) {
                            lowestArrayLength = mTargets[n].internals.hasNotComponentArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            // Terrain Name
            BeginChange();
            EditorGuiFunction.DrawFoldout(terrainNameExpand, EditorTextSoundPhysicsCondition.terrainNameHeaderLabel, EditorTextSoundPhysicsCondition.terrainNameHeaderTooltip);
            EndChange();
            if (terrainNameExpand.boolValue) {
                if (isTerrainNameUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Terrain Name
                BeginChange();
                isTerrainNameUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.terrainNameIsLabel, EditorTextSoundPhysicsCondition.terrainNameIsTooltip), isTerrainNameUse.boolValue);
                EndChange();
                if (isTerrainNameUse.boolValue) {
                    SerializedProperty abortAllOnNoMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainNameAbortAllOnNoMatch));
                    BeginChange();
                    abortAllOnNoMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnNoMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnNoMatchTooltip), abortAllOnNoMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainNameArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isTerrainNameArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isTerrainNameArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();

                if (isNotTerrainNameUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Not Terrain Name
                BeginChange();
                isNotTerrainNameUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.terrainNameIsNotLabel, EditorTextSoundPhysicsCondition.terrainNameIsNotTooltip), isNotTerrainNameUse.boolValue);
                EndChange();
                if (isNotTerrainNameUse.boolValue) {
                    SerializedProperty abortAllOnMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainNameAbortAllOnMatch));
                    BeginChange();
                    abortAllOnMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnMatchTooltip), abortAllOnMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainNameArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isNotTerrainNameArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isNotTerrainNameArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            // Terrain Index
            BeginChange();
            EditorGuiFunction.DrawFoldout(terrainIndexExpand, EditorTextSoundPhysicsCondition.terrainIndexHeaderLabel, EditorTextSoundPhysicsCondition.terrainIndexHeaderTooltip);
            EndChange();
            if (terrainIndexExpand.boolValue) {
                if (isTerrainIndexUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Terrain Index
                BeginChange();
                isTerrainIndexUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.terrainIndexIsLabel, EditorTextSoundPhysicsCondition.terrainIndexIsTooltip), isTerrainIndexUse.boolValue);
                EndChange();
                if (isTerrainIndexUse.boolValue) {
                    SerializedProperty abortAllOnNoMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainIndexAbortAllOnNoMatch));
                    BeginChange();
                    abortAllOnNoMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnNoMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnNoMatchTooltip), abortAllOnNoMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isTerrainIndexArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isTerrainIndexArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isTerrainIndexArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();

                if (isNotTerrainIndexUse.boolValue) {
                    StartBackgroundColor(colorPhysicsCondition);
                } else {
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                }
                // Is Not Terrain Index
                BeginChange();
                isNotTerrainIndexUse.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.terrainIndexIsNotLabel, EditorTextSoundPhysicsCondition.terrainIndexIsNotTooltip), isNotTerrainIndexUse.boolValue);
                EndChange();
                if (isNotTerrainIndexUse.boolValue) {
                    SerializedProperty abortAllOnMatch = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainIndexAbortAllOnMatch));
                    BeginChange();
                    abortAllOnMatch.boolValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundPhysicsCondition.abortAllOnMatchLabel, EditorTextSoundPhysicsCondition.abortAllOnMatchTooltip), abortAllOnMatch.boolValue);
                    EndChange();
                    SerializedProperty partArray = internals.FindPropertyRelative(nameof(SoundPhysicsConditionInternals.isNotTerrainIndexArray));
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.isNotTerrainIndexArray.Length) {
                            lowestArrayLength = mTargets[n].internals.isNotTerrainIndexArray.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(partArray, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Property);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            // Disabled Group
            EditorGUI.EndDisabledGroup();

            // Reset
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f)); // Transparent background so the offset will be right
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth)); // For offsetting the buttons to the right

            // Reset Settings
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundPhysicsCondition.resetSettingsLabel, EditorTextSoundPhysicsCondition.resetSettingsTooltip))) {
                soundTag.objectReferenceValue = null;
                physicsPlayOn.enumValueIndex = (int)PhysicsPlayOn.OnCollisionAndTrigger;
                playDisregardingConditions.boolValue = false;

                tagExpand.boolValue = true;
                layerExpand.boolValue = true;
                terrainIndexExpand.boolValue = true;
                terrainNameExpand.boolValue = true;
                componentExpand.boolValue = true;

                isTagUse.boolValue = false; ;
                isNotTagUse.boolValue = false;
                isLayerUse.boolValue = false;
                isNotLayerUse.boolValue = false;
                isTerrainIndexUse.boolValue = false;
                isNotTerrainIndexUse.boolValue = false;
                isTerrainNameUse.boolValue = false;
                isNotTerrainNameUse.boolValue = false;
                hasComponentUse.boolValue = false;
                hasNotComponentUse.boolValue = false;
            }
            EndChange();

            // Reset All
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundPhysicsCondition.resetAllLabel, EditorTextSoundPhysicsCondition.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset");
                    mTargets[i].internals = new SoundPhysicsConditionInternals();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();

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