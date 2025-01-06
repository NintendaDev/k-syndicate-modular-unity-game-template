// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public enum EditorSoundContainerCurveType {
        Volume,
        Pitch,
        StereoPan,
        ReverbZoneMix,
        SpatialBlend,
        SpatialSpread,
        Distortion,
        LowpassFrequency,
        LowpassAmount,
        HighpassFrequency,
        HighpassAmount,
    }
}
#endif