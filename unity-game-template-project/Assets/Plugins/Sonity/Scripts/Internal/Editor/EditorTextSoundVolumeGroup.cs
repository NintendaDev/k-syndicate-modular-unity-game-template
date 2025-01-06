// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public static class EditorTextSoundVolumeGroup {

        public static string soundVolumeGroupTooltip =
            $"{nameof(NameOf.SoundVolumeGroup)} objects are used for grouped live volume control of multiple {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"You can assign them in the {nameof(NameOf.SoundEvent)} settings." + "\n" +
            "\n" +
            $"It is perfect for quickly mixing the volume of a large group of sounds." + "\n" +
            "\n" +
            $"It also supports +12dB volume if you “Enable Volume Increase” in the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundVolumeGroup)} objects are multi-object editable." + "\n" +
            "\n" +
            $"Example use: Add the same {nameof(NameOf.SoundVolumeGroup)} to all e.g. gunshot {nameof(NameOf.SoundEvent)}s so you quickly can change their volumes." + EditorTrial.trialTooltip;

        public static string volumeLabel = "Volume dB";
        public static string volumeTooltip =
            $"Volume offset in decibel." + "\n" +
            "\n" +
            $"If you want to be able to raise the volume and not just lower it you have 2 options:" + "\n" +
            "\n" +
            $"Option A:" + "\n" +
            "\n" +
            $"Enable \"Enable Volume Increase\" in the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            $"This will enable you to raise the volumes of {nameof(NameOf.SoundContainer)}s and {nameof(NameOf.SoundEvent)}s by +24 dB each and {nameof(NameOf.SoundVolumeGroup)}s by +12dB." + "\n" +
            "\n" +
            $"Option B:" + "\n" +
            "\n" +
            $"Select all the {nameof(NameOf.SoundVolumeGroup)}s and lower the volume by -20 dB with the -1dB button." + "\n" +
            "\n" +
            $"Then to compensate you can increase the global volume with an Audio Mixer (which you then can set to +20 dB)." + EditorTrial.trialTooltip;

        public static string volumeRelativeLowerLabel = "-1 dB";
        public static string volumeRelativeLowerTooltip =
            $"Lowers the relative volume of all the selected {nameof(NameOf.SoundVolumeGroup)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to raise the volume of one {nameof(NameOf.SoundVolumeGroup)} and keep the relative volume." + "\n" +
            "\n" +
            $"Because then you can lower all of them to get more headroom." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundVolumeGroup)}s are selected it will show the lowest volume." + EditorTrial.trialTooltip;

        public static string volumeRelativeIncreaseLabel = "+1 dB";
        public static string volumeRelativeIncreaseTooltip =
            $"Raises the relative volume of all the selected {nameof(NameOf.SoundVolumeGroup)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to set the loudest volume to 0 dB but keep the relative volumes." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundVolumeGroup)}s are selected it will show the highest volume." + EditorTrial.trialTooltip;

        public static string volumeOverLimitWarning =
            $"Volume is over 0dB, please add scripting define symbol:" + "\n" +
            $"\"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume." + "\n" +
            $"Tip: Select all {nameof(NameOf.SoundVolumeGroup)}s and press \"- 1dB\" until max is 0dB.";
    }
}
#endif