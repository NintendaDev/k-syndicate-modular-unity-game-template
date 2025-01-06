// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR


namespace Sonity.Internal {

    public static class EditorTextPreview {

        public static string soundContainerPlayLabel = "Play";
        public static string soundContainerPlayTooltip =
            $"Previews the {nameof(NameOf.SoundContainer)}." + "\n" +
            "\n" +
            $"Does not work if Unity cannot build the project, or if the game is paused." + "\n" +
            "\n" +
            $"The default shortcut is Ctrl+Q." + EditorTrial.trialTooltip;

        public static string soundEventBasePlayLabel = "Play";
        public static string soundEventBasePlayTooltip =
            $"Previews the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"Does not work if Unity cannot build the project, or if the game is paused." + "\n" +
            "\n" +
            $"Preview doesn't play more than one level of TriggerOnPlay/Stop/Tail at the moment (there is full functionality ingame)." + "\n" +
            "\n" +
            $"The default shortcut is Ctrl+Q." + EditorTrial.trialTooltip;

        public static string soundEventSoundTagPlayLabel = "Play";
        public static string soundEventSoundTagPlayTooltip =
            $"Previews the {nameof(NameOf.SoundEvent)} with {nameof(NameOf.SoundTag)}." + "\n" +
            "\n" +
            $"Does not work if Unity cannot build the project, or if the game is paused." + "\n" +
            "\n" +
            $"The default shortcut is Ctrl+Q." + EditorTrial.trialTooltip;

        public static string stopLabel = "Stop";
        public static string stopTooltip = 
            "Press two times to skip the fade out." + "\n" +
            "\n" +
            $"The default shortcut is Ctrl+W." + EditorTrial.trialTooltip;

        public static string resetLabel = "Reset";
        public static string resetTooltip = "Resets the preview settings." + EditorTrial.trialTooltip;

        public static string intensityLabel = "Intensity";
        public static string intensityTooltip = $"Controls the intensity value of the played {nameof(NameOf.SoundContainer)}s." + EditorTrial.trialTooltip;

        public static string audioMixerGroupLabel = "AudioMixerGroup";
        public static string audioMixerGroupTooltip = $"Only used for preview." + EditorTrial.trialTooltip;

        public static string boxFront = "Front";
        public static string boxBack = "Back";
        public static string boxLeft = "Left";
        public static string boxRight = "Right";
    }
}
#endif