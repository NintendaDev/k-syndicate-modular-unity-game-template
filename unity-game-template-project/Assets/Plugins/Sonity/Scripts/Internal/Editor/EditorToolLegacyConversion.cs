// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

namespace Sonity.Internal {

    public static class EditorToolLegacyConversionConvertSoundEventVolumePolyphony {

        [MenuItem("Tools/Sonity 🔊/Legacy Conversion/Convert SoundEvent Modifier Volume and Polyphony to Base", false, 110)] // Sonity
        private static void ConvertSoundEventVolumePolyphony() {
            foreach (UnityEngine.Object objectInstance in Selection.objects) {
                SoundEventBase soundEvent = null;
                try {
                    soundEvent = (SoundEventBase)objectInstance;
                } catch {

                }
                if (soundEvent != null) {
                    bool changeAnything = false;
                    if (soundEvent.internals.data.soundEventModifier.volumeUse) {
                        changeAnything = true;
                        soundEvent.internals.data.soundEventModifier.volumeUse = false;
                        soundEvent.internals.data.volumeRatio *= soundEvent.internals.data.soundEventModifier.volumeRatio;
                        soundEvent.internals.data.volumeDecibel = VolumeScale.ConvertRatioToDecibel(soundEvent.internals.data.volumeRatio);
                        soundEvent.internals.data.soundEventModifier.volumeRatio = 1f;
                        soundEvent.internals.data.soundEventModifier.volumeDecibel = 0f;
                    }
                    if (soundEvent.internals.data.soundEventModifier.polyphonyUse) {
                        changeAnything = true;
                        soundEvent.internals.data.soundEventModifier.polyphonyUse = false;
                        soundEvent.internals.data.polyphony *= soundEvent.internals.data.soundEventModifier.polyphony;
                        soundEvent.internals.data.soundEventModifier.polyphony = 1;
                    }
                    if (changeAnything) {
                        Debug.Log("Sonity.SoundEvent: \"" + soundEvent.name + "\" converted SoundEvent modifier volume and polyphony to base");
                    }
                    EditorUtility.SetDirty(objectInstance);
                }
            }
        }
    }


    public static class EditorToolLegacyConversionApplySoundContainerAudioMixerGroupToSoundEvent {

        [MenuItem("Tools/Sonity 🔊/Legacy Conversion/Apply SoundContainer AudioMixerGroup to SoundEvent if None", false, 111)] // Sonity
        private static void ConvertSoundEventVolumePolyphony() {
            foreach (UnityEngine.Object objectInstance in Selection.objects) {
                SoundEventBase soundEvent = null;
                try {
                    soundEvent = (SoundEventBase)objectInstance;
                } catch {

                }
                if (soundEvent != null) {
                    // Only take if SoundEvent AudioMixerGroup is null
                    if (soundEvent.internals.data.audioMixerGroup == null) {
                        if (soundEvent.internals.soundContainers.Length > 0) {
                            if (soundEvent.internals.soundContainers[0] != null) {
                                if (soundEvent.internals.soundContainers[0].internals.data.audioMixerGroup != null) {
                                    soundEvent.internals.data.audioMixerGroup = soundEvent.internals.soundContainers[0].internals.data.audioMixerGroup;
                                    Debug.Log("Sonity.SoundEvent: \"" + soundEvent.name + "\" took AudioMixerGroup \"" + soundEvent.internals.soundContainers[0].internals.data.audioMixerGroup.name + "\" from \"" + soundEvent.internals.soundContainers[0].name + "\"");
                                }
                            }
                        }
                    }
                    EditorUtility.SetDirty(objectInstance);
                }
            }
        }
    }
}
#endif