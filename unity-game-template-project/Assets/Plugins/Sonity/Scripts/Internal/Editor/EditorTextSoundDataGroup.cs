// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public class EditorTextSoundDataGroup {

        public static string soundDataGroupTooltip =
            $"{nameof(NameOf.SoundDataGroup)} objects are used to easily load and unload the audio data of the {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundDataGroup)} objects are multi-object editable." + EditorTrial.trialTooltip;

        public static string childSoundDataGroupsLabel = $"Child {nameof(NameOf.SoundDataGroup)}s";
        public static string childSoundDataGroupsTooltip = $"Nesting {nameof(NameOf.SoundDataGroup)}s makes it easy to load/unload all audio data or just parts of it." + EditorTrial.trialTooltip;

        public static string soundEventsLabel = $"{nameof(NameOf.SoundEvent)}s";
        public static string soundEventsTooltip = $"The {nameof(NameOf.SoundEvent)} whoms audio data will be loaded or unloaded." + EditorTrial.trialTooltip;
    }
}
#endif