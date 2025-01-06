// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundPickerEditorBase : PropertyDrawer {

        private class SoundPickerSerializedProperty {
            public SerializedProperty soundPickerPart;
            public SerializedProperty soundEvent;
            public SerializedProperty soundEventModifier;
            public SerializedProperty expandModifier;
        }

        private SoundPickerBase mTarget;
        private SoundPickerBase[] mTargets;

        private bool initialized = false;

        private SerializedProperty internals;
        private SerializedProperty soundPickerPart;
        private SerializedProperty isEnabled;
        private SerializedProperty expand;

        private List<SoundPickerSerializedProperty> soundPickerSerializedPropertyList = new List<SoundPickerSerializedProperty>();

        private int oldIndentLevel;
        private Rect rectRow;
        private float rowHeight = 18f;
        private float numberOfRows;
        private GUIStyle guiStyleBold = new GUIStyle();
        private GUIStyle guiStyleBoldCenter = new GUIStyle();

        private float xRectPadding = 2f;
        private float yRectPadding = 1f;

        private float modifiersOffset = 1.35f;
        private float backgroundBoxExtendDown = 2f;
        private float backgroundBoxExtendRight = 2f;

        private Color defaultGuiColor;

        private string labelText;

        private void IncrementRow(float scale = 1f) {
            rectRow.y += rowHeight * scale;
            numberOfRows += 1f * scale;
        }

        private void DecrementRow(float scale = 1f) {
            rectRow.y -= rowHeight * scale;
            numberOfRows -= 1f * scale;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            // Overrides the Property Height for the editor to the desired height
            return base.GetPropertyHeight(property, label) + (numberOfRows * rowHeight) + 5f;
        }

        private void FindProperties() {

            // Add SoundPicker Properties
            while (soundPickerSerializedPropertyList.Count < soundPickerPart.arraySize) {
                soundPickerSerializedPropertyList.Add(new SoundPickerSerializedProperty());
            }

            // Remove SoundPicker Properties
            while (soundPickerSerializedPropertyList.Count > soundPickerPart.arraySize) {
                soundPickerSerializedPropertyList.RemoveAt(soundPickerSerializedPropertyList.Count - 1);
            }

            // Update SoundPicker Properties
            for (int i = 0; i < soundPickerSerializedPropertyList.Count; i++) {
                try {
                    soundPickerSerializedPropertyList[i].soundPickerPart = soundPickerPart.GetArrayElementAtIndex(i);
                    soundPickerSerializedPropertyList[i].soundEvent = soundPickerSerializedPropertyList[i].soundPickerPart.FindPropertyRelative(nameof(SoundPickerPart.soundEvent));
                    soundPickerSerializedPropertyList[i].soundEventModifier = soundPickerSerializedPropertyList[i].soundPickerPart.FindPropertyRelative(nameof(SoundPickerPart.soundEventModifier));
                    soundPickerSerializedPropertyList[i].expandModifier = soundPickerSerializedPropertyList[i].soundPickerPart.FindPropertyRelative(nameof(SoundPickerPart.expandModifier));
                } catch {
                    break;
                }
            }
        }

        private void LabelTextRemove(GUIContent label) {
            label.text = " ";
            label.tooltip = "";
        }

        private void LabelTextRestore(GUIContent label) {
            label.text = labelText;
        }

        private void DrawBackgroundColor(Rect position, float yMinSub = 0f, float yMaxAdd = 0f) {
            position.yMin -= yMinSub;
            position.yMax += yMaxAdd;
            position.xMax += backgroundBoxExtendRight;

            // Automatic color increment
            Color color;
            color = GetBackgroundColor();

            // Increase the clarity of the colors for pro skin
            if (EditorGUIUtility.isProSkin) {
                color = EditorColor.ChangeValue(color, 2f);
                color = EditorColor.ChangeSaturation(color, 0.5f);
            } else {
                color = EditorColor.ChangeValue(color, 1f);
                color = EditorColor.ChangeSaturation(color, 0.5f);
            }

            GUI.color = color;
            GUI.Box(position, "");
            // One extra for pro skin (is less visible when proskin)
            if (EditorGUIUtility.isProSkin) {
                GUI.Box(position, "");
            }

            GUI.color = defaultGuiColor;
        }

        private int backgroundColorIncrement = 0;
        private float backgroundColorIncrementScale = 0.05f;

        private void ResetBackgroundColor() {
            backgroundColorIncrement = 0;
        }

        private Color GetBackgroundColor() {
            backgroundColorIncrement++;
            return EditorColor.ChangeHue(EditorColor.GetSettings(EditorColorProSkin.GetCustomPropertyDrawerBackgroundAlpha()), backgroundColorIncrementScale * (backgroundColorIncrement - 1));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            try {
                mTarget = (SoundPickerBase)fieldInfo.GetValue(property.serializedObject.targetObject);
            } catch {
                Debug.LogError($"Sonity: SoundPicker is not nestable in an array or custom class." + "\n" +
                                $"This is because custom serializable classes do not support inheritance and polymorphism for serialization."
                    );
                return;
            }

            mTargets = new SoundPickerBase[property.serializedObject.targetObjects.Length];
            for (int i = 0; i < mTargets.Length; i++) {
                mTargets[i] = (SoundPickerBase)fieldInfo.GetValue(property.serializedObject.targetObjects[i]);
            }

            defaultGuiColor = GUI.color;

            // Reset
            numberOfRows = 0;
            rectRow = position;
            rectRow.height = rowHeight;
            ResetBackgroundColor();

            guiStyleBold.fontStyle = FontStyle.Bold;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBold.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            labelText = label.text;

            oldIndentLevel = EditorGUI.indentLevel;

            if (!initialized || soundPickerPart.arraySize != mTarget.internals.soundPickerPart.Length) {
                initialized = true;
                internals = property.FindPropertyRelative(nameof(SoundPickerBase.internals));
                soundPickerPart = internals.FindPropertyRelative(nameof(SoundPickerInternals.soundPickerPart));
                expand = internals.FindPropertyRelative(nameof(SoundPickerInternals.expand));
                isEnabled = internals.FindPropertyRelative(nameof(SoundPickerInternals.isEnabled));
                FindProperties();
            }

            // Triggered if reset then undo
            if (soundPickerPart.arraySize != soundPickerSerializedPropertyList.Count) {
                FindProperties();
            }

            EditorGUI.indentLevel = 0;

            Rect expandRect = rectRow;
            LabelTextRemove(label);
            label.tooltip = EditorTextSoundPicker.soundPickerTooltip;
            // Make the expand only left side
            expandRect.xMax = EditorGUI.PrefixLabel(rectRow, label).xMin;
            LabelTextRestore(label);

            EditorGUI.BeginChangeCheck();
            bool expandValue = EditorGUI.Foldout(expandRect, expand.boolValue, label);
            if (EditorGUI.EndChangeCheck()) {
                expand.boolValue = expandValue;
            }

            Rect enableRect = rectRow;

            LabelTextRemove(label);
            // Make the foldout only right side
            enableRect.xMin = EditorGUI.PrefixLabel(rectRow, label).xMin;
            LabelTextRestore(label);

            EditorGUI.BeginChangeCheck();
            bool enabledValue = EditorGUI.ToggleLeft(enableRect, new GUIContent("Enabled"), isEnabled.boolValue);
            if (EditorGUI.EndChangeCheck()) {
                isEnabled.boolValue = enabledValue;
            }
            IncrementRow(0.25f);

            if (isEnabled.boolValue) {
                if (expand.boolValue) {
                    IncrementRow();

                    for (int i = 0; i < soundPickerSerializedPropertyList.Count; i++) {

                        // So Reset All wont give errors
                        if (i >= soundPickerSerializedPropertyList.Count || i >= mTarget.internals.soundPickerPart.Length) {
                            FindProperties();
                            break;
                        }

                        // Divider between the array elements
                        if (i > 0) {
                            IncrementRow(1.25f);
                        }

                        EditorGUI.indentLevel = 1;

                        // Resets background color between the SoundEvents
                        ResetBackgroundColor();
                        DrawBackgroundColor(rectRow, backgroundBoxExtendDown, rowHeight * 2.05f + backgroundBoxExtendDown);

                        // Selected SoundEvent
                        if (soundPickerSerializedPropertyList[i].soundEvent == null) {
                            break;
                        }

                        EditorGUI.BeginProperty(position, label, property);
                        EditorGUI.PropertyField(rectRow, soundPickerSerializedPropertyList[i].soundEvent, new GUIContent(EditorTextSoundPicker.soundEventLabel, EditorTextSoundPicker.soundEventTooltip));
                        EditorGUI.EndProperty();
                        IncrementRow();

                        // Rect Preview Left
                        LabelTextRemove(label);
                        Rect previewRectLeft = EditorGUI.PrefixLabel(rectRow, label);
                        previewRectLeft.xMax -= previewRectLeft.width * 0.5f + xRectPadding;
                        previewRectLeft.yMin += yRectPadding;
                        previewRectLeft.yMax -= yRectPadding;

                        // Rect Preview Right
                        Rect previewRectRight = EditorGUI.PrefixLabel(rectRow, label);
                        previewRectRight.xMin += previewRectRight.width * 0.5f + xRectPadding;
                        previewRectRight.yMin += yRectPadding;
                        previewRectRight.yMax -= yRectPadding;
                        LabelTextRestore(label);

                        // Play
                        if (GUI.Button(previewRectLeft, new GUIContent(EditorTextPreview.soundEventBasePlayLabel, EditorTextPreview.soundEventBasePlayTooltip))) {
                            EditorPreviewSound.Stop(false, false);
                            if (mTarget.internals.soundPickerPart[i].soundEvent != null) {
                                for (int ii = 0; ii < mTarget.internals.soundPickerPart[i].soundEvent.internals.soundContainers.Length; ii++) {
                                    EditorPreviewSoundData newPreviewSoundContainerSetting = new EditorPreviewSoundData();
                                    newPreviewSoundContainerSetting.soundContainer = mTarget.internals.soundPickerPart[i].soundEvent.internals.soundContainers[ii];
                                    newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.soundPickerPart[i].soundEvent.internals.data.soundEventModifier);
                                    newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.soundPickerPart[i].soundEventModifier);
                                    // Adding SoundMix and their parents
                                    SoundMixBase tempSoundMix = mTarget.internals.soundPickerPart[i].soundEvent.internals.data.soundMix;
                                    while (tempSoundMix != null && !tempSoundMix.internals.CheckIsInfiniteLoop(tempSoundMix, true)) {
                                        newPreviewSoundContainerSetting.soundEventModifierList.Add(tempSoundMix.internals.soundEventModifier);
                                        tempSoundMix = tempSoundMix.internals.soundMixParent;
                                    }
                                    newPreviewSoundContainerSetting.timelineSoundContainerData = mTarget.internals.soundPickerPart[i].soundEvent.internals.GetTimelineSoundContainerSetting(ii);
                                    EditorPreviewSound.AddEditorPreviewSoundData(newPreviewSoundContainerSetting);
                                }
                                EditorPreviewSound.PlaySoundEvent(mTarget.internals.soundPickerPart[i].soundEvent);
                            }
                        }
                        //Stop all previews
                        if (GUI.Button(previewRectRight, new GUIContent(EditorTextPreview.stopLabel, EditorTextPreview.stopTooltip))) {
                            EditorPreviewSound.Stop(true, true);
                        }
                        IncrementRow();
                        IncrementRow(0.1f);

                        EditorGUI.indentLevel = 1;

                        // Expand Modifiers
                        Rect expandModifiersRect = rectRow;
                        LabelTextRemove(label);
                        // Make the foldout no go over the buttons to the right
                        expandModifiersRect.xMax = EditorGUI.PrefixLabel(rectRow, label).xMin;
                        LabelTextRestore(label);

                        if (EditorSoundEventModifierMenu.ModifierAnyEnabled(soundPickerSerializedPropertyList[i].soundEventModifier)) {
                            soundPickerSerializedPropertyList[i].expandModifier.boolValue = EditorGUI.Foldout(expandModifiersRect, soundPickerSerializedPropertyList[i].expandModifier.boolValue, new GUIContent(EditorTextModifier.modifiersLabel, EditorTextModifier.modifiersTooltip));
                        } else {
                            EditorGUI.LabelField(expandModifiersRect, new GUIContent(EditorTextModifier.modifiersLabel, EditorTextModifier.modifiersTooltip));
                        }

                        LabelTextRemove(label);
                        // Rect SoundEventModifier Left
                        // Halfway to the left
                        Rect rectParameterLeft = EditorGUI.PrefixLabel(rectRow, label);
                        rectParameterLeft.xMax -= rectParameterLeft.width * 0.5f + xRectPadding;
                        rectParameterLeft.yMin += yRectPadding;
                        rectParameterLeft.yMax -= yRectPadding;

                        // Rect SoundEventModifier Right Temp
                        Rect rectParameterRightTemp = EditorGUI.PrefixLabel(rectRow, label);
                        rectParameterRightTemp.xMin += rectParameterRightTemp.width * 0.5f;
                        rectParameterRightTemp.yMin += yRectPadding;
                        rectParameterRightTemp.yMax -= yRectPadding;

                        // Right divided by 2
                        // Rect SoundEventModifier Right A
                        Rect rectParameterRightA = rectParameterRightTemp;
                        rectParameterRightA.xMin += xRectPadding;
                        rectParameterRightA.xMax -= rectParameterRightTemp.width * 0.5f + xRectPadding;

                        // Rect SoundEventModifier Right B
                        Rect rectParameterRightB = rectParameterRightTemp;
                        rectParameterRightB.xMin += rectParameterRightTemp.width * 0.5f + xRectPadding;
                        LabelTextRestore(label);

                        // Getting the soundEventModifiers
                        SoundEventModifier[] soundEventModifiers = new SoundEventModifier[mTargets.Length];
                        for (int ii = 0; ii < mTargets.Length; ii++) {
                            // So that PhysicsSoundPicker reset all does not give an error
                            if (mTargets[ii].internals.soundPickerPart[i] == null) {
                                break;
                            }
                            soundEventModifiers[ii] = mTargets[ii].internals.soundPickerPart[i].soundEventModifier;
                        }
                        // Toggle Menu
                        if (GUI.Button(rectParameterLeft, new GUIContent(EditorTextModifier.addRemoveLabel, EditorTextModifier.addRemoveTooltip))) {
                            EditorSoundEventModifierMenu.ModifierMenuShow(soundEventModifiers, property.serializedObject.targetObjects);
                            soundPickerSerializedPropertyList[i].expandModifier.boolValue = true;
                        }

                        // Reset SoundEventModifier
                        if (GUI.Button(rectParameterRightA, new GUIContent(EditorTextModifier.resetLabel, EditorTextModifier.resetTooltip))) {
                            EditorSoundEventModifierMenu.ModifierReset(soundPickerSerializedPropertyList[i].soundEventModifier);
                            soundPickerSerializedPropertyList[i].expandModifier.boolValue = true;
                        }

                        // Clear SoundEventModifier
                        if (GUI.Button(rectParameterRightB, new GUIContent(EditorTextModifier.clearLabel, EditorTextModifier.clearTooltip))) {
                            EditorSoundEventModifierMenu.ModifierDisableAll(soundPickerSerializedPropertyList[i].soundEventModifier);
                            soundPickerSerializedPropertyList[i].expandModifier.boolValue = true;
                        }

                        IncrementRow(modifiersOffset);

                        if (soundPickerSerializedPropertyList[i].expandModifier.boolValue) {
                            UpdateModifiers(soundPickerSerializedPropertyList[i].soundEventModifier, label, position, property, mTarget.internals.soundPickerPart[i].soundEvent);
                        }

                        // Decrement row for the last modifier
                        DecrementRow();
                    }

                    IncrementRow(1f);

                    LabelTextRemove(label);
                    // Rect Add Event Left
                    Rect rectAddEventLeft = EditorGUI.PrefixLabel(rectRow, label);
                    rectAddEventLeft.xMax -= rectAddEventLeft.width / 1.5f + xRectPadding;
                    rectAddEventLeft.yMin += yRectPadding;
                    rectAddEventLeft.yMax -= yRectPadding;

                    // Rect Add Event Middle
                    Rect rectRemoveEventRight = EditorGUI.PrefixLabel(rectRow, label);
                    rectRemoveEventRight.xMin += rectRemoveEventRight.width / 3f + xRectPadding;
                    rectRemoveEventRight.xMax -= rectRemoveEventRight.width * 0.5f + xRectPadding;
                    rectRemoveEventRight.yMin += yRectPadding;
                    rectRemoveEventRight.yMax -= yRectPadding;

                    // Rect Reset Right
                    Rect resetRect = EditorGUI.PrefixLabel(rectRow, label);
                    resetRect.xMin += resetRect.width / 1.5f + xRectPadding;
                    resetRect.yMin += yRectPadding;
                    resetRect.yMax -= yRectPadding;
                    LabelTextRestore(label);

                    // Add
                    if (GUI.Button(rectAddEventLeft, new GUIContent("+", $"Adds one {nameof(NameOf.SoundEvent)}."))) {
                        soundPickerPart.arraySize++;
                        FindProperties();
                    }

                    // Remove
                    if (GUI.Button(rectRemoveEventRight, new GUIContent("-", $"Removes one {nameof(NameOf.SoundEvent)}."))) {
                        if (soundPickerPart.arraySize > 1) {
                            soundPickerPart.arraySize--;
                            FindProperties();
                        }
                    }

                    // Reset
                    if (GUI.Button(resetRect, new GUIContent("Reset", $"Resets the {nameof(NameOf.SoundPicker)}."))) {
                        initialized = false;
                        Undo.RecordObject(property.serializedObject.targetObject, "Reset");
                        mTarget.internals.soundPickerPart = new SoundPickerPart[1];
                        FindProperties();
                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                    }
                } else {
                    IncrementRow();
                    for (int i = 0; i < soundPickerSerializedPropertyList.Count; i++) {

                        EditorGUI.indentLevel = 1;

                        // Selected SoundEvent
                        if (soundPickerSerializedPropertyList[i].soundEvent == null) {
                            break;
                        }

                        // Start Settings Draw Background Color
                        DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                        
                        EditorGUI.BeginProperty(position, label, property);
                        EditorGUI.PropertyField(rectRow, soundPickerSerializedPropertyList[i].soundEvent, new GUIContent(EditorTextSoundPicker.soundEventLabel, EditorTextSoundPicker.soundEventTooltip));
                        EditorGUI.EndProperty();
                        if (i < soundPickerSerializedPropertyList.Count - 1) {
                            IncrementRow(modifiersOffset); 
                        }
                    }
                }
            }
            //Set indent back to what it was
            EditorGUI.indentLevel = oldIndentLevel;
        }

        private void UpdateModifiers(
            SerializedProperty soundEventModifier, GUIContent label, Rect position, SerializedProperty property, SoundEventBase soundEvent
            ) {

            // Volume
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty volumeDecibel = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeDecibel));
                SerializedProperty volumeRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.volumeRatio));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, volumeDecibel, VolumeScale.lowestVolumeDecibel, 0f, new GUIContent(EditorTextModifier.volumeLabel, EditorTextModifier.volumeTooltip));
                if (volumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                    volumeDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (volumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue)) {
                    volumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(volumeDecibel.floatValue);
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Pitch
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty pitchSemitone = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchSemitone));
                SerializedProperty pitchRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.pitchRatio));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, pitchSemitone, -24f, 24f, new GUIContent(EditorTextModifier.pitchLabel, EditorTextModifier.pitchTooltip));
                if (pitchRatio.floatValue != PitchScale.SemitonesToRatio(pitchSemitone.floatValue)) {
                    pitchRatio.floatValue = PitchScale.SemitonesToRatio(pitchSemitone.floatValue);
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Delay
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.delayUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty delay = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.delay));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, delay, new GUIContent(EditorTextModifier.delayLabel, EditorTextModifier.delayTooltip));
                if (delay.floatValue < 0f) {
                    delay.floatValue = 0f;
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Start Position
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPositionUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty startPosition = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.startPosition));
                EditorGUI.BeginProperty(position, label, property);
                startPosition.floatValue = EditorGUI.Slider(rectRow, new GUIContent(EditorTextModifier.startPositionLabel, EditorTextModifier.startPositionTooltip), startPosition.floatValue, 0f, 1f);
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Reverse
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverseUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty reverse = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverse));
                bool oldReverse = reverse.boolValue;
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, reverse, new GUIContent(EditorTextModifier.reverseEnabledLabel, EditorTextModifier.reverseEnabledTooltip));
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
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Distance Scale
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distanceScaleUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty distanceScale = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distanceScale));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, distanceScale, new GUIContent(EditorTextModifier.distanceScaleLabel, EditorTextModifier.distanceScaleTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
                if (distanceScale.floatValue <= 0f) {
                    distanceScale.floatValue = 0f;
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.distanceScaleWarning), EditorStyles.helpBox);
                    IncrementRow(modifiersOffset);
                }
                // If none of the SoundEvent have distance enabled
                bool distanceEnabled = false;
                if (soundEvent != null) {
                    if (soundEvent.internals.soundContainers != null) {
                        for (int i = 0; i < soundEvent.internals.soundContainers.Length; i++) {
                            if (soundEvent.internals.soundContainers[i] != null) {
                                if (soundEvent.internals.soundContainers[i].internals.data.distanceEnabled) {
                                    distanceEnabled = true;
                                }
                            }
                        }
                    }
                }
                if (!distanceEnabled) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.distanceScaleNotEnabledText, EditorTextModifier.distanceScaleNotEnabledTooltip), EditorStyles.helpBox);
                    IncrementRow(modifiersOffset);
                }
            }
            // Reverb Zone Mix Decibel
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty reverbZoneMixDecibel = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixDecibel));
                SerializedProperty reverbZoneMixRatio = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixRatio));
                EditorGUI.BeginProperty(position, label, property);
                reverbZoneMixDecibel.floatValue = EditorGUI.Slider(rectRow, new GUIContent(EditorTextModifier.reverbZoneMixDecibelLabel, EditorTextModifier.reverbZoneMixDecibelTooltip), reverbZoneMixDecibel.floatValue, VolumeScale.lowestReverbMixDecibel, 0f);
                if (reverbZoneMixDecibel.floatValue <= VolumeScale.lowestReverbMixDecibel) {
                    reverbZoneMixDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (reverbZoneMixRatio.floatValue != VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue)) {
                    reverbZoneMixRatio.floatValue = VolumeScale.ConvertDecibelToRatio(reverbZoneMixDecibel.floatValue);
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Fade In Length
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInLengthUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty fadeInLength = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInLength));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, fadeInLength, new GUIContent(EditorTextModifier.fadeInLengthLabel, EditorTextModifier.fadeInLengthTooltip));
                if (fadeInLength.floatValue < 0f) {
                    fadeInLength.floatValue = 0f;
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Fade In Shape
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInShapeUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty fadeInShape = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeInShape));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, fadeInShape, -16f, 16f, new GUIContent(EditorTextModifier.fadeInShapeLabel, EditorTextModifier.fadeInShapeTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
                if (fadeInShape.floatValue < 0f) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeExponential), EditorStyles.helpBox);
                } else if (fadeInShape.floatValue > 0f) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeLogarithmic), EditorStyles.helpBox);
                } else {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeLinear), EditorStyles.helpBox);
                }
                IncrementRow(modifiersOffset);
            }
            // Fade Out Length
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutLengthUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty fadeOutLength = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutLength));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, fadeOutLength, new GUIContent(EditorTextModifier.fadeOutLengthLabel, EditorTextModifier.fadeOutLengthTooltip));
                if (fadeOutLength.floatValue < 0f) {
                    fadeOutLength.floatValue = 0f;
                }
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Fade Out Shape
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutShapeUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty fadeOutShape = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.fadeOutShape));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, fadeOutShape, -16f, 16f, new GUIContent(EditorTextModifier.fadeOutShapeLabel, EditorTextModifier.fadeOutShapeTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
                if (fadeOutShape.floatValue < 0f) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeExponential), EditorStyles.helpBox);
                } else if (fadeOutShape.floatValue > 0f) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeLogarithmic), EditorStyles.helpBox);
                } else {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.fadeShapeLinear), EditorStyles.helpBox);
                }
                IncrementRow(modifiersOffset);
            }
            // Increase 2D
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.increase2DUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty increase2D = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.increase2D));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, increase2D, 0f, 1f, new GUIContent(EditorTextModifier.increase2DLabel, EditorTextModifier.increase2DTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Stereo Pan
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.stereoPanUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty stereoPan = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.stereoPan));
                EditorGUI.BeginProperty(position, label, property);
                stereoPan.floatValue = EditorGUI.Slider(rectRow, new GUIContent(EditorTextModifier.stereoPanLabel, EditorTextModifier.stereoPanTooltip), stereoPan.floatValue, -1f, 1f);
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Intensity
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.intensityUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty intensity = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.intensity));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, intensity, new GUIContent(EditorTextModifier.intensityLabel, EditorTextModifier.intensityTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
                // If none of the SoundEvent have intensity enabled
                bool intensityEnabled = false;
                if (soundEvent != null) {
                    if (soundEvent.internals.soundContainers != null) {
                        for (int i = 0; i < soundEvent.internals.soundContainers.Length; i++) {
                            if (soundEvent.internals.soundContainers[i] != null) {
                                if (soundEvent.internals.soundContainers[i].internals.data.GetIntensityEnabled()) {
                                    intensityEnabled = true;
                                }
                            }
                        }
                    }
                }
                if (!intensityEnabled) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.intensityNotEnabledText, EditorTextModifier.intensityNotEnabledTooltip), EditorStyles.helpBox);
                    IncrementRow(modifiersOffset);
                }
            }
            // Distortion Increase
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distortionIncreaseUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty distortionIncrease = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.distortionIncrease));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.Slider(rectRow, distortionIncrease, 0, 1, new GUIContent(EditorTextModifier.distortionIncreaseLabel, EditorTextModifier.distortionIncreaseTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
                if (distortionIncrease.floatValue == 1) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.distortionIncreaseWarning), EditorStyles.helpBox);
                    IncrementRow(modifiersOffset);
                }
                // If none of the SoundEvent have distortion enabled
                bool distortionEnabled = false;
                if (soundEvent != null) {
                    if (soundEvent.internals.soundContainers != null) {
                        for (int i = 0; i < soundEvent.internals.soundContainers.Length; i++) {
                            if (soundEvent.internals.soundContainers[i] != null) {
                                if (soundEvent.internals.soundContainers[i].internals.data.distortionEnabled) {
                                    distortionEnabled = true;
                                }
                            }
                        }
                    }
                }
                if (!distortionEnabled) {
                    EditorGUI.LabelField(rectRow, new GUIContent(EditorTextModifier.distortionNotEnabledText, EditorTextModifier.distortionNotEnabledTooltip), EditorStyles.helpBox);
                    IncrementRow(modifiersOffset);
                }
            }
            // Polyphony
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.polyphonyUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty polyphony = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.polyphony));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, polyphony, new GUIContent(EditorTextModifier.polyphonyLabel, EditorTextModifier.polyphonyTooltip));
                EditorGUI.EndProperty();
                if (polyphony.intValue < 1) {
                    polyphony.intValue = 1;
                }
                IncrementRow(modifiersOffset);
            }
            // Follow Position
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.followPositionUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty followPosition = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.followPosition));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, followPosition, new GUIContent(EditorTextModifier.followPositionLabel, EditorTextModifier.followPositionTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }

            // Bypass Reverb Zones
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassReverbZonesUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty bypassReverbZones = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassReverbZones));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, bypassReverbZones, new GUIContent(EditorTextModifier.bypassReverbZonesLabel, EditorTextModifier.bypassReverbZonesTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Bypass Voice Effects
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassVoiceEffectsUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty bypassVoiceEffects = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassVoiceEffects));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, bypassVoiceEffects, new GUIContent(EditorTextModifier.bypassVoiceEffectsLabel, EditorTextModifier.bypassVoiceEffectsTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
            // Bypass Listener Effects
            if (soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassListenerEffectsUse)).boolValue) {
                DrawBackgroundColor(rectRow, backgroundBoxExtendDown, backgroundBoxExtendDown);
                SerializedProperty bypassListenerEffects = soundEventModifier.FindPropertyRelative(nameof(SoundEventModifier.bypassListenerEffects));
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(rectRow, bypassListenerEffects, new GUIContent(EditorTextModifier.bypassListenerEffectsLabel, EditorTextModifier.bypassListenerEffectsTooltip));
                EditorGUI.EndProperty();
                IncrementRow(modifiersOffset);
            }
        }
    }
}
#endif