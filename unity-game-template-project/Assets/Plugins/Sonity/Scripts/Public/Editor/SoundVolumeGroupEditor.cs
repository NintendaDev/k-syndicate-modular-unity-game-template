// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    [CustomEditor(typeof(SoundVolumeGroup))]
    [CanEditMultipleObjects]
    public class SoundVolumeGroupEditor : SoundVolumeGroupEditorBase {

    }
}
#endif