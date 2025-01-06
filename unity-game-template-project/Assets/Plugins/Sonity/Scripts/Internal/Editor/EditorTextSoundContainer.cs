// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Audio;

namespace Sonity.Internal {

    public class EditorTextSoundContainer {

        public static string soundContainerTooltip =
        $"{nameof(NameOf.SoundContainer)}s are the building blocks of Sonity." + "\n" +
        "\n" +
        $"They contain {nameof(AudioClip)}s and options of how the sound should be played." + "\n" +
        "\n" +
        $"All {nameof(NameOf.SoundContainer)}s are multi-object editable." + EditorTrial.trialTooltip;

        public static string presetsLabel = "Presets";
        public static string presetsTooltip =
            "SFX 3D:" + "\n" +
            "Enable Distance = true" + "\n" +
            "Spatial Blend = 1f" + "\n" +
            "Never Steal Voice = false" + "\n" +
            "Never Steal Voice Effects = false" + "\n" +
            "Pitch Random = true" + "\n" +
            "Priority = 0.5" + "\n" +
            "\n" +
            "SFX 2D:" + "\n" +
            "Enable Distance = false" + "\n" +
            "Spatial Blend = 0" + "\n" +
            "Never Steal Voice = false" + "\n" +
            "Never Steal Voice Effects = false" + "\n" +
            "Pitch Random = true" + "\n" +
            "Priority = 0.5" + "\n" +
            "\n" +
            "Music:" + "\n" +
            "Enable Distance = false" + "\n" +
            "Spatial Blend = 0" + "\n" +
            "Never Steal Voice = true" + "\n" +
            "Never Steal Voice Effects = true" + "\n" +
            "Pitch Random = false" + "\n" +
            "Volume Random = false" + "\n" +
            "Priority = 1" + "\n" +
            "\n" +
            "Automatic Looping:" + "\n" +
            $"If the name of the selected {nameof(NameOf.SoundContainer)}s contains “loop” then it will automatically enable “Loop”, “Follow Position”, “Stop if Transform is Null” and “Random Start Position”." + "\n" +
            "\n" +
            "Automatic Crossfades:" + "\n" +
            $"If the names of the selected {nameof(NameOf.SoundContainer)}s end in certain combinations it will automatically set up distance or intensity crossfades." + "\n" +
            "It works on multiple groups at the same time." + "\n" +
            "These are the combinations and their result:" + "\n" +
            "\n" +
            "Distance Crossfade:" + "\n" +
            "Close + Distant + Far = 3 layers" + "\n" +
            "Close + Distant = 2 layers" + "\n" +
            "Close + Far = 2 layers" + "\n" +
            "\n" +
            "Intensity Crossfade:" + "\n" +
            "Soft + Medium + Hard = 3 layers" + "\n" +
            "Soft + Hard = 2 layers" + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundPreset)}:" + "\n" +
            $"Create {nameof(NameOf.SoundPreset)} objects to get custom settings which you can apply to your {nameof(NameOf.SoundContainer)}s and {nameof(NameOf.SoundEvent)}s." + "\n" +
            $"Either manually select the preset you want to apply in the dropdown, or use “Auto Match” to automatically apply the settings based on the name of the asset." + EditorTrial.trialTooltip;

        public static string updateAudioClipsLabel = "Update AudioClips";
        public static string updateAudioClipsTooltip =
            "Updating AudioClips can be used to automatically add/remove variations." + "\n" +
            "Can be used on multiple items at once (tip: search \"t:soundContainer\")." + "\n" +
            "\n" +
            $"Refresh {nameof(AudioClip)} Group" + "\n" +
            $"Adds all {nameof(AudioClip)}s with the same name as the first {nameof(AudioClip)} (disregarding numbers, e.g. 01 02)." + "\n" +
            "\n" +
            $"Find AudioClip Group" + "\n" +
            $"Automatically finds all {nameof(AudioClip)}s containing the same name as this {nameof(NameOf.SoundContainer)} (disregarding _SC, numbers)." + "\n" +
            "\n" +
            $"If no matching {nameof(AudioClip)}s are found, it will try and remove one character at the end of the name at a time until it finds a hit." + EditorTrial.trialTooltip;

        public static string findReferencesLabel = $"Find References";
        public static string findReferencesTooltip = $"Finds all the references to the {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string findReferencesSelectAllLabel = $"Select All";
        public static string findReferencesSelectAllTooltip = $"Selects all the assets with references to the {nameof(NameOf.SoundContainer)}." + EditorTrial.trialTooltip;

        public static string findReferencesClearLabel = $"Clear";
        public static string findReferencesClearTooltip = $"Removes all the found references." + EditorTrial.trialTooltip;

        // Warning
        public static string audioClipWarningEmpty = "Empty AudioClip";
        public static string audioClipWarningNull = "Null AudioClip";

        public static string enableDistanceLabel = "Enable Distance";
        public static string enableDistanceTooltip = $"Otherwise the {nameof(NameOf.SoundContainer)} will not be affected by distance (disable for music etc)." + EditorTrial.trialTooltip;

        public static string distanceScaleLabel = "Distance Scale";
        public static string distanceScaleTooltip = 
            $"Range scale multiplier." +
            "\n" +
            $"It is multiplied by the Distance Scale of the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string loopLabel = "Loop";
        public static string loopTooltip =
            $"Makes the sound loop." + "\n" +
            "\n" +
            "If you use \"Create Assets from Selection\"  and the AudioClip name contains \"loop\"" + "\n" +
            "\n" +
            "Then it will automatically enable “Loop”, “Follow Position”, “Stop if Transform is Null” and “Random Start Position”." + EditorTrial.trialTooltip;

        public static string followPositionLabel = "Follow Position";
        public static string followPositionTooltip = $"If the {nameof(NameOf.SoundContainer)} should follow the given Transform position." + EditorTrial.trialTooltip;

        public static string stopIfTransformIsNullLabel = "Stop if Transform is Null";
        public static string stopIfTransformIsNullTooltip =
            "Automatically stops the sound if the Transform it's played at is destroyed (either the owner or position Transform)." + "\n" +
            "\n" +
            "Useful safety precaution for loops." + EditorTrial.trialTooltip;

        public static string virtualizeLabel = "Virtualize";
        public static string virtualizeTooltip = "" + EditorTrial.trialTooltip;

        public static string randomStartPositionLabel = "Random Start Position";
        public static string randomStartPositionTooltip = 
            "Starts the sound at a random position." + "\n" +
            "\n" +
            "Overrides the Start Position setting." + "\n" +
            "\n" +
            "Useful for loops." + EditorTrial.trialTooltip;

        public static string randomStartPositionMinMaxLabel = "Random Range";
        public static string randomStartPositionMinMaxTooltip = "Min/max range within the sound can start at." + EditorTrial.trialTooltip;

        public static string startPositionLabel = "Start Position";
        public static string startPositionTooltip = "0 is the start and 1 is the end." + EditorTrial.trialTooltip;

        public static string reverseLabel = "Reverse";
        public static string reverseTooltip =
            $"If enabled the AudioClip will be played backwards." + "\n" +
            "\n" +
            $"Make sure to set the start position to the end." + "\n" +
            "\n" +
            $"Reverse is only supported for AudioClips which are stored in an uncompressed format or will be decompressed at load time." + EditorTrial.trialTooltip;

        // SoundContainer Settings Warnings
        public static string distanceScaleWarningLabel = $"Distance Scale is 0. The {nameof(NameOf.SoundContainer)} will not be heard";

        public static string lockAxisEnableLabel = $"Lock Axis";
        public static string lockAxisEnableTooltip = 
            "Locks the selected axis to the selected position." + "\n" +
            "\n" +
            "Useful for 2D games if you want to lock the sound to a position along an axis." + EditorTrial.trialTooltip;

        public static string lockAxisLabel = $"Axis";
        public static string lockAxisTooltip = "The axis to lock." + EditorTrial.trialTooltip;

        public static string lockAxisPositionLabel = $"Position";
        public static string lockAxisPositionTooltip = "The position to set the locked axis to." + EditorTrial.trialTooltip;

        public static string preventEndClicksLabel = "Prevent End Clicks";
        public static string preventEndClicksTooltip =
            $"If enabled it will fade out the volume 0.1 seconds before the end of the AudioClip to prevent clicks." + "\n" +
            "\n" +
            $"If the AudioClips is shorter than 0.1 seconds or set to loop the fade will be skipped." + "\n" +
            "\n" +
            $"DC offsets and some settings in Unity make an AudioClip click at the end." + "\n" +
            "\n" +
            $"Tip: If you still experience sporadic clicks, try changing the Load Type of the AudioClips to e.g. \"Compressed In Memory\", it might help." + EditorTrial.trialTooltip;

        public static string priorityLabel = "Priority";
        public static string priorityTooltip =
            "The priority the Voice has when Voice stealing." + "\n" +
            "\n" +
            "Also the priority the Voice Effects has when Voice Effects stealing." + "\n" +
            "\n" +
            "1 is high priority, 0.5 is default priority and 0 is low priority." + "\n" +
            "\n" +
            "It's multiplied with the volume of the Voice when evaluating final priority." + EditorTrial.trialTooltip;

        public static string neverStealVoiceLabel = "Never Steal Voice";
        public static string neverStealVoiceTooltip =
            $"The {nameof(NameOf.SoundManager)} will never steal this Voice if the Voice Limit is reached (use on music etc)." + EditorTrial.trialTooltip;

        public static string neverStealVoiceEffectsLabel = "Never Steal Voice Effects";
        public static string neverStealVoiceEffectsTooltip =
            $"The {nameof(NameOf.SoundManager)} will never steal the Voice Effects on this Voice if the Voice Effect Limit is reached (use on music etc)." + EditorTrial.trialTooltip;

        public static string playOrderLabel = $"Play Order";
        public static string playOrderTooltip =
            $"Determines in which order the AudioClips will be played " + "\n" +
            "\n" +
            $"Global Random" + "\n" +
            $"All {nameof(NameOf.SoundEvent)}s will share the same global random {nameof(AudioClip)} pool, which ensures less repetition." + "\n" +
            $"Uses a pseudo random function remembering half of the length of available {nameof(AudioClip)}s it last played to avoid repetition." + "\n" +
            "\n" +
            $"Local Random" + "\n" +
            $"Same as global random except its per {nameof(NameOf.SoundEvent)} owner." + EditorTrial.trialTooltip;


        public static string dopplerAmountLabel = "Doppler Amount";
        public static string dopplerAmountTooltip =
            $"How much the pitch of the sound is changed by the relative velocity between the {nameof(AudioListener)} and the {nameof(AudioSource)}." + EditorTrial.trialTooltip;

        public static string bypassReverbZonesLabel = "Bypass Reverb Zones";
        public static string bypassReverbZonesTooltip = "Bypasses any reverb zones" + EditorTrial.trialTooltip;

        public static string bypassVoiceEffectsLabel = "Bypass Voice Effects";
        public static string bypassVoiceEffectsTooltip = 
            "Bypasses any effects on the AudioSource, e.g. Distortion and Filters." + "\n" +
            "\n" +
            "Voice effects are automatically bypassed if you don't have distortion/lowpass/highpass enabled." + EditorTrial.trialTooltip;

        public static string bypassListenerEffectsLabel = "Bypass Listener Effects";
        public static string bypassListenerEffectsTooltip = "Bypasses any effects on the listener" + EditorTrial.trialTooltip;

        // HRTF Plugin Spatialize
        public static string hrtfPluginSpatializeLabel = "HRTF Plugin Spatialize";
        public static string hrtfPluginSpatializeTooltip =
            $"Enables spatialization using external third party plugins." + "\n" +
            "\n" +
            $"You need to add the plugin and select it in the project audio settings." + "\n" +
            "\n" +
            $"Example of third party spatializers:" + "\n" +
            "\n" +
            $"Steam Audio Spatializer" + "\n" +
            $"Oculus Spatializer Unity" + "\n" +
            $"Google Resonance Audio Spatializer" + "\n" +
            $"Vive 3DSP Audio SDK Spatializer" + "\n" +
            $"Microsoft Spatializer" + "\n" +
            $"Apple PHASE Spatializer" + "\n" +
            $"Qualcomm 3D Spatializer" + EditorTrial.trialTooltip;

        public static string hrtfPluginSpatializePostEffectsLabel = "HRTF Plugin Post Effects";
        public static string hrtfPluginSpatializePostEffectsTooltip = 
            "If the spatialization plugin effect happens before before or after any effects." + EditorTrial.trialTooltip;

        // Volume
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
            $"Select all the {nameof(NameOf.SoundContainer)}s and lower the volume by -20 dB with the -1dB button." + "\n" +
            "\n" +
            $"Then to compensate you can increase the global volume with an Audio Mixer (which you then can set to +20 dB)." + EditorTrial.trialTooltip;

        public static string volumeRelativeLowerLabel = "-1 dB";
        public static string volumeRelativeLowerTooltip = 
            $"Lowers the relative volume of all the selected {nameof(NameOf.SoundContainer)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to raise the volume of one {nameof(NameOf.SoundContainer)} and keep the relative volume." + "\n" +
            "\n" +
            $"Because then you can lower all of them to get more headroom." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundContainer)}s are selected it will show the lowest volume." + EditorTrial.trialTooltip;

        public static string volumeRelativeIncreaseLabel = "+1 dB";
        public static string volumeRelativeIncreaseTooltip =
            $"Raises the relative volume of all the selected {nameof(NameOf.SoundContainer)}s." + "\n" +
            "\n" +
            $"Useful for example if you want to set the loudest volume to 0 dB but keep the relative volumes." + "\n" +
            "\n" +
            $"If multiple {nameof(NameOf.SoundContainer)}s are selected it will show the highest volume." + EditorTrial.trialTooltip;

        public static string volumeOverLimitWarning =
            $"Volume is over 0dB, please add scripting define symbol:" + "\n" +
            $"\"SONITY_ENABLE_VOLUME_INCREASE\" or lower the volume." + "\n" +
            $"Tip: Select all {nameof(NameOf.SoundContainer)}s and press \"- 1dB\" until max is 0dB.";

        public static string volumeRandomLabel = "Random";
        public static string volumeRandomTooltip = "Toggles random volume." + EditorTrial.trialTooltip;

        public static string volumeRandomRangeLabel = "Range dB";
        public static string volumeRandomRangeTooltip = "Amount of random volume to lower by in decibel." + EditorTrial.trialTooltip;

        public static string volumeDistanceLabel = "Distance";
        public static string volumeDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string volumeDistanceRolloffLabel = "Rolloff";
        public static string volumeDistanceRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string volumeDistanceCurveLabel = "Curve";
        public static string volumeDistanceCurveTooltip = "Curve of the volume over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string volumeDistanceCrossfadeEnabledLabel = "Distance Crossfade";
        public static string volumeDistanceCrossfadeEnabledTooltip =
            $"With distance crossfade you can easily crossfade between different sounds over distance." +
            "\n" +
            $"For e.g. gunshots you could add sounds with close, distant and far perspectives." +
            "\n" +
            $"You’d set the “Layers” setting to 3 for all the {nameof(NameOf.SoundContainer)}s." +
            "\n" +
            $"Then you’d set “This Is” of close to 1, distant to 2, and far to 3." + EditorTrial.trialTooltip;

        public static string volumeDistanceCrossfadeLayersLabel = "Layers";
        public static string volumeDistanceCrossfadeLayersTooltip = 
            "The number of layers the crossfade is based on." +
            "\n" +
            "You must have at least 2 layers." + EditorTrial.trialTooltip;

        public static string volumeDistanceCrossfadeThisIsLabel = "This Is";
        public static string volumeDistanceCrossfadeThisIsTooltip = 
            $"Which layer this is." + "\n" +
            "\n" +
            $"Set up with other {nameof(NameOf.SoundContainer)}s for the other layers." + "\n" +
            "\n" +
            $"Lower numbers are closer and higher are more distant." + EditorTrial.trialTooltip;

        public static string volumeDistanceCrossfadeRolloffLabel = "Rolloff";
        public static string volumeDistanceCrossfadeRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string volumeDistanceCrossfadeCurveLabel = "Curve";
        public static string volumeDistanceCrossfadeCurveTooltip = 
            "How the layers are crossfaded over distance." + "\n" +
            "\n" +
            "Standard is from 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string volumeIntensityEnableLabel = "Intensity";
        public static string volumeIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string volumeIntensityRolloffLabel = "Rolloff";
        public static string volumeIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string volumeIntensityStrengthLabel = "Strength";
        public static string volumeIntensityStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string volumeIntensityCurveLabel = "Curve";
        public static string volumeIntensityCurveTooltip = "Curve of the volume over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        public static string volumeIntensityCrossfadeEnabledLabel = "Intensity Crossfade";
        public static string volumeIntensityCrossfadeEnabledTooltip =
            $"With intensity crossfade you can easily crossfade between different sounds over intensity." +
            "\n" +
            $"For e.g. impacts you could add sounds with hard, medium and soft variations." +
            "\n" +
            $"You’d set the “Layers” setting to 3 for all the {nameof(NameOf.SoundContainer)}s." +
            "\n" +
            $"Then you’d set “This Is” of hard to 3, medium to 2 and soft to 1." + EditorTrial.trialTooltip;

        public static string volumeIntensityCrossfadeLayersLabel = "Layers";
        public static string volumeIntensityCrossfadeLayersTooltip = 
            "The number of layers the crossfade is based on." + "\n" +
            "\n" +
            "You must have at least 2 layers." + EditorTrial.trialTooltip;

        public static string volumeIntensityCrossfadeThisIsLabel = "This Is";
        public static string volumeIntensityCrossfadeThisIsTooltip = 
            $"Which layer this is." + "\n" +
            "\n" +
            $"Set up with other {nameof(NameOf.SoundContainer)}s for the other layers." + "\n" +
            "\n" +
            $"Higher numbers are harder and lower numbers are softer." + EditorTrial.trialTooltip;

        public static string volumeIntensityCrossfadeRolloffLabel = "Rolloff";
        public static string volumeIntensityCrossfadeRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string intensityCrossfadeCurveLabel = "Curve";
        public static string intensityCrossfadeCurveTooltip = "How the layers are crossfaded over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Pitch
        public static string pitchLabel = "Pitch st";
        public static string pitchTooltip = $"The pitch in semitones.\n\nRange -24 to +24." + EditorTrial.trialTooltip;

        public static string pitchRandomLabel = "Random";
        public static string pitchRandomTooltip = "Toggle random pitch." + EditorTrial.trialTooltip;

        public static string pitchRandomRangeLabel = "Range st";
        public static string pitchRandomRangeTooltip = "Amount of random pitch variation in semitones (bipolar)." + EditorTrial.trialTooltip;

        public static string pitchIntensityEnableLabel = "Intensity";
        public static string pitchIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string pitchIntensityBaseLabel = "Low st";
        public static string pitchIntensityBaseTooltip = "The lowest intensity in semitones.\n\nRange -128 to 128" + EditorTrial.trialTooltip;

        public static string pitchIntensityRangeLabel = "High st";
        public static string pitchIntensityRangeTooltip = "The highest intensity in semitones.\n\nRange -128 to 128" + EditorTrial.trialTooltip;

        public static string pitchIntensityRolloffLabel = "Rolloff";
        public static string pitchIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string pitchIntensityCurveLabel = "Curve";
        public static string pitchIntensityCurveTooltip = "Curve of the pitch over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Spatial Blend
        public static string spatialBlendBaseLabel = "Spatial Blend";
        public static string spatialBlendBaseTooltip = "Amount of spatial blend.\n\n0 is 2D and 1 is 3D." + EditorTrial.trialTooltip;

        public static string spatialBlendDistanceLabel = "Distance";
        public static string spatialBlendDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string spatialBlendDistanceRolloffLabel = "Rolloff";
        public static string spatialBlendDistanceRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string spatialBlendDistance3DIncreaseLabel = "Increase";
        public static string spatialBlendDistance3DIncreaseTooltip = "Increase the amount of spatial blend." + EditorTrial.trialTooltip;

        public static string spatialBlendDistanceCurveLabel = "Curve";
        public static string spatialBlendDistanceCurveTooltip = "Curve of the spatial blend over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string spatialBlendIntensityEnableLabel = "Intensity";
        public static string spatialBlendIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string spatialBlendIntensityRolloffLabel = "Rolloff";
        public static string spatialBlendIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string spatialBlendIntensityStrengthLabel = "Strength";
        public static string spatialBlendIntensityStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string spatialBlendIntensityCurveLabel = "Curve";
        public static string spatialBlendIntensityCurveTooltip = "Curve of the spatial blend over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Spatial Spread
        public static string spatialSpreadBaseLabel = "Spatial Spread °";
        public static string spatialSpreadBaseTooltip = 
            "From 0 to 360 degrees." + "\n" +
            "\n" +
            "Only the 3D part of the sound is affected by the spatial spread." + EditorTrial.trialTooltip;

        public static string spatialSpreadDistanceLabel = "Distance";
        public static string spatialSpreadDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string spatialSpreadDistanceRolloffLabel = "Rolloff";
        public static string spatialSpreadDistanceRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string spatialSpreadDistanceCurveLabel = "Curve";
        public static string spatialSpreadDistanceCurveTooltip = "Curve of the spatial spread over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string spatialSpreadIntensityEnableLabel = "Intensity";
        public static string spatialSpreadIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string spatialSpreadIntensityRolloffLabel = "Rolloff";
        public static string spatialSpreadIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string spatialSpreadIntensityStrengthLabel = "Strength";
        public static string spatialSpreadIntensityStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string spatialSpreadIntensityCurveLabel = "Curve";
        public static string spatialSpreadIntensityCurveTooltip = "Curve of the spatial spread over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Stereo Pan
        public static string stereoPanOffsetLabel = "Stereo Pan L/R";
        public static string stereoPanOffsetTooltip =
            "-1 is left and 1 right." + "\n" +
            "\n" +
            "Only the 2D part of the sound is affected by the stereo pan." + EditorTrial.trialTooltip;

        public static string stereoPanAngleToSteroPanUseLabel = "Angle To Stereo Pan";
        public static string stereoPanAngleToSteroPanUseTooltip =
            "Pans the sound depending on the angle between the Voice and the AudioListener." + "\n" +
            "\n" +
            "Only the 2D part of the sound is affected by the stereo pan." + EditorTrial.trialTooltip;

        public static string stereoPanAngleToSteroPanStrengthLabel = "Strength";
        public static string stereoPanAngleToSteroPanStrengthTooltip = "The amount of angle to stereo pan." + EditorTrial.trialTooltip;

        public static string stereoPanAngleToSteroPanRolloffLabel = "Rolloff";
        public static string stereoPanAngleToSteroPanRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        // Reverb Zone Mix
        public static string reverbZoneMixDecibelLabel = "Reverb dB";
        public static string reverbZoneMixDecibelTooltip = "The amount of reverb zone send in decibel." + EditorTrial.trialTooltip;

        public static string reverbZoneMixDistanceLabel = "Distance";
        public static string reverbZoneMixDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string reverbZoneMixDistanceRolloffLabel = "Rolloff";
        public static string reverbZoneMixDistanceRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string reverbZoneMixDistanceIncreaseLabel = "Increase";
        public static string reverbZoneMixDistanceIncreaseTooltip = "Increase the amount of reverb mix." + EditorTrial.trialTooltip;

        public static string reverbZoneMixDistanceCurveLabel = "Curve";
        public static string reverbZoneMixDistanceCurveTooltip = "Curve of the reverb zone over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string reverbZoneMixIntensityEnableLabel = "Intensity";
        public static string reverbZoneMixIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string reverbZoneMixIntensityRolloffLabel = "Rolloff";
        public static string reverbZoneMixIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string reverbZoneMixIntensityStrengthLabel = "Strength";
        public static string reverbZoneMixIntensityStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string reverbZoneMixIntensityCurveLabel = "Curve";
        public static string reverbZoneMixIntensityCurveTooltip = "Curve of the reverb zone over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Distortion
        public static string distortionEnableLabel = "Distortion";
        public static string distortionEnableTooltip = 
            "Waveshaper type distortion." + "\n" +
            "\n" +
            "0 is unchanged, 1 is distorted." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundContainer)} Voice Effects are applied per Voice." + "\n" +
            "\n" +
            "If distortion amount is 0 the effect is disabled internally for performance." + "\n" +
            "\n" +
            $"The number of active Voice Effects are limited by the “Voice Effect Limit” on the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            "DSP effects are not available in WebGL." + EditorTrial.trialTooltip;

        public static string distortionAmountLabel = "Amount";
        public static string distortionAmountTooltip =
            "The amount of distortion." + "\n" +
            "\n" +
            "0 is clean, 1 is distorted." + EditorTrial.trialTooltip;

        public static string distortionDistanceLabel = "Distance";
        public static string distortionDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string distortionDistanceRolloffLabel = "Rolloff";
        public static string distortionDistanceRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string distortionDistanceCurveLabel = "Curve";
        public static string distortionDistanceCurveTooltip =  
            "Curve of the distortion over distance." + "\n" +
            "\n" +
            "1 (close) is distorted, 0 (far) is clean." + EditorTrial.trialTooltip;

        public static string distortionIntensityEnableLabel = "Intensity";
        public static string distortionIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string distortionIntensityRolloffLabel = "Rolloff";
        public static string distortionIntensityRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string distortionIntensityStrengthLabel = "Strength";
        public static string distortionIntensityStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string distortionIntensityCurveLabel = "Curve";
        public static string distortionIntensityCurveTooltip = 
            "Curve of the distortion over intensity." + "\n" +
            "\n" +
            "0 (soft) is clean, 1 (hard) is distortion." + EditorTrial.trialTooltip;

        // Lowpass
        public static string lowpassEnableLabel = "Lowpass";
        public static string lowpassEnableTooltip =
            "Lowpass filter with a variable amount." + "\n" + 
            "\n" +
            "Maximum of 6dB per octave." + "\n" +
             "\n" +
            $"{nameof(NameOf.SoundContainer)} voice effects are applied per Voice." + "\n" +
             "\n" +
            "If frequency is 20,000 Hz or amount is 0 dB the effect is disabled internally for performance." + "\n" +
             "\n" +
            $"The number of active Voice Effects are limited by the “Voice Effect Limit” on the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            "DSP effects are not available in WebGL." + EditorTrial.trialTooltip;

        public static string lowpassFrequencyLabel = "Frequency Hz";
        public static string lowpassFrequencyTooltip = "The cutoff frequency of the lowpass filter in Hz." + EditorTrial.trialTooltip;

        public static string lowpassAmountLabel = "Amount dB";
        public static string lowpassAmountTooltip = "The base lowpass slope in dB per octave." + EditorTrial.trialTooltip;

        public static string lowpassDistanceLabel = "Distance";
        public static string lowpassDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string lowpassDistanceFrequencyRolloffLabel = "Frequency Rolloff";
        public static string lowpassDistanceFrequencyRolloffTooltip =
            "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string lowpassDistanceFrequencyCurveLabel = "Frequency Curve";
        public static string lowpassDistanceFrequencyCurveTooltip =
            "Curve of the lowpass frequency over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string lowpassDistanceAmountRolloffLabel = "Amount Rolloff";
        public static string lowpassDistanceAmountRolloffTooltip =
            "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string lowpassDistanceAmountCurveLabel = "Amount Curve";
        public static string lowpassDistanceAmountCurveTooltip =
            "Curve of the lowpass slope amount over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string lowpassIntensityEnableLabel = "Intensity";
        public static string lowpassIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string lowpassIntensityFrequencyLabel = "Intensity Frequency";
        public static string lowpassIntensityFrequencyTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string lowpassIntensityFrequencyRolloffLabel = "Frequency Rolloff";
        public static string lowpassIntensityFrequencyRolloffTooltip =
            "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string lowpassIntensityFrequencyStrengthLabel = "Frequency Strength";
        public static string lowpassIntensityFrequencyStrengthTooltip =
            "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string lowpassIntensityFrequencyCurveLabel = "Frequency Curve";
        public static string lowpassIntensityFrequencyCurveTooltip =
            "Curve of the lowpass frequency over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        public static string lowpassIntensityAmountLabel = "Intensity Amount";
        public static string lowpassIntensityAmountTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string lowpassIntensityAmountRolloffLabel = "Amount Rolloff";
        public static string lowpassIntensityAmountRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string lowpassIntensityAmountStrengthLabel = "Amount Strength";
        public static string lowpassIntensityAmountStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string lowpassIntensityAmountCurveLabel = "Amount Curve";
        public static string lowpassIntensityAmountCurveTooltip = "Curve of the lowpass amount over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Highpass
        public static string highpassEnableLabel = "Highpass";
        public static string highpassEnableTooltip =
            "Highpass filter with a variable amount." + "\n" +
            "\n" +
            "Maximum of 6dB per octave." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundContainer)} voice effects are applied per Voice." + "\n" +
             "\n" +
            "If frequency is 20 Hz or amount is 0 dB the effect is disabled internally for performance." + "\n" +
             "\n" +
            $"The number of active Voice Effects are limited by the “Voice Effect Limit” on the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            "DSP effects are not available in WebGL." + EditorTrial.trialTooltip;

        public static string highpassFrequencyLabel = "Frequency Hz";
        public static string highpassFrequencyTooltip = "The cutoff frequency of the highpass filter in Hz." + EditorTrial.trialTooltip;

        public static string highpassAmountLabel = "Amount dB";
        public static string highpassAmountTooltip = "The base highpass slope in dB per octave." + EditorTrial.trialTooltip;

        public static string highpassDistanceLabel = "Distance";
        public static string highpassDistanceTooltip = $"Changes the sound over distance." + EditorTrial.trialTooltip;

        public static string highpassDistanceFrequencyRolloffLabel = "Frequency Rolloff";
        public static string highpassDistanceFrequencyRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string highpassDistanceFrequencyCurveLabel = "Frequency Curve";
        public static string highpassDistanceFrequencyCurveTooltip = "Curve of the highpass frequency over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string highpassDistanceAmountRolloffLabel = "Amount Rolloff";
        public static string highpassDistanceAmountRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string highpassDistanceAmountCurveLabel = "Amount Curve";
        public static string highpassDistanceAmountCurveTooltip = "Curve of the highpass slope amount over distance.\n\nFrom 0 (close) to 1 (distant)." + EditorTrial.trialTooltip;

        public static string highpassIntensityEnableLabel = "Intensity";
        public static string highpassIntensityEnableTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string highpassIntensityFrequencyLabel = "Intensity Frequency";
        public static string highpassIntensityFrequencyTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string highpassIntensityFrequencyRolloffLabel = "Frequency Rolloff";
        public static string highpassIntensityFrequencyRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string highpassIntensityFrequencyStrengthLabel = "Frequency Strength";
        public static string highpassIntensityFrequencyStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string highpassIntensityFrequencyCurveLabel = "Frequency Curve";
        public static string highpassIntensityFrequencyCurveTooltip = "Curve of the highpass frequency over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        public static string highpassIntensityAmountLabel = "Intensity Amount";
        public static string highpassIntensityAmountTooltip = 
            $"Changes the sound over intensity.\n\nUse on for example physics sounds where you pass the velocity with a {nameof(SoundParameterIntensity)}." + EditorTrial.trialTooltip;

        public static string highpassIntensityAmountRolloffLabel = "Amount Rolloff";
        public static string highpassIntensityAmountRolloffTooltip = "The power of the rolloff.\n\n0 is linear." + EditorTrial.trialTooltip;

        public static string highpassIntensityAmountStrengthLabel = "Amount Strength";
        public static string highpassIntensityAmountStrengthTooltip = "How much effect the intensity should have." + EditorTrial.trialTooltip;

        public static string highpassIntensityAmountCurveLabel = "Amount Curve";
        public static string highpassIntensityAmountCurveTooltip = "Curve of the highpass amount over intensity.\n\nFrom 0 (soft) to 1 (hard)." + EditorTrial.trialTooltip;

        // Bottom
        public static string showPreviewCurvesLabel = "Show Preview Curves";
        public static string showPreviewCurvesTooltip = "Toggles showing the preview curves." + EditorTrial.trialTooltip;

        public static string resetSettingsLabel = "Reset Options";
        public static string resetSettingsTooltip = $"Resets the {nameof(NameOf.SoundContainer)} to the default options." + EditorTrial.trialTooltip;

        public static string resetAllLabel = "Reset All";
        public static string resetAllTooltip = $"Resets the {nameof(NameOf.SoundContainer)} and the {nameof(AudioClip)}s to the default settings." + EditorTrial.trialTooltip;
    }
}
#endif