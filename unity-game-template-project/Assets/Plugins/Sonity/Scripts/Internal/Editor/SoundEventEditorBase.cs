// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public abstract class SoundEventEditorBase : Editor {

        public SoundEventBase mTarget;
        public SoundEventBase[] mTargets;

        public EditorPreviewControls previewEditorSetting = new EditorPreviewControls();
        public SoundEventEditorIntensityDebugDraw intensityDebugDraw;

        public float pixelsPerIndentLevel = 10f;
        public float guiCurveHeight = 25f;

        public SerializedProperty foundReferences;

        public SerializedProperty assetGuid;

        // SoundContainer
        public SerializedProperty soundContainers;

        public SerializedProperty timelineSoundContainerSetting;

        public SerializedProperty internals;
        public SerializedProperty data;

        public SerializedProperty notes;

        // Expand
        public SerializedProperty expandSoundContainers;
        public SerializedProperty expandTimeline;
        public SerializedProperty expandPreview;
        public SerializedProperty expandAllSoundTag;

        public SerializedProperty previewAudioMixerGroup;

        public SerializedProperty disableEnable;
        // Mute & Solo
        public SerializedProperty muteEnable;
        public SerializedProperty soloEnable;

        public SerializedProperty baseExpand;
        public SerializedProperty volumeDecibel;
        public SerializedProperty volumeRatio;
        public SerializedProperty polyphony;

        // SoundEvent Modifier
        public SerializedProperty soundEventModifier;

        // Setting
        public SerializedProperty settingsExpandBase;
        public SerializedProperty settingsExpandAdvanced;
        public SerializedProperty polyphonyMode;
        public SerializedProperty audioMixerGroup;
        public SerializedProperty soundVolumeGroup;
        public SerializedProperty soundMix;
        public SerializedProperty soundPolyGroup;
        public SerializedProperty soundPolyGroupPriority;
        public SerializedProperty cooldownTime;
        public SerializedProperty probability;
        public SerializedProperty passParameters;
        public SerializedProperty ignoreLocalPause;
        public SerializedProperty ignoreGlobalPause;

        // Debug
        public SerializedProperty debugExpand;
        public SerializedProperty debugLogSoundEventShow;
        public SerializedProperty debugDrawSoundEventShow;
        public SerializedProperty debugDrawSoundEventStyleOverride;
        public SerializedProperty debugDrawSoundEventFontSize;
        public SerializedProperty debugDrawSoundEventOpacityMultiplier;
        public SerializedProperty debugDrawSoundEventColorStart;
        public SerializedProperty debugDrawSoundEventColorEnd;
        public SerializedProperty debugDrawSoundEventColorOutline;

        // Intensity
        public SerializedProperty expandIntensity;
        public SerializedProperty intensityAdd;
        public SerializedProperty intensityMultiplier;
        public SerializedProperty intensityRolloff;
        public SerializedProperty intensitySeekTime;
        public SerializedProperty intensityCurve;
        public SerializedProperty intensityThresholdEnable;
        public SerializedProperty intensityThreshold;
        public SerializedProperty intensityScaleAdd;
        public SerializedProperty intensityScaleMultiplier;
        public SerializedProperty intensityRecord;
        public SerializedProperty intensityDebugResolution;
        public SerializedProperty intensityDebugZoom;
        public SerializedProperty intensityDebugValueList;

        // Trigger On
        public SerializedProperty triggerOnExpand;

        // TriggerOnPlay
        public SerializedProperty triggerOnPlayEnable;
        public SerializedProperty triggerOnPlaySoundEvents;
        public SerializedProperty triggerOnPlayWhichToPlay;

        // TriggerOnStop
        public SerializedProperty expandTriggerOnStop;
        public SerializedProperty triggerOnStopEnable;
        public SerializedProperty triggerOnStopSoundEvents;
        public SerializedProperty triggerOnStopWhichToPlay;

        // TriggerOnTail
        public SerializedProperty expandTriggerOnTail;
        public SerializedProperty triggerOnTailEnable;
        public SerializedProperty triggerOnTailSoundEvents;
        public SerializedProperty triggerOnTailWhichToPlay;
        public SerializedProperty triggerOnTailLength;
        public SerializedProperty triggerOnTailBpm;
        public SerializedProperty triggerOnTailBeatLength;

        // SoundTag
        public SerializedProperty soundTagEnable;
        public SerializedProperty soundTagMode;
        public SerializedProperty soundTagGroups;

        // The material to use when drawing with OpenGL
        public Material cachedMaterial;

        [NonSerialized]
        public bool initialized;
        public bool resetZoomAndHorizontal;

        public SoundEventEditorFindAssets updateSoundContainers;
        public SoundEventEditorTimelineData soundEventEditorTimelineData;
        public SoundEventEditorPreview soundEventEditorPreview;
        public SoundEventEditorTimeline soundEventEditorTimeline;
        public SoundEventEditorTimelineDraw soundEventEditorTimelineDraw;

        public void Initialize() {
            if (!initialized) {
                initialized = true;
                soundEventEditorTimelineData = new SoundEventEditorTimelineData();
                soundEventEditorPreview = CreateInstance<SoundEventEditorPreview>();
                soundEventEditorPreview.Initialize(this, soundEventEditorTimelineData);
                soundEventEditorTimeline = CreateInstance<SoundEventEditorTimeline>();
                soundEventEditorTimeline.Initialize(this, soundEventEditorTimelineData);
                soundEventEditorTimelineDraw = CreateInstance<SoundEventEditorTimelineDraw>();
                soundEventEditorTimelineDraw.Initialize(this, soundEventEditorTimelineData);
                updateSoundContainers = CreateInstance<SoundEventEditorFindAssets>();
                updateSoundContainers.Initialize(this);
                intensityDebugDraw = CreateInstance<SoundEventEditorIntensityDebugDraw>();
                intensityDebugDraw.Initialize(this);
            }
        }
        
        public void OnEnable() {
            FindProperties();
            // Cache the "Hidden/Internal-Colored" shader
            cachedMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        public void FindProperties() {
            assetGuid = serializedObject.FindProperty(nameof(SoundEventBase.assetGuid));

            internals = serializedObject.FindProperty(nameof(SoundEventBase.internals));

            notes = internals.FindPropertyRelative(nameof(SoundEventBase.internals.notes));

            soundContainers = internals.FindPropertyRelative(nameof(SoundEventBase.internals.soundContainers));

            data = internals.FindPropertyRelative(nameof(SoundEventBase.internals.data));

            timelineSoundContainerSetting = data.FindPropertyRelative(nameof(SoundEventInternalsData.timelineSoundContainerData));

            soundEventModifier = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundEventModifier));

            foundReferences = data.FindPropertyRelative(nameof(SoundEventInternalsData.foundReferences));

            expandSoundContainers = data.FindPropertyRelative(nameof(SoundEventInternalsData.expandSoundContainers));
            expandTimeline = data.FindPropertyRelative(nameof(SoundEventInternalsData.expandTimeline));
            expandPreview = data.FindPropertyRelative(nameof(SoundEventInternalsData.expandPreview));
            expandAllSoundTag = data.FindPropertyRelative(nameof(SoundEventInternalsData.expandAllSoundTag));

            previewAudioMixerGroup = data.FindPropertyRelative(nameof(SoundEventInternalsData.previewAudioMixerGroup));

            disableEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.disableEnable));
            muteEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.muteEnable));
            soloEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.soloEnable));

            // Base
            baseExpand = data.FindPropertyRelative(nameof(SoundEventInternalsData.baseExpand));
            volumeDecibel = data.FindPropertyRelative(nameof(SoundEventInternalsData.volumeDecibel));
            volumeRatio = data.FindPropertyRelative(nameof(SoundEventInternalsData.volumeRatio));
            polyphony = data.FindPropertyRelative(nameof(SoundEventInternalsData.polyphony));

            // Settings
            settingsExpandBase = data.FindPropertyRelative(nameof(SoundEventInternalsData.settingsExpandBase));
            settingsExpandAdvanced = data.FindPropertyRelative(nameof(SoundEventInternalsData.settingsExpandAdvanced));
            polyphonyMode = data.FindPropertyRelative(nameof(SoundEventInternalsData.polyphonyMode));
            audioMixerGroup = data.FindPropertyRelative(nameof(SoundEventInternalsData.audioMixerGroup));
            soundVolumeGroup = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundVolumeGroup));
            soundMix = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundMix));
            soundPolyGroup = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundPolyGroup));
            soundPolyGroupPriority = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundPolyGroupPriority));
            cooldownTime = data.FindPropertyRelative(nameof(SoundEventInternalsData.cooldownTime));
            probability = data.FindPropertyRelative(nameof(SoundEventInternalsData.probability));
            passParameters = data.FindPropertyRelative(nameof(SoundEventInternalsData.passParameters));
            ignoreLocalPause = data.FindPropertyRelative(nameof(SoundEventInternalsData.ignoreLocalPause));
            ignoreGlobalPause = data.FindPropertyRelative(nameof(SoundEventInternalsData.ignoreGlobalPause));

            // Debug
            debugExpand = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugExpand));
            debugLogSoundEventShow = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugLogShow));
            debugDrawSoundEventShow = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawShow));
            debugDrawSoundEventStyleOverride = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventStyleOverride));
            debugDrawSoundEventFontSize = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventFontSizeMultiplier));
            debugDrawSoundEventOpacityMultiplier = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventOpacityMultiplier));
            debugDrawSoundEventColorStart = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventColorStart));
            debugDrawSoundEventColorEnd = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventColorEnd));
            debugDrawSoundEventColorOutline = data.FindPropertyRelative(nameof(SoundEventInternalsData.debugDrawSoundEventColorOutline));

            // Intensity
            expandIntensity =  data.FindPropertyRelative(nameof(SoundEventInternalsData.expandIntensity));
            intensityAdd =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityAdd));
            intensityMultiplier =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityMultiplier));
            intensityRolloff =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityRolloff));
            intensitySeekTime =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensitySeekTime));
            intensityCurve =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityCurve));
            intensityThresholdEnable =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityThresholdEnable));
            intensityThreshold =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityThreshold));
            intensityScaleAdd = data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityScaleAdd));
            intensityScaleMultiplier = data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityScaleMultiplier));
            intensityRecord =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityRecord));
            intensityDebugResolution =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityDebugResolution));
            intensityDebugZoom = data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityDebugZoom));
            intensityDebugValueList =  data.FindPropertyRelative(nameof(SoundEventInternalsData.intensityDebugValueList));

            triggerOnExpand = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnExpand));

            triggerOnPlayEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnPlayEnable));
            triggerOnPlaySoundEvents = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnPlaySoundEvents));
            triggerOnPlayWhichToPlay = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnPlayWhichToPlay));

            triggerOnStopEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnStopEnable));
            triggerOnStopSoundEvents = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnStopSoundEvents));
            triggerOnStopWhichToPlay = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnStopWhichToPlay));

            triggerOnTailEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailEnable));
            triggerOnTailSoundEvents = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailSoundEvents));
            triggerOnTailWhichToPlay = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailWhichToPlay));
            triggerOnTailLength = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailLength));
            triggerOnTailBpm = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailBpm));
            triggerOnTailBeatLength = data.FindPropertyRelative(nameof(SoundEventInternalsData.triggerOnTailBeatLength));

            soundTagEnable = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundTagEnable));
            soundTagMode = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundTagMode));
            soundTagGroups = data.FindPropertyRelative(nameof(SoundEventInternalsData.soundTagGroups));
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

            mTarget = (SoundEventBase)target;

            mTargets = new SoundEventBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundEventBase)targets[i];
            }

            Initialize();

            defaultGuiColor = GUI.color;

            guiStyleBoldCenter.fontSize = 16;
            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundEvent, EditorTextSoundEvent.soundEventTooltip, true);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            GuiNotes();
            GuiPresetsMenu();
            GuiMuteSoloDisable();
            GuiPreview();
            GuiSoundContainers();
            GuiTimeline();
            GuiBase();
            GuiModifiers();
            GuiSettings();
            GuiIntensity();
            GuiTriggerOther();
            GuiSoundTag();
            GuiDebug();
            GuiReset();
            GuiFindReferences();

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

        private void GuiNotes() {
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
        }

        private void GuiMuteSoloDisable() {
            EditorGUI.indentLevel = 0;
            // Transparent background so the offset will be right
            if (muteEnable.boolValue) {
                // Red
                StartBackgroundColor(new Color(1f, 0f, 0f, 1f));
            } else if (soloEnable.boolValue) {
                // Yellow
                StartBackgroundColor(new Color(1f, 1f, 0f, 1f));
            } else if (disableEnable.boolValue) {
                // Magenta
                StartBackgroundColor(new Color(0.5f, 0f, 1f, 1f));
            } else {
                // Transparent
                StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            }
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < mTargets.Length; i++) {
                if (mTargets[i].internals.data.soloEnable && SoundManagerBase.Instance != null && !SoundManagerBase.Instance.Internals.GetIsInSolo(mTarget)) {
                    mTargets[i].internals.data.soloEnable = false;
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }

            // For offsetting the buttons to the right
            if (muteEnable.boolValue) {
                EditorGUILayout.LabelField(new GUIContent("Muted"), GUILayout.Width(EditorGUIUtility.labelWidth));
            } else if (soloEnable.boolValue) {
                if (SoundManagerBase.Instance == null) {
                    EditorGUILayout.LabelField(new GUIContent($"Needs {nameof(NameOf.SoundManager)} to Solo"), GUILayout.Width(EditorGUIUtility.labelWidth));
                } else {
                    EditorGUILayout.LabelField(new GUIContent("Solo"), GUILayout.Width(EditorGUIUtility.labelWidth));
                }
            } else if (disableEnable.boolValue) {
                EditorGUILayout.LabelField(new GUIContent("Disabled"), GUILayout.Width(EditorGUIUtility.labelWidth));
            } else {
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            }
            // Mute
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.muteEnableLabel, EditorTextSoundEvent.muteEnableTooltip))) {
                muteEnable.boolValue = !muteEnable.boolValue;
                soloEnable.boolValue = false;
                disableEnable.boolValue = false;
            }
            EndChange();
            // Solo
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.soloEnableLabel, EditorTextSoundEvent.soloEnableTooltip))) {
                if (!soloEnable.boolValue) {
                    if (SoundManagerBase.Instance != null) {
                        SoundManagerBase.Instance.Internals.AddSolo(mTarget);
                    }
                    soloEnable.boolValue = true;
                    muteEnable.boolValue = false;
                    disableEnable.boolValue = false;
                } else {
                    soloEnable.boolValue = false;
                    muteEnable.boolValue = false;
                    disableEnable.boolValue = false;
                }
            }
            EndChange();
            // Disable
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.disableEnableLabel, EditorTextSoundEvent.disableEnableTooltip))) {
                disableEnable.boolValue = !disableEnable.boolValue;
                muteEnable.boolValue = false;
                soloEnable.boolValue = false;
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiPresetsMenu() {
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.presetsLabel, EditorTextSoundEvent.presetsTooltip))) {
                PresetsMenuDraw();
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private enum PresetType {
            PresetSame = 0,
            PresetMatchName = 1,
        }

        private class PresetsMenuObject {
            public PresetType presetType;
            public SoundEventBase soundEventPreset;

            public PresetsMenuObject(PresetType presetType, SoundEventBase soundEventPreset = null) {
                this.presetType = presetType;
                this.soundEventPreset = soundEventPreset;
            }
        }

        private void PresetsMenuDraw() {
            GenericMenu menu = new GenericMenu();

            bool anyMenuItemAdded = false;

            // Tooltips dont work for menu
            SoundPresetFind.FindAllSoundPresets();
            if (SoundPresetFind.soundPresets != null && SoundPresetFind.soundPresets.Length > 0) {
                for (int i = 0; i < SoundPresetFind.soundPresets.Length; i++) {
                    SoundPresetBase soundPreset = SoundPresetFind.soundPresets[i];
                    if (soundPreset != null && !soundPreset.internals.disableAll) {
                        for (int ii = 0; ii < SoundPresetFind.soundPresets[i].internals.soundPresetGroup.Length; ii++) {
                            SoundPresetGroup soundPresetGroup = SoundPresetFind.soundPresets[i].internals.soundPresetGroup[ii];
                            if (soundPresetGroup != null && !soundPresetGroup.disable && soundPresetGroup.soundEventPreset != null) {
                                anyMenuItemAdded = true;
                                string parentName = "Preset - " + soundPresetGroup.soundEventPreset.name;
                                menu.AddItem(new GUIContent(parentName), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.PresetSame, soundPresetGroup.soundEventPreset));
                            }
                        }
                    }
                }
                if (anyMenuItemAdded) {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Preset - Auto Match"), false, PresetsMenuCallback, new PresetsMenuObject(PresetType.PresetMatchName));
                } else {
                    menu.AddItem(new GUIContent($"All {NameOf.SoundPreset}s are disabled or empty"), false, PresetsMenuCallback, null);
                }
            } else {
                menu.AddItem(new GUIContent($"Make custom presets using the {NameOf.SoundPreset} object"), false, PresetsMenuCallback, null);
            }

            menu.ShowAsContext();
        }

        private void PresetsMenuCallback(object obj) {
            try {
                PresetsMenuObject menuObject = (PresetsMenuObject)obj;
                // If updating this, update the tooltip and documentation also
                if (menuObject == null) {
                    return;
                }
                if (menuObject.presetType == PresetType.PresetSame) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Set to Preset Settings");
                        SoundEventCopy.CopyTo(mTargets[i], menuObject.soundEventPreset);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                } else if (menuObject.presetType == PresetType.PresetMatchName) {
                    SoundPresetFind.FindAllSoundPresets();
                    if (SoundPresetFind.soundPresets != null) {
                        for (int i = 0; i < SoundPresetFind.soundPresets.Length; i++) {
                            SoundPresetBase soundPreset = SoundPresetFind.soundPresets[i];
                            if (soundPreset != null && !soundPreset.internals.disableAll) {
                                for (int ii = 0; ii < soundPreset.internals.soundPresetGroup.Length; ii++) {
                                    SoundPresetGroup soundPresetGroup = soundPreset.internals.soundPresetGroup[ii];
                                    if (soundPresetGroup != null && !soundPresetGroup.ShouldUseMatch(false)) {
                                        for (int iii = 0; iii < mTargets.Length; iii++) {
                                            if (soundPresetGroup.GetNameMatches(mTargets[iii].name, false)) {
                                                Undo.RecordObject(mTargets[iii], "Set to Preset Settings");
                                                SoundEventCopy.CopyTo(mTargets[iii], soundPresetGroup.soundEventPreset);
                                                Debug.Log($"Sonity: Preset \"" + soundPresetGroup.soundEventPreset.name + $"\" is applied to \"" + mTargets[iii].name + "\"", mTargets[iii]);
                                                EditorUtility.SetDirty(mTargets[iii]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch {
                return;
            }
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

        private static string RemoveTrailingJunk(string input, bool removeNumbers = true) {
            char[] charsToRemove;
            if (removeNumbers) {
                charsToRemove = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };
            } else {
                charsToRemove = new char[] { ' ', '_', '-' };
            }
            return input.TrimEnd(charsToRemove);
        }

        private void GuiPreview() {
            soundEventEditorPreview.PreviewDraw();
            EditorGUILayout.Separator();
            StartBackgroundColor(EditorColor.GetSoundEvent(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
        }

        private void GuiSoundContainers() {
            // Extra horizontal for labelWidth
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            EditorGuiFunction.DrawFoldout(expandSoundContainers, $"{nameof(NameOf.SoundContainer)}s");
            EndChange();
            EditorGUILayout.EndHorizontal();

            if (expandSoundContainers.boolValue) {

                EditorGUI.indentLevel = 1;

                // Menu for updating SoundContainers
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.findSoundContainersLabel, EditorTextSoundEvent.findSoundContainersTooltip))) {
                    updateSoundContainers.MenuFindAsset();
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // SoundContainer find lowest array length in multi selection
                int lowestArrayLength = int.MaxValue;
                for (int n = 0; n < mTargets.Length; n++) {
                    if (lowestArrayLength > mTargets[n].internals.soundContainers.Length) {
                        lowestArrayLength = mTargets[n].internals.soundContainers.Length;
                    }
                }

                EditorGuiFunction.DrawReordableArray(soundContainers, serializedObject, lowestArrayLength, false, true, true, EditorGuiFunction.EditorGuiTypeIs.Property, 0, timelineSoundContainerSetting);

                // SoundContainer Drag and Drop Area
                EditorDragAndDropArea.DrawDragAndDropAreaCustomEditor<SoundContainerBase>(new EditorDragAndDropArea.DragAndDropAreaInfo($"{nameof(NameOf.SoundContainer)}"), DragAndDropCallback);
            }

            if (ShouldDebug.GuiWarnings()) {
                // Waring if null/empty SoundContainers
                if (mTarget.internals.soundContainers.Length == 0) {
                    // Dont warn if trigger on play is used
                    if (!mTarget.internals.data.triggerOnPlayEnable || mTarget.internals.data.triggerOnPlaySoundEvents.Length == 0) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundEvent.soundContainerWarningEmpty, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                } else {
                    bool soundContainersNull = false;
                    for (int i = 0; i < mTarget.internals.soundContainers.Length; i++) {
                        if (mTarget.internals.soundContainers[i] == null) {
                            soundContainersNull = true;
                            break;
                        }
                    }
                    if (soundContainersNull) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundEvent.soundContainerWarningNull, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void DragAndDropCallback<T>(T[] draggedObjects) where T : UnityEngine.Object {
            SoundContainerBase[] newObjects = draggedObjects as SoundContainerBase[];
            // If there are any objects of the right type dragged
            for (int i = 0; i < mTargets.Length; i++) {
                Undo.RecordObject(mTargets[i], $"Drag and Dropped {nameof(NameOf.SoundContainer)}");
                mTargets[i].internals.soundContainers = new SoundContainerBase[newObjects.Length];
                for (int ii = 0; ii < newObjects.Length; ii++) {
                    mTargets[i].internals.soundContainers[ii] = newObjects[ii];
                }
                // Expands the SoundContainer array
                soundContainers.isExpanded = true;
                EditorUtility.SetDirty(mTargets[i]);
            }
        }

        private void GuiTimeline() {
            // Resize Timeline SoundContainer Array
            if (Event.current.type == EventType.Layout) {
                // Multiple Objects eg mTargets does not work with this. Use mTarget
                BeginChange();
                if (mTarget.internals.data.timelineSoundContainerData.Length != mTarget.internals.soundContainers.Length) {
                    Array.Resize(ref mTarget.internals.data.timelineSoundContainerData, mTarget.internals.soundContainers.Length);
                    for (int i = 0; i < mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                        if (mTarget.internals.data.timelineSoundContainerData[i] == null) {
                            mTarget.internals.data.timelineSoundContainerData[i] = new SoundEventTimelineData();
                        }
                    }
                    EditorUtility.SetDirty(mTarget);
                }
                EndChange();
                // If null events then reset parameters
                // Multiple Objects
                BeginChange();
                for (int i = 0; i < mTarget.internals.soundContainers.Length; i++) {
                    if (mTarget.internals.soundContainers[i] == null) {
                        mTarget.internals.data.timelineSoundContainerData[i] = new SoundEventTimelineData();
                        EditorUtility.SetDirty(mTarget);
                    }
                }
                EndChange();
            }

            // Timeline Expand
            StartBackgroundColor(EditorColor.GetSoundEvent(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            EditorGUI.indentLevel = 1;

            EditorGUILayout.BeginHorizontal();

            // Extra horizontal for labelWidth
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            EditorGuiFunction.DrawFoldout(expandTimeline, EditorTextSoundEvent.timelineExpandLabel, EditorTextSoundEvent.timelineExpandTooltip);
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Reset Timeline Data
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.timelineResetLabel, EditorTextSoundEvent.timelineResetTooltip))) {
                // Timeline Reset Data
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset Timeline Data");
                    for (int ii = 0; ii < mTargets[i].internals.data.timelineSoundContainerData.Length; ii++) {
                        mTargets[i].internals.data.timelineSoundContainerData[ii].volumeDecibel = 0f;
                        mTargets[i].internals.data.timelineSoundContainerData[ii].volumeRatio = 1f;
                        mTargets[i].internals.data.timelineSoundContainerData[ii].delay = 0f;
                    }
                    EditorUtility.SetDirty(mTargets[i]);
                }
                // Timeline Reset Zoom and Horizontal
                resetZoomAndHorizontal = true;
            }
            EndChange();
            // Focus On Items
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.timelineFocusLabel, EditorTextSoundEvent.timelineFocusTooltip))) {
                // Timeline Reset Zoom and Horizontal
                resetZoomAndHorizontal = true;
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();

            // Timeline Draw
            if (expandTimeline.boolValue && mTarget.internals.data.timelineSoundContainerData.Length > 0) {
                // Is sound is playing, so to refresh the timeline position
                if (EditorPreviewSound.IsUpdating()) {
                    Repaint();
                }
                soundEventEditorTimeline.TimelineInteraction();
                soundEventEditorTimelineDraw.Draw();
            }
            EditorGUILayout.Separator();

            // Focus On Items
            BeginChange();
            // Timeline Reset Zoom and Horizontal when Repaint so that soundEventEditorTimelineData.layoutRectangle.width wont be 1
            if (resetZoomAndHorizontal && Event.current.type == EventType.Repaint) {
                resetZoomAndHorizontal = false;
                soundEventEditorTimeline.ResetZoomAndHorizontal();
            }
            EndChange();
        }

        private void GuiBase() {
            StartBackgroundColor(EditorColor.GetSoundEvent(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;

            BeginChange();
            EditorGuiFunction.DrawFoldout(baseExpand, EditorTextSoundEvent.baseLabel, EditorTextSoundEvent.baseTooltip);
            EndChange();

            if (baseExpand.boolValue) {

                // Polyphony
                BeginChange();
                EditorGUILayout.PropertyField(polyphony, new GUIContent(EditorTextSoundEvent.polyphonyLabel, EditorTextSoundEvent.polyphonyTooltip));
                if (polyphony.intValue < 1) {
                    polyphony.intValue = 1;
                }
                EndChange();

                // Volume
                BeginChange();
                EditorGUILayout.Slider(volumeDecibel, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease24dbMaxDecibel, new GUIContent(EditorTextSoundEvent.volumeLabel, EditorTextSoundEvent.volumeTooltip));
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
                        if (minVolumeDecibel > mTargets[i].internals.data.volumeDecibel) {
                            minVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
                        }
                    }
                    if (minVolumeDecibel < VolumeScale.lowestVolumeDecibel) {
                        minVolumeName = " (Min " + "-Infinity" + ")";
                    } else {
                        minVolumeName = " (Min " + Mathf.FloorToInt(minVolumeDecibel) + ")";
                    }
                }
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.volumeRelativeLowerLabel + minVolumeName, EditorTextSoundEvent.volumeRelativeLowerTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Volume -1 dB");
                        // Don't clamp max when lowering volume so you always can lower it
                        mTargets[i].internals.data.volumeDecibel = Mathf.Clamp(mTargets[i].internals.data.volumeDecibel - 1f, VolumeScale.lowestVolumeDecibel, Mathf.Infinity);
                        mTargets[i].internals.data.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.data.volumeDecibel);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();

                string maxVolumeName = "";
                if (mTargets.Length > 1) {
                    // Finds the highest volume in dB
                    float maxVolumeDecibel = Mathf.NegativeInfinity;
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (maxVolumeDecibel < mTargets[i].internals.data.volumeDecibel) {
                            maxVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
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
                if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.volumeRelativeIncreaseLabel + maxVolumeName, EditorTextSoundEvent.volumeRelativeIncreaseTooltip))) {
                    // Finds the highest volume in dB
                    float maxVolumeDecibel = Mathf.NegativeInfinity;
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (maxVolumeDecibel < mTargets[i].internals.data.volumeDecibel) {
                            maxVolumeDecibel = mTargets[i].internals.data.volumeDecibel;
                        }
                    }
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Volume +1 dB");
                        mTargets[i].internals.data.volumeDecibel = Mathf.Clamp(mTargets[i].internals.data.volumeDecibel + 1f, VolumeScale.lowestVolumeDecibel, VolumeScale.volumeIncrease24dbMaxDecibel);
                        mTargets[i].internals.data.volumeRatio = VolumeScale.ConvertDecibelToRatio(mTargets[i].internals.data.volumeDecibel);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // Warning if volume is over 0
                for (int i = 0; i < mTargets.Length; i++) {
                    if (mTargets[i].internals.data.volumeRatio > (VolumeScale.volumeIncrease24dbMaxRatio + 0.00001)) {
                        EditorGUILayout.HelpBox(EditorTextSoundEvent.volumeOverLimitWarning, MessageType.Warning);
                        break;
                    }
                }
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiModifiers() {
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            // Getting the soundEventModifiers
            SoundEventModifier[] soundEventModifiers = new SoundEventModifier[mTargets.Length];
            for (int i = 0; i < mTargets.Length; i++) {
                soundEventModifiers[i] = mTargets[i].internals.data.soundEventModifier;
            }
            EditorGUI.indentLevel = 1;
            AddRemoveModifier(EditorTextModifier.modifiersLabel, EditorTextModifier.modifiersTooltip, soundEventModifier, soundEventModifiers, 0, false);
            if (soundEventModifier.isExpanded) {
                // Modifiers
                EditorGUI.indentLevel = 1;
                UpdateModifier(soundEventModifier);
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();

            EditorGUI.indentLevel = 1;
        }

        public void AddRemoveModifier(string label, string tooltip, SerializedProperty modifierProperty, SoundEventModifier[] soundEventModifiers, int indentLevel, bool smallFont) {

            EditorGUILayout.BeginHorizontal();
            // Extra horizontal for labelWidth
            if (smallFont) {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth - 15));
            } else {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            }
            BeginChange();
            if (EditorSoundEventModifierMenu.ModifierAnyEnabled(modifierProperty)) {
                EditorGuiFunction.DrawFoldout(modifierProperty, label, tooltip, indentLevel, true, smallFont);
            } else {
                EditorGuiFunction.DrawFoldoutTitle(label, tooltip, indentLevel, smallFont);
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
                // If none of the SoundEvent have distance enabled
                bool distanceEnabled = false;
                for (int i = 0; i < mTarget.internals.soundContainers.Length; i++) {
                    if (mTarget.internals.soundContainers[i] != null) {
                        if (mTarget.internals.soundContainers[i].internals.data.distanceEnabled) {
                            distanceEnabled = true;
                        }
                    }
                }
                if (!distanceEnabled) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.distanceScaleNotEnabledText, EditorTextModifier.distanceScaleNotEnabledTooltip), EditorStyles.helpBox);
                }
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
                // If none of the SoundContainers have intensity enabled
                bool intensityEnabled = false;
                for (int i = 0; i < mTarget.internals.soundContainers.Length; i++) {
                    if (mTarget.internals.soundContainers[i] != null) {
                        if (mTarget.internals.soundContainers[i].internals.data.GetIntensityEnabled()) {
                            intensityEnabled = true;
                        }
                    }
                }
                if (!intensityEnabled) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.intensityNotEnabledText, EditorTextModifier.intensityNotEnabledTooltip), EditorStyles.helpBox);
                }
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
                // If none of the SoundContainers have distortion enabled
                bool distortionEnabled = false;
                for (int i = 0; i < mTarget.internals.soundContainers.Length; i++) {
                    if (mTarget.internals.soundContainers[i] != null) {
                        if (mTarget.internals.soundContainers[i].internals.data.distortionEnabled) {
                            distortionEnabled = true;
                        }
                    }
                }
                if (!distortionEnabled) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextModifier.distortionNotEnabledText, EditorTextModifier.distortionNotEnabledTooltip), EditorStyles.helpBox);
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

        private void GuiSettings() {
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));

            BeginChange();
            EditorGuiFunction.DrawFoldout(settingsExpandBase, "Settings");
            EndChange();

            EditorGUILayout.EndHorizontal();
            if (settingsExpandBase.boolValue) {

                // Polyphony Mode
                BeginChange();
                EditorGUILayout.PropertyField(polyphonyMode, new GUIContent(EditorTextSoundEvent.polyphonyModeLabel, EditorTextSoundEvent.polyphonyModeTooltip));
                EndChange();

                // AudioMixerGroup
                BeginChange();
                EditorGUILayout.ObjectField(audioMixerGroup, new GUIContent(EditorTextSoundEvent.audioMixerGroupLabel, EditorTextSoundEvent.audioMixerGroupTooltip));
                EndChange();

                // SoundVolumeGroup
                BeginChange();
                EditorGUILayout.PropertyField(soundVolumeGroup, new GUIContent(EditorTextSoundEvent.soundVolumeGroupLabel, EditorTextSoundEvent.soundVolumeGroupTooltip));
                EndChange();

                // Advanced
                BeginChange();
                EditorGuiFunction.DrawFoldout(settingsExpandAdvanced, "Advanced", "", 0, false, false, true);
                EndChange();

                if (settingsExpandAdvanced.boolValue) {

                    // SoundMix
                    BeginChange();
                    EditorGUILayout.PropertyField(soundMix, new GUIContent(EditorTextSoundEvent.soundMixLabel, EditorTextSoundEvent.soundMixTooltip));
                    EndChange();

                    // SoundPolyGroup
                    BeginChange();
                    EditorGUILayout.PropertyField(soundPolyGroup, new GUIContent(EditorTextSoundEvent.soundPolyGroupLabel, EditorTextSoundEvent.soundPolyGroupTooltip));
                    EndChange();
                    EditorGUI.BeginDisabledGroup(soundPolyGroup.objectReferenceValue == null);
                    EditorGUI.indentLevel++;
                    // SoundPolyGroup Priority
                    BeginChange();
                    EditorGUILayout.Slider(soundPolyGroupPriority, 0f, 1f, new GUIContent(EditorTextSoundEvent.soundPolyGroupPriorityLabel, EditorTextSoundEvent.soundPolyGroupPriorityTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                    EditorGUI.EndDisabledGroup();

                    // Cooldown time
                    BeginChange();
                    EditorGUILayout.PropertyField(cooldownTime, new GUIContent(EditorTextSoundEvent.cooldownTimeLabel, EditorTextSoundEvent.cooldownTimeTooltip));
                    if (cooldownTime.floatValue < 0f) {
                        cooldownTime.floatValue = 0f;
                    }
                    EndChange();

                    BeginChange();
                    float previousProbability = probability.floatValue;
                    EditorGUILayout.Slider(probability, 0f, 100f, new GUIContent(EditorTextSoundEvent.probabilityLabel, EditorTextSoundEvent.probabilityTooltip));
                    if (probability.floatValue == 0f) {
                        probability.floatValue = previousProbability;
                    }
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(passParameters, new GUIContent(EditorTextSoundEvent.passParametersLabel, EditorTextSoundEvent.passParametersTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(ignoreLocalPause, new GUIContent(EditorTextSoundEvent.ignoreLocalPauseLabel, EditorTextSoundEvent.ignoreLocalPauseTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(ignoreGlobalPause, new GUIContent(EditorTextSoundEvent.ignoreGlobalPauseLabel, EditorTextSoundEvent.ignoreGlobalPauseTooltip));
                    EndChange();
                }
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiIntensity() {
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            // Intensity Settings
            BeginChange();
            EditorGuiFunction.DrawFoldout(expandIntensity, EditorTextSoundEvent.intensityFoldoutLabel, EditorTextSoundEvent.intensityFoldoutTooltip, 0);
            EndChange();

            if (expandIntensity.boolValue) {

                // Intensity Add
                BeginChange();
                EditorGUILayout.PropertyField(intensityAdd, new GUIContent(EditorTextSoundEvent.intensityAddLabel, EditorTextSoundEvent.intensityAddTooltip));
                EndChange();

                // Intensity Multiplier
                BeginChange();
                EditorGUILayout.PropertyField(intensityMultiplier, new GUIContent(EditorTextSoundEvent.intensityMultiplierLabel, EditorTextSoundEvent.intensityMultiplierTooltip));
                EndChange();

                // Intensity Rolloff
                BeginChange();
                EditorGUILayout.Slider(intensityRolloff, -LogLinExp.bipolarRange, LogLinExp.bipolarRange, new GUIContent(EditorTextSoundEvent.intensityRolloffLabel, EditorTextSoundEvent.intensityRolloffTooltip));
                EndChange();

                // Intensity Curve
                BeginChange();
                EditorGUILayout.CurveField(intensityCurve, EditorColor.GetIntensityMax(1f), new Rect(0f, 0f, 1f, 1f), new GUIContent(EditorTextSoundEvent.intensityCurveLabel, EditorTextSoundEvent.intensityCurveTooltip), GUILayout.Height(guiCurveHeight));
                EndChange();

                // Intensity Smoothing
                BeginChange();
                EditorGUILayout.PropertyField(intensitySeekTime, new GUIContent(EditorTextSoundEvent.intensitySeekTimeLabel, EditorTextSoundEvent.intensitySeekTimeTooltip));
                intensitySeekTime.floatValue = Mathf.Clamp(intensitySeekTime.floatValue, 0f, Mathf.Infinity);
                EndChange();

                // Intensity Threshold Enable
                BeginChange();
                EditorGUILayout.PropertyField(intensityThresholdEnable, new GUIContent(EditorTextSoundEvent.intensityThresholdEnableLabel, EditorTextSoundEvent.intensityThresholdEnableTooltip));
                EndChange();

                // Intensity Threshold
                if (intensityThresholdEnable.boolValue) {
                    BeginChange();
                    intensityThreshold.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundEvent.intensityThresholdLabel, EditorTextSoundEvent.intensityThresholdTooltip), intensityThreshold.floatValue, 0f, 1f);
                    EndChange();
                }

                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent("Reset Intensity Settings", ""))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Intensity Settings");
                        mTargets[i].internals.data.intensityMultiplier = 1f;
                        mTargets[i].internals.data.intensityAdd = 0f;
                        mTargets[i].internals.data.intensityRolloff = 0f;
                        mTargets[i].internals.data.intensitySeekTime = 0f;
                        mTargets[i].internals.data.intensityCurve = AnimationCurve.Linear(0, 0, 1, 1);
                        mTargets[i].internals.data.intensityThresholdEnable = false;
                        mTargets[i].internals.data.intensityThreshold = 0f;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // Intensity Scale Add
                BeginChange();
                EditorGUILayout.PropertyField(intensityScaleAdd, new GUIContent(EditorTextSoundEvent.intensityScaleAddLabel, EditorTextSoundEvent.intensityScaleAddTooltip));
                EndChange();

                // Intensity Scale Multiplier
                BeginChange();
                EditorGUILayout.PropertyField(intensityScaleMultiplier, new GUIContent(EditorTextSoundEvent.intensityScaleMultiplierLabel, EditorTextSoundEvent.intensityScaleMultiplierTooltip));
                EndChange();

                // Intensity Record
                BeginChange();
                EditorGUILayout.PropertyField(intensityRecord, new GUIContent(EditorTextSoundEvent.intensityDebugLabel, EditorTextSoundEvent.intensityDebugTooltip));
                EndChange();

                if (!Application.isPlaying) {
                    EditorGUILayout.LabelField(new GUIContent(EditorTextSoundEvent.intensityDebugRecordLabel, EditorTextSoundEvent.intensityDebugRecordTooltip), EditorStyles.helpBox);
                }

                EditorGUILayout.Separator();
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField(new GUIContent("Recorded Intensity", ""), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                intensityDebugDraw.Draw();

                // How many values are recorded
                EditorGUILayout.LabelField(new GUIContent((mTarget.internals.data.intensityDebugValueList.Count + " " + EditorTextSoundEvent.intensityValuesRecordedLabel), EditorTextSoundEvent.intensityValuesRecordedTooltip), EditorStyles.helpBox);

                // Zoom Vertical
                BeginChange();
                intensityDebugZoom.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundEvent.intensityDebugZoomLabel, EditorTextSoundEvent.intensityDebugZoomTooltip), intensityDebugZoom.floatValue, 0f, 1f);
                EndChange();

                // Debug Resolution
                BeginChange();
                EditorGUILayout.PropertyField(intensityDebugResolution, new GUIContent(EditorTextSoundEvent.intensityDebugResolutionLabel, EditorTextSoundEvent.intensityDebugResolutionTooltip));
                if (intensityDebugResolution.intValue < 3) {
                    intensityDebugResolution.intValue = 3;
                }
                EndChange();

                // Intensity Debug Scale Max
                EditorGUILayout.BeginHorizontal();
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.intensityDebugScaleMaxLabel, EditorTextSoundEvent.intensityDebugScaleMaxTooltip), GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f))) {
                    // Remade to use mTargets in case SerializedProperties are breaking if they're added between refreshing SP and pressing the button
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (mTargets[i].internals.data.intensityDebugValueList.Count > 0) {
                            Undo.RecordObject(mTargets[i], "Intensity Debug Scale Values");
                            mTargets[i].internals.data.intensityScaleAdd = 0f;
                            // Find Highest Value
                            float maxValue = Mathf.NegativeInfinity;
                            for (int ii = 0; ii < mTargets[i].internals.data.intensityDebugValueList.Count; ii++) {
                                if (mTargets[i].internals.data.intensityDebugValueList[ii] > maxValue) {
                                    maxValue = mTargets[i].internals.data.intensityDebugValueList[ii];
                                }
                            }
                            // Avoid divide by zero
                            if (maxValue + mTargets[i].internals.data.intensityScaleAdd != 0) {
                                mTargets[i].internals.data.intensityScaleMultiplier = 1f / (maxValue + mTargets[i].internals.data.intensityScaleAdd);
                            }
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent("Clear Logged Values"))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Clear Logged Values");
                        mTargets[i].internals.data.intensityDebugValueList.Clear();
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                // Intensity Debug Scale Min Max
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.intensityDebugScaleMinMaxLabel, EditorTextSoundEvent.intensityDebugScaleMinMaxTooltip), GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f))) {
                    // Remade to use mTargets in case SerializedProperties are breaking if they're added between refreshing SP and pressing the button
                    for (int i = 0; i < mTargets.Length; i++) {
                        if (mTargets[i].internals.data.intensityDebugValueList.Count > 0) {
                            Undo.RecordObject(mTargets[i], "Intensity Debug Scale Values");
                            // Reset intensity multiplier
                            mTargets[i].internals.data.intensityScaleMultiplier = 1f;
                            // Find lowest value
                            float minValue = Mathf.Infinity;
                            for (int ii = 0; ii < mTargets[i].internals.data.intensityDebugValueList.Count; ii++) {
                                if (mTargets[i].internals.data.intensityDebugValueList[ii] < minValue) {
                                    minValue = mTargets[i].internals.data.intensityDebugValueList[ii];
                                }
                            }
                            if (minValue != Mathf.Infinity) {
                                mTargets[i].internals.data.intensityScaleAdd = -minValue * mTargets[i].internals.data.intensityScaleMultiplier;
                            }
                            // Find Highest Value
                            float maxValue = Mathf.NegativeInfinity;
                            for (int ii = 0; ii < mTargets[i].internals.data.intensityDebugValueList.Count; ii++) {
                                if (mTargets[i].internals.data.intensityDebugValueList[ii] > maxValue) {
                                    maxValue = mTargets[i].internals.data.intensityDebugValueList[ii];
                                }
                            }
                            // Avoid divide by zero
                            if (maxValue + mTargets[i].internals.data.intensityScaleAdd != 0) {
                                mTargets[i].internals.data.intensityScaleMultiplier = 1f / (maxValue + mTargets[i].internals.data.intensityScaleAdd);
                            }
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent("Reset All Intensity"))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Intensity Settings");
                        mTargets[i].internals.data.intensityMultiplier = 1f;
                        mTargets[i].internals.data.intensityAdd = 0f;
                        mTargets[i].internals.data.intensityRolloff = 0f;
                        mTargets[i].internals.data.intensitySeekTime = 0f;
                        mTargets[i].internals.data.intensityCurve = AnimationCurve.Linear(0, 0, 1, 1);
                        mTargets[i].internals.data.intensityThresholdEnable = false;
                        mTargets[i].internals.data.intensityThreshold = 0f;
                        mTargets[i].internals.data.intensityScaleMultiplier = 1f;
                        mTargets[i].internals.data.intensityScaleAdd = 0f;
                        mTargets[i].internals.data.intensityRecord = false;
                        // Reset Debug Value List
                        mTargets[i].internals.data.intensityDebugValueList.Clear();
                        // Reset Debug Resolution
                        mTargets[i].internals.data.intensityDebugResolution = 100;
                        mTargets[i].internals.data.intensityDebugZoom = 0.9f;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiTriggerOther() {

            // Trigger Other
            StartBackgroundColor(EditorColor.GetSoundEvent(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            EditorGuiFunction.DrawFoldout(triggerOnExpand, $"Trigger Other");
            EndChange();
            EditorGUILayout.EndHorizontal();

            // Trigger On Play
            if (triggerOnExpand.boolValue) {
                EditorGUI.indentLevel = 1;
                BeginChange();
                EditorGUILayout.PropertyField(triggerOnPlayEnable, new GUIContent(EditorTextSoundEvent.triggerOnPlayLabel, EditorTextSoundEvent.triggerOnPlayTooltip));
                EndChange();

                if (triggerOnPlayEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    // What To Play
                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnPlayWhichToPlay, new GUIContent(EditorTextSoundEvent.triggerOnWhichToPlayLabel, EditorTextSoundEvent.triggerOnWhichToPlayTooltip));
                    EndChange();

                    // Trigger On Play SoundEvent
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.data.triggerOnPlaySoundEvents.Length) {
                            lowestArrayLength = mTargets[n].internals.data.triggerOnPlaySoundEvents.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(triggerOnPlaySoundEvents, serializedObject, lowestArrayLength, true);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Separator();
            }

            // Trigger On Play Check if infinite loop
            if (ShouldDebug.GuiWarnings()) {
                if (triggerOnPlayEnable.boolValue) {
                    if (mTarget.internals.data.GetIfInfiniteLoop(mTarget, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnPlay)) {
                        EditorGUILayout.HelpBox("Trigger On Play: \"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", MessageType.Error);
                        EditorGUILayout.Separator();
                    }
                }
            }

            // Trigger On Stop
            if (triggerOnExpand.boolValue) {
                EditorGUI.indentLevel = 1;
                BeginChange();
                EditorGUILayout.PropertyField(triggerOnStopEnable, new GUIContent(EditorTextSoundEvent.triggerOnStopLabel, EditorTextSoundEvent.triggerOnStopTooltip));
                EndChange();

                if (triggerOnStopEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    // What To Stop
                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnStopWhichToPlay, new GUIContent(EditorTextSoundEvent.triggerOnWhichToPlayLabel, EditorTextSoundEvent.triggerOnWhichToPlayTooltip));
                    EndChange();

                    // Trigger On Stop SoundEvent
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.data.triggerOnStopSoundEvents.Length) {
                            lowestArrayLength = mTargets[n].internals.data.triggerOnStopSoundEvents.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(triggerOnStopSoundEvents, serializedObject, lowestArrayLength, true);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Separator();
            }

            // Trigger On Stop Check if infinite loop
            if (ShouldDebug.GuiWarnings()) {
                if (triggerOnStopEnable.boolValue) {
                    if (mTarget.internals.data.GetIfInfiniteLoop(mTarget, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnStop)) {
                        EditorGUILayout.HelpBox("Trigger On Stop: \"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", MessageType.Error);
                        EditorGUILayout.Separator();
                    }
                }
            }

            // Trigger On Tail
            if (triggerOnExpand.boolValue) {
                EditorGUI.indentLevel = 1;
                BeginChange();
                EditorGUILayout.PropertyField(triggerOnTailEnable, new GUIContent(EditorTextSoundEvent.triggerOnTailLabel, EditorTextSoundEvent.triggerOnTailTooltip));
                EndChange();

                if (triggerOnTailEnable.boolValue) {

                    EditorGUI.indentLevel++;
                    // What To Play
                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnTailWhichToPlay, new GUIContent(EditorTextSoundEvent.triggerOnWhichToPlayLabel, EditorTextSoundEvent.triggerOnWhichToPlayTooltip));
                    EndChange();

                    // Trigger On Tail SoundEvent
                    int lowestArrayLength = int.MaxValue;
                    for (int n = 0; n < mTargets.Length; n++) {
                        if (lowestArrayLength > mTargets[n].internals.data.triggerOnTailSoundEvents.Length) {
                            lowestArrayLength = mTargets[n].internals.data.triggerOnTailSoundEvents.Length;
                        }
                    }
                    EditorGuiFunction.DrawReordableArray(triggerOnTailSoundEvents, serializedObject, lowestArrayLength, true);
                    EditorGUILayout.Separator();

                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnTailLength, new GUIContent(EditorTextSoundEvent.triggerOnTailTailLengthLabel, EditorTextSoundEvent.triggerOnTailTailLengthTooltip));
                    if (triggerOnTailLength.floatValue < 0f) {
                        triggerOnTailLength.floatValue = 0f;
                    }
                    EndChange();

                    // Set Tail Length from BPM
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.triggerOnTailSetTailLengthFromBpmLabel, EditorTextSoundEvent.triggerOnTailSetTailLengthFromBpmTooltip))) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], "Set Tail Length from BPM");
                            // Bpm
                            float tempFloat = EditorBeatLength.BpmToSecondsPerBar(mTargets[i].internals.data.triggerOnTailBpm);
                            // Beat
                            tempFloat = tempFloat * EditorBeatLength.BeatToDivision(mTargets[i].internals.data.triggerOnTailBeatLength);
                            mTargets[i].internals.data.triggerOnTailLength = Mathf.Clamp(tempFloat, 0f, Mathf.Infinity);
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                    // Bpm
                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnTailBpm, new GUIContent(EditorTextSoundEvent.triggerOnTailBpmLabel, EditorTextSoundEvent.triggerOnTailBpmTooltip));
                    if (triggerOnTailBpm.floatValue < 0f) {
                        triggerOnTailBpm.floatValue = 0f;
                    }
                    EndChange();
                    // Beat
                    BeginChange();
                    EditorGUILayout.PropertyField(triggerOnTailBeatLength, new GUIContent(EditorTextSoundEvent.triggerOnTailTailLengthInBeatsLabel, EditorTextSoundEvent.triggerOnTailTailLengthInBeatsTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                } else {
                    EditorGUILayout.Separator();
                }
            }

            if (triggerOnTailEnable.boolValue) {
                // Check if tail length is too long
                float shortestAudioClipLength = 0f;
                if (mTarget.internals.soundContainers.Length > 0 && mTarget.internals.soundContainers[0] != null) {
                    shortestAudioClipLength = mTarget.internals.soundContainers[0].internals.GetShortestAudioClipLength();
                }
                if (ShouldDebug.GuiWarnings()) {
                    if (shortestAudioClipLength == 0f) {
                        EditorGUILayout.HelpBox(EditorTextSoundEvent.triggerOnTailWarningNoAudioClipFound, MessageType.Error);
                        EditorGUILayout.Separator();
                    } else if (shortestAudioClipLength <= triggerOnTailLength.floatValue) {
                        EditorGUILayout.HelpBox(EditorTextSoundEvent.triggerOnTailWarningTailLengthIsTooLong
                            + $"\n" + EditorTextSoundEvent.triggerOnTailWarningLengthWarning + " ~" + shortestAudioClipLength.ToString("0.0") + "s", MessageType.Error);
                        EditorGUILayout.Separator();
                    }
                }

                // Trigger On Tail  Check if infinite loop
                if (ShouldDebug.GuiWarnings()) {
                    if (mTarget.internals.data.GetIfInfiniteLoop(mTarget, out SoundEventBase infiniteInstigator, out SoundEventBase infinitePrevious, TriggerOnType.TriggerOnTail)) {
                        EditorGUILayout.HelpBox(
                            "Trigger On Tail: \"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop" +
                            "\n" +
                            "(which might be fine for music etc)", MessageType.Warning
                            );
                        EditorGUILayout.Separator();
                    }
                }
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void GuiSoundTag() {
            StartBackgroundColor(EditorColor.GetSoundTag(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            BeginChange();
            EditorGuiFunction.DrawFoldout(expandAllSoundTag, $"{nameof(NameOf.SoundTag)}");
            EndChange();

            if (expandAllSoundTag.boolValue) {
                EditorGUI.indentLevel = 1;

                BeginChange();
                EditorGUILayout.PropertyField(soundTagEnable, new GUIContent(EditorTextSoundEvent.soundTagEnableLabel, EditorTextSoundEvent.soundTagEnableTooltip));
                EndChange();

                if (soundTagEnable.boolValue) {

                    // Extra horizontal for labelWidth
                    EditorGUILayout.BeginHorizontal();
                    BeginChange();
                    EditorGUILayout.PropertyField(soundTagMode, new GUIContent(EditorTextSoundEvent.soundTagModeLabel, EditorTextSoundEvent.soundTagModeTooltip));
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                }
            }
            StopBackgroundColor();

            if (expandAllSoundTag.boolValue && soundTagEnable.boolValue) {
                // SoundTagGroups
                for (int i = 0; i < soundTagGroups.arraySize; i++) {
                    EditorGUILayout.Separator();
                    StartBackgroundColor(EditorColor.GetSoundTag(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
                    SerializedProperty soundEventSoundTagGroupElement = this.soundTagGroups.GetArrayElementAtIndex(i);
                    SerializedProperty soundTag = soundEventSoundTagGroupElement.FindPropertyRelative(nameof(SoundEventSoundTagGroup.soundTag));
                    SerializedProperty baseSoundEventModifier = soundEventSoundTagGroupElement.FindPropertyRelative(nameof(SoundEventSoundTagGroup.soundEventModifierBase));
                    SerializedProperty soundTagSoundEventModifier = soundEventSoundTagGroupElement.FindPropertyRelative(nameof(SoundEventSoundTagGroup.soundEventModifierSoundTag));
                    SerializedProperty soundEvent = soundEventSoundTagGroupElement.FindPropertyRelative(nameof(SoundEventSoundTagGroup.soundEvent));
                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.indentLevel = 1;

                    // Extra horizontal for labelWidth
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
                    // SoundTag
                    BeginChange();
                    EditorGUILayout.PropertyField(soundTag, new GUIContent(""));
                    //EditorGUILayout.PropertyField(soundTag, new GUIContent(EditorTextSE.soundTagLabel, EditorTextSE.soundTagTooltip));
                    EndChange();
                    EditorGUILayout.EndHorizontal();

                    // Play
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextPreview.soundEventSoundTagPlayLabel, EditorTextPreview.soundEventSoundTagPlayTooltip))) {
                        EditorPreviewSound.Stop(false, false);
                        // Base SoundEvent
                        if (mTarget.internals.data.soundTagEnable) {
                            for (int ii = 0; ii < mTarget.internals.soundContainers.Length; ii++) {
                                if (mTarget.internals.soundContainers[ii] != null) {
                                    EditorPreviewSoundData newPreviewSoundContainerSetting = new EditorPreviewSoundData();
                                    newPreviewSoundContainerSetting.soundEvent = mTarget;
                                    newPreviewSoundContainerSetting.soundContainer = mTarget.internals.soundContainers[ii];
                                    newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.data.soundEventModifier);
                                    newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.data.soundTagGroups[i].soundEventModifierBase);
                                    // Adding SoundMix and their parents
                                    SoundMixBase tempSoundMix = mTarget.internals.data.soundMix;
                                    while (tempSoundMix != null && !tempSoundMix.internals.CheckIsInfiniteLoop(tempSoundMix, true)) {
                                        newPreviewSoundContainerSetting.soundEventModifierList.Add(tempSoundMix.internals.soundEventModifier);
                                        tempSoundMix = tempSoundMix.internals.soundMixParent;
                                    }
                                    newPreviewSoundContainerSetting.timelineSoundContainerData = mTarget.internals.data.timelineSoundContainerData[ii];
                                    EditorPreviewSound.AddEditorPreviewSoundData(newPreviewSoundContainerSetting);
                                }
                            }
                            // SoundTag SoundEvent
                            for (int ii = 0; ii < mTarget.internals.data.soundTagGroups[i].soundEvent.Length; ii++) {
                                if (mTarget.internals.data.soundTagGroups[i].soundEvent[ii] != null) {
                                    for (int iii = 0; iii < mTarget.internals.data.soundTagGroups[i].soundEvent[ii].internals.soundContainers.Length; iii++) {
                                        EditorPreviewSoundData newPreviewSoundContainerSetting = new EditorPreviewSoundData();
                                        newPreviewSoundContainerSetting.soundEvent = mTarget.internals.data.soundTagGroups[i].soundEvent[ii];
                                        newPreviewSoundContainerSetting.soundContainer = mTarget.internals.data.soundTagGroups[i].soundEvent[ii].internals.soundContainers[iii];
                                        newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.data.soundTagGroups[i].soundEvent[ii].internals.data.soundEventModifier);
                                        newPreviewSoundContainerSetting.soundEventModifierList.Add(mTarget.internals.data.soundTagGroups[i].soundEventModifierSoundTag);
                                        // Adding SoundMix and their parents
                                        SoundMixBase tempSoundMix = mTarget.internals.data.soundTagGroups[i].soundEvent[ii].internals.data.soundMix;
                                        while (tempSoundMix != null && !tempSoundMix.internals.CheckIsInfiniteLoop(tempSoundMix, true)) {
                                            newPreviewSoundContainerSetting.soundEventModifierList.Add(tempSoundMix.internals.soundEventModifier);
                                            tempSoundMix = tempSoundMix.internals.soundMixParent;
                                        }
                                        newPreviewSoundContainerSetting.timelineSoundContainerData = mTarget.internals.data.soundTagGroups[i].soundEvent[ii].internals.data.timelineSoundContainerData[iii];
                                        EditorPreviewSound.AddEditorPreviewSoundData(newPreviewSoundContainerSetting);
                                    }
                                }
                            }
                        }
                        EditorPreviewSound.SetEditorSetting(previewEditorSetting);
                        EditorPreviewSound.PlaySoundEvent(mTarget);
                    }
                    EndChange();
                    // Stop
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextPreview.stopLabel, EditorTextPreview.stopTooltip))) {
                        EditorPreviewSound.Stop(true, true);
                    }
                    EndChange();

                    BeginChange();
                    if (GUILayout.Button("+")) {
                        soundTagGroups.InsertArrayElementAtIndex(i);
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("-")) {
                        if (soundTagGroups.arraySize > 1) {
                            soundTagGroups.DeleteArrayElementAtIndex(i);
                        }
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("↑")) {
                        soundTagGroups.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, soundTagGroups.arraySize));
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button("↓")) {
                        soundTagGroups.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, soundTagGroups.arraySize));
                    }
                    EndChange();

                    EditorGUILayout.EndHorizontal();

                    if (i >= soundTagGroups.arraySize) {
                        break;
                    }

                    if (expandAllSoundTag.boolValue) {

                        EditorGUI.indentLevel = 1;
                        
                        // SoundTag SoundEvent
                        int lowestArrayLength = int.MaxValue;
                        for (int n = 0; n < mTargets.Length; n++) {
                            if (lowestArrayLength > mTargets[n].internals.data.soundTagGroups[i].soundEvent.Length) {
                                lowestArrayLength = mTargets[n].internals.data.soundTagGroups[i].soundEvent.Length;
                            }
                        }
                        EditorGuiFunction.DrawReordableArray(soundEvent, serializedObject, lowestArrayLength, true);

                        // Getting the SoundTag soundEventModifiers
                        SoundEventModifier[] soundEventModifiersSoundTag = new SoundEventModifier[mTargets.Length];
                        for (int ii = 0; ii < mTargets.Length; ii++) {
                            if (i < mTargets[ii].internals.data.soundTagGroups.Length) {
                                soundEventModifiersSoundTag[ii] = mTargets[ii].internals.data.soundTagGroups[i].soundEventModifierSoundTag;
                            }
                        }
                        AddRemoveModifier("SoundTag Modifiers", EditorTextModifier.modifiersTooltip, soundTagSoundEventModifier, soundEventModifiersSoundTag, 1, true);
                        if (soundTagSoundEventModifier.isExpanded) {
                            // Modifiers
                            EditorGUI.indentLevel = 2;
                            UpdateModifier(soundTagSoundEventModifier);
                        }

                        // Getting the Base soundEventModifier
                        SoundEventModifier[] soundEventModifiersBase = new SoundEventModifier[mTargets.Length];
                        for (int ii = 0; ii < mTargets.Length; ii++) {
                            if (i < mTargets[ii].internals.data.soundTagGroups.Length) {
                                soundEventModifiersBase[ii] = mTargets[ii].internals.data.soundTagGroups[i].soundEventModifierBase;
                            }
                        }
                        AddRemoveModifier("Base Modifiers", EditorTextModifier.modifiersTooltip, baseSoundEventModifier, soundEventModifiersBase, 1, true);
                        if (baseSoundEventModifier.isExpanded) {
                            // Modifiers
                            EditorGUI.indentLevel = 2;
                            UpdateModifier(baseSoundEventModifier);
                        }

                        EditorGUI.indentLevel = 1;
                    }
                    StopBackgroundColor();
                }
            }
            EditorGUILayout.Separator();
        }

        private void GuiDebug() {
            StartBackgroundColor(EditorColor.GetDebug(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));
            EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            EditorGuiFunction.DrawFoldout(debugExpand, "Debug");
            EndChange();
            EditorGUILayout.EndHorizontal();

            if (debugExpand.boolValue) {

                BeginChange();
                EditorGUILayout.PropertyField(debugLogSoundEventShow, new GUIContent(EditorTextSoundEvent.debugLogSoundEventShowLabel, EditorTextSoundEvent.debugLogSoundEventShowTooltip));
                EndChange();

                BeginChange();
                EditorGUILayout.PropertyField(debugDrawSoundEventShow, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventShowLabel, EditorTextSoundEvent.debugDrawSoundEventShowTooltip));
                EndChange();

                BeginChange();
                EditorGUILayout.PropertyField(debugDrawSoundEventStyleOverride, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventStyleOverrideLabel, EditorTextSoundEvent.debugDrawSoundEventStyleOverrideTooltip));
                EndChange();

                if (debugDrawSoundEventStyleOverride.boolValue) {
                    EditorGUI.indentLevel++;

                    BeginChange();
                    EditorGUILayout.Slider(debugDrawSoundEventFontSize, 0f, 10f, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventFontSizeLabel, EditorTextSoundEvent.debugDrawSoundEventFontSizeTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.Slider(debugDrawSoundEventOpacityMultiplier, 0f, 10f, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventOpacityMultiplierLabel, EditorTextSoundEvent.debugDrawSoundEventOpacityMultiplierTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(debugDrawSoundEventColorStart, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventColorStartLabel, EditorTextSoundEvent.debugDrawSoundEventColorStartTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(debugDrawSoundEventColorEnd, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventColorEndLabel, EditorTextSoundEvent.debugDrawSoundEventColorEndTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(debugDrawSoundEventColorOutline, new GUIContent(EditorTextSoundEvent.debugDrawSoundEventColorOutlineLabel, EditorTextSoundEvent.debugDrawSoundEventColorOutlineTooltip));
                    EndChange();

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent("Reset Debug Settings", ""))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Debug Settings");
                        mTargets[i].internals.data.debugDrawShow = DebugSoundEventDrawShow.Show;
                        mTargets[i].internals.data.debugLogShow = DebugSoundEventLogShow.Show;
                        mTargets[i].internals.data.debugDrawSoundEventStyleOverride = false;
                        mTargets[i].internals.data.debugDrawSoundEventFontSizeMultiplier = 1f;
                        mTargets[i].internals.data.debugDrawSoundEventOpacityMultiplier = 1f;
                        mTargets[i].internals.data.debugDrawSoundEventColorStart = new Color(1f, 0f, 1f);
                        mTargets[i].internals.data.debugDrawSoundEventColorEnd = new Color(0.66f, 0f, 1f);
                        mTargets[i].internals.data.debugDrawSoundEventColorOutline = new Color(0f, 0f, 0f);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
            }
            StopBackgroundColor();
        }

        private void GuiReset() {
            EditorGUI.indentLevel = 0;
            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            // Reset Settings
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.resetSettingsLabel, EditorTextSoundEvent.resetSettingsTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset Settings");
                    mTargets[i].internals.data = new SoundEventInternalsData();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();

            // Reset All
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.resetAllLabel, EditorTextSoundEvent.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].internals.soundContainers = new SoundContainerBase[1];
                    mTargets[i].internals.data = new SoundEventInternalsData();
                    mTargets[i].internals.notes = "Notes";
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private void GuiFindReferences() {
            EditorGUI.indentLevel = 0;
            StartBackgroundColor(Color.grey);
            EditorGUILayout.BeginHorizontal();
            if (mTarget.internals.data.foundReferences == null || mTarget.internals.data.foundReferences.Length == 0) {
                EditorGUILayout.LabelField(new GUIContent("No Search"), GUILayout.Width(EditorGUIUtility.labelWidth));
            } else {
                EditorGUILayout.LabelField(new GUIContent(mTarget.internals.data.foundReferences.Length + " References Found"), GUILayout.Width(EditorGUIUtility.labelWidth));
            }
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.findReferencesLabel, EditorTextSoundEvent.findReferencesTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    mTargets[i].internals.data.foundReferences = EditorFindReferences.GetObjects(mTargets[i]);
                    EditorUtility.SetDirty(mTargets[i]);
                }
                GUIUtility.ExitGUI();
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.findReferencesSelectAllLabel, EditorTextSoundEvent.findReferencesSelectAllTooltip))) {
                // Select objects
                List<UnityEngine.Object> newSelection = new List<UnityEngine.Object>();
                for (int i = 0; i < mTargets.Length; i++) {
                    if (mTargets[i].internals.data.foundReferences != null) {
                        newSelection.AddRange(mTargets[i].internals.data.foundReferences);
                    }
                }
                // Only select if something is found
                if (newSelection != null && newSelection.Count > 0) {
                    Selection.objects = newSelection.ToArray();
                }
            }
            EndChange();
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundEvent.findReferencesClearLabel, EditorTextSoundEvent.findReferencesClearTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    mTargets[i].internals.data.foundReferences = new UnityEngine.Object[0];
                    EditorUtility.SetDirty(mTargets[i]);
                }
                GUIUtility.ExitGUI();
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            // Showing the references
            for (int i = 0; i < foundReferences.arraySize; i++) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(foundReferences.GetArrayElementAtIndex(i), new GUIContent(foundReferences.GetArrayElementAtIndex(i).objectReferenceValue.name));
                EditorGUILayout.EndHorizontal();
            }

            StopBackgroundColor();
            EditorGUILayout.Separator();
        }
    }
}
#endif