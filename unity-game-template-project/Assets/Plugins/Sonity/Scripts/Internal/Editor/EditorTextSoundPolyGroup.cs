// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public class EditorTextSoundPolyGroup {

        public static string soundPolyGroupTooltip =
            $"{nameof(NameOf.SoundPolyGroup)} objects are used to create a polyphony limit shared by multiple different {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"You can assign them in the {nameof(NameOf.SoundEvent)} settings." + "\n" +
            "\n" +
            $"The priority for voice allocation is calculated by multiplying the priority set in the {nameof(NameOf.SoundEvent)} by the volume of the instance." + "\n" +
            "\n" +
            $"A perfect use case would be to have a {nameof(NameOf.SoundPolyGroup)} for all bullet impacts of all the different materials so that when combined, they don’t use too many voices." + "\n" +
            "\n" +
            $"If you want simple individual polyphony control, use the polyphony modifier on the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundPolyGroup)} objects are multi-object editable." + EditorTrial.trialTooltip;

        public static string polyphonyLimitLabel = "Polyphony Limit";
        public static string polyphonyLimitTooltip = $"The maximum number of {nameof(NameOf.SoundEvent)}s which can be played at the same time in this {nameof(NameOf.SoundPolyGroup)}." + EditorTrial.trialTooltip;
    }
}
#endif