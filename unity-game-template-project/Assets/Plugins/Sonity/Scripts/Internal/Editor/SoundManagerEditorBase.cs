// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Sonity.Internal {

    public abstract class SoundManagerEditorBase : Editor {

        public SoundManagerBase mTarget;
        public SoundManagerBase[] mTargets;

        public GUIStyle guiStyleBoldCenter = new GUIStyle();

        public GUIStyle[] statisticsStyles = new GUIStyle[5];
        public string[] statisticsStrings = new string[5];

        public Color defaultGuiStyleTextColor;
        public Color defaultGuiColor;

        public SerializedProperty internals;
        public SerializedProperty settings;
        public SerializedProperty editorTools;
        public SerializedProperty debug;
        public SerializedProperty statistics;

        public SerializedProperty debugExpand;
        public SerializedProperty drawSoundEventsInSceneViewEnabled;
        public SerializedProperty drawSoundEventsInGameViewEnabled;
        public SerializedProperty drawSoundEventsHideIfCloserThan;
        public SerializedProperty drawSoundEventsFontSize;
        public SerializedProperty drawSoundEventsVolumeToOpacity;
        public SerializedProperty drawSoundEventsLifetimeToOpacity;
        public SerializedProperty drawSoundEventsLifetimeFadeLength;
        public SerializedProperty drawSoundEventsColorStart;
        public SerializedProperty drawSoundEventsColorEnd;
        public SerializedProperty drawSoundEventsColorOutline;

        public SerializedProperty logSoundEventsEnabled;
        public SerializedProperty logSoundEventsLogType;
        public SerializedProperty logSoundEventsSelectObject;
        public SerializedProperty logSoundEventsPlay;
        public SerializedProperty logSoundEventsStop;
        public SerializedProperty logSoundEventsPool;
        public SerializedProperty logSoundEventsPause;
        public SerializedProperty logSoundEventsUnpause;
        public SerializedProperty logSoundEventsGlobalPause;
        public SerializedProperty logSoundEventsGlobalUnpause;
        public SerializedProperty logSoundEventsSoundParametersOnce;
        public SerializedProperty logSoundEventsSoundParametersContinious;

        public SerializedProperty settingsExpandBase;
        public SerializedProperty settingsExpandAdvanced;
        public SerializedProperty dontDestoyOnLoad;
        public SerializedProperty debugWarnings;
        public SerializedProperty debugInPlayMode;
        public SerializedProperty guiWarnings;
        public SerializedProperty disablePlayingSounds;

        public SerializedProperty soundTimeScale;
        public SerializedProperty globalPause;
        public SerializedProperty globalVolumeRatio;
        public SerializedProperty globalVolumeDecibel;
        public SerializedProperty volumeIncreaseEnable;
        public SerializedProperty globalSoundTag;
        public SerializedProperty distanceScale;
        public SerializedProperty overrideListenerDistance;
        public SerializedProperty overrideListenerDistanceAmount;
        public SerializedProperty speedOfSoundEnabled;
        public SerializedProperty speedOfSoundScale;

        public SerializedProperty adressableAudioMixerUse;

#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
        public SerializedProperty adressableAudioMixerReference;
#endif

        public SerializedProperty voicePreload;
        public SerializedProperty voiceLimit;
        public SerializedProperty voiceEffectLimit;
        public SerializedProperty voiceStopTime;

        public SerializedProperty editorToolExpand;
        public SerializedProperty editorToolSelectionHistoryEnable;
        public SerializedProperty editorToolReferenceFinderEnable;
        public SerializedProperty editorToolSelectSameTypeEnable;

        public SerializedProperty statisticsExpandBase;
        public SerializedProperty statisticsExpandInstances;
        public SerializedProperty statisticsSorting;

        public SerializedProperty statisticsShowActive;
        public SerializedProperty statisticsShowDisabled;
        public SerializedProperty statisticsShowVoices;
        public SerializedProperty statisticsShowPlays;
        public SerializedProperty statisticsShowVolume;

        private void OnEnable() {
            for (int i = 0; i < statisticsStyles.Length; i++) {
                if (statisticsStyles[i] == null) {
                    statisticsStyles[i] = new GUIStyle();
                }
            }
            FindProperties();
        }

        private void FindProperties() {

            internals = serializedObject.FindProperty(nameof(SoundManagerBase.Internals).ToLowerInvariant());
            settings = internals.FindPropertyRelative(nameof(SoundManagerInternals.settings));
            statistics = internals.FindPropertyRelative(nameof(SoundManagerInternals.statistics));
            debug = internals.FindPropertyRelative(nameof(SoundManagerInternals.debug));
            editorTools = internals.FindPropertyRelative(nameof(SoundManagerInternals.editorTools));

            debugExpand = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.debugExpand));

            // Draw SoundEvents
            drawSoundEventsInSceneViewEnabled = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsInSceneViewEnabled));
            drawSoundEventsInGameViewEnabled = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsInGameViewEnabled));
            drawSoundEventsHideIfCloserThan = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsHideIfCloserThan));
            drawSoundEventsFontSize = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsFontSize));
            drawSoundEventsVolumeToOpacity = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsVolumeToOpacity));
            drawSoundEventsLifetimeToOpacity = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsLifetimeToOpacity));
            drawSoundEventsLifetimeFadeLength = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsLifetimeFadeLength));
            drawSoundEventsColorStart = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsColorStart));
            drawSoundEventsColorEnd = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsColorEnd));
            drawSoundEventsColorOutline = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.drawSoundEventsColorOutline));

            // Log SoundEvents
            logSoundEventsEnabled = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsEnabled));
            logSoundEventsLogType = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsLogType));
            logSoundEventsSelectObject = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsSelectObject));
            logSoundEventsPlay = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsPlay));
            logSoundEventsStop = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsStop));
            logSoundEventsPool = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsPool));
            logSoundEventsPause = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsPause));
            logSoundEventsUnpause = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsUnpause));
            logSoundEventsGlobalPause = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsGlobalPause));
            logSoundEventsGlobalUnpause = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsGlobalUnpause));
            logSoundEventsSoundParametersOnce = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsSoundParametersOnce));
            logSoundEventsSoundParametersContinious = debug.FindPropertyRelative(nameof(SoundManagerInternalsDebug.logSoundEventsSoundParametersContinious));

            settingsExpandBase = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.settingExpandBase));
            settingsExpandAdvanced = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.settingsExpandAdvanced));
            dontDestoyOnLoad = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.dontDestroyOnLoad));
            debugWarnings = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugWarnings));
            debugInPlayMode = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugInPlayMode));
            guiWarnings = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.guiWarnings));
            disablePlayingSounds = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.disablePlayingSounds));

            soundTimeScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.soundTimeScale));
            globalPause = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalPause));
            globalVolumeRatio = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalVolumeRatio));
            globalVolumeDecibel = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalVolumeDecibel));
            volumeIncreaseEnable = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.volumeIncreaseEnable));
            globalSoundTag = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalSoundTag));
            distanceScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.distanceScale));
            overrideListenerDistance = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.overrideListenerDistance));
            overrideListenerDistanceAmount = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.overrideListenerDistanceAmount));
            speedOfSoundEnabled = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.speedOfSoundEnabled));
            speedOfSoundScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.speedOfSoundScale));

            adressableAudioMixerUse = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.adressableAudioMixerUse));

#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
            adressableAudioMixerReference = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.adressableAudioMixerReference));
#endif

            voicePreload = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voicePreload));
            voiceLimit = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceLimit));
            voiceEffectLimit = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceEffectLimit));
            voiceStopTime = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceStopTime));

            editorToolExpand = editorTools.FindPropertyRelative(nameof(SoundManagerInternalsEditorTools.editorToolExpand));
            editorToolSelectionHistoryEnable = editorTools.FindPropertyRelative(nameof(SoundManagerInternalsEditorTools.editorToolSelectionHistoryEnable));
            editorToolReferenceFinderEnable = editorTools.FindPropertyRelative(nameof(SoundManagerInternalsEditorTools.editorToolReferenceFinderEnable));
            editorToolSelectSameTypeEnable = editorTools.FindPropertyRelative(nameof(SoundManagerInternalsEditorTools.editorToolSelectSameTypeEnable));

            statisticsExpandBase = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsExpandBase));
            statisticsExpandInstances = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsExpandInstances));
            statisticsSorting = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsSorting));
            statisticsShowActive = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowActive));
            statisticsShowDisabled = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowDisabled));
            statisticsShowVoices = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowVoices));
            statisticsShowPlays = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowPlays));
            statisticsShowVolume = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowVolume));
        }

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

        public override void OnInspectorGUI() {

            mTarget = (SoundManagerBase)target;

            mTargets = new SoundManagerBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundManagerBase)targets[i];
            }

            defaultGuiColor = GUI.color;
            if (EditorGUIUtility.isProSkin) {
                defaultGuiStyleTextColor = EditorColorProSkin.GetDarkSkinTextColor();
            } else {
                defaultGuiStyleTextColor = new GUIStyle().normal.textColor;
            }

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundManager, EditorTextSoundManager.soundManagerTooltip, false);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            DrawSettings();
            DrawDebug();
            DrawStatistics();
            DrawEditorTools();

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundManager.resetAllLabel, EditorTextSoundManager.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].Internals.settings = new SoundManagerInternalsSettings();
                    mTargets[i].Internals.editorTools = new SoundManagerInternalsEditorTools();
                    mTargets[i].Internals.debug = new SoundManagerInternalsDebug();
                    mTargets[i].Internals.statistics = new SoundManagerInternalsStatistics();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private void DrawSettings() {
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(settingsExpandBase, "Settings");
            EndChange();

            if (settingsExpandBase.boolValue) {

                // Free Trial Button
                if (EditorTrial.isTrial) {
                    // Enable Sound in Builds is disabled
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Toggle(new GUIContent(EditorTextSoundManager.enableSoundInBuildsLabel, EditorTextSoundManager.enableSoundInBuildsTooltip), false);
                    EditorGUI.EndDisabledGroup();

                    // Warning
                    EditorGUILayout.Separator();
                    EditorGUILayout.HelpBox(EditorTextSoundManager.enableSoundInBuildsWarning, MessageType.Warning);
                    EditorGUILayout.Separator();
                }

                if (EditorTrial.isTrial || EditorTrial.isEducational) {
                    // Increase Volume Disabled
                    EditorGUI.BeginDisabledGroup(true);
                    BeginChange();
                    EditorGUILayout.PropertyField(volumeIncreaseEnable, new GUIContent(EditorTextSoundManager.audioListenerVolumeIncreaseEnableLabel, EditorTextSoundManager.audioListenerVolumeIncreaseEnableTooltip));
                    EndChange();
                    EditorGUI.EndDisabledGroup();

                    // DLL doesnt support volume increase warning
                    EditorGUILayout.Separator();
                    if (EditorTrial.isTrial) {
                        EditorGUILayout.HelpBox(EditorTextSoundManager.audioListenerVolumeIncreaseFreeTrialWarningLabel, MessageType.Warning);
                    }
                    if (EditorTrial.isEducational) {
                        EditorGUILayout.HelpBox(EditorTextSoundManager.audioListenerVolumeIncreaseEducationalVersionWarningLabel, MessageType.Warning);
                    }
                    EditorGUILayout.Separator();
                } else {

                    // Increase Volume
                    BeginChange();
                    EditorGUILayout.PropertyField(volumeIncreaseEnable, new GUIContent(EditorTextSoundManager.audioListenerVolumeIncreaseEnableLabel, EditorTextSoundManager.audioListenerVolumeIncreaseEnableTooltip));
                    EndChange();

                    if (volumeIncreaseEnable.boolValue) {
                        if (!EditorScriptingDefineSymbols.AudioListenerVolumeIncreaseExists()) {
                            EditorGUILayout.BeginHorizontal();
                            // For offsetting the buttons to the right
                            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                            BeginChange();
                            if (GUILayout.Button(new GUIContent(EditorTextSoundManager.audioListenerVolumeIncreaseScriptingDefineSymbolAddLabel, EditorTextSoundManager.audioListenerVolumeIncreaseScriptingDefineSymbolAddTooltip))) {
                                EditorScriptingDefineSymbols.AudioListenerVolumeIncreaseShouldExist(true);
                            }
                            EndChange();
                            EditorGUILayout.EndHorizontal();
                        }
                    } else {
                        if (EditorScriptingDefineSymbols.AudioListenerVolumeIncreaseExists()) {
                            EditorGUILayout.BeginHorizontal();
                            // For offsetting the buttons to the right
                            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                            BeginChange();
                            if (GUILayout.Button(new GUIContent(EditorTextSoundManager.audioListenerVolumeIncreaseScriptingDefineSymbolRemoveLabel, EditorTextSoundManager.audioListenerVolumeIncreaseScriptingDefineSymbolRemoveTooltip))) {
                                EditorScriptingDefineSymbols.AudioListenerVolumeIncreaseShouldExist(false);
                            }
                            EndChange();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Global Volume
                BeginChange();
                EditorGUILayout.Slider(globalVolumeDecibel, VolumeScale.lowestVolumeDecibel, 0f, new GUIContent(EditorTextSoundManager.globalVolumeLabel, EditorTextSoundManager.globalVolumeTooltip));
                if (globalVolumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                    globalVolumeDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (globalVolumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(globalVolumeDecibel.floatValue)) {
                    globalVolumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(globalVolumeDecibel.floatValue);
                }
                EndChange();

                // Set AudioListener.volume after global volume and increase volume is set
                if (Application.isPlaying) {
#if SONITY_ENABLE_VOLUME_INCREASE
                    if (AudioListener.volume != globalVolumeRatio.floatValue * VolumeScale.volumeIncrease60dbAudioListenerRatioMax) {
                        AudioListener.volume = globalVolumeRatio.floatValue * VolumeScale.volumeIncrease60dbAudioListenerRatioMax;
                    }
#else
                    if (AudioListener.volume != globalVolumeRatio.floatValue) {
                        AudioListener.volume = globalVolumeRatio.floatValue;
                    }
#endif
                }

                // Global Pause
                BeginChange();
                bool tempGlobalPauseValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundManager.globalPauseLabel, EditorTextSoundManager.globalPauseTooltip), globalPause.boolValue);
                if (globalPause.boolValue != tempGlobalPauseValue) {
                    globalPause.boolValue = tempGlobalPauseValue;
                    mTarget.Internals.InternalSetGlobalPauseUnpause(tempGlobalPauseValue);
                }
                EndChange();

                // Global SoundTag
                BeginChange();
                EditorGUILayout.PropertyField(globalSoundTag, new GUIContent(EditorTextSoundManager.globalSoundTagLabel, EditorTextSoundManager.globalSoundTagTooltip));
                EndChange();

                // Distance Scale
                BeginChange();
                float distanceScaleValue = distanceScale.floatValue;
                distanceScaleValue = EditorGUILayout.FloatField(new GUIContent(EditorTextSoundManager.distanceScaleLabel, EditorTextSoundManager.distanceScaleTooltip), distanceScaleValue);
                if (distanceScaleValue > 0f) {
                    distanceScale.floatValue = distanceScaleValue;
                }
                EndChange();

                BeginChange();
                EditorGuiFunction.DrawFoldout(settingsExpandAdvanced, "Advanced", "", 0, false, false, true);
                EndChange();

                // Advanced Settings
                if (settingsExpandAdvanced.boolValue) {

                    // Sound Time Scale
                    BeginChange();
                    EditorGUILayout.PropertyField(soundTimeScale, new GUIContent(EditorTextSoundManager.soundTimeScaleLabel, EditorTextSoundManager.soundTimeScaleTooltip));
                    EndChange();

                    // Audio Listener Distance Use
                    BeginChange();
                    EditorGUILayout.PropertyField(overrideListenerDistance, new GUIContent(EditorTextSoundManager.overrideListenerDistanceLabel, EditorTextSoundManager.overrideListenerDistanceTooltip));
                    EndChange();

                    if (overrideListenerDistance.boolValue) {
                        EditorGUI.indentLevel++;
                        BeginChange();
                        overrideListenerDistanceAmount.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.overrideListenerDistanceAmountLabel, EditorTextSoundManager.overrideListenerDistanceAmountTooltip), overrideListenerDistanceAmount.floatValue, 0f, 100f);
                        EndChange();
                        EditorGUI.indentLevel--;
                    }

                    // Speed of Sound
                    BeginChange();
                    EditorGUILayout.PropertyField(speedOfSoundEnabled, new GUIContent(EditorTextSoundManager.speedOfSoundEnabledLabel, EditorTextSoundManager.speedOfSoundEnabledTooltip));
                    EndChange();

                    if (speedOfSoundEnabled.boolValue) {
                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.PropertyField(speedOfSoundScale, new GUIContent(EditorTextSoundManager.speedOfSoundScaleLabel, EditorTextSoundManager.speedOfSoundScaleTooltip));
                        if (speedOfSoundScale.floatValue <= 0f) {
                            if (speedOfSoundScale.floatValue < 0f) {
                                speedOfSoundScale.floatValue = 0f;
                            }
                            if (ShouldDebug.GuiWarnings()) {
                                EditorGUILayout.Separator();
                                EditorGUILayout.HelpBox(EditorTextSoundManager.speedOfSoundScaleWarning, MessageType.Warning);
                                EditorGUILayout.Separator();
                            }
                        }
                        EndChange();
                        EditorGUI.indentLevel--;
                    }

                    // Adressable AudioMixer
                    if (EditorTrial.isTrial || EditorTrial.isEducational) {

                        // Adressable AudioMixer Disabled
                        EditorGUI.BeginDisabledGroup(true);
                        BeginChange();
                        EditorGUILayout.PropertyField(adressableAudioMixerUse, new GUIContent(EditorTextSoundManager.adressableAudioMixerUseLabel, EditorTextSoundManager.adressableAudioMixerUseTooltip));
                        EndChange();
                        EditorGUI.EndDisabledGroup();

                        // DLL doesnt support adressable AudioMixer warning
                        EditorGUILayout.Separator();
                        if (EditorTrial.isTrial) {
                            EditorGUILayout.HelpBox(EditorTextSoundManager.adressableAudioMixerFreeTrialWarningLabel, MessageType.Warning);
                        }
                        if (EditorTrial.isEducational) {
                            EditorGUILayout.HelpBox(EditorTextSoundManager.adressableAudioMixerEducationalVersionWarningLabel, MessageType.Warning);
                        }
                        EditorGUILayout.Separator();
                    } else {

                        // Adressable AudioMixer
                        BeginChange();
                        EditorGUILayout.PropertyField(adressableAudioMixerUse, new GUIContent(EditorTextSoundManager.adressableAudioMixerUseLabel, EditorTextSoundManager.adressableAudioMixerUseTooltip));
                        EndChange();

                        if (adressableAudioMixerUse.boolValue) {
                            if (!EditorScriptingDefineSymbols.AdressableAudioMixerExists()) {
                                EditorGUILayout.BeginHorizontal();
                                // For offsetting the buttons to the right
                                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                                BeginChange();
                                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.adressableAudioMixerScriptingDefineSymbolAddLabel, EditorTextSoundManager.adressableAudioMixerScriptingDefineSymbolAddTooltip))) {
                                    EditorScriptingDefineSymbols.AdressableAudioMixerShouldExist(true);
                                }
                                EndChange();
                                EditorGUILayout.EndHorizontal();
                            }
#if SONITY_ENABLE_ADRESSABLE_AUDIOMIXER
                            EditorGUI.indentLevel++;
                            BeginChange();
                            EditorGUILayout.PropertyField(adressableAudioMixerReference, new GUIContent(EditorTextSoundManager.adressableAudioMixerReferenceLabel, EditorTextSoundManager.adressableAudioMixerReferenceTooltip));
                            EndChange();
                            EditorGUI.indentLevel--;
#endif
                        } else {
                            if (EditorScriptingDefineSymbols.AdressableAudioMixerExists()) {
                                EditorGUILayout.BeginHorizontal();
                                // For offsetting the buttons to the right
                                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                                BeginChange();
                                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.adressableAudioMixerScriptingDefineSymbolRemoveLabel, EditorTextSoundManager.adressableAudioMixerScriptingDefineSymbolRemoveTooltip))) {
                                    EditorScriptingDefineSymbols.AdressableAudioMixerShouldExist(false);
                                }
                                EndChange();
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }

                    // DontDestroyOnLoad
                    BeginChange();
                    EditorGUILayout.PropertyField(dontDestoyOnLoad, new GUIContent(EditorTextSoundManager.dontDestoyOnLoadLabel, EditorTextSoundManager.dontDestoyOnLoadTooltip));
                    EndChange();

                    // Debug Warnings
                    BeginChange();
                    EditorGUILayout.PropertyField(debugWarnings, new GUIContent(EditorTextSoundManager.debugWarningsLabel, EditorTextSoundManager.debugWarningsTooltip));
                    EndChange();
                    if (debugWarnings.boolValue) {
                        EditorGUI.indentLevel++;
                        BeginChange();
                        EditorGUILayout.PropertyField(debugInPlayMode, new GUIContent(EditorTextSoundManager.debugInPlayModeLabel, EditorTextSoundManager.debugInPlayModeTooltip));
                        EndChange();
                        EditorGUI.indentLevel--;
                    }

                    // Gui Warnings
                    BeginChange();
                    EditorGUILayout.PropertyField(guiWarnings, new GUIContent(EditorTextSoundManager.guiWarningsLabel, EditorTextSoundManager.guiWarningsTooltip));
                    EndChange();

                    // Disable Playing Sounds
                    BeginChange();
                    EditorGUILayout.PropertyField(disablePlayingSounds, new GUIContent(EditorTextSoundManager.disablePlayingSoundsLabel, EditorTextSoundManager.disablePlayingSoundsTooltip));
                    EndChange();

                    if (ShouldDebug.GuiWarnings()) {
                        if (disablePlayingSounds.boolValue) {
                            EditorGUILayout.Separator();
                            EditorGUILayout.HelpBox(EditorTextSoundManager.disablePlayingSoundsWarning, MessageType.Warning);
                            EditorGUILayout.Separator();
                        }
                    }

                    EditorGUILayout.Separator();

                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.LabelField(new GUIContent("Performance"), EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    // Voice Limit
                    BeginChange();
                    EditorGUILayout.PropertyField(voiceLimit, new GUIContent(EditorTextSoundManager.voiceLimitLabel, EditorTextSoundManager.voiceLimitTooltip));
                    if (voiceLimit.intValue < 1) {
                        voiceLimit.intValue = 1;
                    }
                    // Sets the voice limit to the voice preload
                    if (voicePreload.intValue > voiceLimit.intValue) {
                        voicePreload.intValue = voiceLimit.intValue;
                    }
                    EndChange();

                    // Source Preload On Awake
                    BeginChange();
                    EditorGUILayout.PropertyField(voicePreload, new GUIContent(EditorTextSoundManager.voicePreloadLabel, EditorTextSoundManager.voicePreloadTooltip));
                    if (voicePreload.intValue < 0) {
                        voicePreload.intValue = 0;
                    }
                    // Sets the voice limit to the voice preload
                    if (voiceLimit.intValue < voicePreload.intValue) {
                        voiceLimit.intValue = voicePreload.intValue;
                    }
                    EndChange();

                    // Project Audio Settings Info
                    int oldIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    EditorGUILayout.LabelField(new GUIContent(
                        "Project Audio Settings:" + "\n" +
                        AudioSettings.GetConfiguration().numRealVoices.ToString() + " " +
                        EditorTextSoundManager.audioSettingsRealVoicesLabel + "\n" +
                        AudioSettings.GetConfiguration().numVirtualVoices.ToString() + " " +
                        EditorTextSoundManager.audioSettingsVirtualVoicesLabel
                        ,
                        EditorTextSoundManager.audioSettingsRealAndVirtualVoicesTooltip),
                        EditorStyles.helpBox);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel = oldIndentLevel;

                    // Apply voice limit to audio settings
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.applyVoiceLimitToAudioSettingsLabel, EditorTextSoundManager.applyVoiceLimitToAudioSettingsTooltip))) {
                        // Load the AudioManager asset
                        UnityEngine.Object audioManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/AudioManager.asset")[0];
                        SerializedObject serializedManager = new SerializedObject(audioManager);
                        // Set RealVoiceCount
                        SerializedProperty m_RealVoiceCount = serializedManager.FindProperty("m_RealVoiceCount");
                        m_RealVoiceCount.intValue = voiceLimit.intValue;
                        // Apply properties
                        serializedManager.ApplyModifiedProperties();
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();

                    // If Real Voices are lower than Voice Limit
                    if (ShouldDebug.GuiWarnings()) {
                        if (AudioSettings.GetConfiguration().numRealVoices < voiceLimit.intValue) {
                            EditorGUILayout.Separator();
                            EditorGUILayout.HelpBox(EditorTextSoundManager.audioSettingsRealVoicesWarning, MessageType.Warning);
                            EditorGUILayout.Separator();
                        }

                        // If Virtual Voices are lower than Real Voices
                        if (AudioSettings.GetConfiguration().numVirtualVoices < AudioSettings.GetConfiguration().numRealVoices) {
                            EditorGUILayout.Separator();
                            EditorGUILayout.HelpBox(EditorTextSoundManager.audioSettingsVirtualVoicesWarning, MessageType.Warning);
                            EditorGUILayout.Separator();
                        }
                    }

                    // Voice Stop Time
                    BeginChange();
                    EditorGUILayout.PropertyField(voiceStopTime, new GUIContent(EditorTextSoundManager.voiceStopTimeLabel, EditorTextSoundManager.voiceStopTimeTooltip));
                    if (voiceStopTime.floatValue < 0f) {
                        voiceStopTime.floatValue = 0f;
                    }
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(voiceEffectLimit, new GUIContent(EditorTextSoundManager.voiceEffectLimitLabel, EditorTextSoundManager.voiceEffectLimitTooltip));
                    if (voiceEffectLimit.intValue < 0) {
                        voiceEffectLimit.intValue = 0;
                    }
                    EndChange();
                    EditorGUILayout.Separator();
                }

                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.resetSettingsLabel, EditorTextSoundManager.resetSettingsTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Settings");
                        mTargets[i].Internals.settings = new SoundManagerInternalsSettings();
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private void DrawEditorTools() {
            StartBackgroundColor(EditorColor.GetEditorTools(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(editorToolExpand, EditorTextSoundManager.editorToolHeaderLabel, EditorTextSoundManager.editorToolHeaderTooltip);
            EndChange();

            if (editorToolExpand.boolValue) {

                // Editor Tools
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent("Enable Tool"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Reference Finder
                BeginChange();
                EditorGUILayout.PropertyField(editorToolReferenceFinderEnable, new GUIContent(EditorTextSoundManager.editorToolReferenceFinderEnableLabel, EditorTextSoundManager.editorToolReferenceFinderEnableTooltip));
                EndChange();

                if (editorToolReferenceFinderEnable.boolValue) {
                    if (!EditorScriptingDefineSymbols.EditorToolReferenceFinderExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolReferenceFinderAddLabel, EditorTextSoundManager.editorToolReferenceFinderAddTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolReferenceFinderShouldExist(true);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                } else {
                    if (EditorScriptingDefineSymbols.EditorToolReferenceFinderExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolReferenceFinderRemoveLabel, EditorTextSoundManager.editorToolReferenceFinderRemoveTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolReferenceFinderShouldExist(false);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Select Same Type
                BeginChange();
                EditorGUILayout.PropertyField(editorToolSelectSameTypeEnable, new GUIContent(EditorTextSoundManager.editorToolSelectSameTypeEnableLabel, EditorTextSoundManager.editorToolSelectSameTypeEnableTooltip));
                EndChange();

                if (editorToolSelectSameTypeEnable.boolValue) {
                    if (!EditorScriptingDefineSymbols.EditorToolSelectSameTypeExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolSelectSameTypeAddLabel, EditorTextSoundManager.editorToolReferenceFinderAddTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolSelectSameTypeShouldExist(true);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                } else {
                    if (EditorScriptingDefineSymbols.EditorToolSelectSameTypeExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolSelectSameTypeRemoveLabel, EditorTextSoundManager.editorToolReferenceFinderRemoveTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolSelectSameTypeShouldExist(false);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Selection History
                BeginChange();
                EditorGUILayout.PropertyField(editorToolSelectionHistoryEnable, new GUIContent(EditorTextSoundManager.editorToolSelectionHistoryEnableLabel, EditorTextSoundManager.editorToolSelectionHistoryEnableTooltip));
                EndChange();

                if (editorToolSelectionHistoryEnable.boolValue) {
                    if (!EditorScriptingDefineSymbols.EditorToolSelectionHistoryExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolSelectionHistoryAddLabel, EditorTextSoundManager.editorToolSelectionHistoryAddTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolSelectionHistoryShouldExist(true);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                } else {
                    if (EditorScriptingDefineSymbols.EditorToolSelectionHistoryExists()) {
                        EditorGUILayout.BeginHorizontal();
                        // For offsetting the buttons to the right
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                        BeginChange();
                        if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolSelectionHistoryRemoveLabel, EditorTextSoundManager.editorToolSelectionHistoryRemoveTooltip))) {
                            EditorScriptingDefineSymbols.EditorToolSelectionHistoryShouldExist(false);
                        }
                        EndChange();
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Add Remove All Editor Tools
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolsAddAllLabel, EditorTextSoundManager.editorToolsAddAllTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Editor Tools Add All");
                        mTargets[i].Internals.editorTools.editorToolSelectionHistoryEnable = true;
                        mTargets[i].Internals.editorTools.editorToolReferenceFinderEnable = true;
                        mTargets[i].Internals.editorTools.editorToolSelectSameTypeEnable = true;
                        EditorScriptingDefineSymbols.EditorToolSelectionHistoryShouldExist(true);
                        EditorScriptingDefineSymbols.EditorToolReferenceFinderShouldExist(true);
                        EditorScriptingDefineSymbols.EditorToolSelectSameTypeShouldExist(true);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.editorToolsRemoveAllLabel, EditorTextSoundManager.editorToolsRemoveAllTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Editor Tools Remove All");
                        mTargets[i].Internals.editorTools.editorToolSelectionHistoryEnable = false;
                        mTargets[i].Internals.editorTools.editorToolReferenceFinderEnable = false;
                        mTargets[i].Internals.editorTools.editorToolSelectSameTypeEnable = false;
                        EditorScriptingDefineSymbols.EditorToolSelectionHistoryShouldExist(false);
                        EditorScriptingDefineSymbols.EditorToolReferenceFinderShouldExist(false);
                        EditorScriptingDefineSymbols.EditorToolSelectSameTypeShouldExist(false);
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            StopBackgroundColor();
        }

        private void DrawDebug() {
            StartBackgroundColor(EditorColor.GetDebug(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));


            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(debugExpand, EditorTextSoundManager.debugExpandLabel, EditorTextSoundManager.debugExpandTooltip);
            EndChange();

            if (debugExpand.boolValue) {

                // Show Current Cached AudioListener & AudioListener Distance
                EditorGUI.BeginDisabledGroup(true);

                EditorGUILayout.ObjectField(new GUIContent(EditorTextSoundManager.debugAudioListenerLabel, EditorTextSoundManager.debugAudioListenerTooltip), mTarget.Internals.cachedAudioListener, typeof(Object), true);

                if (overrideListenerDistance.boolValue) {
                    EditorGUILayout.ObjectField(new GUIContent(EditorTextSoundManager.debugAudioListenerDistanceLabel, EditorTextSoundManager.debugAudioListenerDistanceTooltip), mTarget.Internals.cachedAudioListenerDistance, typeof(Object), true);
                }
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Separator();

                // Log SoundEvents
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.logSoundEventsHeaderLabel, EditorTextSoundManager.logSoundEventsHeaderTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                BeginChange();
                EditorGUILayout.PropertyField(logSoundEventsEnabled, new GUIContent(EditorTextSoundManager.logSoundEventsEnableLabel, EditorTextSoundManager.logSoundEventsEnableTooltip));
                EndChange();

                if (logSoundEventsEnabled.boolValue) {

                    BeginChange();
                    EditorGUILayout.PropertyField(logSoundEventsLogType, new GUIContent(EditorTextSoundManager.logSoundEventsLogTypeLabel, EditorTextSoundManager.logSoundEventsLogTypeTooltip));
                    EndChange();

                    BeginChange();
                    EditorGUILayout.PropertyField(logSoundEventsSelectObject, new GUIContent(EditorTextSoundManager.logSoundEventsSelectObjectLabel, EditorTextSoundManager.logSoundEventsSelectObjectTooltip));
                    EndChange();

                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.logSoundEventsSettingsLabel, EditorTextSoundManager.logSoundEventsSettingsTooltip))) {
                        GenericMenu menu = new GenericMenu();

                        // Tooltips dont work for menu
                        menu.AddItem(new GUIContent("SoundEvent Play"), logSoundEventsPlay.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.Play);
                        menu.AddItem(new GUIContent("SoundEvent Stop"), logSoundEventsStop.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.Stop);
                        menu.AddItem(new GUIContent("SoundEvent Pool"), logSoundEventsPool.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.Pool);
                        menu.AddItem(new GUIContent("SoundEvent Pause"), logSoundEventsPause.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.Pause);
                        menu.AddItem(new GUIContent("SoundEvent Unpause"), logSoundEventsUnpause.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.Unpause);
                        menu.AddItem(new GUIContent("SoundEvent Global Pause"), logSoundEventsGlobalPause.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.GlobalPause);
                        menu.AddItem(new GUIContent("SoundEvent Global Unpause"), logSoundEventsGlobalUnpause.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.GlobalUnpause);
                        menu.AddItem(new GUIContent("SoundParameters Once"), logSoundEventsSoundParametersOnce.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.SoundParametersOnce);
                        menu.AddItem(new GUIContent("SoundParameters Continious"), logSoundEventsSoundParametersContinious.boolValue, SoundEventLogMenuCallback, SoundEventLogButtonType.SoundParametersContinious);

                        menu.ShowAsContext();
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.logSoundEventsResetLabel, EditorTextSoundManager.logSoundEventsResetTooltip))) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], "Reset Debug Log Settings");
                            mTargets[i].Internals.debug.logSoundEventsLogType = DebugSoundEventsToLogLogType.DebugLog;
                            mTargets[i].Internals.debug.logSoundEventsSelectObject = DebugSoundEventsToLogSelectObject.Owner;
                            mTargets[i].Internals.debug.logSoundEventsPlay = true;
                            mTargets[i].Internals.debug.logSoundEventsStop = true;
                            mTargets[i].Internals.debug.logSoundEventsPool = false;
                            mTargets[i].Internals.debug.logSoundEventsPause = true;
                            mTargets[i].Internals.debug.logSoundEventsUnpause = true;
                            mTargets[i].Internals.debug.logSoundEventsGlobalPause = false;
                            mTargets[i].Internals.debug.logSoundEventsGlobalUnpause = false;
                            mTargets[i].Internals.debug.logSoundEventsSoundParametersOnce = true;
                            mTargets[i].Internals.debug.logSoundEventsSoundParametersContinious = false;
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Separator();

                // Draw SoundEvents
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.drawSoundEventsLabel, EditorTextSoundManager.drawSoundEventsTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Draw SoundEvents in Scene View
                BeginChange();
                EditorGUILayout.PropertyField(drawSoundEventsInSceneViewEnabled, new GUIContent(EditorTextSoundManager.drawSoundEventsInSceneViewEnabledLabel, EditorTextSoundManager.drawSoundEventsInSceneViewEnabledTooltip));
                EndChange();

                // Draw SoundEvents in Game View
                BeginChange();
                EditorGUILayout.PropertyField(drawSoundEventsInGameViewEnabled, new GUIContent(EditorTextSoundManager.drawSoundEventsInGameViewEnabledLabel, EditorTextSoundManager.drawSoundEventsInGameViewEnabledTooltip));
                EndChange();

                if (drawSoundEventsInGameViewEnabled.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.PropertyField(drawSoundEventsHideIfCloserThan, new GUIContent(EditorTextSoundManager.drawSoundEventsHideIfCloserThanLabel, EditorTextSoundManager.drawSoundEventsHideIfCloserThanTooltip));
                    if (drawSoundEventsHideIfCloserThan.floatValue < 0f) {
                        drawSoundEventsHideIfCloserThan.floatValue = 0f;
                    }
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                if (drawSoundEventsInSceneViewEnabled.boolValue || drawSoundEventsInGameViewEnabled.boolValue) {
                    EditorGUILayout.LabelField(new GUIContent("Style"));
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.IntSlider(drawSoundEventsFontSize, 1, 100, new GUIContent(EditorTextSoundManager.drawSoundEventsFontSizeLabel, EditorTextSoundManager.drawSoundEventsFontSizeTooltip));
                    EndChange();
                    BeginChange();
                    drawSoundEventsVolumeToOpacity.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.drawSoundEventsVolumeToOpacityLabel, EditorTextSoundManager.drawSoundEventsVolumeToOpacityTooltip), drawSoundEventsVolumeToOpacity.floatValue, 0f, 1f);
                    EndChange();
                    BeginChange();
                    drawSoundEventsLifetimeToOpacity.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.drawSoundEventsLifetimeToOpacityLabel, EditorTextSoundManager.drawSoundEventsLifetimeToOpacityTooltip), drawSoundEventsLifetimeToOpacity.floatValue, 0f, 1f);
                    EndChange();
                    BeginChange();
                    drawSoundEventsLifetimeFadeLength.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.drawSoundEventsLifetimeFadeLengthLabel, EditorTextSoundManager.drawSoundEventsLifetimeFadeLengthTooltip), drawSoundEventsLifetimeFadeLength.floatValue, 0f, 10f);
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(drawSoundEventsColorStart, new GUIContent(EditorTextSoundManager.drawSoundEventsColorStartLabel, EditorTextSoundManager.drawSoundEventsColorStartTooltip));
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(drawSoundEventsColorEnd, new GUIContent(EditorTextSoundManager.drawSoundEventsColorEndLabel, EditorTextSoundManager.drawSoundEventsColorEndTooltip));
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(drawSoundEventsColorOutline, new GUIContent(EditorTextSoundManager.drawSoundEventsColorOutlineLabel, EditorTextSoundManager.drawSoundEventsColorOutlineTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;

                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.drawSoundEventsResetLabel, EditorTextSoundManager.drawSoundEventsResetTooltip))) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], "Reset Style");
                            mTargets[i].Internals.debug.drawSoundEventsFontSize = 16;
                            mTargets[i].Internals.debug.drawSoundEventsVolumeToOpacity = 0.5f;
                            mTargets[i].Internals.debug.drawSoundEventsLifetimeToOpacity = 0.75f;
                            mTargets[i].Internals.debug.drawSoundEventsLifetimeFadeLength = 1f;
                            mTargets[i].Internals.debug.drawSoundEventsColorStart = EditorColor.GetVolumeMax(1f);
                            mTargets[i].Internals.debug.drawSoundEventsColorEnd = EditorColor.GetVolumeMin(1f);
                            mTargets[i].Internals.debug.drawSoundEventsColorOutline = Color.black;
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.debugResetAllLabel, EditorTextSoundManager.debugResetAllTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset All Debug Settings");
                        // Draw SoundEvents
                        mTargets[i].Internals.debug.drawSoundEventsInSceneViewEnabled = false;
                        mTargets[i].Internals.debug.drawSoundEventsInGameViewEnabled = false;
                        mTargets[i].Internals.debug.drawSoundEventsHideIfCloserThan = 0.01f;
                        mTargets[i].Internals.debug.drawSoundEventsFontSize = 16;
                        mTargets[i].Internals.debug.drawSoundEventsVolumeToOpacity = 0.5f;
                        mTargets[i].Internals.debug.drawSoundEventsLifetimeToOpacity = 0.75f;
                        mTargets[i].Internals.debug.drawSoundEventsLifetimeFadeLength = 1f;
                        mTargets[i].Internals.debug.drawSoundEventsColorStart = EditorColor.GetVolumeMax(1f);
                        mTargets[i].Internals.debug.drawSoundEventsColorEnd = EditorColor.GetVolumeMin(1f);
                        mTargets[i].Internals.debug.drawSoundEventsColorOutline = Color.black;
                        // Log SoundEvents
                        mTargets[i].Internals.debug.logSoundEventsEnabled = false;
                        mTargets[i].Internals.debug.logSoundEventsLogType = DebugSoundEventsToLogLogType.DebugLog;
                        mTargets[i].Internals.debug.logSoundEventsSelectObject = DebugSoundEventsToLogSelectObject.Owner;
                        mTargets[i].Internals.debug.logSoundEventsPlay = true;
                        mTargets[i].Internals.debug.logSoundEventsStop = true;
                        mTargets[i].Internals.debug.logSoundEventsPool = false;
                        mTargets[i].Internals.debug.logSoundEventsPause = true;
                        mTargets[i].Internals.debug.logSoundEventsUnpause = true;
                        mTargets[i].Internals.debug.logSoundEventsGlobalPause = false;
                        mTargets[i].Internals.debug.logSoundEventsGlobalUnpause = false;
                        mTargets[i].Internals.debug.logSoundEventsSoundParametersOnce = true;
                        mTargets[i].Internals.debug.logSoundEventsSoundParametersContinious = false;
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private Dictionary<long, SoundEventInstanceDictionaryValue>.Enumerator managedUpdateTempInstanceDictionaryValueEnumerator;
        private Dictionary<int, SoundEventInstance>.Enumerator managedUpdateTempInstanceEnumerator;

        private class StatisticsListClass {
            public SoundEventBase soundEvent;
            public int soundEventInstancesActive;
            public int soundEventInstancesDisabled;
            public int numberOfUsedVoices;
            public float averageVolume;
        }

        private List<StatisticsListClass> statisticsList = new List<StatisticsListClass>();

        private void DrawStatistics() {
            StartBackgroundColor(EditorColor.GetSoundManagerStatistics(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(statisticsExpandBase, EditorTextSoundManager.statisticsExpandLabel, EditorTextSoundManager.statisticsExpandTooltip);
            EndChange();

            if (statisticsExpandBase.boolValue) {

                if (Application.isPlaying) {
                    // Repaint so its updated all the time
                    Repaint();
                }

                // SoundEvent Statistics
                int usedSoundEvents = 0;
                int disabledSoundEvents = 0;

                foreach (var soundEventDictionaryValue in mTarget.Internals.soundEventDictionary.Values) {
                    foreach (var transformDictionaryValue in soundEventDictionaryValue.instanceDictionary) {
                        usedSoundEvents++;
                    }
                    disabledSoundEvents += soundEventDictionaryValue.unusedInstanceStack.Count;
                }

                // SoundEvents
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsSoundEventsLabel, EditorTextSoundManager.statisticsSoundEventsTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // Created
                EditorGUILayout.LabelField(new GUIContent((usedSoundEvents + disabledSoundEvents) + "\t " + EditorTextSoundManager.statisticsSoundEventsCreatedLabel, EditorTextSoundManager.statisticsSoundEventsCreatedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(usedSoundEvents + "\t " + EditorTextSoundManager.statisticsSoundEventsActiveLabel, EditorTextSoundManager.statisticsSoundEventsActiveTooltip));
                // Disabled
                EditorGUILayout.LabelField(new GUIContent(disabledSoundEvents + "\t " + EditorTextSoundManager.statisticsSoundEventsDisabledLabel, EditorTextSoundManager.statisticsSoundEventsDisabledTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                // Source Pool
                int numberOfActiveSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].isAssigned) {
                        numberOfActiveSources++;
                    }
                }

                // Max Simultaneous
                if (mTarget.Internals.statistics.statisticsMaxSimultaneousVoices < numberOfActiveSources) {
                    mTarget.Internals.statistics.statisticsMaxSimultaneousVoices = numberOfActiveSources;
                }

                // Voices
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsVoicesLabel, EditorTextSoundManager.statisticsVoicesTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                // Played
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.statistics.statisticsVoicesPlayed + "\t " + EditorTextSoundManager.statisticsVoicesPlayedLabel, EditorTextSoundManager.statisticsVoicesPlayedTooltip));
                // Max Simultaneous
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.statistics.statisticsMaxSimultaneousVoices + "\t " + EditorTextSoundManager.statisticsMaxSimultaneousVoicesLabel, EditorTextSoundManager.statisticsMaxSimultaneousVoicesTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Stolen
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.statisticsVoicesStolen + "\t " + EditorTextSoundManager.statisticsVoicesStolenLabel, EditorTextSoundManager.statisticsVoicesStolenTooltip));
                // Created
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.voicePool.Length + "\t " + EditorTextSoundManager.statisticsVoicesCreatedLabel, EditorTextSoundManager.statisticsVoicesCreatedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(numberOfActiveSources + "\t " + EditorTextSoundManager.statisticsVoicesActiveLabel, EditorTextSoundManager.statisticsVoicesActiveTooltip));
                // Inactive
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.voicePool.Length - numberOfActiveSources + "\t " + EditorTextSoundManager.statisticsVoicesInactiveLabel, EditorTextSoundManager.statisticsVoicesInactiveTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                int pausedSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].GetState() == VoiceState.Pause) {
                        pausedSources++;
                    }
                }
                // Paused
                EditorGUILayout.LabelField(new GUIContent(pausedSources + "\t " + EditorTextSoundManager.statisticsVoicesPausedLabel, EditorTextSoundManager.statisticsVoicesPausedTooltip));
                int stoppedSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].GetState() == VoiceState.Stop) {
                        stoppedSources++;
                    }
                }
                // Stopped
                EditorGUILayout.LabelField(new GUIContent(stoppedSources + "\t " + EditorTextSoundManager.statisticsVoicesStoppedLabel, EditorTextSoundManager.statisticsVoicesStoppedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                // Voice Effect Statistics
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsVoiceEffectsLabel, EditorTextSoundManager.statisticsVoiceEffectsTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                int numberOfActiveVoiceEffects = 0;

                for (int i = 0; i < mTarget.Internals.voiceEffectPool.voiceEffectPool.Length; i++) {
                    if (mTarget.Internals.voiceEffectPool.voiceEffectPool[i] != null) {
                        if (mTarget.Internals.voiceEffectPool.voiceEffectPool[i].cachedVoiceEffect.GetEnabled()) {
                            numberOfActiveVoiceEffects++;
                        }
                    }
                }
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(numberOfActiveVoiceEffects + "\t " + EditorTextSoundManager.statisticsVoiceEffectsActiveLabel, EditorTextSoundManager.statisticsVoiceEffectsActiveTooltip));
                // Available
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.settings.voiceEffectLimit - numberOfActiveVoiceEffects + "\t " + EditorTextSoundManager.statisticsVoiceEffectsAvailableLabel, EditorTextSoundManager.statisticsVoiceEffectsAvailableTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();

                // Statistics Instances
                BeginChange();
                EditorGuiFunction.DrawFoldout(statisticsExpandInstances, EditorTextSoundManager.statisticsInstanceLabel, EditorTextSoundManager.statisticsInstanceTooltip, 0, false, false, true);
                EndChange();

                if (statisticsExpandInstances.boolValue) {

                    BeginChange();
                    EditorGUILayout.PropertyField(statisticsSorting, new GUIContent(EditorTextSoundManager.statisticsSortingLabel, EditorTextSoundManager.statisticsSortingTooltip));
                    EndChange();

                    // Statistics Dropdown Menu
                    EditorGUILayout.BeginHorizontal();
                    // For offsetting the buttons to the right
                    EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.statisticsShowLabel, EditorTextSoundManager.statisticsShowTooltip))) {
                        GenericMenu menu = new GenericMenu();

                        // Tooltips dont work for menu
                        menu.AddItem(new GUIContent("Show Voices"), statisticsShowVoices.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowVoices);
                        menu.AddItem(new GUIContent("Show Volume"), statisticsShowVolume.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowVolume);
                        menu.AddItem(new GUIContent("Show Active"), statisticsShowActive.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowActive);
                        menu.AddItem(new GUIContent("Show Disabled"), statisticsShowDisabled.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowDisabled);
                        menu.AddItem(new GUIContent("Show Plays"), statisticsShowPlays.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowPlays);

                        menu.ShowAsContext();
                    }
                    EndChange();
                    BeginChange();
                    if (GUILayout.Button(new GUIContent(EditorTextSoundManager.statisticsInstancesResetLabel, EditorTextSoundManager.statisticsInstancesResetTooltip))) {
                        for (int i = 0; i < mTargets.Length; i++) {
                            Undo.RecordObject(mTargets[i], "Reset Statistics Instances");
                            mTargets[i].Internals.statistics.statisticsSorting = SoundManagerStatisticsSorting.Voices;
                            mTargets[i].Internals.statistics.statisticsShowVoices = true;
                            mTargets[i].Internals.statistics.statisticsShowVolume = true;
                            mTargets[i].Internals.statistics.statisticsShowActive = false;
                            mTargets[i].Internals.statistics.statisticsShowDisabled = false;
                            mTargets[i].Internals.statistics.statisticsShowPlays = false;
                            EditorUtility.SetDirty(mTargets[i]);
                        }
                    }
                    EndChange();
                    EditorGUILayout.EndHorizontal();

                    if (!Application.isPlaying) {
                        EditorGUILayout.LabelField(new GUIContent("Available in Playmode"), EditorStyles.boldLabel);
                    } else {
                        // Only when application is playing

                        // Iterating over all the SoundEvents
                        statisticsList.Clear();
                        managedUpdateTempInstanceDictionaryValueEnumerator = mTarget.Internals.soundEventDictionary.GetEnumerator();
                        while (managedUpdateTempInstanceDictionaryValueEnumerator.MoveNext()) {
                            StatisticsListClass newStatisticsListClass = new StatisticsListClass();
                            newStatisticsListClass.soundEvent = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.soundEvent;
                            newStatisticsListClass.soundEventInstancesActive = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.Count;
                            newStatisticsListClass.soundEventInstancesDisabled = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.unusedInstanceStack.Count;
                            managedUpdateTempInstanceEnumerator = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                            int numberOfInstances = 0;
                            float tempVolume = 0f;
                            while (managedUpdateTempInstanceEnumerator.MoveNext()) {
                                numberOfInstances++;
                                // Number of voices
                                newStatisticsListClass.numberOfUsedVoices += managedUpdateTempInstanceEnumerator.Current.Value.StatisticsGetNumberOfUsedVoices();
                                // Average Volume
                                tempVolume += managedUpdateTempInstanceEnumerator.Current.Value.StatisticsGetAverageVolumeRatio();
                            }
                            // Avoid divide by zero
                            if (numberOfInstances > 0) {
                                newStatisticsListClass.averageVolume = tempVolume / numberOfInstances;
                            } else {
                                newStatisticsListClass.averageVolume = tempVolume;
                            }

                            statisticsList.Add(newStatisticsListClass);
                        }

                        // All but time should be sorted by name
                        if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex != SoundManagerStatisticsSorting.Time) {
                            // Sort by name
                            statisticsList = statisticsList.OrderBy(parameter => parameter.soundEvent.name).ToList();
                        }

                        if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Voices) {
                            // Sort by voices
                            statisticsList = statisticsList.OrderByDescending(parameter => parameter.numberOfUsedVoices).ToList();
                        } else if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Plays) {
                            // Sort by number of plays
                            statisticsList = statisticsList.OrderByDescending(parameter => SoundManagerBase.Instance.Internals.InternalStatisticsNumberOfPlays(parameter.soundEvent, false)).ToList();
                        } else if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Volume) {
                            // Sort by volume
                            statisticsList = statisticsList.OrderByDescending(parameter => parameter.averageVolume).ToList();
                        }

                        for (int i = 0; i < statisticsList.Count; i++) {

                            // SoundEvent
                            EditorGUILayout.LabelField(new GUIContent(statisticsList[i].soundEvent.name), EditorStyles.boldLabel);

                            int numberOfFields = 0;
                            if (statisticsShowVoices.boolValue) {
                                statisticsStrings[numberOfFields] = statisticsList[i].numberOfUsedVoices + "\t " + "Voices";
                                // Voices
                                if (statisticsList[i].numberOfUsedVoices > 0) {
                                    // Green
                                    statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextGreen();
                                } else {
                                    statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                                }
                                numberOfFields++;
                            }
                            if (statisticsShowVolume.boolValue) {
                                // Average Volume
                                float tempVolume = VolumeScale.ConvertRatioToDecibel(statisticsList[i].averageVolume);
                                if (tempVolume < -140f) {
                                    tempVolume = Mathf.NegativeInfinity;
                                }
                                statisticsStrings[numberOfFields] = tempVolume.ToString("0.0") + "\t " + "dB Average";
                                statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                                numberOfFields++;
                            }
                            if (statisticsShowActive.boolValue) {
                                statisticsStrings[numberOfFields] = statisticsList[i].soundEventInstancesActive + "\t " + "Active";
                                // Active
                                if (statisticsList[i].soundEventInstancesActive > 0) {
                                    // Green
                                    statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextGreen();
                                } else {
                                    statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                                }
                                numberOfFields++;
                            }
                            if (statisticsShowDisabled.boolValue) {
                                statisticsStrings[numberOfFields] = statisticsList[i].soundEventInstancesDisabled + "\t " + "Disabled";
                                // Disabled
                                if (statisticsList[i].soundEventInstancesDisabled > 0) {
                                    // Red
                                    statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextRed();
                                } else {
                                    statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                                }
                                numberOfFields++;
                            }
                            if (statisticsShowPlays.boolValue) {
                                // Number of Plays
                                statisticsStrings[numberOfFields] = SoundManagerBase.Instance.Internals.InternalStatisticsNumberOfPlays(statisticsList[i].soundEvent, false) + "\t " + "Plays";
                                statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                                numberOfFields++;
                            }

                            int currentField = 0;

                            if (numberOfFields > 0) {
                                EditorGUI.indentLevel++;
                                if (numberOfFields >= 2) {
                                    EditorGUILayout.BeginHorizontal();
                                }
                                if (numberOfFields >= 1) {
                                    EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                    currentField++;
                                }
                                if (numberOfFields >= 2) {
                                    EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                    currentField++;
                                }
                                if (numberOfFields >= 2) {
                                    EditorGUILayout.EndHorizontal();
                                }
                                if (numberOfFields >= 4) {
                                    EditorGUILayout.BeginHorizontal();
                                }
                                if (numberOfFields >= 3) {
                                    EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                    currentField++;
                                }
                                if (numberOfFields >= 4) {
                                    EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                    currentField++;
                                }
                                if (numberOfFields >= 4) {
                                    EditorGUILayout.EndHorizontal();
                                }
                                if (numberOfFields >= 5) {
                                    EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                    currentField++;
                                }
                                EditorGUI.indentLevel--;
                            }
                            EditorGUILayout.Separator();
                        }
                    }
                }
                // Reset Statistics All
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.statisticsAllResetLabel, EditorTextSoundManager.statisticsAllResetTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset All Statistics");
                        mTargets[i].Internals.statistics = new SoundManagerInternalsStatistics();
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
            StopBackgroundColor();
            EditorGUILayout.Separator();
        }

        private enum SoundEventLogButtonType {
            Play,
            Stop,
            Pool,
            Pause,
            Unpause,
            SoundParametersOnce,
            SoundParametersContinious,
            GlobalStop,
            GlobalPause,
            GlobalUnpause,
        }

        private void SoundEventLogMenuCallback(object obj) {

            SoundEventLogButtonType buttonType;

            try {
                buttonType = (SoundEventLogButtonType)obj;
            } catch {
                return;
            }

            bool tempToggle = false;

            for (int i = 0; i < mTargets.Length; i++) {
                if (mTargets[i] != null) {
                    Undo.RecordObject(mTargets[i], "SoundEvent Log");
                    // Show Active
                    if (buttonType == SoundEventLogButtonType.Play) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsPlay;
                        }
                        mTargets[i].Internals.debug.logSoundEventsPlay = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.Stop) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsStop;
                        }
                        mTargets[i].Internals.debug.logSoundEventsStop = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.Pool) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsPool;
                        }
                        mTargets[i].Internals.debug.logSoundEventsPool = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.Pause) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsPause;
                        }
                        mTargets[i].Internals.debug.logSoundEventsPause = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.Unpause) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsUnpause;
                        }
                        mTargets[i].Internals.debug.logSoundEventsUnpause = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.GlobalPause) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsGlobalPause;
                        }
                        mTargets[i].Internals.debug.logSoundEventsGlobalPause = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.GlobalUnpause) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsGlobalUnpause;
                        }
                        mTargets[i].Internals.debug.logSoundEventsGlobalUnpause = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.SoundParametersOnce) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsSoundParametersOnce;
                        }
                        mTargets[i].Internals.debug.logSoundEventsSoundParametersOnce = tempToggle;
                    }
                    if (buttonType == SoundEventLogButtonType.SoundParametersContinious) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.debug.logSoundEventsSoundParametersContinious;
                        }
                        mTargets[i].Internals.debug.logSoundEventsSoundParametersContinious = tempToggle;
                    }
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
        }

        private enum StatisticsButtonType {
            ShowActive,
            ShowDisabled,
            ShowVoices,
            ShowPlays,
            ShowVolume,
        }

        private void StatisticsMenuCallback(object obj) {

            StatisticsButtonType buttonType;

            try {
                buttonType = (StatisticsButtonType)obj;
            } catch {
                return;
            }

            bool tempToggle = false;

            for (int i = 0; i < mTargets.Length; i++) {
                if (mTargets[i] != null) {
                    Undo.RecordObject(mTargets[i], "Statistics Setting");
                    // Show Active
                    if (buttonType == StatisticsButtonType.ShowActive) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowActive;
                        }
                        mTargets[i].Internals.statistics.statisticsShowActive = tempToggle;
                    }
                    // Show Disabled
                    if (buttonType == StatisticsButtonType.ShowDisabled) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowDisabled;
                        }
                        mTargets[i].Internals.statistics.statisticsShowDisabled = tempToggle;
                    }
                    // Show Voices
                    if (buttonType == StatisticsButtonType.ShowVoices) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowVoices;
                        }
                        mTargets[i].Internals.statistics.statisticsShowVoices = tempToggle;
                    }
                    // Show Plays
                    if (buttonType == StatisticsButtonType.ShowPlays) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowPlays;
                        }
                        mTargets[i].Internals.statistics.statisticsShowPlays = tempToggle;
                    }
                    // Show Volume
                    if (buttonType == StatisticsButtonType.ShowVolume) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowVolume;
                        }
                        mTargets[i].Internals.statistics.statisticsShowVolume = tempToggle;
                    }
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
        }
    }
}
#endif