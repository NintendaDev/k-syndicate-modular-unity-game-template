// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundEventSoundTagGroup {
        public SoundTagBase soundTag;
        public SoundEventModifier soundEventModifierBase = new SoundEventModifier();
        public SoundEventModifier soundEventModifierSoundTag = new SoundEventModifier();
        public SoundEventBase[] soundEvent = new SoundEventBase[1];
    }
}