// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTextSoundMix {

        public static string soundMixTooltip =
            $"{nameof(NameOf.SoundMix)} objects are used for grouped control of e.g. volume or distance scale for multiple {nameof(NameOf.SoundEvent)} at the same time." + "\n" +
            "\n" +
            $"You can assign them in the {nameof(NameOf.SoundEvent)} settings." + "\n" +
            "\n" +
            $"Because {nameof(NameOf.SoundMix)} use modifiers they only calculate the values once when the {nameof(NameOf.SoundEvent)} is started." + "\n" +
            "\n" +
            $"If you want to have realtime volume control over sounds, use an AudioMixerGroup." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundMix)} objects are multi-object editable." + "\n" +
            "\n" +
            $"Example use: Set up a “Master_MIX” and a “SFX_MIX” where the Master_MIX is a parent of the SFX_MIX." + EditorTrial.trialTooltip;
    }
}
#endif