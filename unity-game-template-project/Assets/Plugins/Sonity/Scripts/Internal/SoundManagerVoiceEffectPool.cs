// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundManagerVoiceEffectPool {

        [NonSerialized]
        public Voice[] voiceEffectPool = new Voice[0];

        public bool VoiceEffectShouldPlay(Voice voice) {
            // Resize Array if voice effect limit has changed
            if (voiceEffectPool.Length != SoundManagerBase.Instance.Internals.settings.voiceEffectLimit) {
                if (voiceEffectPool.Length < SoundManagerBase.Instance.Internals.settings.voiceEffectLimit) {
                    // If there are too few
                    Array.Resize(ref voiceEffectPool, SoundManagerBase.Instance.Internals.settings.voiceEffectLimit);
                } else {
                    // If there are too many
                    for (int i = 0; i < SoundManagerBase.Instance.Internals.settings.voiceEffectLimit - voiceEffectPool.Length; i++) {
                        // Disable them before removing them
                        if (voiceEffectPool[SoundManagerBase.Instance.Internals.settings.voiceEffectLimit + i] != null) {
                            voiceEffectPool[SoundManagerBase.Instance.Internals.settings.voiceEffectLimit + i].cachedVoiceEffect.SetEnabled(false);
                        }
                    }
                    Array.Resize(ref voiceEffectPool, SoundManagerBase.Instance.Internals.settings.voiceEffectLimit);
                }
            }

            for (int i = 0; i < voiceEffectPool.Length; i++) {
                // If already contains itself
                if (voiceEffectPool[i] == voice) {
                    return true;
                }
            }

            for (int i = 0; i < voiceEffectPool.Length; i++) {
                // If slot is null
                if (voiceEffectPool[i] == null) {
                    voiceEffectPool[i] = voice;
                    return true;
                }
            }
            
            for (int i = 0; i < voiceEffectPool.Length; i++) {
                // If not enabled
                if (!voiceEffectPool[i].cachedVoiceEffect.GetEnabled()) {
                    voiceEffectPool[i] = voice;
                    return true;
                }
            }

            for (int i = 0; i < voiceEffectPool.Length; i++) {
                // If volume is lower than new voice
                if (!voiceEffectPool[i].soundContainer.internals.data.neverStealVoiceEffects 
                    && voiceEffectPool[i].GetVolumeRatioWithoutFadeWithPriority() < voice.GetVolumeRatioWithoutFadeWithPriority()
                    ) {
                    // Steal voice effect
                    voiceEffectPool[i].cachedVoiceEffect.SetEnabled(false);
                    voiceEffectPool[i] = voice;
                    return true;
                }
            }
            return false;
        }
    }
}