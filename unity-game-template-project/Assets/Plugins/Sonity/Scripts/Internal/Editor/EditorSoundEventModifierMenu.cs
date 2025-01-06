// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public static class EditorSoundEventModifierMenu {

        private class MenuSetting {
            public UnityEngine.Object[] mTargets;
            public SoundParameterType soundParameterType;
            public SoundEventModifier[] soundEventModifier;

            public MenuSetting(SoundParameterType soundParameterType, SoundEventModifier[] soundEventModifier, UnityEngine.Object[] mTargets) {
                this.soundParameterType = soundParameterType;
                this.soundEventModifier = soundEventModifier;
                this.mTargets = mTargets;
            }
        }

        public static bool ModifierAnyEnabled(SerializedProperty settingProperty) {
            for (int i = 0; i < System.Enum.GetValues(typeof(SoundParameterType)).Length; i++) {
                string propertyName = ((SoundParameterType)i).ToString() + "Use";
                // Make enum first character lowercase to match variable name
                propertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                SerializedProperty enabledProperty = settingProperty.FindPropertyRelative(propertyName);
                if (enabledProperty.boolValue) {
                    return true;
                }
            }
            return false;
        }

        public static void ModifierDisableAll(SerializedProperty settingProperty) {
            for (int i = 0; i < System.Enum.GetValues(typeof(SoundParameterType)).Length; i++) {
                ModifierEnable(settingProperty, (SoundParameterType)i, false);
            }
        }

        private static void ModifierEnable(SerializedProperty settingProperty, SoundParameterType soundParameterType, bool enabled) {
            string propertyName = soundParameterType.ToString() + "Use";
            // Make enum first character lowercase to match variable name
            propertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            SerializedProperty enabledProperty = settingProperty.FindPropertyRelative(propertyName);
            enabledProperty.boolValue = enabled;
        }

        public static void ModifierReset(SerializedProperty settingProperty) {
            // Used by the editor. Range -60dB to -0 dB
            SerializedProperty volumeDecibel = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.volumeDecibel));
            volumeDecibel.floatValue = 0f;
            SerializedProperty volumeRatio = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.volumeRatio));
            volumeRatio.floatValue = 1f;
            // Used By the editor range -24 to +24 semitones
            SerializedProperty pitchSemitone = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.pitchSemitone));
            pitchSemitone.floatValue = 0f;
            SerializedProperty pitchRatio = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.pitchRatio));
            pitchRatio.floatValue = 1f;
            // In Seconds
            SerializedProperty delay = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.delay));
            delay.floatValue = 0f;
            SerializedProperty increase2D = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.increase2D));
            increase2D.floatValue = 0f;
            SerializedProperty intensity = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.intensity));
            intensity.floatValue = 1f;

            // Used by the editor
            SerializedProperty reverbZoneMixDecibel = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixDecibel));
            reverbZoneMixDecibel.floatValue = 0f;
            SerializedProperty reverbZoneMixRatio = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.reverbZoneMixRatio));
            reverbZoneMixRatio.floatValue = 1f;
            SerializedProperty startPosition = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.startPosition));
            startPosition.floatValue = 0f;
            SerializedProperty reverse = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.reverse));
            reverse.boolValue = false;
            SerializedProperty stereoPan = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.stereoPan));
            stereoPan.floatValue = 0f;

            SerializedProperty polyphony = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.polyphony));
            polyphony.intValue = 1;
            SerializedProperty distanceScale = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.distanceScale));
            distanceScale.floatValue = 1f;
            SerializedProperty distortionIncrease = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.distortionIncrease));
            distortionIncrease.floatValue = 0f;

            SerializedProperty fadeInLength = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.fadeInLength));
            fadeInLength.floatValue = 0f;
            SerializedProperty fadeInShape = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.fadeInShape));
            fadeInShape.floatValue = 2f;

            SerializedProperty fadeOutLength = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.fadeOutLength));
            fadeOutLength.floatValue = 0f;
            SerializedProperty fadeOutShape = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.fadeOutShape));
            fadeOutShape.floatValue = -2f;

            SerializedProperty followPosition = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.followPosition));
            followPosition.boolValue = true;

            SerializedProperty bypassReverbZones = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.bypassReverbZones));
            bypassReverbZones.boolValue = false;
            SerializedProperty bypassVoiceEffects = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.bypassVoiceEffects));
            bypassVoiceEffects.boolValue = false;
            SerializedProperty bypassListenerEffects = settingProperty.FindPropertyRelative(nameof(SoundEventModifier.bypassListenerEffects));
            bypassListenerEffects.boolValue = false;
        }

        public static void ModifierMenuShow(SoundEventModifier[] settings, UnityEngine.Object[] mTargets) {

            if (settings[0] == null) {
                return;
            }

            GenericMenu menu = new GenericMenu();

            // Tooltips dont work for menu
            menu.AddItem(new GUIContent("Volume"), settings[0].volumeUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Volume, settings, mTargets));
            menu.AddItem(new GUIContent("Pitch"), settings[0].pitchUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Pitch, settings, mTargets));
            menu.AddItem(new GUIContent("Delay"), settings[0].delayUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Delay, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Start Position"), settings[0].startPositionUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.StartPosition, settings, mTargets));
            menu.AddItem(new GUIContent("Reverse"), settings[0].reverseUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Reverse, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Distance Scale"), settings[0].distanceScaleUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.DistanceScale, settings, mTargets));
            menu.AddItem(new GUIContent("Reverb Zone Mix"), settings[0].reverbZoneMixUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.ReverbZoneMix, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Fade In Length"), settings[0].fadeInLengthUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.FadeInLength, settings, mTargets));
            menu.AddItem(new GUIContent("Fade In Shape"), settings[0].fadeInShapeUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.FadeInShape, settings, mTargets));
            menu.AddItem(new GUIContent("Fade Out Length"), settings[0].fadeOutLengthUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.FadeOutLength, settings, mTargets));
            menu.AddItem(new GUIContent("Fade Out Shape"), settings[0].fadeOutShapeUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.FadeOutShape, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Increase 2D"), settings[0].increase2DUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Increase2D, settings, mTargets));
            menu.AddItem(new GUIContent("Stereo Pan"), settings[0].stereoPanUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.StereoPan, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Intensity"), settings[0].intensityUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Intensity, settings, mTargets));
            menu.AddItem(new GUIContent("Distortion Increase"), settings[0].distortionIncreaseUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.DistortionIncrease, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Polyphony"), settings[0].polyphonyUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.Polyphony, settings, mTargets));
            menu.AddItem(new GUIContent("Follow Position"), settings[0].followPositionUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.FollowPosition, settings, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Bypass Reverb Zones"), settings[0].bypassReverbZonesUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.BypassReverbZones, settings, mTargets));
            menu.AddItem(new GUIContent("Bypass Voice Effects"), settings[0].bypassVoiceEffectsUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.BypassVoiceEffects, settings, mTargets));
            menu.AddItem(new GUIContent("Bypass Listener Effects"), settings[0].bypassListenerEffectsUse, ModifierMenuCallback, new MenuSetting(SoundParameterType.BypassListenerEffects, settings, mTargets));

            menu.ShowAsContext();
        }

        private static void ModifierMenuCallback(object obj) {
            MenuSetting menuSetting;

            try {
                menuSetting = (MenuSetting)obj;
            } catch {
                return;
            }

            bool tempUse = false;
            for (int i = 0; i < menuSetting.soundEventModifier.Length; i++) {
                if (menuSetting.soundEventModifier[i] != null) {
                    Undo.RecordObject(menuSetting.mTargets[i], "Setting Toggle");
                    // Volume
                    if (menuSetting.soundParameterType == SoundParameterType.Volume) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].volumeUse;
                        }
                        menuSetting.soundEventModifier[i].volumeUse = tempUse;
                    }
                    // Pitch
                    else if (menuSetting.soundParameterType == SoundParameterType.Pitch) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].pitchUse;
                        }
                        menuSetting.soundEventModifier[i].pitchUse = tempUse;
                    }
                    // Delay
                    else if (menuSetting.soundParameterType == SoundParameterType.Delay) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].delayUse;
                        }
                        menuSetting.soundEventModifier[i].delayUse = tempUse;
                    }
                    // Increase 2D
                    else if (menuSetting.soundParameterType == SoundParameterType.Increase2D) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].increase2DUse;
                        }
                        menuSetting.soundEventModifier[i].increase2DUse = tempUse;
                    }
                    // Intensity
                    else if (menuSetting.soundParameterType == SoundParameterType.Intensity) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].intensityUse;
                        }
                        menuSetting.soundEventModifier[i].intensityUse = tempUse;
                    }
                    // Reverb Zone Mix Ratio
                    else if (menuSetting.soundParameterType == SoundParameterType.ReverbZoneMix) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].reverbZoneMixUse;
                        }
                        menuSetting.soundEventModifier[i].reverbZoneMixUse = tempUse;
                    }
                    // Start Position
                    else if (menuSetting.soundParameterType == SoundParameterType.StartPosition) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].startPositionUse;
                        }
                        menuSetting.soundEventModifier[i].startPositionUse = tempUse;
                    }
                    // Reverse Enabled
                    else if (menuSetting.soundParameterType == SoundParameterType.Reverse) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].reverseUse;
                        }
                        menuSetting.soundEventModifier[i].reverseUse = tempUse;
                    }
                    // Stereo Pan
                    else if (menuSetting.soundParameterType == SoundParameterType.StereoPan) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].stereoPanUse;
                        }
                        menuSetting.soundEventModifier[i].stereoPanUse = tempUse;
                    }
                    // Polyphony
                    else if (menuSetting.soundParameterType == SoundParameterType.Polyphony) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].polyphonyUse;
                        }
                        menuSetting.soundEventModifier[i].polyphonyUse = tempUse;
                    }
                    // Distance Scale
                    else if (menuSetting.soundParameterType == SoundParameterType.DistanceScale) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].distanceScaleUse;
                        }
                        menuSetting.soundEventModifier[i].distanceScaleUse = tempUse;
                    }
                    // Distortion Increase
                    else if (menuSetting.soundParameterType == SoundParameterType.DistortionIncrease) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].distortionIncreaseUse;
                        }
                        menuSetting.soundEventModifier[i].distortionIncreaseUse = tempUse;
                    }
                    // Fade In Length
                    else if (menuSetting.soundParameterType == SoundParameterType.FadeInLength) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].fadeInLengthUse;
                        }
                        menuSetting.soundEventModifier[i].fadeInLengthUse = tempUse;
                    }
                    // Fade In Shape
                    else if (menuSetting.soundParameterType == SoundParameterType.FadeInShape) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].fadeInShapeUse;
                        }
                        menuSetting.soundEventModifier[i].fadeInShapeUse = tempUse;
                    }
                    // Fade Out Length
                    else if (menuSetting.soundParameterType == SoundParameterType.FadeOutLength) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].fadeOutLengthUse;
                        }
                        menuSetting.soundEventModifier[i].fadeOutLengthUse = tempUse;
                    }
                    // Fade Out Shape
                    else if (menuSetting.soundParameterType == SoundParameterType.FadeOutShape) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].fadeOutShapeUse;
                        }
                        menuSetting.soundEventModifier[i].fadeOutShapeUse = tempUse;
                    }
                    // Follow Position
                    else if (menuSetting.soundParameterType == SoundParameterType.FollowPosition) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].followPositionUse;
                        }
                        menuSetting.soundEventModifier[i].followPositionUse = tempUse;
                    }
                    // Bypass Reverb Zones
                    else if (menuSetting.soundParameterType == SoundParameterType.BypassReverbZones) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].bypassReverbZonesUse;
                        }
                        menuSetting.soundEventModifier[i].bypassReverbZonesUse = tempUse;
                    }
                    // Bypass Voice Effects
                    else if (menuSetting.soundParameterType == SoundParameterType.BypassVoiceEffects) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].bypassVoiceEffectsUse;
                        }
                        menuSetting.soundEventModifier[i].bypassVoiceEffectsUse = tempUse;
                    }
                    // Bypass Listener Effects
                    else if (menuSetting.soundParameterType == SoundParameterType.BypassListenerEffects) {
                        if (i == 0) {
                            tempUse = !menuSetting.soundEventModifier[0].bypassListenerEffectsUse;
                        }
                        menuSetting.soundEventModifier[i].bypassListenerEffectsUse = tempUse;
                    }
                    EditorUtility.SetDirty(menuSetting.mTargets[i]);
                }
            }
        }
    }
}
#endif