// Created by Victor Engstr�m
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    [CustomEditor(typeof(SoundPhysics))]
    [CanEditMultipleObjects]
    public class SoundPhysicsEditor : SoundPhysicsEditorBase {

    }
}
#endif