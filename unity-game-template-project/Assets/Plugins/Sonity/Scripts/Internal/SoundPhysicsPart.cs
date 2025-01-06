// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPhysicsPart {

#if UNITY_EDITOR
        // Legacy, left so it can be copied to the array by the editor
        public SoundEventBase soundEvent;
#endif
        public SoundEventBase[] soundEvents = new SoundEventBase[1];

        public SoundTagBase soundTag;

        [NonSerialized]
        public bool frictionIsPlaying = false;

        public PhysicsPlayOn physicsPlayOn = PhysicsPlayOn.OnCollision;

        public bool conditionsUse = false;
        public SoundPhysicsConditionBase[] conditions = new SoundPhysicsConditionBase[1];
    }
}