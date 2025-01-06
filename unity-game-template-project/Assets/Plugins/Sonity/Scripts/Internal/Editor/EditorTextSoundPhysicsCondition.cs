// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

namespace Sonity.Internal {

    public class EditorTextSoundPhysicsCondition {

        public static string soundPhysicsConditionTooltip =
            $"{nameof(NameOf.SoundPhysicsCondition)} objects are used in the {nameof(NameOf.SoundPhysics)} component to decide if a physics interaction should play a {nameof(NameOf.SoundEvent)} or not." + "\n" +
            "\n" +
            $"They make it easy to manage a large amount of physics objects and linking conditions together with the child nesting feature." + "\n" +
            "\n" +
            $"You can also use them for playing different sounds with a single {nameof(NameOf.SoundEvent)} by using SoundTags." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundPhysicsCondition)} objects are multi-object editable." + EditorTrial.trialTooltip;

        public static string childrenLabel = $"Children";
        public static string childrenTooltip =
            $"You can nest {nameof(NameOf.SoundPhysicsCondition)}s so you don't have to update all your {nameof(NameOf.SoundPhysics)} components when adding a new {nameof(NameOf.SoundPhysicsCondition)}." + "\n" +
            "\n" +
            $"The children of the parent {nameof(NameOf.SoundPhysicsCondition)} are evaluated after the parent." + EditorTrial.trialTooltip;

        public static string parentLabel = $"Parent";
        public static string parentTooltip = $"The parent {nameof(NameOf.SoundPhysicsCondition)} is evaluated before the children." + EditorTrial.trialTooltip;


        public static string soundTagLabel = $"SoundTag";
        public static string soundTagTooltip =
            $"By utilizing the {nameof(NameOf.SoundTag)}s you can play different sounds with a single {nameof(NameOf.SoundEvent)} (see the example physics assets on how you can set that up)." + "\n" +
            "\n" +
            $"If no {nameof(NameOf.SoundTag)} is assigned it won't affect the output {nameof(NameOf.SoundTag)}, so if a child has an assigned {nameof(NameOf.SoundTag)} and the parent doesn't, the child one will be used." + "\n" +
            "\n" +
            $"They are useful to split into different child {nameof(NameOf.SoundPhysicsCondition)}s with different {nameof(NameOf.SoundTag)}." + EditorTrial.trialTooltip;

        public static string playOnLabel = $"Play On";
        public static string playOnTooltip =
            $"Selects if it should be played when a OnCollision and/or OnTrigger event occurs." + "\n" +
            "\n" +
            $"OnTrigger is when you have a collider which is set to “Is Trigger”." + EditorTrial.trialTooltip;

        public static string playDisregardingConditionsLabel = $"Play Disregarding Conditions";
        public static string playDisregardingConditionsTooltip =
            $"If enabled the {nameof(NameOf.SoundPhysicsCondition)} will play regardless of its own conditions." + "\n" +
            "\n" +
            $"Useful to combine with child {nameof(NameOf.SoundPhysicsCondition)}s with different {nameof(NameOf.SoundTag)}." + EditorTrial.trialTooltip;

        public static string abortAllOnNoMatchLabel = $"Abort All On No Match";
        public static string abortAllOnNoMatchTooltip =
            $"If enabled and the assigned condition is not matched it will abort playing and disregard all other conditions." + "\n" +
            "\n" +
            $"If not enabled and the assigned condition is not matched another condition which is met can play the sound." + EditorTrial.trialTooltip;

        public static string abortAllOnMatchLabel = $"Abort All On Match";
        public static string abortAllOnMatchTooltip =
            $"If enabled and the assigned condition is matched it will abort playing and disregard all other conditions." + "\n" +
            "\n" +
            $"If not enabled and the assigned condition is matched another condition which is met can play the sound." + EditorTrial.trialTooltip;

        public static string tagHeaderLabel = $"Tag";
        public static string tagHeaderTooltip = $"Checks if the Tag of the colliding GameObject matches any of the specified Tags." + EditorTrial.trialTooltip;

        public static string tagIsLabel = $"Is Tag";
        public static string tagIsTooltip = tagHeaderTooltip;

        public static string tagIsNotLabel = $"Is Not Tag";
        public static string tagIsNotTooltip = tagHeaderTooltip;

        public static string layerHeaderLabel = $"Layer";
        public static string layerHeaderTooltip = $"Checks if the Layer of the colliding GameObject matches any of the specified Layers." + EditorTrial.trialTooltip;

        public static string layerIsLabel = $"Is Layer";
        public static string layerIsTooltip = layerHeaderTooltip;

        public static string layerIsNotLabel = $"Is Not Layer";
        public static string layerIsNotTooltip = layerHeaderTooltip;

        public static string terrainNameHeaderLabel = $"Terrain Name";
        public static string terrainNameHeaderTooltip =
            $"Checks if the name of the most dominant Terrain Layer of the colliding Terrain contains any of the specified strings (it is not case sensitive)." + "\n" +
            "\n" +
            $"E.g. you have multiple Terrain Layers with different grass textures, if the names of the Terrain Layers contain the string it will play the sound." + "\n" +
            "\n" +
            $"Terrain conditions are only evaluated if the colliding object has a Terrain component." + "\n" +
            "\n" +
            $"The center point of the {nameof(NameOf.SoundPhysics)} object is used to calculate the dominant Terrain Layer because it is more stable than the contacts." + EditorTrial.trialTooltip;

        public static string terrainNameIsLabel = $"Is Terrain Name";
        public static string terrainNameIsTooltip = terrainNameHeaderTooltip;

        public static string terrainNameIsNotLabel = $"Is Not Terrain Name";
        public static string terrainNameIsNotTooltip = terrainNameHeaderTooltip;

        public static string terrainIndexHeaderLabel = $"Terrain Index";
        public static string terrainIndexHeaderTooltip =
            $"Checks if the index of the most dominant Terrain Layer of the colliding Terrain is any of the specified indexes." + "\n" +
            "\n" +
            $"Terrain conditions are only evaluated if the colliding object has a Terrain component." + "\n" +
            "\n" +
            $"The center point of the {nameof(NameOf.SoundPhysics)} object is used to calculate the dominant Terrain Layer because it is more stable than the contacts." + EditorTrial.trialTooltip;

        public static string terrainIndexIsLabel = $"Is Terrain Index";
        public static string terrainIndexIsTooltip = terrainIndexHeaderTooltip;

        public static string terrainIndexIsNotLabel = $"Is Not Terrain Index";
        public static string terrainIndexIsNotTooltip = terrainIndexHeaderTooltip;

        public static string componentHeaderLabel = $"Component";
        public static string componentHeaderTooltip = 
            $"Checks if the Components of the colliding GameObject match any of the specified Components." + "\n" +
            "\n" +
            $"The names are case sensitive." + EditorTrial.trialTooltip;

        public static string componentHasLabel = $"Has Component";
        public static string componentHasTooltip = componentHeaderTooltip;

        public static string componentHasNotLabel = $"Has Not Component";
        public static string componentHasNotTooltip = componentHeaderTooltip;

        public static string resetSettingsLabel = $"Reset Settings";
        public static string resetSettingsTooltip = $"Disables all the conditions of the parent and resets the basic settings." + EditorTrial.trialTooltip;

        public static string resetAllLabel = $"Reset All";
        public static string resetAllTooltip = $"Resets everything." + EditorTrial.trialTooltip;
    }
}
#endif