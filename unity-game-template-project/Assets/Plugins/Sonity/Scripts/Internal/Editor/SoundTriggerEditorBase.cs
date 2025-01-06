// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Sonity.Internal {

    public abstract class SoundTriggerEditorBase : Editor {

        public SoundTriggerBase mTarget;
        public SoundTriggerBase[] mTargets;

        public SerializedProperty internals;

        public SerializedProperty soundEventsExpand;
        public SerializedProperty soundTriggerPart;

        public SerializedProperty soundParameterDistanceScale;
        public SerializedProperty soundParameterDistanceScaleData;
        public SerializedProperty soundParameterDistanceScaleDataValueFloat;

        private List<SoundEventBase> soundEventList = new List<SoundEventBase>();
        private float maxDistanceLocalScale = 1f;
        private float maxDistanceGlobalScale = 1f;
        private bool soundEventIsAny = false;
        private bool soundEventAnyDistanceEnabled = false;
        private bool resetSoundTriggerPart = false;

        protected virtual void OnEnable() {
            FindProperties();
        }

        private void FindProperties() {
            internals = serializedObject.FindProperty(nameof(SoundTriggerBase.internals));

            soundEventsExpand = internals.FindPropertyRelative(nameof(SoundTriggerInternals.soundEventsExpand));
            soundTriggerPart = internals.FindPropertyRelative(nameof(SoundTriggerInternals.soundTriggerPart));

            soundParameterDistanceScale = internals.FindPropertyRelative(nameof(SoundTriggerInternals.soundParameterDistanceScale));
            soundParameterDistanceScaleData = soundParameterDistanceScale.FindPropertyRelative(nameof(SoundParameterDistanceScale.internals));
            soundParameterDistanceScaleDataValueFloat = soundParameterDistanceScaleData.FindPropertyRelative(nameof(SoundParameterDistanceScale.internals.valueFloat));
        }

        protected void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        protected void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private GUIStyle guiStyleBoldCenter = new GUIStyle();
        private Color defaultGuiColor;

        private void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public void OnSceneGUI() {
            mTarget = (SoundTriggerBase)target;
            if (mTarget != null && mTarget.isActiveAndEnabled && soundEventIsAny) {
                // Radius
                Handles.color = EditorColor.GetDistance(0.5f);
                float handleValue = maxDistanceGlobalScale * mTarget.internals.soundParameterDistanceScale.internals.valueFloat;
                EditorGUI.BeginChangeCheck();
                handleValue = Handles.RadiusHandle(Quaternion.identity, mTarget.transform.position, handleValue, false);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(mTarget, "Changed Radius Handle");
                    // Avoid Divide By 0
                    if (handleValue > 0) {
                        mTarget.internals.soundParameterDistanceScale.internals.valueFloat = handleValue / maxDistanceGlobalScale;
                    }
                }
            }
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundTriggerBase)target;

            mTargets = new SoundTriggerBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundTriggerBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundTrigger, EditorTextSoundTrigger.soundTriggerTooltip, false);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            // 3D
            if (ShouldDebug.GuiWarnings()) {
                bool needsRigidbody = false;
                bool needsCollider = false;
                bool needsRigidbody2D = false;
                bool needsCollider2D = false;
                for (int i = 0; i < mTarget.internals.soundTriggerPart.Length; i++) {
                    // So it doesnt nullref on reset
                    if (mTarget.internals.soundTriggerPart[i] == null) {
                        break;
                    }
                    // Trigger
                    if (mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnterUse || mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExitUse) {
                        needsCollider = true;
                    }
                    // Collision
                    if (mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnterUse || mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExitUse) {
                        needsRigidbody = true;
                        needsCollider = true;
                    }
                    // Trigger 2D
                    if (mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnter2DUse || mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExit2DUse) {
                        needsCollider2D = true;
                    }
                    // Collision 2D
                    if (mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnter2DUse || mTarget.internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExit2DUse) {
                        needsRigidbody2D = true;
                        needsCollider2D = true;
                    }
                }
                if (needsRigidbody && mTarget.gameObject.GetComponent<Rigidbody>() == null) {
                    EditorGUILayout.HelpBox(EditorTextSoundTrigger.warningRigidbody3D, MessageType.Warning);
                    EditorGUILayout.Separator();
                }
                if (needsCollider && mTarget.gameObject.GetComponent<Collider>() == null) {
                    EditorGUILayout.HelpBox(EditorTextSoundTrigger.warningCollider3D, MessageType.Warning);
                    EditorGUILayout.Separator();
                }
                if (needsRigidbody2D && mTarget.gameObject.GetComponent<Rigidbody2D>() == null) {
                    EditorGUILayout.HelpBox(EditorTextSoundTrigger.warningRigidbody2D, MessageType.Warning);
                    EditorGUILayout.Separator();
                }
                if (needsCollider2D && mTarget.gameObject.GetComponent<Collider2D>() == null) {
                    EditorGUILayout.HelpBox(EditorTextSoundTrigger.warningCollider2D, MessageType.Warning);
                    EditorGUILayout.Separator();
                }
            }

            // Distance Scale
            StartBackgroundColor(EditorColor.GetDistance(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            // Updates max distance
            UpdateMaxDistance();
            if (soundEventIsAny) {
                if (soundEventAnyDistanceEnabled) {
                    BeginChange();
                    float scaledFloat = maxDistanceGlobalScale * soundParameterDistanceScaleDataValueFloat.floatValue;
                    scaledFloat = EditorGUILayout.FloatField(new GUIContent(EditorTextSoundTrigger.distanceRadiusLabel, EditorTextSoundTrigger.distanceRadiusTooltip), scaledFloat);
                    if (EditorGUI.EndChangeCheck()) {
                        if (soundParameterDistanceScaleDataValueFloat.floatValue != scaledFloat && scaledFloat > 0f) {
                            soundParameterDistanceScaleDataValueFloat.floatValue = scaledFloat / maxDistanceGlobalScale;
                            serializedObject.ApplyModifiedProperties();
                        }
                    }
                } else {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.radiusHandleWarningNoDistance));
                }
            } else {
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.radiusHandleWarningNoSoundEvents));
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();

            // SoundEvents
            BeginChange();
            EditorGuiFunction.DrawFoldout(soundEventsExpand, "SoundEvents");
            EndChange();

            // SoundEvents
            for (int i = 0; i < soundTriggerPart.arraySize; i++) {
                // Out of bounds check
                bool outOfBoundsPart = false;
                for (int n = 0; n < mTargets.Length; n++) {
                    if (i >= mTargets[n].internals.soundTriggerPart.Length) {
                        outOfBoundsPart = true;
                        break;
                    }
                }
                if (outOfBoundsPart) {
                    break;
                }
                // To avoid reset nullref
                if (mTarget.internals.soundTriggerPart[i] == null) {
                    break;
                }

                StartBackgroundColor(EditorColor.ChangeHue(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()), i * -0.075f));
                
                SerializedProperty part = soundTriggerPart.GetArrayElementAtIndex(i);

                EditorGUI.indentLevel = 0;

                BeginChange();
                SerializedProperty soundEvent = part.FindPropertyRelative(nameof(SoundTriggerPart.soundEvent));
                EditorGUILayout.PropertyField(soundEvent, new GUIContent(EditorTextSoundTrigger.soundEventLabel, EditorTextSoundTrigger.soundEventTooltip));
                EndChange();

                if (!soundEventsExpand.boolValue) {
                    StopBackgroundColor();
                    continue;
                }

                // Modifiers
                // Getting the soundEventModifiers
                SoundEventModifier[] soundEventModifiers = new SoundEventModifier[mTargets.Length];
                for (int ii = 0; ii < mTargets.Length; ii++) {
                    soundEventModifiers[ii] = mTargets[ii].internals.soundTriggerPart[i].soundEventModifier;
                }
                SerializedProperty soundEventModifier = part.FindPropertyRelative(nameof(SoundTriggerPart.soundEventModifier));
                AddRemoveModifier(EditorTextModifier.modifiersLabel, EditorTextModifier.modifiersTooltip, soundEventModifier, soundEventModifiers);
                if (soundEventModifier.isExpanded) {
                    // Modifiers
                    EditorGUI.indentLevel = 1;
                    UpdateModifier(soundEventModifier);
                }

                EditorGUI.indentLevel = 0;

                // Triggers
                // Getting the soundTriggerTodo
                SoundTriggerTodo[] soundTriggerTodos = new SoundTriggerTodo[mTargets.Length];
                for (int ii = 0; ii < mTargets.Length; ii++) {
                    soundTriggerTodos[ii] = mTargets[ii].internals.soundTriggerPart[i].soundTriggerTodo;
                }
                SerializedProperty soundTriggerTodo = part.FindPropertyRelative(nameof(SoundTriggerPart.soundTriggerTodo));
                AddRemovePlayerTrigger("Triggers", soundTriggerTodo, soundTriggerTodos);
                if (soundTriggerTodo.isExpanded) {
                    // Triggers
                    UpdatePlayerTrigger(soundTriggerTodo, part, i);
                }
                StopBackgroundColor();
            }
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (GUILayout.Button("+")) {
                soundTriggerPart.arraySize++;
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button("-")) {
                if (soundTriggerPart.arraySize > 1) {
                    soundTriggerPart.arraySize--;
                }
            }
            EndChange();
            resetSoundTriggerPart = false;
            BeginChange();
            if (GUILayout.Button("Reset")) {
                resetSoundTriggerPart = true;
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], $"Reset {nameof(NameOf.SoundTrigger)}");
                    mTargets[i].internals.soundParameterDistanceScale = new SoundParameterDistanceScale(1f);
                    mTargets[i].internals.soundTriggerPart = new SoundTriggerPart[1];
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
        }

        private float GetLargestValue(float value1, float value2) {
            if (value1 > value2) {
                return value1;
            } else {
                return value2;
            }
        }

        private void UpdateMaxDistance() {
            soundEventIsAny = false;
            soundEventAnyDistanceEnabled = false;
            if (!resetSoundTriggerPart) {
                soundEventList.Clear();
                for (int i = 0; i < mTarget.internals.soundTriggerPart.Length; i++) {
                    if (mTarget.internals.soundTriggerPart[i].soundEvent != null) {
                        soundEventList.Add(mTarget.internals.soundTriggerPart[i].soundEvent);
                        soundEventIsAny = true;
                    }
                }
                if (soundEventList.Count > 0) {
                    // Max Distance
                    float maxRange = 0f;
                    float maxRangeTemp = 0f;
                    for (int i = 0; i < soundEventList.Count; i++) {
                        SoundEventModifier soundEventModifier = new SoundEventModifier();
                        // Get the modifiers from the SoundEvent
                        soundEventModifier.AddValuesTo(soundEventList[i].internals.data.soundEventModifier);
                        // Add the modifiers from the SoundPicker
                        soundEventModifier.AddValuesTo(mTarget.internals.soundTriggerPart[i].soundEventModifier);
                        // Find the max range
                        for (int ii = 0; ii < soundEventList[i].internals.soundContainers.Length; ii++) {
                            if (soundEventList[i].internals.soundContainers[ii] != null && soundEventList[i].internals.soundContainers[ii].internals.data.distanceEnabled) {
                                soundEventAnyDistanceEnabled = true;
                                maxRangeTemp = soundEventList[i].internals.soundContainers[ii].internals.data.distanceScale * soundEventModifier.distanceScale;
                                maxRange = GetLargestValue(maxRange, maxRangeTemp);
                            }
                        }
                    }
                    maxDistanceLocalScale = maxRange;
                    if (SoundManagerBase.Instance == null) {
                        maxDistanceGlobalScale = maxDistanceLocalScale;
                    } else {
                        maxDistanceGlobalScale = maxDistanceLocalScale * SoundManagerBase.Instance.Internals.settings.distanceScale;
                    }
                }
            }
        }

        public void AddRemoveModifier(string label, string tooltip, SerializedProperty modifierProperty, SoundEventModifier[] soundEventModifiers) {

            EditorGUILayout.BeginHorizontal();

            // Extra horizontal for labelWidth (- 16 is one indentation level)
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (EditorSoundEventModifierMenu.ModifierAnyEnabled(modifierProperty)) {
                EditorGuiFunction.DrawFoldout(modifierProperty, label, "", 0, true, true);
            } else {
                EditorGuiFunction.DrawFoldoutTitle(label, "", 0, true);
            }
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Toggle Menu
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.addRemoveLabel, EditorTextModifier.addRemoveTooltip))) {
                EditorSoundEventModifierMenu.ModifierMenuShow(soundEventModifiers, mTargets);
                modifierProperty.isExpanded = true;
            }
            EndChange();
            // Reset
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.resetLabel, EditorTextModifier.resetTooltip))) {
                EditorSoundEventModifierMenu.ModifierReset(modifierProperty);
                modifierProperty.isExpanded = true;
            }
            EndChange();
            // Clear
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.clearLabel, EditorTextModifier.clearTooltip))) {
                EditorSoundEventModifierMenu.ModifierDisableAll(modifierProperty);
                modifierProperty.isExpanded = true;
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
        }

        private void UpdateModifier(SerializedProperty soundEventModifier) {
            // Volume
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeUse)).boolValue) {
                SerializedProperty volumeDecibel = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeDecibel));
                SerializedProperty volumeRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeRatio));
                BeginChange();
                EditorGUILayout.Slider(volumeDecibel, VolumeScale.lowestVolumeDecibel, 0f, new GUIContent(EditorTextModifier.volumeLabel, EditorTextModifier.volumeTooltip));
                if (volumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                    volumeDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (volumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue)) {
                    volumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue);
                }
                EndChange();
            }
            // Pitch
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchUse)).boolValue) {
                SerializedProperty pitchSemitone = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchSemitone));
                SerializedProperty pitchRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchRatio));
                BeginChange();
                EditorGUILayout.Slider(pitchSemitone, -24f, 24f, new GUIContent(EditorTextModifier.pitchLabel, EditorTextModifier.pitchTooltip));
                if (pitchRatio.floatValue != PitchScale.SemitonesToRatio(pitchSemitone.floatValue)) {
                    pitchRatio.floatValue = PitchScale.SemitonesToRatio(pitchSemitone.floatValue);
                }
                EndChange();
            }
            // Delay
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.delayUse)).boolValue) {
                SerializedProperty delay = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.delay));
                BeginChange();
                EditorGUILayout.PropertyField(delay, new GUIContent(EditorTextModifier.delayLabel, EditorTextModifier.delayTooltip));
                if (delay.floatValue < 0f) {
                    delay.floatValue = 0f;
                }
                EndChange();
            }
            // Start Position
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPositionUse)).boolValue) {
                SerializedProperty startPosition = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPosition));
                BeginChange();
                EditorGUILayout.Slider(startPosition, 0f, 1f, new GUIContent(EditorTextModifier.startPositionLabel, EditorTextModifier.startPositionTooltip));
                EndChange();
            }
            // Reverse
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverseUse)).boolValue) {
                SerializedProperty reverse = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverse));
                BeginChange();
                bool oldReverse = reverse.boolValue;
                EditorGUILayout.PropertyField(reverse, new GUIContent(EditorTextModifier.reverseEnabledLabel, EditorTextModifier.reverseEnabledTooltip));
                // Enable start postion also
                if (oldReverse != reverse.boolValue) {
                    SerializedProperty startPosition = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPosition));
                    SerializedProperty startPositionUse = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPositionUse));
                    if (reverse.boolValue) {
                        startPositionUse.boolValue = true;
                    }
                    // Invert if changed
                    startPosition.floatValue = 1f - startPosition.floatValue;
                }
                EndChange();
            }
            // Distance Scale
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distanceScaleUse)).boolValue) {
                SerializedProperty distanceScale = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distanceScale));
                BeginChange();
                EditorGUILayout.PropertyField(distanceScale, new GUIContent(EditorTextModifier.distanceScaleLabel, EditorTextModifier.distanceScaleTooltip));
                if (distanceScale.floatValue <= 0f) {
                    distanceScale.floatValue = 0f;
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.distanceScaleWarning), EditorStyles.helpBox);
                }
                EndChange();
            }
            // Reverb Zone Mix Decibel
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixUse)).boolValue) {
                SerializedProperty reverbZoneMixDecibel = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixDecibel));
                SerializedProperty reverbZoneMixRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixRatio));
                BeginChange();
                EditorGUILayout.Slider(reverbZoneMixDecibel, VolumeScale.lowestReverbMixDecibel, 0f, new GUIContent(EditorTextModifier.reverbZoneMixDecibelLabel, EditorTextModifier.reverbZoneMixDecibelTooltip));
                if (reverbZoneMixDecibel.floatValue <= VolumeScale.lowestReverbMixDecibel) {
                    reverbZoneMixDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (reverbZoneMixRatio.floatValue != VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue)) {
                    reverbZoneMixRatio.floatValue = VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue);
                }
                EndChange();
            }
            // Fade In Length
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInLengthUse)).boolValue) {
                SerializedProperty fadeInLength = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInLength));
                BeginChange();
                EditorGUILayout.PropertyField(fadeInLength, new GUIContent(EditorTextModifier.fadeInLengthLabel, EditorTextModifier.fadeInLengthTooltip));
                if (fadeInLength.floatValue < 0f) {
                    fadeInLength.floatValue = 0f;
                }
                EndChange();
            }
            // Fade In Shape
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInShapeUse)).boolValue) {
                SerializedProperty fadeInShape = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInShape));
                BeginChange();
                EditorGUILayout.Slider(fadeInShape, -16f, 16f, new GUIContent(EditorTextModifier.fadeInShapeLabel, EditorTextModifier.fadeInShapeTooltip));
                EndChange();
                if (fadeInShape.floatValue < 0f) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeExponential), EditorStyles.helpBox);
                } else if (fadeInShape.floatValue > 0f) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeLogarithmic), EditorStyles.helpBox);
                } else {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeLinear), EditorStyles.helpBox);
                }
            }
            // Fade Out Length
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutLengthUse)).boolValue) {
                SerializedProperty fadeOutLength = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutLength));
                BeginChange();
                EditorGUILayout.PropertyField(fadeOutLength, new GUIContent(EditorTextModifier.fadeOutLengthLabel, EditorTextModifier.fadeOutLengthTooltip));
                if (fadeOutLength.floatValue < 0f) {
                    fadeOutLength.floatValue = 0f;
                }
                EndChange();
            }
            // Fade Out Shape
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutShapeUse)).boolValue) {
                SerializedProperty fadeOutShape = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutShape));
                BeginChange();
                EditorGUILayout.Slider(fadeOutShape, -16f, 16f, new GUIContent(EditorTextModifier.fadeOutShapeLabel, EditorTextModifier.fadeOutShapeTooltip));
                EndChange();
                if (fadeOutShape.floatValue < 0) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeExponential), EditorStyles.helpBox);
                } else if (fadeOutShape.floatValue > 0) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeLogarithmic), EditorStyles.helpBox);
                } else {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeLinear), EditorStyles.helpBox);
                }
            }
            // Increase 2D
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.increase2DUse)).boolValue) {
                SerializedProperty increase2D = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.increase2D));
                BeginChange();
                EditorGUILayout.Slider(increase2D, 0f, 1f, new GUIContent(EditorTextModifier.increase2DLabel, EditorTextModifier.increase2DTooltip));
                EndChange();
            }
            // Stereo Pan
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.stereoPanUse)).boolValue) {
                SerializedProperty stereoPan = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.stereoPan));
                BeginChange();
                EditorGUILayout.Slider(stereoPan, -1f, 1f, new GUIContent(EditorTextModifier.stereoPanLabel, EditorTextModifier.stereoPanTooltip));
                EndChange();
            }
            // Intensity
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.intensityUse)).boolValue) {
                SerializedProperty intensity = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.intensity));
                BeginChange();
                EditorGUILayout.PropertyField(intensity, new GUIContent(EditorTextModifier.intensityLabel, EditorTextModifier.intensityTooltip));
                EndChange();
            }
            // Distortion Increase
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distortionIncreaseUse)).boolValue) {
                SerializedProperty distortionIncrease = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distortionIncrease));
                BeginChange();
                EditorGUILayout.Slider(distortionIncrease, 0f, 1f, new GUIContent(EditorTextModifier.distortionIncreaseLabel, EditorTextModifier.distortionIncreaseTooltip));
                EndChange();
                if (distortionIncrease.floatValue == 1) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.distortionIncreaseWarning), EditorStyles.helpBox);
                }
            }
            // Polyphony
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.polyphonyUse)).boolValue) {
                SerializedProperty polyphony = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.polyphony));
                BeginChange();
                EditorGUILayout.PropertyField(polyphony, new GUIContent(EditorTextModifier.polyphonyLabel, EditorTextModifier.polyphonyTooltip));
                if (polyphony.intValue < 1) {
                    polyphony.intValue = 1;
                }
                EndChange();
            }
            // Follow Position
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.followPositionUse)).boolValue) {
                SerializedProperty followPosition = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.followPosition));
                BeginChange();
                EditorGUILayout.PropertyField(followPosition, new GUIContent(EditorTextModifier.followPositionLabel, EditorTextModifier.followPositionTooltip));
                EndChange();
            }
            // Bypass Reverb Zones
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassReverbZonesUse)).boolValue) {
                SerializedProperty bypassReverbZones = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassReverbZones));
                BeginChange();
                EditorGUILayout.PropertyField(bypassReverbZones, new GUIContent(EditorTextModifier.bypassReverbZonesLabel, EditorTextModifier.bypassReverbZonesTooltip));
                EndChange();
            }
            // Bypass Voice Effects
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassVoiceEffectsUse)).boolValue) {
                SerializedProperty bypassVoiceEffects = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassVoiceEffects));
                BeginChange();
                EditorGUILayout.PropertyField(bypassVoiceEffects, new GUIContent(EditorTextModifier.bypassVoiceEffectsLabel, EditorTextModifier.bypassVoiceEffectsTooltip));
                EndChange();
            }
            // Bypass Listener Effects
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassListenerEffectsUse)).boolValue) {
                SerializedProperty bypassListenerEffects = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassListenerEffects));
                BeginChange();
                EditorGUILayout.PropertyField(bypassListenerEffects, new GUIContent(EditorTextModifier.bypassListenerEffectsLabel, EditorTextModifier.bypassListenerEffectsTooltip));
                EndChange();
            }
        }

        public void AddRemovePlayerTrigger(string label, SerializedProperty playerTriggerProperty, SoundTriggerTodo[] playerTriggers) {

            EditorGUILayout.BeginHorizontal();
            // Extra horizontal for labelWidth (- 16 is one indentation level)
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (EditorSoundTriggerTypeMenu.PlayerTriggerTypeAnyEnabled(playerTriggerProperty)) {
                EditorGuiFunction.DrawFoldout(playerTriggerProperty, label, "", 0, true, true);
            } else {
                EditorGuiFunction.DrawFoldoutTitle(label, "", 0, true);
            }
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Toggle Menu
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.addRemoveLabel, EditorTextModifier.addRemoveTooltip))) {
                EditorSoundTriggerTypeMenu.TriggerTypeMenuShow(playerTriggers, mTargets);
                playerTriggerProperty.isExpanded = true;
            }
            EndChange();

            // Reset
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.resetLabel, EditorTextModifier.resetTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], $"Reset {nameof(NameOf.SoundTrigger)}");
                    for (int ii = 0; ii < mTargets[i].internals.soundTriggerPart.Length; ii++) {
                        mTargets[i].internals.soundTriggerPart[ii].ResetTodo();
                    }
                    EditorUtility.SetDirty(mTargets[i]);
                }
                playerTriggerProperty.isExpanded = true;
            }
            EndChange();

            // Clear
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextModifier.clearLabel, EditorTextModifier.clearTooltip))) {
                EditorSoundTriggerTypeMenu.TriggerTypeDisableAll(playerTriggerProperty);
                playerTriggerProperty.isExpanded = true;
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
        }

        private void UpdatePlayerTrigger(SerializedProperty soundTriggerTodo, SerializedProperty part, int i) {

            // Basic
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onEnableUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDisableUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onStartUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDestroyUse)).boolValue
                ) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.onBasicLabel, EditorTextSoundTrigger.onBasicTooltip), EditorStyles.boldLabel);
            }
            EditorGUI.indentLevel = 2;
            // OnEnable
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onEnableUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onEnableAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onEnableLabel, EditorTextSoundTrigger.onEnableTooltip));
                EndChange();
            }
            // OnDisable
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDisableUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDisableAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onDisableLabel, EditorTextSoundTrigger.onDisableTooltip));
                EndChange();
            }
            // OnStart
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onStartUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onStartAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onStartLabel, EditorTextSoundTrigger.onStartTooltip));
                EndChange();
            }
            // OnDestroy
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDestroyUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onDestroyAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onDestroyLabel, EditorTextSoundTrigger.onDestroyTooltip));
                EndChange();
            }

            // Trigger
            bool triggerAnyEnabled = false;
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnterUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExitUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnter2DUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExit2DUse)).boolValue) {
                triggerAnyEnabled = true;
            }
            if (triggerAnyEnabled) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.onTriggerLabel, EditorTextSoundTrigger.onTriggerTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel = 2;
            }
            EditorGUI.indentLevel = 2;
            // OnTriggerEnter
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnterUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnterAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onTriggerEnterLabel, EditorTextSoundTrigger.onTriggerEnterTooltip));
                EndChange();
            }
            // OnTriggerExit
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExitUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExitAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onTriggerExitLabel, EditorTextSoundTrigger.onTriggerExitTooltip));
                EndChange();
            }
            // OnTriggerEnter2D
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnter2DUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerEnter2DAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onTriggerEnter2DLabel, EditorTextSoundTrigger.onTriggerEnter2DTooltip));
                EndChange();
            }
            // OnTriggerExit2D
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExit2DUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onTriggerExit2DAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onTriggerExit2DLabel, EditorTextSoundTrigger.onTriggerExit2DTooltip));
                EndChange();
            }
            if (triggerAnyEnabled) {
                // Trigger Tag
                BeginChange();
                SerializedProperty tagUse = part.FindPropertyRelative(nameof(SoundTriggerPart.triggerTagUse));
                EditorGUILayout.PropertyField(tagUse, new GUIContent(EditorTextSoundTrigger.triggerTagLabel, EditorTextSoundTrigger.triggerTagTooltip));
                EndChange();
                
                if (tagUse.boolValue) {
                    SerializedProperty tags = part.FindPropertyRelative(nameof(SoundTriggerPart.triggerTags));

                    // Tags
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.soundTriggerPart[i].triggerTags.Length) {
                            lowestArrayLength = mTargets[n].internals.soundTriggerPart[i].triggerTags.Length;
                        }
                    }
                    EditorGUI.indentLevel = 2;
                    EditorGuiFunction.DrawReordableArray(tags, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Tag, 0);
                }
            }

            // Collision
            bool collisionAnyEnabled = false;
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnterUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExitUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnter2DUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExit2DUse)).boolValue
                ) {
                collisionAnyEnabled = true;
            }
            if (collisionAnyEnabled) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.onCollisionLabel, EditorTextSoundTrigger.onCollisionTooltip), EditorStyles.boldLabel);
            }
            EditorGUI.indentLevel = 2;
            // OnCollisionEnter
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnterUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnterAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onCollisionEnterLabel, EditorTextSoundTrigger.onCollisionEnterTooltip));
                EndChange();
            }
            // OnCollisionExit
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExitUse)).boolValue) {
                SerializedProperty PlayerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExitAction));
                BeginChange();
                EditorGUILayout.PropertyField(PlayerTriggerAction, new GUIContent(EditorTextSoundTrigger.onCollisionExitLabel, EditorTextSoundTrigger.onCollisionExitTooltip));
                EndChange();
            }
            // OnCollisionEnter2D
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnter2DUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionEnter2DAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onCollisionEnter2DLabel, EditorTextSoundTrigger.onCollisionEnter2DTooltip));
                EndChange();
            }
            // OnCollisionExit2D
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExit2DUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onCollisionExit2DAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onCollisionExit2DLabel, EditorTextSoundTrigger.onCollisionExit2DTooltip));
                EndChange();
            }
            if (collisionAnyEnabled) {
                EditorGUI.indentLevel = 2;

                SerializedProperty collisionVelocityToIntensity = part.FindPropertyRelative(nameof(SoundTriggerPart.collisionVelocityToIntensity));
                BeginChange();
                EditorGUILayout.PropertyField(collisionVelocityToIntensity, new GUIContent(EditorTextSoundTrigger.velocityToIntensityLabel, EditorTextSoundTrigger.velocityToIntensityTooltip));
                EndChange();

                EditorGUI.indentLevel = 2;

                // Collision Tag
                BeginChange();
                SerializedProperty tagUse = part.FindPropertyRelative(nameof(SoundTriggerPart.collisionTagUse));

                EditorGUILayout.PropertyField(tagUse, new GUIContent(EditorTextSoundTrigger.collisionTagLabel, EditorTextSoundTrigger.collisionTagTooltip));
                EndChange();
                if (tagUse.boolValue) {
                    SerializedProperty tags = part.FindPropertyRelative(nameof(SoundTriggerPart.collisionTags));

                    // Tags
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.soundTriggerPart[i].collisionTags.Length) {
                            lowestArrayLength = mTargets[n].internals.soundTriggerPart[i].collisionTags.Length;
                        }
                    }
                    EditorGUI.indentLevel = 2;
                    EditorGuiFunction.DrawReordableArray(tags, serializedObject, lowestArrayLength, true, false, false, EditorGuiFunction.EditorGuiTypeIs.Tag, 0);
                }
            }

            // Mouse
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseEnterUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseExitUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseDownUse)).boolValue ||
                soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseUpUse)).boolValue
                ) {
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundTrigger.onMouseLabel, EditorTextSoundTrigger.onMouseTooltip), EditorStyles.boldLabel);
            }
            EditorGUI.indentLevel = 2;
            // On Mouse Enter
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseEnterUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseEnterAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onMouseEnterLabel, EditorTextSoundTrigger.onMouseEnterTooltip));
                EndChange();
            }
            // On Mouse Exit
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseExitUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseExitAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onMouseExitLabel, EditorTextSoundTrigger.onMouseExitTooltip));
                EndChange();
            }
            // On Mouse Down
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseDownUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseDownAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onMouseDownLabel, EditorTextSoundTrigger.onMouseDownTooltip));
                EndChange();
            }
            // On Mouse Up
            if (soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseUpUse)).boolValue) {
                SerializedProperty playerTriggerAction = soundTriggerTodo.FindPropertyRelative(nameof(SoundTriggerTodo.onMouseUpAction));
                BeginChange();
                EditorGUILayout.PropertyField(playerTriggerAction, new GUIContent(EditorTextSoundTrigger.onMouseUpLabel, EditorTextSoundTrigger.onMouseUpTooltip));
                EndChange();
            }
        }
    }
}
#endif