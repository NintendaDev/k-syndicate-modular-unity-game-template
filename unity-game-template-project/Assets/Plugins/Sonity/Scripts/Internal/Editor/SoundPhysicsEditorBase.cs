// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundPhysicsEditorBase : Editor {

        public SoundPhysicsBase mTarget;
        public SoundPhysicsBase[] mTargets;

        public SerializedProperty internals;
        public SerializedProperty soundPhysicsComponentType;
        public SerializedProperty physicsDimensions;
        public SerializedProperty soundTagOverride;
        public SerializedProperty impactSoundPhysicsParts;
        public SerializedProperty frictionSoundPhysicsParts;
        public SerializedProperty exitSoundPhysicsParts;

        private void OnEnable() {
            FindProperties();
            SetNames();
            LegacyAssignSoundEventsToArray();
        }

        private void FindProperties() {
            internals = serializedObject.FindProperty(nameof(SoundPhysicsBase.internals));
            soundPhysicsComponentType = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.soundPhysicsComponentType));
            physicsDimensions = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.physicsDimension));
            soundTagOverride = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.soundTagOverride));
            impactSoundPhysicsParts = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.impactSoundPhysicsParts));
            frictionSoundPhysicsParts = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.frictionSoundPhysicsParts));
            exitSoundPhysicsParts = internals.FindPropertyRelative(nameof(SoundPhysicsBase.internals.exitSoundPhysicsParts));
        }

        string componentName = "";
        string is2dname = "";

        private void SetNames() {
            if (soundPhysicsComponentType.enumValueIndex == (int)SoundPhysicsComponentType.SoundPhysics) {
                if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._3D) {
                    componentName = NameOf.SoundPhysics;
                } else if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._2D) {
                    componentName = NameOf.SoundPhysics2d;
                }
            } else if (soundPhysicsComponentType.enumValueIndex == (int)SoundPhysicsComponentType.SoundPhysicsNoFriction) {
                if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._3D) {
                    componentName = NameOf.SoundPhysicsNoFriction;
                } else if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._2D) {
                    componentName = NameOf.SoundPhysicsNoFriction2d;
                }
            }
            if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._2D) {
                is2dname = " 2D";
            }
        }

        private void LegacyAssignSoundEventsToArray() {
            mTargets = new SoundPhysicsBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundPhysicsBase)targets[i];
            }
            // Legacy, copies the old single soundevent to the array
            for (int i = 0; i < mTargets.Length; i++) {
                bool anyAdded = false;
                for (int ii = 0; ii < mTargets[i].internals.impactSoundPhysicsParts.Length; ii++) {
                    if (mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvent != null) {
                        if (mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvents.Length > 0 && mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvents[0] == null) {
                            if (anyAdded == false) {
                                Undo.RecordObject(mTargets[i], "Legacy automatic convert SoundEvent to array");
                            }
                            anyAdded = true;
                            mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvents[0] = mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvent;
                        }
                        mTargets[i].internals.impactSoundPhysicsParts[ii].soundEvent = null;
                    }
                }
                for (int ii = 0; ii < mTargets[i].internals.frictionSoundPhysicsParts.Length; ii++) {
                    if (mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvent != null) {
                        if (mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvents.Length > 0 && mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvents[0] == null) {
                            if (anyAdded == false) {
                                Undo.RecordObject(mTargets[i], "Legacy automatic convert SoundEvent to array");
                            }
                            anyAdded = true;
                            mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvents[0] = mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvent;
                        }
                        mTargets[i].internals.frictionSoundPhysicsParts[ii].soundEvent = null;
                    }
                }
                // Exit did not exist before
                if (anyAdded) {
                    EditorUtility.SetDirty(mTargets[i]);
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPhysics)}: Converted Legacy SoundEvent to Array - " + mTargets[i].gameObject.name);
                }
            }
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

        Color backgroundColorHue;
        float backgroundColorHueShift = -0.075f;

        private void BackgroundColorHueOffset() {
            StartBackgroundColor(backgroundColorHue);
            backgroundColorHue = EditorColor.ChangeHue(backgroundColorHue, backgroundColorHueShift);
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundPhysicsBase)target;

            mTargets = new SoundPhysicsBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundPhysicsBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            EditorGUI.indentLevel = 0;

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, componentName, EditorTextSoundPhysics.soundPhysicsTooltip, false);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            backgroundColorHue = EditorColor.GetPhysics(EditorColorProSkin.GetCustomEditorBackgroundAlpha());

            if (ShouldDebug.GuiWarnings()) {
                if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._3D) {
                    // 3D
                    if (mTarget.gameObject.GetComponent<Rigidbody>() == null) {
                        EditorGUILayout.HelpBox(EditorTextSoundPhysics.warningRigidbody3D, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                } else if (physicsDimensions.enumValueIndex == (int)PhysicsDimension._2D) {
                    // 2D
                    if (mTarget.gameObject.GetComponent<Rigidbody2D>() == null) {
                        EditorGUILayout.HelpBox(EditorTextSoundPhysics.warningRigidbody2D, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }
            }

            // Impact /////////////////////////////////////////////////////////////////////////////////////////////////////////
            SerializedProperty impactExpand = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.impactExpand));
            BeginChange();
            EditorGuiFunction.DrawFoldout(impactExpand, EditorTextSoundPhysics.impactHeaderLabel, EditorTextSoundPhysics.impactHeaderTooltip);
            EndChange();

            if (impactExpand.boolValue) {

                // Transparent background so the offset will be right
                StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                SerializedProperty impactPlay = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.impactPlay));
                BeginChange();
                EditorGUILayout.PropertyField(impactPlay, new GUIContent(EditorTextSoundPhysics.impactPlayLabel + is2dname, EditorTextSoundPhysics.impactPlayTooltip));
                EndChange();
                StopBackgroundColor();

                if (impactPlay.boolValue) {
                    for (int i = 0; i < impactSoundPhysicsParts.arraySize; i++) {
                        SerializedProperty part = impactSoundPhysicsParts.GetArrayElementAtIndex(i);

                        BackgroundColorHueOffset();

                        // SoundEvents Impact
                        SerializedProperty soundEvents = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundEvents));
                        int soundEventsLowestArrayLength = int.MaxValue;
                        for (int n = 0; n < mTargets.Length; n++) {
                            if (soundEventsLowestArrayLength > mTargets[n].internals.impactSoundPhysicsParts[i].soundEvents.Length) {
                                soundEventsLowestArrayLength = mTargets[n].internals.impactSoundPhysicsParts[i].soundEvents.Length;
                            }
                        }
                        EditorGuiFunction.DrawReordableArray(soundEvents, serializedObject, soundEventsLowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);

                        // SoundTag
                        if (soundTagOverride.boolValue) {
                            SerializedProperty soundTag = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundTag));
                            BeginChange();
                            EditorGUILayout.PropertyField(soundTag, new GUIContent(EditorTextSoundPhysics.soundTagLabel, EditorTextSoundPhysics.soundTagTooltip));
                            EndChange();
                        }

                        SerializedProperty physicsPlayOn = part.FindPropertyRelative(nameof(SoundPhysicsPart.physicsPlayOn));
                        BeginChange();
                        EditorGUILayout.PropertyField(physicsPlayOn, new GUIContent(EditorTextSoundPhysics.playOnLabel, EditorTextSoundPhysics.playOnTooltip));
                        EndChange();

                        SerializedProperty conditionsUse = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditionsUse));
                        BeginChange();
                        EditorGUILayout.PropertyField(conditionsUse, new GUIContent(EditorTextSoundPhysics.conditionsLabel, EditorTextSoundPhysics.conditionsTooltip));
                        EndChange();

                        if (conditionsUse.boolValue) {
                            SerializedProperty conditionsArray = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditions));
                            int lowestArrayLength = int.MaxValue;
                            for (int n = 0; n < mTargets.Length; n++) {
                                if (lowestArrayLength > mTargets[n].internals.impactSoundPhysicsParts[i].conditions.Length) {
                                    lowestArrayLength = mTargets[n].internals.impactSoundPhysicsParts[i].conditions.Length;
                                }
                            }
                            EditorGuiFunction.DrawReordableArray(conditionsArray, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                        }
                        StopBackgroundColor();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth)); // For offsetting the buttons to the right
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("+", ""))) {
                            impactSoundPhysicsParts.InsertArrayElementAtIndex(i);
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("-", ""))) {
                            if (impactSoundPhysicsParts.arraySize > 1) {
                                impactSoundPhysicsParts.DeleteArrayElementAtIndex(i);
                            }
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("↑", ""))) {
                            impactSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, impactSoundPhysicsParts.arraySize));
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("↓", ""))) {
                            impactSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, impactSoundPhysicsParts.arraySize));
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button("Reset")) {
                            for (int ii = 0; ii < mTargets.Length; ii++) {
                                Undo.RecordObject(mTargets[ii], $"Reset {nameof(NameOf.SoundPhysics)}");
                                if (i < mTargets[ii].internals.impactSoundPhysicsParts.Length && mTargets[ii].internals.impactSoundPhysicsParts[i] != null) {
                                    mTargets[ii].internals.impactSoundPhysicsParts[i] = new SoundPhysicsPart();
                                }
                                EditorUtility.SetDirty(mTargets[ii]);
                            }
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Separator();
                    }
                }
            }

            // Friction /////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (soundPhysicsComponentType.enumValueIndex == (int)SoundPhysicsComponentType.SoundPhysics) {
                SerializedProperty frictionExpand = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.frictionExpand));
                BeginChange();
                EditorGuiFunction.DrawFoldout(frictionExpand, EditorTextSoundPhysics.frictionHeaderLabel, EditorTextSoundPhysics.frictionHeaderTooltip);
                EndChange();

                if (frictionExpand.boolValue) {

                    // Transparent background so the offset will be right
                    StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                    SerializedProperty frictionPlay = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.frictionPlay));
                    BeginChange();
                    EditorGUILayout.PropertyField(frictionPlay, new GUIContent(EditorTextSoundPhysics.frictionPlayLabel + is2dname, EditorTextSoundPhysics.frictionPlayTooltip));
                    EndChange();
                    StopBackgroundColor();

                    if (frictionPlay.boolValue) {
                        for (int i = 0; i < frictionSoundPhysicsParts.arraySize; i++) {
                            SerializedProperty part = frictionSoundPhysicsParts.GetArrayElementAtIndex(i);

                            BackgroundColorHueOffset();

                            // SoundEvents Friction
                            SerializedProperty soundEvents = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundEvents));
                            int soundEventsLowestArrayLength = int.MaxValue;
                            for (int n = 0; n < mTargets.Length; n++) {
                                if (soundEventsLowestArrayLength > mTargets[n].internals.frictionSoundPhysicsParts[i].soundEvents.Length) {
                                    soundEventsLowestArrayLength = mTargets[n].internals.frictionSoundPhysicsParts[i].soundEvents.Length;
                                }
                            }
                            EditorGuiFunction.DrawReordableArray(soundEvents, serializedObject, soundEventsLowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);

                            // SoundTag
                            if (soundTagOverride.boolValue) {
                                SerializedProperty soundTag = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundTag));
                                BeginChange();
                                EditorGUILayout.PropertyField(soundTag, new GUIContent(EditorTextSoundPhysics.soundTagLabel, EditorTextSoundPhysics.soundTagTooltip));
                                EndChange();
                            }

                            SerializedProperty physicsPlayOn = part.FindPropertyRelative(nameof(SoundPhysicsPart.physicsPlayOn));
                            BeginChange();
                            EditorGUILayout.PropertyField(physicsPlayOn, new GUIContent(EditorTextSoundPhysics.playOnLabel, EditorTextSoundPhysics.playOnTooltip));
                            EndChange();

                            SerializedProperty conditionsUse = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditionsUse));
                            BeginChange();
                            EditorGUILayout.PropertyField(conditionsUse, new GUIContent(EditorTextSoundPhysics.conditionsLabel, EditorTextSoundPhysics.conditionsTooltip));
                            EndChange();

                            if (conditionsUse.boolValue) {
                                SerializedProperty conditionsArray = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditions));
                                int lowestArrayLength = int.MaxValue;
                                for (int n = 0; n < mTargets.Length; n++) {
                                    if (lowestArrayLength > mTargets[n].internals.frictionSoundPhysicsParts[i].conditions.Length) {
                                        lowestArrayLength = mTargets[n].internals.frictionSoundPhysicsParts[i].conditions.Length;
                                    }
                                }
                                EditorGuiFunction.DrawReordableArray(conditionsArray, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                            }
                            StopBackgroundColor();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth)); // For offsetting the buttons to the right
                            BeginChange();
                            if (GUILayout.Button(new GUIContent("+", ""))) {
                                frictionSoundPhysicsParts.InsertArrayElementAtIndex(i);
                            }
                            EndChange();
                            BeginChange();
                            if (GUILayout.Button(new GUIContent("-", ""))) {
                                if (frictionSoundPhysicsParts.arraySize > 1) {
                                    frictionSoundPhysicsParts.DeleteArrayElementAtIndex(i);
                                }
                            }
                            EndChange();
                            BeginChange();
                            if (GUILayout.Button(new GUIContent("↑", ""))) {
                                frictionSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, frictionSoundPhysicsParts.arraySize));
                            }
                            EndChange();
                            BeginChange();
                            if (GUILayout.Button(new GUIContent("↓", ""))) {
                                frictionSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, frictionSoundPhysicsParts.arraySize));
                            }
                            EndChange();
                            BeginChange();
                            if (GUILayout.Button("Reset")) {
                                for (int ii = 0; ii < mTargets.Length; ii++) {
                                    Undo.RecordObject(mTargets[ii], $"Reset {nameof(NameOf.SoundPhysics)}");
                                    if (i < mTargets[ii].internals.frictionSoundPhysicsParts.Length && mTargets[ii].internals.frictionSoundPhysicsParts[i] != null) {
                                        mTargets[ii].internals.frictionSoundPhysicsParts[i] = new SoundPhysicsPart();
                                    }
                                    EditorUtility.SetDirty(mTargets[ii]);
                                }
                            }
                            EndChange();
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Separator();
                        }
                    }
                }
            }

            // Exit /////////////////////////////////////////////////////////////////////////////////////////////////////////
            SerializedProperty exitExpand = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.exitExpand));
            BeginChange();
            EditorGuiFunction.DrawFoldout(exitExpand, EditorTextSoundPhysics.exitHeaderLabel, EditorTextSoundPhysics.exitHeaderTooltip);
            EndChange();

            if (exitExpand.boolValue) {

                // Transparent background so the offset will be right
                StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
                SerializedProperty exitPlay = internals.FindPropertyRelative(nameof(SoundPhysicsInternals.exitPlay));
                BeginChange();
                EditorGUILayout.PropertyField(exitPlay, new GUIContent(EditorTextSoundPhysics.exitPlayLabel + is2dname, EditorTextSoundPhysics.exitPlayTooltip));
                EndChange();
                StopBackgroundColor();

                if (exitPlay.boolValue) {
                    for (int i = 0; i < exitSoundPhysicsParts.arraySize; i++) {
                        SerializedProperty part = exitSoundPhysicsParts.GetArrayElementAtIndex(i);

                        BackgroundColorHueOffset();

                        // SoundEvents Exit
                        SerializedProperty soundEvents = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundEvents));
                        int soundEventsLowestArrayLength = int.MaxValue;
                        for (int n = 0; n < mTargets.Length; n++) {
                            if (soundEventsLowestArrayLength > mTargets[n].internals.exitSoundPhysicsParts[i].soundEvents.Length) {
                                soundEventsLowestArrayLength = mTargets[n].internals.exitSoundPhysicsParts[i].soundEvents.Length;
                            }
                        }
                        EditorGuiFunction.DrawReordableArray(soundEvents, serializedObject, soundEventsLowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);

                        // SoundTag
                        if (soundTagOverride.boolValue) {
                            SerializedProperty soundTag = part.FindPropertyRelative(nameof(SoundPhysicsPart.soundTag));
                            BeginChange();
                            EditorGUILayout.PropertyField(soundTag, new GUIContent(EditorTextSoundPhysics.soundTagLabel, EditorTextSoundPhysics.soundTagTooltip));
                            EndChange();
                        }

                        SerializedProperty physicsPlayOn = part.FindPropertyRelative(nameof(SoundPhysicsPart.physicsPlayOn));
                        BeginChange();
                        EditorGUILayout.PropertyField(physicsPlayOn, new GUIContent(EditorTextSoundPhysics.playOnLabel, EditorTextSoundPhysics.playOnTooltip));
                        EndChange();

                        SerializedProperty conditionsUse = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditionsUse));
                        BeginChange();
                        EditorGUILayout.PropertyField(conditionsUse, new GUIContent(EditorTextSoundPhysics.conditionsLabel, EditorTextSoundPhysics.conditionsTooltip));
                        EndChange();

                        if (conditionsUse.boolValue) {
                            SerializedProperty conditionsArray = part.FindPropertyRelative(nameof(SoundPhysicsPart.conditions));
                            int lowestArrayLength = int.MaxValue;
                            for (int n = 0; n < mTargets.Length; n++) {
                                if (lowestArrayLength > mTargets[n].internals.exitSoundPhysicsParts[i].conditions.Length) {
                                    lowestArrayLength = mTargets[n].internals.exitSoundPhysicsParts[i].conditions.Length;
                                }
                            }
                            EditorGuiFunction.DrawReordableArray(conditionsArray, serializedObject, lowestArrayLength, false, false, false, EditorGuiFunction.EditorGuiTypeIs.Property, 0, null, false);
                        }
                        StopBackgroundColor();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth)); // For offsetting the buttons to the right
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("+", ""))) {
                            exitSoundPhysicsParts.InsertArrayElementAtIndex(i);
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("-", ""))) {
                            if (exitSoundPhysicsParts.arraySize > 1) {
                                exitSoundPhysicsParts.DeleteArrayElementAtIndex(i);
                            }
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("↑", ""))) {
                            exitSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, exitSoundPhysicsParts.arraySize));
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button(new GUIContent("↓", ""))) {
                            exitSoundPhysicsParts.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, exitSoundPhysicsParts.arraySize));
                        }
                        EndChange();
                        BeginChange();
                        if (GUILayout.Button("Reset")) {
                            for (int ii = 0; ii < mTargets.Length; ii++) {
                                Undo.RecordObject(mTargets[ii], $"Reset {nameof(NameOf.SoundPhysics)}");
                                if (i < mTargets[ii].internals.exitSoundPhysicsParts.Length && mTargets[ii].internals.exitSoundPhysicsParts[i] != null) {
                                    mTargets[ii].internals.exitSoundPhysicsParts[i] = new SoundPhysicsPart();
                                }
                                EditorUtility.SetDirty(mTargets[ii]);
                            }
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Separator();
                    }
                }
            }

            EditorGUILayout.Separator();
            // Override SoundTag
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f)); // Transparent background so the offset will be right
            BeginChange();
            EditorGUILayout.PropertyField(soundTagOverride, new GUIContent(EditorTextSoundPhysics.soundTagLabel, EditorTextSoundPhysics.soundTagTooltip));
            EndChange();
            StopBackgroundColor();

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            
            // Reset Settings
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundPhysics.resetSettingsLabel, EditorTextSoundPhysics.resetSettingsTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset Settings");

                    // Impact
                    mTargets[i].internals.impactExpand = true;
                    mTargets[i].internals.impactPlay = true;
                    for (int ii = 0; ii < mTargets[i].internals.impactSoundPhysicsParts.Length; ii++) {
                        mTargets[i].internals.impactSoundPhysicsParts[ii].physicsPlayOn = PhysicsPlayOn.OnCollision;
                    }

                    // Friction
                    mTargets[i].internals.frictionExpand = true;
                    mTargets[i].internals.frictionPlay = false;
                    for (int ii = 0; ii < mTargets[i].internals.frictionSoundPhysicsParts.Length; ii++) {
                        mTargets[i].internals.frictionSoundPhysicsParts[ii].physicsPlayOn = PhysicsPlayOn.OnCollision;
                    }

                    // Exit
                    mTargets[i].internals.exitExpand = true;
                    mTargets[i].internals.exitPlay = false;
                    for (int ii = 0; ii < mTargets[i].internals.exitSoundPhysicsParts.Length; ii++) {
                        mTargets[i].internals.exitSoundPhysicsParts[ii].physicsPlayOn = PhysicsPlayOn.OnCollision;
                    }

                    soundTagOverride.boolValue = false;

                    EditorUtility.SetDirty(mTargets[i]);
                }
                EndChange();
            }

            // Reset All
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundPhysics.resetAllLabel, EditorTextSoundPhysics.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");

                    // Impact
                    mTargets[i].internals.impactExpand = true;
                    mTargets[i].internals.impactPlay = true;
                    mTargets[i].internals.impactSoundPhysicsParts = new SoundPhysicsPart[1];
                    mTargets[i].internals.impactSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

                    // Friction
                    mTargets[i].internals.frictionExpand = true;
                    mTargets[i].internals.frictionPlay = false;
                    mTargets[i].internals.frictionSoundPhysicsParts = new SoundPhysicsPart[1];
                    mTargets[i].internals.frictionSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Continuous);

                    // Exit
                    mTargets[i].internals.exitExpand = true;
                    mTargets[i].internals.exitPlay = false;
                    mTargets[i].internals.exitSoundPhysicsParts = new SoundPhysicsPart[1];
                    mTargets[i].internals.exitSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

                    mTargets[i].internals.soundTagOverride = false;

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