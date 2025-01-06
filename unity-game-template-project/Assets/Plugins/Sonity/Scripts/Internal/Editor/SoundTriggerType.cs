// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public enum SoundTriggerType {

        OnEnable,
        OnDisable,
        OnStart,
        OnDestroy,

        OnTriggerEnter,
        OnTriggerExit,
        OnTriggerEnter2D,
        OnTriggerExit2D,

        OnCollisionEnter,
        OnCollisionExit,
        OnCollisionEnter2D,
        OnCollisionExit2D,

        OnMouseEnter,
        OnMouseExit,
        OnMouseDown,
        OnMouseUp,
    }
}
#endif