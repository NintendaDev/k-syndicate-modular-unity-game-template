// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorTextSoundPhysics {

        public static string soundPhysicsTooltip =
            $"{nameof(NameOf.SoundPhysics)} is a component used for easily playing {nameof(NameOf.SoundEvent)}s on physics interactions." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundPhysics)} is split up into 3D/2D versions with and without friction because of performance reasons." + "\n" +
            "\n" +
            $"If friction sounds aren’t needed and performance is a priority then use the {nameof(NameOf.SoundPhysics)} with no friction." + "\n" +
            "\n" +
            $"A {nameof(Rigidbody)} or {nameof(Rigidbody2D)} is required on this object." + "\n" +
            "\n" +
            $"One or several {nameof(Collider)} or {nameof(Collider2D)} should be placed on this object or its children." + "\n" +
            "\n" +
            $"Use intensity record in the {nameof(NameOf.SoundEvent)} for easy scaling of the velocity into a 0 to 1 range." + "\n" +
            "\n" +
            $"All {nameof(NameOf.SoundPhysics)} components are multi-object editable." + EditorTrial.trialTooltip;

        public static string warningRigidbody3D = $"{nameof(Rigidbody)} required";
        public static string warningRigidbody2D = $"{nameof(Rigidbody2D)} required";

        public static string impactHeaderLabel = $"Impact";
        public static string impactHeaderTooltip =
            $"Is triggered when the object starts touching a collider." + "\n" +
            "\n" +
            $"OnCollision uses velocity from Collision.relativeVelocity.magnitude and the position of the Collision.contacts with the highest impulse.magnitude." + "\n" +
            "\n" +
            $"OnTrigger uses velocity from Rigidbody.velocity.magnitude." + EditorTrial.trialTooltip;

        public static string impactPlayLabel = $"Play Impact";
        public static string impactPlayTooltip = impactHeaderTooltip;

        public static string frictionHeaderLabel = $"Friction";
        public static string frictionHeaderTooltip =
            $"Is triggered when the object is continuously touching a collider." + "\n" +
            "\n" +
            $"Uses velocity from Rigidbody.velocity.magnitude." + EditorTrial.trialTooltip;

        public static string frictionPlayLabel = $"Play Friction";
        public static string frictionPlayTooltip = frictionHeaderTooltip;

        public static string exitHeaderLabel = $"Exit";
        public static string exitHeaderTooltip =
            $"Is triggered when the object stops touching a collider." + "\n" +
            "\n" +
            $"Uses velocity from Rigidbody.velocity.magnitude." + EditorTrial.trialTooltip;

        public static string exitPlayLabel = $"Play Exit";
        public static string exitPlayTooltip = exitHeaderTooltip;

        public static string playOnLabel = $"Play On";
        public static string playOnTooltip =
            $"Selects if the {nameof(NameOf.SoundEvent)}s should be played when a OnCollision and/or OnTrigger event occurs." + "\n" +
            "\n" +
            $"OnTrigger is when you have a collider which is set to “Is Trigger”." + EditorTrial.trialTooltip;

        public static string conditionsLabel = $"Conditions";
        public static string conditionsTooltip =
            $"If enabled then any {nameof(NameOf.SoundPhysicsCondition)} added will be used to decide if the physics interaction should play a {nameof(NameOf.SoundEvent)} or not." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundPhysicsCondition)}s can also be used for playing different sounds with a single {nameof(NameOf.SoundEvent)} by assigning {nameof(NameOf.SoundTag)}s in the {nameof(NameOf.SoundPhysicsCondition)}s." + EditorTrial.trialTooltip;

        public static string soundTagLabel = $"SoundTags";
        public static string soundTagTooltip =
            $"If enabled then any {nameof(NameOf.SoundTag)}s added will be sent when triggering the {nameof(NameOf.SoundEvent)}." + "\n" +
            "\n" +
            $"If a {nameof(NameOf.SoundTag)} is not null it will override any {nameof(NameOf.SoundTag)}s which might be sent through the {nameof(NameOf.SoundPhysicsCondition)}s." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundTag)}s can be used for triggering different sounds through a single {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        // Reset
        public static string resetSettingsLabel = $"Reset Settings";
        public static string resetSettingsTooltip = $"Resets to default values without removing any objects." + EditorTrial.trialTooltip;

        public static string resetAllLabel = $"Reset All";
        public static string resetAllTooltip = $"Resets everything." + EditorTrial.trialTooltip;

    }
}
#endif