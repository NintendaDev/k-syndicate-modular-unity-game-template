// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

#if UNITY_EDITOR

namespace Sonity.Internal {

    public abstract class SoundPresetBase : ScriptableObject {

        public SoundPresetInternals internals = new SoundPresetInternals();
    }
}
#endif