// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundVolumeGroupBase">SoundVolumeGroup</see> objects are used for grouped live volume control of multiple <see cref="SoundEventBase">SoundEvents</see>.
    /// You can assign them in the <see cref="SoundEventBase">SoundEvent</see> settings.
    /// It is perfect for quickly mixing the volume of a large group of sounds.
    /// It also supports +12dB volume if you “Enable Volume Increase” in the <see cref="SoundManagerBase">SoundManager</see>.
    /// All <see cref="SoundVolumeGroupBase">SoundVolumeGroup</see> objects are multi-object editable.
    /// Example use: Add the same <see cref="SoundVolumeGroupBase">SoundVolumeGroup</see> to all e.g. gunshot <see cref="SoundEventBase">SoundEvents</see> so you quickly can change their volumes.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_VOL", menuName = "Sonity 🔊/SoundVolumeGroup", order = 102)] // Having a big gap in indexes creates dividers
    public class SoundVolumeGroup : SoundVolumeGroupBase {

    }
}