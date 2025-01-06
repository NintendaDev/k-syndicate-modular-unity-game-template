// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorTextSoundManager {

        public static string soundManagerTooltip =
        $"The {nameof(NameOf.SoundManager)} is the master object which is used to play sounds and manage global settings." + "\n" +
        "\n" +
        $"An instance of this object is required in the scene in order to play {nameof(NameOf.SoundEvent)}s." + "\n" +
        "\n" +
        $"You can add the pre-made prefab called “SoundManager” found in “Assets\\Plugins\\Sonity\\Prefabs” to your scene." + "\n" +
        "\n" +
        $"Or you can add the “Sonity - Sound Manager” component to an empty {nameof(GameObject)} in the scene, it works just as well." + "\n" +
        "\n" +
        $"Tip: If you need to play {nameof(NameOf.SoundEvent)}s on Awake() when starting your game you need to edit the Script Execution Order." + "\n" +
        "\n" +
        $"Go to \"Project Settings...\" -> \"Script Execution Order\" and add \"Sonity.SoundManager\"." + "\n" +
        "\n" +
        $"Then set it to a negative value (like -50) so it loads before the code which you want to use Awake() to play sounds when starting your game." + EditorTrial.trialTooltip;

        // Warnings
        public static string speedOfSoundScaleWarning = "Speed of Sound Scale is 0. It will have no effect";
        public static string disablePlayingSoundsWarning = $"No {nameof(NameOf.SoundEvent)}s can be played";
        public static string audioSettingsRealVoicesWarning = $"Real Voices are lower than Voice Limit";
        public static string audioSettingsVirtualVoicesWarning = $"Virtual Voices are lower than Real Voices";

        // Reset Settings
        public static string resetSettingsLabel = "Reset Settings";
        public static string resetSettingsTooltip = "Resets all settings." + EditorTrial.trialTooltip;

        // Reset All
        public static string resetAllLabel = "Reset All";
        public static string resetAllTooltip = "Resets settings and statistics." + EditorTrial.trialTooltip;

        // Free Trial Text
        public static string enableSoundInBuildsLabel = "Enable Sound in Builds";
        public static string enableSoundInBuildsTooltip =
            "This feature is removed from the Free Trial version of Sonity." + "\n" +
            "\n" + " Please buy the full version to get this feature." + EditorTrial.trialTooltip;
        public static string enableSoundInBuildsWarning = $"Sounds in Builds is not Available in the Free Trial Version of Sonity";

        // Settings ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string disablePlayingSoundsLabel = "Disable Playing Sounds";
        public static string disablePlayingSoundsTooltip =
            "Disables all the Play functionality." + "\n" +
            "\n" +
            "Useful if you've for example implemented temp sounds and don't want everyone else to hear them." + EditorTrial.trialTooltip;

        public static string soundTimeScaleLabel = $"Sound Time Scale";
        public static string soundTimeScaleTooltip =
            $"Change the Time Scale used for sound related calculations ingame." + "\n" +
            "\n" +
            $"When the game is not playing in the editor, only Time.realtimeSinceStartup can be used (for preview, etc)." + "\n" +
            "\n" +
            $"The Time Scale is used to calculate:" + "\n" +
            $"- Modifier Delay" + "\n" +
            $"- Modifier Fade In/Out Length" + "\n" +
            $"- SoundEvent Timeline Delay" + "\n" +
            $"- SoundEvent Cooldown Time" + "\n" +
            $"- SoundEvent Intensity Seek Time" + "\n" +
            $"- SoundContainer Prevent End Clicks Fade" + "\n" +
            $"- SoundParameter Delay" + "\n" +
            $"- SoundManager Speed of Sound Delay" + "\n" +
            $"- SoundManager Voice Disable Time" + "\n" +
            $"- SoundManager SoundEvent Debug Live Fade" + EditorTrial.trialTooltip;

        public static string globalPauseLabel = $"Global Pause";
        public static string globalPauseTooltip =
            $"Pauses or unpauses all sound using {nameof(AudioListener)}.pause." + "\n" +
            "\n" +
            $"Except the {nameof(NameOf.SoundEvent)}s which are set to \"Ignore Global Pause\"." + "\n" +
            "\n" +
            $"Note that because {nameof(AudioListener)}.pause is used, all other non-Sonity AudioSources will also be paused." + "\n" +
            "\n" +
            $"This can be remedied with using AudioSource.ignoreListenerPause." + "\n" +
            "\n" +
            $"Can be set with {nameof(SoundManagerBase.Internals.SetGlobalPause)}() and {nameof(SoundManagerBase.Internals.SetGlobalUnpause)}() in the SoundManager." + EditorTrial.trialTooltip;

        public static string globalVolumeLabel = $"Global Volume dB";
        public static string globalVolumeTooltip =
            $"Sets the global volume using {nameof(AudioListener)}.volume." + "\n" +
            "\n" +
            $"Note that because {nameof(AudioListener)}.volume is used, all other non-Sonity AudioSources will also be affected." + "\n" +
            "\n" +
            $"This can be remedied with using AudioSource.ignoreListenerVolume." + "\n" +
            "\n" +
            $"Can be set with {nameof(SoundManagerBase.Internals.SetGlobalVolumeDecibel)}() and {nameof(SoundManagerBase.Internals.SetGlobalVolumeRatio)}() in the SoundManager." + EditorTrial.trialTooltip;

        public static string audioListenerVolumeIncreaseEnableLabel = $"Enable Volume Increase";
        public static string audioListenerVolumeIncreaseEnableTooltip =
            $"If enabled you get the ability to raise the volume of {nameof(NameOf.SoundContainer)}s and {nameof(NameOf.SoundEvent)}s by +24dB each and {nameof(NameOf.SoundVolumeGroup)}s by +12dB." + "\n" +
            "\n" +
            $"This is done by increasing the {nameof(AudioListener)}.volume +60dB and reducing the volume of the AudioSource to compensate." + "\n" +
            "\n" +
            $"Note that all other non-Sonity AudioSources will also be affected, you can avoid this by enabling AudioSource.ignoreListenerVolume." + "\n" +
            "\n" +
            $"For volume increase to work, you need to set the Scripting Define Symbol \"SONITY_ENABLE_VOLUME_INCREASE\"." + "\n" +
            "\n" +
            $"This can be done by pressing the button below (make sure all platforms has this scripting define symbol)." + "\n" +
            "\n" +
            $"The free trial version of Sonity doesn’t support this feature because it uses Scripting Define Symbols." + EditorTrial.trialTooltip;

        public static string audioListenerVolumeIncreaseFreeTrialWarningLabel = $"Volume Increase is not Available in the Free Trial Version of Sonity";

        public static string audioListenerVolumeIncreaseEducationalVersionWarningLabel = $"Volume Increase is not Available in the Educational Version of Sonity";

        public static string audioListenerVolumeIncreaseScriptingDefineSymbolAddLabel = $"Add Scripting Define Symbol:\n\"SONITY_ENABLE_VOLUME_INCREASE\"";
        public static string audioListenerVolumeIncreaseScriptingDefineSymbolAddTooltip =
            $"Pressing on this button will automatically add the Scripting Define Symbol \"SONITY_ENABLE_VOLUME_INCREASE\" if it doesn't exist." + "\n" +
            "\n" +
            $"This enables you to raise the volume of {nameof(NameOf.SoundEvent)}s and {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string audioListenerVolumeIncreaseScriptingDefineSymbolRemoveLabel = $"Remove Scripting Define Symbol:\n\"SONITY_ENABLE_VOLUME_INCREASE\"";
        public static string audioListenerVolumeIncreaseScriptingDefineSymbolRemoveTooltip =
            $"Pressing on this button will automatically remove the Scripting Define Symbol \"SONITY_ENABLE_VOLUME_INCREASE\" if it exists." + "\n" +
            "\n" +
            $"This removes the ability to raise the volume of {nameof(NameOf.SoundEvent)}s and {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string globalSoundTagLabel = $"Global {nameof(NameOf.SoundTag)}";
        public static string globalSoundTagTooltip = 
            $"The selected Global {nameof(NameOf.SoundTag)}." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundEvent)}s using global {nameof(NameOf.SoundTag)} are affected by this." + EditorTrial.trialTooltip;

        public static string distanceScaleLabel = "Distance Scale";
        public static string distanceScaleTooltip =
            "Global range scale multiplier for all the sounds in Sonity." + "\n" +
            "\n" +
            "Distance is calculated by Unity units of distance." + "\n" +
            "\n" +
            $"E.g. if Distance Scale is set to 100, a {nameof(NameOf.SoundEvent)} with the distance multiplier of 1 will be heard up to 100 Unity units away." + EditorTrial.trialTooltip;

        public static string overrideListenerDistanceLabel = "Override Listener Distance";
        public static string overrideListenerDistanceTooltip =
            $"If enabled an {nameof(NameOf.AudioListenerDistance)} component is required in the scene." + "\n" +
            "\n" +
            $"The position of the {nameof(NameOf.AudioListenerDistance)} component will determine all distance based calculations (like volume falloff)." + "\n" +
            "\n" +
            $"While the AudioListener position will be used for spatialization and Angle to Stereo Pan calculations." + "\n" +
            "\n" +
            $"Example of usage in a 3rd person or top down game:" + "\n" +
            "\n" +
            $"Enable \"Override Listener Distance\" in the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            $"Put the AudioListener on the main camera and the {nameof(NameOf.AudioListenerDistance)} on the player character." + "\n" +
            "\n" +
            $"Try changing the Amount slider to find a nice balance between the different positions." + EditorTrial.trialTooltip;

        public static string overrideListenerDistanceAmountLabel = "Amount %";
        public static string overrideListenerDistanceAmountTooltip =
            $"How much weight the {nameof(NameOf.AudioListenerDistance)} position has over the AudioListener position." + "\n" +
            "\n" +
            $"The position is linearly interpolated between the two of them." + "\n" +
            "\n" +
            $"100% is at the {nameof(NameOf.AudioListenerDistance)} component position." + "\n" +
            "\n" +
            $"50% is halfway between them." + "\n" +
            "\n" +
            $"0% is at the AudioListener position.";

        public static string speedOfSoundEnabledLabel = "Enable Speed of Sound";
        public static string speedOfSoundEnabledTooltip = 
            $"Speed of sound is a delay based on the distance between the Audio Listener and a {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string speedOfSoundScaleLabel = "Speed of Sound Scale";
        public static string speedOfSoundScaleTooltip =
            $"Global speed of sound delay scale multiplier." + "\n" +
            "\n" +
            $"1 equals 430 Unity units per second." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string adressableAudioMixerUseLabel = $"Adressable AudioMixer";
        public static string adressableAudioMixerUseTooltip =
            $"If enabled, all AudioMixer references in all {nameof(NameOf.SoundEvent)}s and {nameof(NameOf.SoundContainer)}s to the selected Adressable AudioMixer Reference Asset." + "\n" +
            "\n" +
            $"This is to fix problems when the AudioMixer is an adressable asset because it might create both a normal and an adressable instance with different IDs." + "\n" +
            "\n" +
            $"The AudioMixerGroups are matched using FindMatchingGroups using the name of the AudioMixerGroup, so you have to name your groups to something unique." + "\n" +
            "\n" +
            $"For builds the AudioMixerGroups are cached and wont be updated once the sound has been played." + "\n" +
            "\n" +
            $"When using AudioMixer.SetFloat() etc you need to access this specific AudioMixer instance by using \"SoundManager.Instance.GetAdressableAudioMixer();\"." + "\n" +
            "\n" +
            $"For Adressable AudioMixer to work, you need to add the package \"com.unity.addressables\"." + "\n" +
            "\n" +
            $"You also need to define the Script Define Symbol \"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\"." + "\n" +
            "\n" +
            $"Asmdef_Sonity.Internal.Runtime references Unity.Addressables and Unity.ResourceManager." + "\n" +
            "\n" +
            $"Asmdef_Sonity.Internal.Editor references Unity.Addressables." + "\n" +
            "\n" +
            $"The free trial version of Sonity doesn’t support this feature because it uses Scripting Define Symbols." + EditorTrial.trialTooltip;

        public static string adressableAudioMixerReferenceLabel = $"AudioMixer Reference";
        public static string adressableAudioMixerReferenceTooltip =
            $"Assign the AudioMixer reference here, it will be automatically loaded." + "\n" +
            "\n" +
            $"Only one AudioMixer per project is supported with this feature." + EditorTrial.trialTooltip;

        public static string adressableAudioMixerScriptingDefineSymbolAddLabel = $"Add Scripting Define Symbol:\n\"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\"";
        public static string adressableAudioMixerScriptingDefineSymbolAddTooltip =
            $"Pressing on this button will automatically add the Scripting Define Symbol \"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\" if it doesn't exist." + "\n" +
            "\n" +
            $"This is so the \"com.unity.addressables\" asset package isn't needed if you don't use the Adressable AudioMixer." + EditorTrial.trialTooltip;

        public static string adressableAudioMixerScriptingDefineSymbolRemoveLabel = $"Remove Scripting Define Symbol:\n\"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\"";
        public static string adressableAudioMixerScriptingDefineSymbolRemoveTooltip =
            $"Pressing on this button will automatically remove the Scripting Define Symbol \"SONITY_ENABLE_ADRESSABLE_AUDIOMIXER\" if it exists." + "\n" +
            "\n" +
            $"This is so the \"com.unity.addressables\" asset package isn't needed if you don't use the Adressable AudioMixer." + EditorTrial.trialTooltip;

        public static string adressableAudioMixerFreeTrialWarningLabel = $"Adressable AudioMixer is not Available in the Free Trial Version of Sonity";
        public static string adressableAudioMixerEducationalVersionWarningLabel = $"Adressable AudioMixer is not Available in the Educational Version of Sonity";

        public static string debugWarningsLabel = "Debug Warnings";
        public static string debugWarningsTooltip =
            "Makes Sonity output Debug Warnings if anything is wrong." + EditorTrial.trialTooltip;

        public static string debugInPlayModeLabel = "Debug In Play Mode";
        public static string debugInPlayModeTooltip =
            "Makes Sonity output Debug Warnings if anything is wrong in Play Mode." + EditorTrial.trialTooltip;

        public static string guiWarningsLabel = "GUI Warnings";
        public static string guiWarningsTooltip =
            "Makes Sonity show GUI Warnings if anything is wrong in the editor." + EditorTrial.trialTooltip;

        public static string dontDestoyOnLoadLabel = "Use DontDestroyOnLoad()";
        public static string dontDestoyOnLoadTooltip =
            "Calls DontDestroyOnLoad() at Start for Sonity objects." + "\n" +
            "\n" +
            "Which makes them persistent when switching scenes." + "\n" +
            "\n" +
            "For this to work the parent is set to null, which can move the objects." + EditorTrial.trialTooltip;

        // Performance
        public static string voicePreloadLabel = "Voice Preload";
        public static string voicePreloadTooltip = 
            "How many Voices to preload on Awake()." + "\n" +
            "\n" +
            "Voice Limit cannot be lower than Voice Preload." + EditorTrial.trialTooltip;

        public static string voiceLimitLabel = "Voice Limit";
        public static string voiceLimitTooltip = 
            "Maximum number of Voices." + "\n" +
            "\n" +
            "If the limit is reached it will steal the Voice with the lowest priority." + "\n" +
            "\n" +
            "If you need extra performance, you could try lowering the real and virtual voices to a lower number." + "\n" +
            "\n" +
            "Voice Limit cannot be lower than Voice Preload." + EditorTrial.trialTooltip;

        public static string audioSettingsRealVoicesLabel = "Max Real Voices";
        public static string audioSettingsVirtualVoicesLabel = "Max Virtual Voices";
        public static string audioSettingsRealAndVirtualVoicesTooltip =
            $"Max Real Voices:" + "\n" +
            $"The maximum number of real (heard) {nameof(AudioSource)}s that can be played at the same time." + "\n" +
            "\n" +
            $"\"Real Voices\" should be the same as the \"Voice Limit\", or more if you play other sounds outside of Sonity." + "\n" +
            "\n" + 
            $"Max Virtual Voices:" + "\n" +
            $"The maximum number of virtual (not heard) {nameof(AudioSource)}s that can be played at the same time." + "\n" +
            "\n" +
            $"This should always be more than the number of real voices." + "\n" +
            "\n" +
            "You can change these values manually in:" + "\n" +
            "\"Edit\" > \"Project Settings\" > \"Audio\"" + EditorTrial.trialTooltip;

        public static string applyVoiceLimitToAudioSettingsLabel = "Apply to Project Audio Settings";
        public static string applyVoiceLimitToAudioSettingsTooltip =
            "Applies the Voice Limit to the Project Audio Settings." + "\n" +
            "\n" +
            "Sets \"Real Voices\" to the \"Voice Limit\"." + "\n" +
            "\n" +
            "You can change these values manually in:" + "\n" +
            "\"Edit\" > \"Project Settings\" > \"Audio\"" + EditorTrial.trialTooltip;

        public static string voiceStopTimeLabel = "Voice Disable Time";
        public static string voiceStopTimeTooltip =
            $"How long in seconds to wait before disabling a Voice when they've stopped playing. " + "\n" +
            "\n" +
            $"Retriggering a voice which is not disabled is more performant than retriggering a voice which is disabled." + "\n" +
            "\n" +
            $"But having a lot of voices enabled which aren't used is also not good for performance, so don't set this value too high." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string voiceEffectLimitLabel = "Voice Effect Limit";
        public static string voiceEffectLimitTooltip =
            "Maximum number of Voice Effects which can be used at the same time." + "\n" +
            "\n" +
            "A Voice with any combination of waveshaper/lowpass/highpass counts as one Voice Effect." + "\n" +
            "\n" +
            "If the values of a Voice Effect doesn't have any effect it is disabled automatically (e.g. distortion amount is 0)." + "\n" +
            "\n" +
            "If the Voice Effect limit is reached, the Voice Effects are prioritized by the Voices with the highest volume * priority." + "\n" +
            "\n" +
            "Watch out for high load on the audio thread if set too high." + "\n" +
            "\n" +
            "Try setting the buffer size to \"Best Performance\" in \"Edit\" > \"Project Settings\" > \"Audio\" if you want to run more Voice Effects." + EditorTrial.trialTooltip;

        // Editor Tools ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Main Header
        public static string editorToolHeaderLabel = $"Editor Tools";
        public static string editorToolHeaderTooltip =
            $"This is a collection of nifty editor tools which you can use to speed up your workflow." + "\n" +
            "\n" +
            $"They are enabled with the use of “Script Define Symbols” in Project Settings -> Player -> Other Settings -> Script Compilation -> Script Define Symbols." + "\n" +
            "\n" +
            $"This way they can be bound to default hotkeys and they won’t hog up your toolbars if you don’t use them." + EditorTrial.trialTooltip;

        // Reference Finder
        public static string editorToolReferenceFinderEnableLabel = $"Reference Finder";
        public static string editorToolReferenceFinderEnableTooltip =
            $"Reference Finder is an editor tool for finding where an asset is referenced by another asset." + "\n" +
            $"It is enabled with the use of the Script Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_REFERENCE_FINDER\"." + "\n" +
            $"This is assigned in Project Settings -> Player -> Other Settings -> Script Compilation -> Script Define Symbols." + "\n" +
            $"\"Find References for Selected Assets\" is useful for e.g. figuring out to which (if any) prefabs a {nameof(NameOf.SoundEvent)} is assigned to." + "\n" +
            $"\"Open Reference Finder Window\" is useful for figuring out e.g. which AudioClips are completely unreferenced and therefore unused." + "\n" +
            $"Tip: Close the window before renaming or moving any file in the project or it might lag a lot." + "\n" +
            "\n" +
            $"Toolbar Actions" + "\n" +
            $"Tools/Sonity Tools 🛠/Reference Finder 🔍/Open Reference Finder Window" + "\n" +
            $"Tools/Sonity Tools 🛠/Reference Finder 🔍/Find References for Selected Assets" + "\n" +
            $"Assets/Sonity Tools 🛠/Reference Finder 🔍/Open Reference Finder Window" + "\n" +
            $"Assets/Sonity Tools 🛠/Reference Finder 🔍/Find References for Selected Assets" + "\n" +
            "\n" +
            $"Default Shortcuts" + "\n" +
            $"Ctrl+Shift+Alt+F - Reference Finder 🔍/Find References for Selected Assets" + "\n" +
            "\n" +
            $"Reference Finder is made by Alexey Perov and licensed under a free to use and distribute MIT license." + EditorTrial.trialTooltip;

        public static string editorToolReferenceFinderAddLabel = $"Add Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nREFERENCE_FINDER\"";
        public static string editorToolReferenceFinderAddTooltip =
            $"Pressing on this button will automatically add the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_REFERENCE_FINDER\" if it doesn't exist." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        public static string editorToolReferenceFinderRemoveLabel = $"Remove Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nREFERENCE_FINDER\"";
        public static string editorToolReferenceFinderRemoveTooltip =
            $"Pressing on this button will automatically remove the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_REFERENCE_FINDER\" if it exists." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        // Select Same Type
        public static string editorToolSelectSameTypeEnableLabel = $"Select Same Type";
        public static string editorToolSelectSameTypeEnableTooltip =
            $"Select Same Type is an editor tool for quickly selecting all assets of the same type in a folder which enables you to quickly edit a lot of assets." + "\n" +
            $"It is enabled with the use of the Script Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECT_SAME_TYPE\"." + "\n" +
            $"This is assigned in Project Settings -> Player -> Other Settings -> Script Compilation -> Script Define Symbols." + "\n" +
            "\n" +
            $"Toolbar Actions" + "\n" +
            $"Tools/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder" + "\n" +
            $"Tools/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders" + "\n" +
            $"Assets/Sonity Tools 🛠/Select Same Type 🤏/In Same Folder" + "\n" +
            $"Assets/Sonity Tools 🛠/Select Same Type 🤏/In Subfolders" + "\n" +
            "\n" +
            $"Default Shortcuts" + "\n" +
            $"Ctrl+Alt+A - Select Same Type 🤏/In Same Folder" + "\n" +
            $"Ctrl+Alt+Shift+A - Select Same Type 🤏/In Subfolders" + EditorTrial.trialTooltip;

        public static string editorToolSelectSameTypeAddLabel = $"Add Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nSELECT_SAME_TYPE\"";
        public static string editorToolSelectSameTypeAddTooltip =
            $"Pressing on this button will automatically add the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECT_SAME_TYPE\" if it doesn't exist." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        public static string editorToolSelectSameTypeRemoveLabel = $"Remove Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nSELECT_SAME_TYPE\"";
        public static string editorToolSelectSameTypeRemoveTooltip =
            $"Pressing on this button will automatically remove the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECT_SAME_TYPE\" if it exists." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        // Selection History
        public static string editorToolSelectionHistoryEnableLabel = $"Selection History";
        public static string editorToolSelectionHistoryEnableTooltip =
            $"Selection History is an editor tool for quickly undoing and redoing selections." + "\n" +
            $"This enables you to quickly move between objects you’ve previously selected." + "\n" +
            $"It is enabled with the use of the Script Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECTION_HISTORY\"." + "\n" +
            $"This is assigned in Project Settings -> Player -> Other Settings -> Script Compilation -> Script Define Symbols." + "\n" +
            "\n" +
            $"Toolbar Actions" + "\n" +
            $"Tools/Sonity Tools 🛠/Selection History 📜/Back" + "\n" +
            $"Tools/Sonity Tools 🛠/Selection History 📜/Forward" + "\n" +
            $"Assets/Sonity Tools 🛠/Selection History 📜/Back" + "\n" +
            $"Assets/Sonity Tools 🛠/Selection History 📜/Forward" + "\n" +
            "\n" +
            $"Default Shortcuts" + "\n" +
            $"U - Selection History 📜/Back" + "\n" +
            $"Shift+U - Selection History 📜/Forward" + "\n" +
            "\n" +
            $"Selection History is made by Matthew Miner and licensed under a free to use and distribute MIT license." + EditorTrial.trialTooltip;

        public static string editorToolSelectionHistoryAddLabel = $"Add Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nSELECTION_HISTORY\"";
        public static string editorToolSelectionHistoryAddTooltip =
            $"Pressing on this button will automatically add the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECTION_HISTORY\" if it doesn't exist." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        public static string editorToolSelectionHistoryRemoveLabel = $"Remove Scripting Define Symbol:\n\"SONITY_ENABLE_EDITOR_TOOL_\nSELECTION_HISTORY\"";
        public static string editorToolSelectionHistoryRemoveTooltip =
            $"Pressing on this button will automatically remove the Scripting Define Symbol \"SONITY_ENABLE_EDITOR_TOOL_SELECTION_HISTORY\" if it exists." + "\n" +
            "\n" +
            $"" + EditorTrial.trialTooltip;

        // All Tools
        public static string editorToolsAddAllLabel = "Add All Tools";
        public static string editorToolsAddAllTooltip = "Adds all editor tools scripting define symbols." + EditorTrial.trialTooltip;

        public static string editorToolsRemoveAllLabel = "Remove All Tools";
        public static string editorToolsRemoveAllTooltip = "Adds all editor tools scripting define symbols." + EditorTrial.trialTooltip;

        // Debug ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string debugExpandLabel = "Debug";
        public static string debugExpandTooltip = "Debug of Sonity." + EditorTrial.trialTooltip;

        public static string debugAudioListenerLabel = $"{nameof(AudioListener)}";
        public static string debugAudioListenerTooltip =
            $"Here you can see and select the currently cached {nameof(AudioListener)} in runtime." + "\n" +
            "\n" +
            $"If you click on the field your selection will show you the currently used {nameof(AudioListener)}." + "\n" +
            "\n" +
            $"The {nameof(AudioListener)} component is found and cached automatically and cannot be assigned here." + EditorTrial.trialTooltip;

        public static string debugAudioListenerDistanceLabel = $"{nameof(NameOf.AudioListenerDistance)}";
        public static string debugAudioListenerDistanceTooltip =
            $"Here you can see and select the currently cached {nameof(NameOf.AudioListenerDistance)} in runtime." + "\n" +
            "\n" +
            $"If you click on the field your selection will show you the currently used {nameof(NameOf.AudioListenerDistance)}." + "\n" +
            "\n" +
            $"The {nameof(NameOf.AudioListenerDistance)} component is found and cached automatically and cannot be assigned here." + "\n" +
            "\n" +
            $"Is only shown if \"" + overrideListenerDistanceLabel + "\" is enabled." + EditorTrial.trialTooltip;

        // Log SoundEvents /////////////////////////////////////////////////////////////////////////////

        public static string logSoundEventsHeaderLabel = $"Log SoundEvents";
        public static string logSoundEventsHeaderTooltip =
            $"When enabled {nameof(NameOf.SoundEvent)} actions will be logged to the console." + "\n" +
            "\n" +
            $"This is useful to debug what happens with {nameof(NameOf.SoundEvent)}s during runtime." + "\n" +
            "\n" +
            $"It also enables you to see where in the code the {nameof(NameOf.SoundEvent)} is played/stopped from if you enable Stack Tracke Logging “Script Only”." + EditorTrial.trialTooltip;

        public static string logSoundEventsEnableLabel = $"To Console";
        public static string logSoundEventsEnableTooltip =
            $"If enabled it will log {nameof(NameOf.SoundEvent)} actions to the console." + EditorTrial.trialTooltip;

        public static string logSoundEventsSelectObjectLabel = $"Click On Log Selects";
        public static string logSoundEventsSelectObjectTooltip =
            $"If you click the log in the console, you will be redirected to one of either object:" + "\n" +
            "\n" +
            $"Owner" + "\n" +
            $"Selects the owner transform which is used to play the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"Position" + "\n" +
            $"Selects the transform position which is used to play the {nameof(NameOf.SoundEvent)} if its played with PlayAtPosition with a Transform." + "\n" +
            "\n" +
            $"SoundEvent" + "\n" +
            $"Selects the {nameof(NameOf.SoundEvent)} which is logged." + "\n" +
            "\n" +
            $"If an Owner or Position can't be found (for example when logging stop etc) it will redirect to the {nameof(NameOf.SoundEvent)} instead." + EditorTrial.trialTooltip;

        public static string logSoundEventsLogTypeLabel = $"Debug Log Type";
        public static string logSoundEventsLogTypeTooltip =
            $"Select which kind of Debug.Log you want to use." + "\n" +
            "\n" +
            $"Switch between either Debug.Log, Debug.LogWarning or Debug.LogError." + EditorTrial.trialTooltip;

        public static string logSoundEventsSettingsLabel = $"Log Settings";
        public static string logSoundEventsSettingsTooltip =
            $"Opens a dropdown menu where you can select which events should be logged." + "\n" +
            "\n" +
            $"SoundEvent Play" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is played." + "\n" +
            "\n" +
            $"SoundEvent Stop" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is stopped with a stop command." + "\n" +
            "\n" +
            $"SoundEvent Pool" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is pooled after its stopped." + "\n" +
            "\n" +
            $"SoundEvent Pause" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is paused." + "\n" +
            "\n" +
            $"SoundEvent Unpause" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is unpaused." + "\n" +
            "\n" +
            $"SoundEvent Global Pause" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is paused using global pause." + "\n" +
            "\n" +
            $"SoundEvent Global Unpause" + "\n" +
            $"Logs when a {nameof(NameOf.SoundEvent)} is unpaused using global unpause." + "\n" +
            "\n" +
            $"SoundParameters Once" + "\n" +
            $"Logs any {nameof(NameOf.SoundParameter)} set to UpdateMode Once passed when playing a {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"SoundParameters Continious" + "\n" +
            $"Logs any {nameof(NameOf.SoundParameter)} set to UpdateMode Continious on an active {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string logSoundEventsResetLabel = "Reset";
        public static string logSoundEventsResetTooltip = "Resets Log Settings." + EditorTrial.trialTooltip;

        // Draw SoundEvents /////////////////////////////////////////////////////////////////////////////

        public static string drawSoundEventsLabel = $"Draw SoundEvents";
        public static string drawSoundEventsTooltip =
            $"Draws the names of all currently playing {nameof(NameOf.SoundEvent)} in the scene and/or game view." + "\n" +
            "\n" +
            $"Useful for debugging when you want to see what is playing and where." + EditorTrial.trialTooltip;

        public static string drawSoundEventsInSceneViewEnabledLabel = $"In Scene View";
        public static string drawSoundEventsInSceneViewEnabledTooltip = 
            $"Draws debug names in the Unity scene view." + "\n" +
            "\n" +
            $"Doesn't work in Unity versions older than 2019.1." + EditorTrial.trialTooltip;

        public static string drawSoundEventsInGameViewEnabledLabel = $"In Game View";
        public static string drawSoundEventsInGameViewEnabledTooltip = 
            $"Draws debug names in the Unity game view." + "\n" +
            "\n" +
            $"Only applied in the Unity editor." + EditorTrial.trialTooltip;

        public static string drawSoundEventsHideIfCloserThanLabel = $"Hide if Closer Than";
        public static string drawSoundEventsHideIfCloserThanTooltip =
            $"Hides the debug if its closer to the camera than the allowed value in Game view." + "\n" +
            "\n" +
            $"Useful for hiding {nameof(NameOf.SoundEvent)}s which are played on the camera which you don't want to see." + EditorTrial.trialTooltip;

        public static string drawSoundEventsFontSizeLabel = $"Font Size";
        public static string drawSoundEventsFontSizeTooltip = "The font size of the text." + EditorTrial.trialTooltip;

        public static string drawSoundEventsVolumeToOpacityLabel = "Volume to Opacity";
        public static string drawSoundEventsVolumeToOpacityTooltip = 
            $"How much of the volume of the {nameof(NameOf.SoundEvent)} will be applied to the transparency of the text." + "\n" +
            "\n" +
            $"E.g lower volumes will be more transparent." + EditorTrial.trialTooltip;

        public static string drawSoundEventsLifetimeToOpacityLabel = "Lifetime to Opacity";
        public static string drawSoundEventsLifetimeToOpacityTooltip =
            $"How much the lifetime of the {nameof(NameOf.SoundEvent)} will affect the transparency of the text." + EditorTrial.trialTooltip;

        public static string drawSoundEventsLifetimeFadeLengthLabel = "Lifetime Fade Length";
        public static string drawSoundEventsLifetimeFadeLengthTooltip =
            $"How long the fade should be." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string drawSoundEventsColorStartLabel = "Start Color";
        public static string drawSoundEventsColorStartTooltip = "The color the text should have when it starts playing." + EditorTrial.trialTooltip;

        public static string drawSoundEventsColorEndLabel = "End Color";
        public static string drawSoundEventsColorEndTooltip = "Which color the text should fade to over the lifetime." + EditorTrial.trialTooltip;

        public static string drawSoundEventsColorOutlineLabel = "Outline Color";
        public static string drawSoundEventsColorOutlineTooltip = "The color of the text outline." + EditorTrial.trialTooltip;

        public static string drawSoundEventsResetLabel = $"Reset Style";
        public static string drawSoundEventsResetTooltip = $"Resets style of drawing {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        // Reset All Debug
        public static string debugResetAllLabel = $"Reset All Debug";
        public static string debugResetAllTooltip = $"Resets All Debug Settings." + EditorTrial.trialTooltip;

        // Statistics ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static string statisticsExpandLabel = "Statistics";
        public static string statisticsExpandTooltip = "Statistics of Sonity." + EditorTrial.trialTooltip;

        public static string statisticsInstanceLabel = "Instances";
        public static string statisticsInstanceTooltip = "Sound Event Instance Statistics of Sonity." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsLabel = $"{nameof(NameOf.SoundEvent)}s";
        public static string statisticsSoundEventsTooltip = $"Statistics of {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsCreatedLabel = "Created";
        public static string statisticsSoundEventsCreatedTooltip = $"The number of instantiated {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsActiveLabel = "Active";
        public static string statisticsSoundEventsActiveTooltip = $"The number of active {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsDisabledLabel = "Disabled";
        public static string statisticsSoundEventsDisabledTooltip = $"The number of unused and disabled {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsVoicesLabel = "Voices";
        public static string statisticsVoicesTooltip = "Statistics of Voices." + EditorTrial.trialTooltip;

        public static string statisticsVoicesPlayedLabel = "Played";
        public static string statisticsVoicesPlayedTooltip = "The number of played Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsMaxSimultaneousVoicesLabel = "Max Simultaneous";
        public static string statisticsMaxSimultaneousVoicesTooltip = "The maximum number of simultaneously playing Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsVoicesStolenLabel = "Stolen";
        public static string statisticsVoicesStolenTooltip = "The number of stolen Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsVoicesCreatedLabel = "Created";
        public static string statisticsVoicesCreatedTooltip = "The number of Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesActiveLabel = "Active";
        public static string statisticsVoicesActiveTooltip = "The number of Voices playing audio." + EditorTrial.trialTooltip;

        public static string statisticsVoicesInactiveLabel = "Inactive";
        public static string statisticsVoicesInactiveTooltip = "The number of inactive Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesPausedLabel = "Paused";
        public static string statisticsVoicesPausedTooltip = "The number of paused Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesStoppedLabel = "Stopped";
        public static string statisticsVoicesStoppedTooltip = "The number of stopped Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsLabel = "Voice Effects";
        public static string statisticsVoiceEffectsTooltip = 
            "Statistics of Voice Effects." + "\n" +
            "\n" +
            "A Voice with any combination of waveshaper/lowpass/highpass counts as one Voice Effect." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsActiveLabel = "Active";
        public static string statisticsVoiceEffectsActiveTooltip = "The number of active Voice Effects." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsAvailableLabel = "Available";
        public static string statisticsVoiceEffectsAvailableTooltip = "How many Voice Effects are available." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventInstancesLabel = "Instance Statistics";
        public static string statisticsSoundEventInstancesTooltip =
            $"Real-time statistics per {nameof(NameOf.SoundEvent)} Instance." + "\n" +
            "\n" +
            "Available in Playmode." + EditorTrial.trialTooltip;

        public static string statisticsSortingLabel = $"Sort By";
        public static string statisticsSortingTooltip =
            $"Which method to sort the list of {nameof(NameOf.SoundEvent)} Instances." + "\n" +
            "\n" +
            "Name" + "\n" +
            "Sorts by alphabetical order." + "\n" +
            "\n" +
            "Voices" + "\n" +
            "Sorts by voice count." + "\n" +
            "\n" +
            "Plays" + "\n" +
            "Sorts by number of plays." + "\n" +
            "\n" +
            "Volume" + "\n" +
            "Sorts by volume." + "\n" +
            "\n" +
            "Time" + "\n" +
            "Sorts by last time played." + EditorTrial.trialTooltip;

        public static string statisticsShowLabel = $"Show";
        public static string statisticsShowTooltip = 
            $"Toggle what information to show about the {nameof(NameOf.SoundEvent)} Instances." + "\n" +
            "\n" +
            "Show Active" + "\n" +
            "How many are currently active." + "\n" +
            "\n" +
            "Show Disabled" + "\n" +
            "How many are currently disabled." + "\n" +
            "\n" +
            "Show Voices" + "\n" +
            "How many voices are currently used." + "\n" +
            "\n" +
            "Show Plays" + "\n" +
            "The number of total plays." + "\n" +
            "\n" +
            "Show Volume" + "\n" +
            "The current average volume." + EditorTrial.trialTooltip;

        public static string statisticsInstancesResetLabel = "Reset";
        public static string statisticsInstancesResetTooltip = "Resets statistics." + EditorTrial.trialTooltip;

        public static string statisticsAllResetLabel = "Reset All Statistics";
        public static string statisticsAllResetTooltip = "Resets all statistics settings." + EditorTrial.trialTooltip;
    }
}
#endif