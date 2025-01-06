// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public class SoundEventInstanceVoiceHolder {

        public SoundEventPlayTypeInstance playTypeInstance = new SoundEventPlayTypeInstance();

        public VoiceParameterInstance voiceParameter = new VoiceParameterInstance();
        public VoiceFade voiceFade = new VoiceFade();
        public Voice voice;

        public float maxRange;
        public bool shouldRestartIfLoop;
        public bool voiceIsToPlay;
        public bool triggerOnTailHasPlayed = false;

        public void PoolSingleVoice(bool shouldRestartIfLoop, bool isCalledByOnDestroy) {
            if (voice != null) {
                SoundManagerBase.Instance.Internals.voicePool.PoolVoice(voice, isCalledByOnDestroy);
                voice = null;
            }
            this.shouldRestartIfLoop = shouldRestartIfLoop;
        }
    }
}