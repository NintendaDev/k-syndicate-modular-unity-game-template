// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundPolyGroupBase">SoundPolyGroup</see> objects are used to create a polyphony limit shared by multiple different <see cref="SoundEventBase">SoundEvents</see>.
    /// You can assign them in the <see cref="SoundEventBase">SoundEvent</see> settings.
    /// The priority for voice allocation is calculated by multiplying the priority set in the <see cref="SoundEventBase">SoundEvent</see> by the volume of the instance.
    /// A perfect use case would be to have a <see cref="SoundPolyGroupBase">SoundPolyGroup</see> for all bullet impacts of all the different materials so they combined dont use too many voices.
    /// If you want simple individual polyphony control, use the polyphony modifier on the <see cref="SoundEventBase">SoundEvent</see>.
    /// All <see cref="SoundPolyGroupBase">SoundPolyGroup</see> objects are multi-object editable.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_POL", menuName = "Sonity 🔊/SoundPolyGroup", order = 103)] // Having a big gap in indexes creates dividers
    public class SoundPolyGroup : SoundPolyGroupBase {

    }
}