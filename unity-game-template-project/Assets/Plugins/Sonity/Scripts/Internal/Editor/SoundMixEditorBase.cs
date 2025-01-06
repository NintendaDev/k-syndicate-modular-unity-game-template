// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundMixEditorBase : Editor {

        public SoundMixBase mTarget;
        public SoundMixBase[] mTargets;

        public float pixelsPerIndentLevel = 10f;

        public SerializedProperty assetGuid;
        public SerializedProperty internals;
        public SerializedProperty notes;
        public SerializedProperty soundEventModifier;
        public SerializedProperty soundMixParent;

        public void OnEnable() {
            FindProperties();
        }

        public void FindProperties() {
            assetGuid = serializedObject.FindProperty(nameof(SoundMixBase.assetGuid));
            internals = serializedObject.FindProperty(nameof(SoundMixBase.internals));
            notes = internals.FindPropertyRelative(nameof(SoundMixInternals.notes));
            soundEventModifier = internals.FindPropertyRelative(nameof(SoundMixInternals.soundEventModifier));
            soundMixParent = internals.FindPropertyRelative(nameof(SoundMixInternals.soundMixParent));
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

            mTarget = (SoundMixBase)target;

            mTargets = new SoundMixBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundMixBase)targets[i];
            }

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundMix, EditorTextSoundMix.soundMixTooltip, true);
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

            // SoundMix parent
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            BeginChange();
            EditorGUILayout.PropertyField(soundMixParent, new GUIContent($"Parent {nameof(NameOf.SoundMix)}", $"Nesting {nameof(NameOf.SoundMix)}s is a nice way to for example set up Master and SFX volume controls"));
            EndChange();
            StopBackgroundColor();
            EditorGUILayout.Separator();

            // Check if infinite loop
            if (ShouldDebug.GuiWarnings()) {
                if (mTarget.internals.GetIfInfiniteLoop(mTarget, out SoundMixBase infiniteInstigator, out SoundMixBase infinitePrevious)) {
                    EditorGUILayout.HelpBox("\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", MessageType.Error);
                    EditorGUILayout.Separator();
                }
            }

            // Modifiers
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            // Getting the soundEventModifiers
            SoundEventModifier[] soundEventModifiers = new SoundEventModifier[mTargets.Length];
            for (int i = 0; i < mTargets.Length; i++) {
                soundEventModifiers[i] = mTargets[i].internals.soundEventModifier;
            }
            EditorGUI.indentLevel = 1;
            AddRemoveModifier(EditorTextModifier.modifiersLabel, EditorTextModifier.modifiersTooltip, soundEventModifier, soundEventModifiers);
            if (soundEventModifier.isExpanded) {
                // Modifiers
                EditorGUI.indentLevel = 1;
                UpdateModifier(soundEventModifier);
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();

            EditorGUI.indentLevel = 0;
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));

            // Reset
            BeginChange();
            if (GUILayout.Button("Reset All")) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].internals.soundEventModifier = new SoundEventModifier();
                    mTargets[i].internals.soundMixParent = null;
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

        public void AddRemoveModifier(string label, string tooltip, SerializedProperty modifierProperty, SoundEventModifier[] soundEventModifiers) {

            EditorGUILayout.BeginHorizontal();

            // Extra horizontal for labelWidth
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (EditorSoundEventModifierMenu.ModifierAnyEnabled(modifierProperty)) {
                EditorGuiFunction.DrawFoldout(modifierProperty, label, "", 0, true);
            } else {
                EditorGuiFunction.DrawFoldoutTitle(label);
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
                if (fadeOutShape.floatValue < 0f) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.fadeShapeExponential), EditorStyles.helpBox);
                } else if (fadeOutShape.floatValue > 0f) {
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
    }
}
#endif