// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR


namespace Sonity.Internal {

    public class EditorTextSoundPicker {

        public static string soundPickerTooltip =
            $"{nameof(NameOf.SoundPicker)} is a serializable class for easily selecting multiple {nameof(NameOf.SoundEvent)}s and modifiers." + "\n" +
            "\n" +
            $"Add a serialized or public {nameof(NameOf.SoundPicker)} to a C# script and edit it in the inspector." + "\n" +
            "\n" +
            $"The {nameof(NameOf.SoundPicker)} is not nestable in an array or custom class because it is built upon CustomPropertyDrawer." + "\n" +
            "\n" +
            $"This is because custom serializable classes do not support inheritance and polymorphism for serialization." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundPicker)} are multi-object editable." + EditorTrial.trialTooltip;

        public static string soundEventLabel = $"{nameof(NameOf.SoundEvent)}";
        public static string soundEventTooltip = $"The {nameof(NameOf.SoundEvent)} to play." + EditorTrial.trialTooltip;
    }
}
#endif