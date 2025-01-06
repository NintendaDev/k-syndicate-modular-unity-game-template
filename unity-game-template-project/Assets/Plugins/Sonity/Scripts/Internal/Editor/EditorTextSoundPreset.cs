// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR


namespace Sonity.Internal {

    public class EditorTextSoundPreset {

        public static string soundPresetTooltip =
            $"Use {nameof(NameOf.SoundPreset)} to apply settings to {nameof(NameOf.SoundContainer)}s and {nameof(NameOf.SoundEvent)}s automatically by matching the names of your assets." + "\n" +
            "\n" +
            $"Its matched and applied when you Create Assets From Selection or manually from the {nameof(NameOf.SoundContainer)} Presets and the {nameof(NameOf.SoundEvent)} Presets toolbars." + "\n" +
            "\n" +
            $"Example use: Name UI sounds to “UI_” and use Is Prefix “UI_” to automatically assign settings like disabling distance and setting Spatial Blend to 0." + "\n" +
            "\n" +
            $"The settings are applied ordering from the one highest up and going downwards and can match an asset multiple times overwriting previous values." + "\n" +
            "\n" +
            $"You can have multiple SoundPreset objects." + EditorTrial.trialTooltip;

        public static string disableAllLabel = $"Disable All";
        public static string disableAllTooltip = $"If disabled no settings from the {nameof(NameOf.SoundPreset)} will be applied automatically and not available in the preset menus." + EditorTrial.trialTooltip;

        public static string automaticLoopLabel = $"Automatic Loop";
        public static string automaticLoopTooltip =
            $"If enabled and the {nameof(NameOf.SoundContainer)} name contains “loop” then it will automatically enable “Loop”, “Follow Position”, “Stop if Transform is Null” and “Random Start Position”." + "\n" +
            "\n" +
            $"This will be applied after all the other settings are copied." + "\n" +
            "\n" +
            $"Is not applied if you manually select a preset from the SoundContainer Presets dropdown." + EditorTrial.trialTooltip;

        public static string disableLabel = $"Disable";
        public static string disableTooltip = $"If disabled this specific preset won’t be available." + EditorTrial.trialTooltip;

        public static string fromSoundContainerLabel = $"From SoundContainer";
        public static string fromSoundContainerTooltip =
            $"The {nameof(NameOf.SoundContainer)} you want to copy the settings from." + "\n" +
            "\n" +
            $"Copies everything except:" + "\n" +
            $"- AudioClips" + EditorTrial.trialTooltip;

        public static string fromSoundEventLabel = $"From SoundEvent";
        public static string fromSoundEventTooltip =
            $"The {nameof(NameOf.SoundEvent)} you want to copy the settings from." + "\n" +
            "\n" +
            $"Copies everything except:" + "\n" +
            $"- {nameof(NameOf.SoundContainer)}s" + "\n" +
            $"- Timeline Settings" + "\n" +
            $"- Intensity Scale Add/Multiplier" + "\n" +
            $"- TriggerOn SoundEvents" + "\n" +
            $"- SoundTag SoundEvents" + EditorTrial.trialTooltip;

        public static string matchNameLabel = $"Auto Match";
        public static string matchNameTooltip =
            $"If enabled then automatic matching will be used when you Create Assets From Selection and in the {nameof(NameOf.SoundContainer)} Presets and the {nameof(NameOf.SoundEvent)} Presets “Auto Match”." + "\n" +
            "\n" +
            $"Disable if you just want presets to apply manually to your assets." + "\n" +
            "\n" +
            $"Auto matching is not case sensitive." + EditorTrial.trialTooltip;

        public static string applyOnAllLabel = $"Apply On All";
        public static string applyOnAllTooltip =
            $"If enabled the settings assigned will be applied to all assets on asset creation and “Auto Match”." + "\n" +
            "\n" +
            $"Useful to combine with other settings with conditions added later in the chain to assign settings to assets which doesn’t match the names." + EditorTrial.trialTooltip;

        public static string isPrefixLabel = $"Is Prefix";
        public static string isPrefixTooltip =
            $"If the prefix of an asset matches the strings, the settings will be applied." + "\n" +
            "\n" +
            $"Example prefixes: “AMB”, “MUS”, “SFX”, “UI”, “VO”." + EditorTrial.trialTooltip;

        public static string isNotPrefixLabel = $"Is Not Prefix";
        public static string isNotPrefixTooltip = $"If the prefix of an asset doesn’t match the strings, the settings will be applied." + EditorTrial.trialTooltip;

        public static string containsLabel = $"Contains";
        public static string containsTooltip = $"If the name of an asset contains the strings, the settings will be applied." + EditorTrial.trialTooltip;

        public static string notContainsLabel = $"Not Contains";
        public static string notContainsTooltip = $"If the name of an asset doesn’t contain the strings, the settings will be applied." + EditorTrial.trialTooltip;

    }
}
#endif