// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundDataGroupBase">SoundDataGroup</see> objects are used to easily load and unload the audio data of the  <see cref="SoundEventBase">SoundEvents</see>.
    /// All <see cref="SoundDataGroupBase"/ objects are multi-object editable.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "_DAT", menuName = "Sonity 🔊/SoundDataGroup", order = 106)] // Having a big gap in indexes creates dividers
    public class SoundDataGroup : SoundDataGroupBase {

        /// <summary>
        /// Loads the audio data for the <see cref="AudioClip"/>s of the assigned <see cref="SoundEventBase">SoundEvents</see>.
        /// </summary>
        /// <param name="includeChildren">
        /// If to load all the audio data of all the child <see cref="SoundDataGroupBase">SoundDataGroups</see> also.
        /// </param>
        public void LoadAudioData(bool includeChildren) {
            internals.LoadUnloadAudioData(true, includeChildren, this);
        }

        /// <summary>
        /// Unloads the audio data for the <see cref="AudioClip"/>s of the assigned <see cref="SoundEventBase">SoundEvents</see>.
        /// </summary>
        /// <param name="includeChildren">
        /// If to unload all the audio data of all the child <see cref="SoundDataGroupBase">SoundDataGroups</see> also.
        /// </param>
        public void UnloadAudioData(bool includeChildren) {
            internals.LoadUnloadAudioData(false, includeChildren, this);
        }
    }
}