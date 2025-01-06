// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundMixBase">SoundMix</see> objects are used for grouped control of e.g. volume or distance scale for multiple <see cref="SoundEventBase">SoundEvent</see> at the same time.
    /// You can assign them in the <see cref="SoundEventBase">SoundEvent</see> settings.
    /// Because SoundMix use modifiers they only calculate the values once when the <see cref="SoundEventBase">SoundEvent</see> is started.
    /// If you want to have realtime volume control over sounds, use an AudioMixerGroup.
    /// All <see cref="SoundMixBase">SoundMix</see> objects are multi-object editable.
    /// Example use: Set up a “Master_MIX” and a “SFX_MIX” where the Master_MIX is a parent of the SFX_MIX.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_MIX", menuName = "Sonity 🔊/SoundMix", order = 105)] // Having a big gap in indexes creates dividers
    public class SoundMix : SoundMixBase {

    }
}