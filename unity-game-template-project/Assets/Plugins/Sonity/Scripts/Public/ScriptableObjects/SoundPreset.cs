// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

#if UNITY_EDITOR

namespace Sonity {

    /// <summary>
    /// Use <see cref="SoundPresetBase">SoundPresets</see> to apply settings to <see cref="SoundContainerBase">SoundContainers</see> and <see cref="SoundEventBase">SoundEvents</see> automatically by matching the names of your assets.
    /// Its matched and applied when you Create Assets From Selection or manually from the <see cref="SoundContainerBase">SoundContainer</see> Presets and the <see cref="SoundEventBase">SoundEvent</see> Presets toolbars.
    /// Example use: Name UI sounds to “UI_” and use Is Prefix “UI_” to automatically assign settings like disabling distance and setting Spatial Blend to 0.
    /// The settings are applied ordering from the one highest up and going downwards and can match an asset multiple times overwriting previous values.
    /// You can have multiple <see cref="SoundPresetBase">SoundPreset</see> objects.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "SoundPreset_", menuName = "Sonity 🔊/SoundPreset", order = 300)] // Having a big gap in indexes creates dividers
    public class SoundPreset : SoundPresetBase {

    }
}
#endif