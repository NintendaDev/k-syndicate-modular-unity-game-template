// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// If “Override Listener Distance” is enabled in the SoundManager Settings this is a required component in the scene.
    /// The position of the AudioListenerDistance component will determine all distance based calculations (like volume falloff).
    /// While the AudioListener position will be used for spatialization and Angle to Stereo Pan calculations.
    /// Example of usage in a 3rd person or top down game:
    /// Enable "Override Listener Distance" in the <see cref="SoundManager"/>.
    /// Put the AudioListener on the main camera and the <see cref="AudioListenerDistanceBase">AudioListenerDistance</see> on the player character.
    /// Try changing the Amount slider to find a nice balance between the different positions.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity 🔊/Sonity - Audio Listener Distance")]
    public class AudioListenerDistance : AudioListenerDistanceBase {

    }
}