// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTextSoundTag {

        public static string soundTagTooltip =
        $"{nameof(NameOf.SoundTag)} objects are passed to modify how a {nameof(NameOf.SoundEvent)} should be played." + "\n" +
        "\n" +
        $"You can assign them in the {nameof(NameOf.SoundEvent)}s {nameof(NameOf.SoundTag)} section." + "\n" +
        "\n" +
        $"You can either pass them when playing a {nameof(NameOf.SoundEvent)} for setting the local {nameof(NameOf.SoundTag)}." + "\n" +
        "\n" +
        $"Or you can set the global {nameof(NameOf.SoundTag)} in the {nameof(NameOf.SoundManager)}." + "\n" +
        "\n" +
        $"This is useful for e.g; weapon reverb zones." + "\n" +
        "\n" +
        $"Because you can set the {nameof(NameOf.SoundTag)} corresponding to the acoustic space which the listener is in." + "\n" +
        "\n" +
        $"And when you play the {nameof(NameOf.SoundEvent)}, your gun reflection layers can correspond to the acoustic space." + EditorTrial.trialTooltip;
    }
}
#endif