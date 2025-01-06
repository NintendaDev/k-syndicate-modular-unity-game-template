// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorTextSoundEvent {

        public static string soundEventTooltip = 
        $"{nameof(NameOf.SoundEvent)}s are what you play in Sonity." + "\n" +
        "\n" +
        $"They contain {nameof(NameOf.SoundContainer)} and options of how the sound should be played." + "\n" +
        "\n" +
        $"All {nameof(NameOf.SoundEvent)}s are multi-object editable." + EditorTrial.trialTooltip;

        public static string presetsLabel = $"Presets";
        public static string presetsTooltip = 
            $"{nameof(NameOf.SoundPreset)}:" + "\n" +
            "\n" +
            $"Create {nameof(NameOf.SoundPreset)} objects to get custom settings which you can apply to your {nameof(NameOf.SoundContainer)}s and {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"Either manually select the preset you want to apply in the dropdown, or use “Auto Match” to automatically apply the settings based on the name of the asset." + EditorTrial.trialTooltip;

        // Reset
        public static string resetSettingsLabel = $"Reset Options";
        public static string resetSettingsTooltip = $"Resets all but the {nameof(NameOf.SoundContainer)}s." + EditorTrial.trialTooltip;

        public static string resetAllLabel = $"Reset All";
        public static string resetAllTooltip = $"Resets everything." + EditorTrial.trialTooltip;

        // Find References
        public static string findReferencesLabel = $"Find References";
        public static string findReferencesTooltip = $"Finds all the references to the {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string findReferencesSelectAllLabel = $"Select All";
        public static string findReferencesSelectAllTooltip = $"Selects all the assets with references to the {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string findReferencesClearLabel = $"Clear";
        public static string findReferencesClearTooltip = $"Removes all the found references." + EditorTrial.trialTooltip;

        // Warnings
        public static string soundContainerWarningEmpty = $"Empty {nameof(NameOf.SoundContainer)}";
        public static string soundContainerWarningNull = $"Null {nameof(NameOf.SoundContainer)}";

        public static string soundTagWarningEmpty = $"Empty {nameof(NameOf.SoundTag)}";

        public static string triggerOtherSoundEventsWarningEmpty = $"Empty {nameof(NameOf.SoundEvent)}";
        public static string triggerOtherSoundEventsWarningNull = $"Null {nameof(NameOf.SoundEvent)}";

        public static string triggerOnTailWarningNoAudioClipFound = $"No {nameof(AudioClip)} found on the first {nameof(NameOf.SoundContainer)}";
        public static string triggerOnTailWarningTailLengthIsTooLong = $"Tail Length is longer than the shortest {nameof(AudioClip)} on the first {nameof(NameOf.SoundContainer)}";
        public static string triggerOnTailWarningLengthWarning = $"The shortest {nameof(AudioClip)} length is";

        // Mute, Solo, Disable
        public static string muteEnableLabel = $"Mute";
        public static string muteEnableTooltip = 
            $"Mutes the {nameof(NameOf.SoundEvent)}. " + "\n" +
            $"Only affects the Unity Editor." + EditorTrial.trialTooltip;

        public static string soloEnableLabel = $"Solo";
        public static string soloEnableTooltip = 
            $"Mutes all other {nameof(NameOf.SoundEvent)}s who don't have solo enabled." + "\n" +
            "\n" +
            $"The Solo property is not serialized (e.g. will be reset on start)." + "\n" +
            "\n" +
            $"This is to prevent leaving a {nameof(NameOf.SoundEvent)} soloed by mistake which would make nothing else sound when the game runs." + "\n" +
            "\n" +
            $"Only affects the Unity Editor." + EditorTrial.trialTooltip;

        public static string disableEnableLabel = $"Disable";
        public static string disableEnableTooltip =
            $"Disables the playing of the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"It is also disabled when building the project." + EditorTrial.trialTooltip;

        public static string findSoundContainersLabel = "Find SoundContainers";
        public static string findSoundContainersTooltip =
            $"Automatically finds all {nameof(NameOf.SoundContainer)}s containing the same name as this {nameof(NameOf.SoundEvent)} (disregarding _SE, numbers)." + "\n" +
            "\n" +
            $"Can be used on multiple items at once (tip: search \"t:soundEvent\")." + "\n" +
            "\n" +
            $"If no matching {nameof(NameOf.SoundContainer)}s are found, it will try and remove one character at the end of the name at a time until it finds a hit." + EditorTrial.trialTooltip;

        // Timeline
        public static string timelineExpandLabel = "Timeline";
        public static string timelineExpandTooltip =
            $"Zoom: Ctrl + mouse wheel scroll." + "\n" +
            "\n" +
            $"Pan: Mouse wheel hold and drag (or left mouse button hold and drag on the background)." + "\n" +
            "\n" +
            $"Volume: Hold and drag top of item up/down or click on the volume to write the decibel value." + "\n" +
            "\n" +
            $"Move item: Hold and drag on item left/right." + "\n" +
            "\n" +
            $"Focus on items: F" + EditorTrial.trialTooltip;

        public static string timelineResetLabel = "Reset";
        public static string timelineResetTooltip = "Reset all the Timeline settings." + EditorTrial.trialTooltip;

        public static string timelineFocusLabel = "Focus";
        public static string timelineFocusTooltip = 
            $"Focus on the {nameof(NameOf.SoundContainer)}s in the timeline." + "\n" +
            "\n" +
            $"Shortcut key: F." + EditorTrial.trialTooltip;

        // Base
        public static string baseLabel = "Base";
        public static string baseTooltip = $"Contains base settings for the {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string polyphonyLabel = "Polyphony";
        public static string polyphonyTooltip =
            $"How many instances of the {nameof(NameOf.SoundEvent)} that can exist at the same transform." + "\n" +
            "\n" +
            $"Modifier polyphony overrides this value. " + EditorTrial.trialTooltip;

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
            $"Select all the {nameof(NameOf.SoundEvent)}s and lower the volume by -20 dB with the -1dB button." + "\n" +
            "\n" +
            $"Then to compensate you can increase the global volume with an Audio Mixer (which you then can set to +20 dB)." + "\n" +
            "\n" +
            $"Tip: You can convert the old Modifier polyphony and volume to the new Base by selecting the assets and running:" + "\n" +
            "\n" +
            $"\"Tools/Sonity/Convert SoundEvent Modifier Volume and Polyphony to Base\"" + "\n" +
            "\n" +
            $"This will offset the Base volume with the Modifer volume and override the Base polphony with the Modifer polyphony." + EditorTrial.trialTooltip;

        public static string volumeRelativeLowerLabel = "-1 dB";
        public static string volumeRelativeLowerTooltip =
            $"Lowers the relative volume of all the selected {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to raise the volume of one {nameof(NameOf.SoundEvent)} and keep the relative volume." + "\n" +
            "\n" +
            $"Because then you can lower all of them to get more headroom." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundEvent)}s are selected it will show the lowest volume." + EditorTrial.trialTooltip;

        public static string volumeRelativeIncreaseLabel = "+1 dB";
        public static string volumeRelativeIncreaseTooltip =
            $"Raises the relative volume of all the selected {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to set the loudest volume to 0 dB but keep the relative volumes." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundEvent)}s are selected it will show the highest volume." + EditorTrial.trialTooltip;

        public static string volumeOverLimitWarning =
            $"Volume is over 0dB, please add scripting define symbol:" + "\n" +
            $"\"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume." + "\n" +
            $"Tip: Select all {nameof(NameOf.SoundEvent)}s and press \"- 1dB\" until max is 0dB.";

        // Settings
        public static string polyphonyModeLabel = "Polyphony Mode";
        public static string polyphonyModeTooltip =
            $"Limited Per Owner:" + "\n" +
            "\n" +
            $"Useful if you want to limit polyphony e.g per player." + "\n" +
            "\n" +
            $"You can use e.g {nameof(NameOf.SoundEvent)}.PlayAtPosition(); to play a {nameof(NameOf.SoundEvent)} at one position with another owner." + "\n" +
            "\n" +
            $"Limited Globally:" + "\n" +
            "\n" +
            $"Useful if you want to limit the polyphony globally e.g for bullet impacts." + "\n" +
            "\n" +
            $"This setting will change the old owner to the new position and set the new owner as the {nameof(Transform)} of the {nameof(NameOf.SoundManager)}.Instance." + "\n" +
            "\n" +
            $"Tip: If you want to limit the polyphony per owner and globally at the same time, you can use {nameof(NameOf.SoundPolyGroup)}s." + EditorTrial.trialTooltip;

        public static string audioMixerGroupLabel = "AudioMixerGroup";
        public static string audioMixerGroupTooltip =
            $"The AudioMixerGroup you want to output to." + "\n" +
            "\n" +
            $"Performance tips:" + "\n" +
            "\n" +
            $"If you have issues with audio performance you might want to make sure you’re not using an unnecessary amount of AudioMixerGroups (like a hundred of them)." + "\n" +
            "\n" +
            $"Because changing AudioMixerGroup for a Voice uses performance (when playing a new Voice Sonity tries to find an unused Voice which uses the same AudioMixerGroup)." + "\n" +
            "\n" +
            $"Use AudioMixerGroups when you want effects per group or e.g. ducking or for larger groups of sounds, but maybe not a have a unique one for each sound." + EditorTrial.trialTooltip;

        public static string soundVolumeGroupLabel = "SoundVolumeGroup";
        public static string soundVolumeGroupTooltip =
            $"{nameof(NameOf.SoundVolumeGroup)} enables grouped volume control with live volume changes." + "\n" +
            "\n" +
            $"It is perfect for quickly changing the volume of a large group of sounds." + "\n" +
            "\n" +
            $"It also supports +12dB volume if you “Enable Volume Increase” in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string soundMixLabel = $"{nameof(NameOf.SoundMix)}";
        public static string soundMixTooltip = $"{nameof(NameOf.SoundMix)} enables hierarchical control of for example volume or distance scale etc." + EditorTrial.trialTooltip;

        public static string soundPolyGroupLabel = $"{nameof(NameOf.SoundPolyGroup)}";
        public static string soundPolyGroupTooltip = $"{nameof(NameOf.SoundPolyGroup)} gives polyphony control grouped over different {nameof(NameOf.SoundEvent)} types." + EditorTrial.trialTooltip;

        public static string soundPolyGroupPriorityLabel = $"Priority";
        public static string soundPolyGroupPriorityTooltip = 
            $"Lower priority {nameof(NameOf.SoundEvent)}s will be stolen first. " + "\n" +
            "\n" +
            $"If \"Skip Lower Priority\" is enabled on the {nameof(NameOf.SoundPolyGroup)} this will determine if this {nameof(NameOf.SoundEvent)} will play or not when the Polyphony Limit is reached." + EditorTrial.trialTooltip;

        public static string cooldownTimeLabel = $"Cooldown Time";
        public static string cooldownTimeTooltip = 
            $"How quick this {nameof(NameOf.SoundEvent)} can be retriggered in seconds." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string probabilityLabel = $"Probability %";
        public static string probabilityTooltip = $"The probability that this {nameof(NameOf.SoundEvent)} should play." + EditorTrial.trialTooltip;

        public static string passParametersLabel = $"Pass Parameters";
        public static string passParametersTooltip = 
            $"If {nameof(NameOf.SoundParameter)}s should be passed to sub {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"E.g SoundTag, TriggerOnPlay, TriggerOnStop, TriggerOnTail." + EditorTrial.trialTooltip;

        public static string ignoreLocalPauseLabel = $"Ignore Local Pause";
        public static string ignoreLocalPauseTooltip = $"If enabled, the {nameof(NameOf.SoundEvent)} won't be paused unless force pause is used." + EditorTrial.trialTooltip;

        public static string ignoreGlobalPauseLabel = $"Ignore Global Pause";
        public static string ignoreGlobalPauseTooltip = $"If enabled, the {nameof(NameOf.SoundEvent)} won't be paused when global pause is used." + EditorTrial.trialTooltip;

        // Debug
        public static string debugLogSoundEventShowLabel = "Log View";
        public static string debugLogSoundEventShowTooltip =
            $"If the {nameof(NameOf.SoundEvent)} should be viewed or hidden from the {nameof(NameOf.SoundManager)} Debug Log {nameof(NameOf.SoundEvent)}s." + "\n" +
            "\n" +
            $"Force show will make it always debug even if {nameof(NameOf.SoundManager)} Debug Log {nameof(NameOf.SoundEvent)}s is disabled." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventShowLabel = "Draw View";
        public static string debugDrawSoundEventShowTooltip =
            $"If the {nameof(NameOf.SoundEvent)} should be viewed or hidden from the {nameof(NameOf.SoundManager)} Debug Draw {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventStyleOverrideLabel = "Override Draw Style ";
        public static string debugDrawSoundEventStyleOverrideTooltip =
            $"Overrides the debug draw style set in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventFontSizeLabel = "Font Size Multiplier";
        public static string debugDrawSoundEventFontSizeTooltip =
            $"Multiplier of the font size." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventOpacityMultiplierLabel = "Opacity Multiplier";
        public static string debugDrawSoundEventOpacityMultiplierTooltip =
            $"Multiplier of the opacity of the Debug Draw text." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventColorStartLabel = "Start Color";
        public static string debugDrawSoundEventColorStartTooltip =
            $"The color the text should have when it starts playing." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventColorEndLabel = "End Color";
        public static string debugDrawSoundEventColorEndTooltip =
            $"Which color the text should fade to over the lifetime." + EditorTrial.trialTooltip;

        public static string debugDrawSoundEventColorOutlineLabel = "Outline Color";
        public static string debugDrawSoundEventColorOutlineTooltip =
            $"The color of the text outline." + EditorTrial.trialTooltip;

        // Intensity
        public static string intensityFoldoutLabel = "Intensity";
        public static string intensityFoldoutTooltip =
            $"Settings for how any used {nameof(SoundParameterIntensity)} is scaled before it is applied to any enabled intensity options e.g Volume, Pitch etc." + EditorTrial.trialTooltip;

        public static string intensityAddLabel = "Add";
        public static string intensityAddTooltip = $"Adds to the {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string intensityMultiplierLabel = "Multiplier";
        public static string intensityMultiplierTooltip = $"Multiplier of the {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string intensityRolloffLabel = "Rolloff";
        public static string intensityRolloffTooltip = $"The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string intensitySeekTimeLabel = "Seek Time";
        public static string intensitySeekTimeTooltip =
            $"The seek time of the {nameof(SoundParameterIntensity)} in seconds ." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string intensityCurveLabel = "Curve";
        public static string intensityCurveTooltip = "Curve of the intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        public static string intensityThresholdEnableLabel = "Enable Threshold";
        public static string intensityThresholdEnableTooltip =
            $"If this {nameof(NameOf.SoundEvent)} is played with a {nameof(SoundParameterIntensity)} and it is under the threshold it won't be played." + EditorTrial.trialTooltip;

        public static string intensityThresholdLabel = "Threshold";
        public static string intensityThresholdTooltip = "The threshold limit after scaling the intensity value." + EditorTrial.trialTooltip;

        public static string intensityScaleAddLabel = "Scale Add";
        public static string intensityScaleAddTooltip = 
            $"Adds to the {nameof(SoundParameterIntensity)}, used by the automatic scaling." + "\n" +
            "\n" +
            $"Is separate from the manual add setting to provide finer control and better multi-editing." + EditorTrial.trialTooltip;

        public static string intensityScaleMultiplierLabel = "Scale Multiplier";
        public static string intensityScaleMultiplierTooltip = 
            $"Multiplier of the {nameof(SoundParameterIntensity)}, used by the automatic scaling." + "\n" +
            "\n" +
            $"Is separated from the manual multiplier setting to provide finer control and better multi-editing." + EditorTrial.trialTooltip;

        public static string intensityDebugLabel = "Intensity Record";
        public static string intensityDebugTooltip =
            $"If enabled it will record all {nameof(SoundParameterIntensity)} used when playing this {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string intensityDebugScaleMaxLabel = "Scale Max to 1";
        public static string intensityDebugScaleMaxTooltip =
            "Sets Intensity Add to 0." + "\n" +
            "\n" +
            "Then uses the recorded values to scale the Max value to 1 with Intensity Multiply." + "\n" +
            "\n" +
            "Useful for e.g. impacts where the Min value is 0 and the Max value is unknown." + EditorTrial.trialTooltip;

        public static string intensityDebugScaleMinMaxLabel = "Scale Min-Max to 0-1";
        public static string intensityDebugScaleMinMaxTooltip =
            "Uses the recorded values to make the Min value 0 with Intensity Add." + "\n" +
            "\n" +
            "Then uses the recorded values to scale the Max value to 1 with Intensity Multiply." + "\n" +
            "\n" +
            "Useful where Min and Max Intensity value ranges are unknown." + EditorTrial.trialTooltip;

        public static string intensityDebugResolutionLabel = "Debug Resolution";
        public static string intensityDebugResolutionTooltip = "The resolution of the displayed values." + EditorTrial.trialTooltip;

        public static string intensityDebugZoomLabel = "Debug Zoom";
        public static string intensityDebugZoomTooltip = "Focuses the vertical zoom scale to the values which are within the min and max range." + EditorTrial.trialTooltip;

        // Intensity Settings Warnings
        public static string intensityDebugRecordLabel = "Record available in Play Mode";
        public static string intensityDebugRecordTooltip = "To record intensity debug data, play the game with Intensity Record enabled." + EditorTrial.trialTooltip;

        public static string intensityValuesRecordedLabel = "Values Recorded";
        public static string intensityValuesRecordedTooltip =
            "If too many values are recorded, drawing the debug values might be slow." + "\n" +
            "\n" +
            "If so, clear the logged values to speed it up." + EditorTrial.trialTooltip;

        // TriggerOn
        public static string triggerOnWhichToPlayLabel = $"Which to Play";
        public static string triggerOnWhichToPlayTooltip =
            $"If Play All is selected, then all assigned {nameof(NameOf.SoundEvent)}s will be played." + "\n" +
            "\n" +
            $"If One Random is selected, then one random of the assigned {nameof(NameOf.SoundEvent)}s will be played." + "\n" +
            "\n" +
            $"The randomizer uses a pseudo random function remembering which {nameof(NameOf.SoundEvent)}s it last played to avoid repetition." + EditorTrial.trialTooltip;

        public static string triggerOnPlayLabel = $"Trigger On Play";
        public static string triggerOnPlayTooltip =
            $"Triggers another {nameof(NameOf.SoundEvent)} when this {nameof(NameOf.SoundEvent)} is played." + EditorTrial.trialTooltip;

        public static string triggerOnStopLabel = $"Trigger On Stop";
        public static string triggerOnStopTooltip =
            $"Triggers another {nameof(NameOf.SoundEvent)} when this {nameof(NameOf.SoundEvent)} is stopped." + EditorTrial.trialTooltip;

        public static string triggerOnTailLabel = $"Trigger On Tail";
        public static string triggerOnTailTooltip = 
            $"Triggers another {nameof(NameOf.SoundEvent)} \"Tail Length\" before the end." + "\n" +
            "\n" +
            $"It looks at the time of the last played voice on the first {nameof(NameOf.SoundContainer)}." + "\n" + 
            "\n" +
            $"Useful for music, e.g. if you have an intro with a 2 second tail and a loop you want to play on the tail of the intro." + "\n" +
            "\n" +
            $"If you play with {nameof(SoundManagerBase.Internals.PlayMusic)}, you can stop the next {nameof(NameOf.SoundEvent)} with {nameof(SoundManagerBase.Internals.StopAllMusic)} without a reference." + "\n" +
            "\n" +
            $"If you want it to trigger itself, make sure to set the \"Settings\" polyphony to 2." + "\n" +
            "\n" +
            $"If the trigger timing is not tight enough, try setting the AudioClip \"Compression Format\" to PCM or ADPCM (Vorbis is less accurate)." + EditorTrial.trialTooltip;

        public static string triggerOnTailTailLengthLabel = $"Tail Length";
        public static string triggerOnTailTailLengthTooltip =
            $"How long in seconds before the end of the {nameof(NameOf.SoundEvent)} to trigger the next {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"It looks at the time of the last played voice on the first {nameof(NameOf.SoundContainer)}." + "\n" + 
            "\n" +
            $"It takes into account pitch when calculating time." + "\n" +
            "\n" +
            $"For example, if the tail length is 2 seconds and you pitch it +12 semitones (2x speed) the internal tail length will be 1 seconds." + "\n" +
            "\n" +
            $"This is because double the speed with half the duration and vice versa." + EditorTrial.trialTooltip;

        public static string triggerOnTailSetTailLengthFromBpmLabel = $"Set Tail Length from BPM & Beats";
        public static string triggerOnTailSetTailLengthFromBpmTooltip = $"Calculates the Tail Length from the BPM and the Beats settings." + EditorTrial.trialTooltip;

        public static string triggerOnTailBpmLabel = $"BPM";
        public static string triggerOnTailBpmTooltip = $"Beats per minute." + EditorTrial.trialTooltip;

        public static string triggerOnTailTailLengthInBeatsLabel = $"Beats";
        public static string triggerOnTailTailLengthInBeatsTooltip = $"How long the tail is in beats." + EditorTrial.trialTooltip;

        // SoundTag
        public static string soundTagEnableLabel = $"{nameof(NameOf.SoundTag)}";
        public static string soundTagEnableTooltip = 
            $"Uses {nameof(NameOf.SoundTag)} to play other {nameof(NameOf.SoundEvent)}s and/or change {nameof(SoundEventModifier)}." + "\n" +
            "\n" +
            $"The {nameof(NameOf.SoundTag)} won't be passed to the {nameof(NameOf.SoundEvent)}s of the {nameof(NameOf.SoundTag)} in order to avoid infinite repetitions." + EditorTrial.trialTooltip;

        public static string soundTagModeLabel = $"Mode";
        public static string soundTagModeTooltip =
            $"If local {nameof(NameOf.SoundTag)} is selected you need to pass an {nameof(NameOf.SoundTag)} when playing the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"If global {nameof(NameOf.SoundTag)} is selected you need to set the {nameof(NameOf.SoundTag)} on the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string soundTagLabel = $"{nameof(NameOf.SoundTag)}";
        public static string soundTagTooltip = $"If this {nameof(NameOf.SoundTag)} is selected the {nameof(NameOf.SoundEvent)}s below will be played." + EditorTrial.trialTooltip;
    }
}
#endif