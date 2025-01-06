// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    [CustomPropertyDrawer(typeof(SoundPicker))]
    public class SoundPickerEditor : SoundPickerEditorBase {

    }
}
#endif