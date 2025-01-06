// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public enum SoundParameterType {

        Volume,
        Pitch,
        Delay,
        Increase2D,
        Intensity,

        ReverbZoneMix,
        StartPosition,
        Reverse,
        StereoPan,

        Polyphony,
        DistanceScale,
        DistortionIncrease,

        FadeInLength,
        FadeInShape,

        FadeOutLength,
        FadeOutShape,

        FollowPosition,

        BypassReverbZones,
        BypassVoiceEffects,
        BypassListenerEffects,
    }
}