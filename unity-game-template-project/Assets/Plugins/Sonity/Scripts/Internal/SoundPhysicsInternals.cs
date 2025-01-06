// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPhysicsInternals {

        public SoundPhysicsComponentType soundPhysicsComponentType = SoundPhysicsComponentType.SoundPhysics;
        public PhysicsDimension physicsDimension = PhysicsDimension._3D;

        [NonSerialized]
        public Rigidbody cachedRigidbody;
        [NonSerialized]
        public Rigidbody2D cachedRigidbody2D;
        [NonSerialized]
        public Transform cachedTransform;
        [NonSerialized]
        public bool initialized = false;

        public void FindComponents(GameObject gameObject) {
            if (!initialized) {
                initialized = true;
                if (physicsDimension == PhysicsDimension._3D) {
                    cachedRigidbody = gameObject.GetComponent<Rigidbody>();
                } else if (physicsDimension == PhysicsDimension._2D) {
                    cachedRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                }
                cachedTransform = gameObject.GetComponent<Transform>();
            }
        }

        public bool soundTagOverride = false;

        // Impact
        public bool impactExpand = true;
        public bool impactPlay = true;
        public SoundPhysicsPart[] impactSoundPhysicsParts = new SoundPhysicsPart[1];
        public SoundParameterIntensity impactSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

        // Friction
        public bool frictionExpand = true;
        public bool frictionPlay = false;
        public SoundPhysicsPart[] frictionSoundPhysicsParts = new SoundPhysicsPart[1];
        public SoundParameterIntensity frictionSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Continuous);

        // Exit
        public bool exitExpand = true;
        public bool exitPlay = false;
        public SoundPhysicsPart[] exitSoundPhysicsParts = new SoundPhysicsPart[1];
        public SoundParameterIntensity exitSoundParameter = new SoundParameterIntensity(0f, UpdateMode.Once);

        public void FixedUpdate() {
            for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                if (part.frictionIsPlaying) {
                    if (physicsDimension == PhysicsDimension._3D) {
#if UNITY_6000_0_OR_NEWER
                        frictionSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                        frictionSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                    } else if (physicsDimension == PhysicsDimension._2D) {
#if UNITY_6000_0_OR_NEWER
                        frictionSoundParameter.Intensity = cachedRigidbody2D.linearVelocity.magnitude;
#else
                        frictionSoundParameter.Intensity = cachedRigidbody2D.velocity.magnitude;
#endif
                    }
                    if (frictionSoundParameter.Intensity == 0f) {
                        // Friction Stop
                        part.frictionIsPlaying = false;
                        for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                            SoundManagerBase.Instance.Internals.Stop(part.soundEvents[ii], cachedTransform);
                        }
                    }
                }
            }
        }

        private SoundTagBase tempSoundTag;

        private void ResetSoundTag() {
            tempSoundTag = null;
        }

        private void SetSoundTag(SoundPhysicsPart part, SoundPhysicsConditionBase condition) {
            if (soundTagOverride && part.soundTag != null) {
                tempSoundTag = part.soundTag;
            } else {
                if (tempSoundTag == null && condition.internals.soundTag != null) {
                    tempSoundTag = condition.internals.soundTag;
                }
            }
        }

#if UNITY_EDITOR
        private bool conditionsIsNullDebugOnce = false;
#endif

        private enum ShouldPlayCheckResult {
            Continue,
            True,
            False,
        }

        private ShouldPlayCheckResult shouldPlayCheckResult;
        private bool shouldPlayTriggered = false;
        private bool shouldNotPlayTriggered = false;
        private bool terrainDataFound = false;
        private int terrainIndex = 0;
        private bool conditionsAllAreNull = true;

        private bool ShouldPlayConditionMain(SoundPhysicsPart part, GameObject gameObject, bool isCollision) {
            ResetSoundTag();
            if (!part.conditionsUse) {
                return true;
            }
            // Reset values
            shouldPlayCheckResult = ShouldPlayCheckResult.Continue;
            shouldPlayTriggered = false;
            shouldNotPlayTriggered = false;
            terrainDataFound = false;
            terrainIndex = 0;
            conditionsAllAreNull = true;
            for (int i = 0; i < part.conditions.Length; i++) {
                SoundPhysicsConditionBase condition = part.conditions[i];
                if (condition.internals.CheckIsInfiniteLoop(condition, false)) {
                    return false;
                }
                shouldPlayCheckResult = ShouldPlayConditionCheckSub(condition, part, gameObject, isCollision, false, null);
                if (shouldPlayCheckResult == ShouldPlayCheckResult.True) {
                    return true;
                } else if (shouldPlayCheckResult == ShouldPlayCheckResult.False) {
                    return false;
                }
            }
            if (conditionsAllAreNull) {
                if (soundTagOverride) {
                    tempSoundTag = part.soundTag;
                }
                // If all null, then play the sound
                return true;
            }
            if (shouldNotPlayTriggered) {
                return false;
            } else {
                return shouldPlayTriggered;
            }
        }

        private ShouldPlayCheckResult ShouldPlayConditionCheckSub(SoundPhysicsConditionBase condition, SoundPhysicsPart part, GameObject gameObject, bool isCollision, bool isSub, SoundPhysicsConditionBase parentCondition) {
            if (condition == null) {
#if UNITY_EDITOR
                if (isSub) {
                    if (ShouldDebug.Warnings()) {
                        if (conditionsIsNullDebugOnce == false) {
                            conditionsIsNullDebugOnce = true;
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPhysicsCondition)} is null in \"" + parentCondition.name + "\".", parentCondition);
                        }
                    }
                } else {
                    if (ShouldDebug.Warnings()) {
                        if (conditionsIsNullDebugOnce == false) {
                            conditionsIsNullDebugOnce = true;
                            Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPhysics)} Condition is null on \"" + cachedTransform.name + "\".", cachedTransform);
                        }
                    }
                }
#endif
                return ShouldPlayCheckResult.Continue;
            }
            // If all null, then play the sound
            conditionsAllAreNull = false;
            shouldNotPlayTriggered = false;

            if ((condition.internals.physicsPlayOn == PhysicsPlayOn.OnCollisionAndTrigger)
                || (condition.internals.physicsPlayOn == PhysicsPlayOn.OnCollision && isCollision)
                || (condition.internals.physicsPlayOn == PhysicsPlayOn.OnTrigger && !isCollision)) {
                if (condition.internals.playDisregardingConditions) {
                    SetSoundTag(part, condition);
                    shouldPlayTriggered = true;
                } else {
                    // Is Not Tag
                    if (condition.internals.isNotTagUse) {
                        for (int ii = 0; ii < condition.internals.isNotTagArray.Length; ii++) {
                            if (gameObject.CompareTag(condition.internals.isNotTagArray[ii])) {
                                if (condition.internals.isNotTagAbortAllOnMatch) {
                                    return ShouldPlayCheckResult.False;
                                } else {
                                    shouldNotPlayTriggered = true;
                                    break;
                                }
                            }
                        }
                    }
                    // Is Not Layer
                    if (condition.internals.isNotLayerUse) {
                        for (int ii = 0; ii < condition.internals.isNotLayerArray.Length; ii++) {
                            if (condition.internals.isNotLayerArray[ii] == gameObject.layer) {
                                if (condition.internals.isNotLayerAbortAllOnMatch) {
                                    return ShouldPlayCheckResult.False;
                                } else {
                                    shouldNotPlayTriggered = true;
                                    break;
                                }
                            }
                        }
                    }
                    // On Collision Terrain
                    if (!terrainDataFound && (condition.internals.isTerrainIndexUse || condition.internals.isNotTerrainIndexUse || condition.internals.isTerrainNameUse || condition.internals.isNotTerrainNameUse)) {
                        tempTerrainComponent = gameObject.GetComponent<Terrain>();
                        if (tempTerrainComponent != null) {
                            terrainDataFound = true;
                            // Skip contacts and use the center so the position is more stable
                            terrainIndex = GetDominantTextureIndex(cachedTransform.position, tempTerrainComponent);
                        }
                    }
                    // Is Not Terrain Index
                    if (condition.internals.isNotTerrainIndexUse && terrainDataFound) {
                        for (int ii = 0; ii < condition.internals.isNotTerrainIndexArray.Length; ii++) {
                            if (condition.internals.isNotTerrainIndexArray[ii] == terrainIndex) {
                                if (condition.internals.isNotTerrainIndexAbortAllOnMatch) {
                                    return ShouldPlayCheckResult.False;
                                } else {
                                    shouldNotPlayTriggered = true;
                                    break;
                                }
                            }
                        }
                    }
                    // Is Not Terrain Name
                    if (condition.internals.isNotTerrainNameUse && terrainDataFound) {
                        for (int ii = 0; ii < condition.internals.isNotTerrainNameArray.Length; ii++) {
                            if (StringOperation.ContainsCaseInsensitive(tempTerrainLayerName, condition.internals.isNotTerrainNameArray[ii])) {
                                if (condition.internals.isNotTerrainNameAbortAllOnMatch) {
                                    return ShouldPlayCheckResult.False;
                                } else {
                                    shouldNotPlayTriggered = true;
                                    break;
                                }
                            }
                        }
                    }
                    // Has Not Component
                    if (condition.internals.hasNotComponentUse) {
                        for (int ii = 0; ii < condition.internals.hasNotComponentArray.Length; ii++) {
                            if (condition.internals.hasNotComponentArray[ii] != null) {
                                if (gameObject.GetComponent(condition.internals.hasNotComponentArray[ii]) != null) {
                                    if (condition.internals.hasNotComponentAbortAllOnMatch) {
                                        return ShouldPlayCheckResult.False;
                                    } else {
                                        shouldNotPlayTriggered = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (shouldNotPlayTriggered) {
                        return ShouldPlayCheckResult.Continue;
                    } else {
                        // If any IsNot are enabled and none are triggered and no Is are enabled then play
                        if (isCollision && terrainDataFound) {
                            // Check With Terrain
                            if (condition.internals.isNotTagUse || condition.internals.isNotLayerUse || condition.internals.isNotTerrainIndexUse || condition.internals.isNotTerrainNameUse || condition.internals.hasNotComponentUse) {
                                if (!condition.internals.isTagUse && !condition.internals.isLayerUse && !condition.internals.isTerrainIndexUse && !condition.internals.isTerrainNameUse && !condition.internals.hasComponentUse) {
                                    SetSoundTag(part, condition);
                                    shouldPlayTriggered = true;
                                }
                            }
                        } else {
                            // Checks without Terrain
                            if (condition.internals.isNotTagUse || condition.internals.isNotLayerUse || condition.internals.hasNotComponentUse) {
                                if (!condition.internals.isTagUse && !condition.internals.isLayerUse && !condition.internals.hasComponentUse) {
                                    SetSoundTag(part, condition);
                                    shouldPlayTriggered = true;
                                }
                            }
                        }
                    }
                    
                    // Is Tag
                    bool isTagMatched = false;
                    if (condition.internals.isTagUse) {
                        for (int ii = 0; ii < condition.internals.isTagArray.Length; ii++) {
                            if (gameObject.CompareTag(condition.internals.isTagArray[ii])) {
                                isTagMatched = true;
                                shouldPlayTriggered = true;
                                SetSoundTag(part, condition);
                                break;
                            }
                        }
                        if (condition.internals.isTagAbortAllOnNoMatch && !isTagMatched) {
                            return ShouldPlayCheckResult.False;
                        }
                    }
                    // Is Layer
                    bool isLayerMatched = false;
                    if (condition.internals.isLayerUse) {
                        for (int ii = 0; ii < condition.internals.isLayerArray.Length; ii++) {
                            if (condition.internals.isLayerArray[ii] == gameObject.layer) {
                                isLayerMatched = true;
                                shouldPlayTriggered = true;
                                SetSoundTag(part, condition);
                                break;
                            }
                        }
                        if (condition.internals.isLayerAbortAllOnNoMatch && !isLayerMatched) {
                            return ShouldPlayCheckResult.False;
                        }
                    }
                    // Is Terrain Index
                    bool isTerrainIndexMatched = false;
                    if (condition.internals.isTerrainIndexUse && terrainDataFound) {
                        for (int ii = 0; ii < condition.internals.isTerrainIndexArray.Length; ii++) {
                            if (condition.internals.isTerrainIndexArray[ii] == terrainIndex) {
                                isTerrainIndexMatched = true;
                                shouldPlayTriggered = true;
                                SetSoundTag(part, condition);
                                break;
                            }
                        }
                        if (condition.internals.isTerrainIndexAbortAllOnNoMatch && !isTerrainIndexMatched) {
                            return ShouldPlayCheckResult.False;
                        }
                    }
                    // Is Terrain Name
                    bool isTerrainNameMatched = false;
                    if (condition.internals.isTerrainNameUse && terrainDataFound) {
                        for (int ii = 0; ii < condition.internals.isTerrainNameArray.Length; ii++) {
                            if (StringOperation.ContainsCaseInsensitive(tempTerrainLayerName, condition.internals.isTerrainNameArray[ii])) {
                                isTerrainNameMatched = true;
                                shouldPlayTriggered = true;
                                SetSoundTag(part, condition);
                                break;
                            }
                        }
                        if (condition.internals.isTerrainNameAbortAllOnNoMatch && !isTerrainNameMatched) {
                            return ShouldPlayCheckResult.False;
                        }
                    }
                    // Has Component
                    bool hasComponentMatched = false;
                    if (condition.internals.hasComponentUse) {
                        for (int ii = 0; ii < condition.internals.hasComponentArray.Length; ii++) {
                            if (gameObject.GetComponent(condition.internals.hasComponentArray[ii]) != null) {
                                hasComponentMatched = true;
                                shouldPlayTriggered = true;
                                SetSoundTag(part, condition);
                                break;
                            }
                        }
                        if (condition.internals.hasComponentAbortAllOnNoMatch && !hasComponentMatched) {
                            return ShouldPlayCheckResult.False;
                        }
                    }
                }
            }

            // Checking Sub SoundPhysicsConditions
            for (int i = 0; i < condition.internals.childConditions.Length; i++) {
                SoundPhysicsConditionBase conditionSub = condition.internals.childConditions[i];
                shouldPlayCheckResult = ShouldPlayConditionCheckSub(conditionSub, part, gameObject, isCollision, true, condition);
            }
            return shouldPlayCheckResult;
        }

        private Terrain tempTerrainComponent;
        private string tempTerrainLayerName = "";
        private float[,,] alphamapData;
        private Vector2Int terrainCoordinate;

        private int GetDominantTextureIndex(Vector3 worldPosition, Terrain terrain) {
            TerrainData terrainData = terrain.terrainData;
            Vector3 relativePosition = worldPosition - terrain.transform.position;

            // Converts the woldposition to terrain coordinates
            // Terrains use X, Z
            terrainCoordinate = new Vector2Int((int)(relativePosition.x / terrainData.size.x * terrainData.alphamapWidth), (int)(relativePosition.z / terrainData.size.z * terrainData.alphamapHeight));

            // Samples one position of the alphamaps
            alphamapData = terrainData.GetAlphamaps(terrainCoordinate.x, terrainCoordinate.y, 1, 1);

            int dominantTextureIndex = -1;
            float greatestTextureWeight = float.MinValue;

            for (int i = 0; i < alphamapData.GetLength(2); i++) {
                float textureWeight = alphamapData[0, 0, i];
                if (greatestTextureWeight < textureWeight) {
                    greatestTextureWeight = textureWeight;
                    dominantTextureIndex = i;
                }
            }

            // Getting terrain layer name
            if (dominantTextureIndex < terrainData.terrainLayers.Length) {
                tempTerrainLayerName = terrainData.terrainLayers[dominantTextureIndex].name;
            }

            return dominantTextureIndex;
        }

        private int highestVelocityContactIndex = 0;
        private float highestVelocityContactValue = 0f;

        private int GetHighestImpulseContactIndex(Collision collision) {
            // Unity 2018.4.36 doesnt have "GetContact().impulse"
#if UNITY_2022_1_OR_NEWER
            highestVelocityContactIndex = 0;
            highestVelocityContactValue = 0;
            // Reducing the number of sounds triggered
            for (int n = 0; n < collision.contactCount; n++) {
                float velocity = collision.GetContact(n).impulse.magnitude;
                if (highestVelocityContactValue < velocity) {
                    highestVelocityContactValue = velocity;
                    highestVelocityContactIndex = n;
                }
            }
            return highestVelocityContactIndex;
#else
            // Just return the first index instead
            return 0;
#endif
        }

        private int GetHighestImpulseContactIndex(Collision2D collision) {
            highestVelocityContactIndex = 0;
            highestVelocityContactValue = 0;
            // Reducing the number of sounds triggered
            for (int n = 0; n < collision.contactCount; n++) {
                // Not sure if best practice is normalImpulse or tangentImpulse
                float velocity = collision.GetContact(n).normalImpulse;
                if (highestVelocityContactValue < velocity) {
                    highestVelocityContactValue = velocity;
                    highestVelocityContactIndex = n;
                }
            }
            return highestVelocityContactIndex;
        }

        // Collision
        public void OnCollisionEnter(Collision collision) {
            if (impactPlay && physicsDimension == PhysicsDimension._3D) {
                for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = impactSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                        if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                            // Play Impact
                            impactSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                            for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                    part.soundEvents[ii],
                                    cachedTransform,
                                    collision.GetContact(GetHighestImpulseContactIndex(collision)).point,
                                    tempSoundTag,
                                    impactSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionEnter2D(Collision2D collision) {
            if (impactPlay && physicsDimension == PhysicsDimension._2D) {
                for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = impactSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                        if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                            // Play Impact
                            impactSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                            for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                    part.soundEvents[ii],
                                    cachedTransform,
                                    collision.GetContact(GetHighestImpulseContactIndex(collision)).point,
                                    tempSoundTag,
                                    impactSoundParameter
                                    );
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionStay(Collision collision) {
            if (frictionPlay && physicsDimension == PhysicsDimension._3D) {
                for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                        if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                            // Play friction
                            if (!part.frictionIsPlaying) {
                                part.frictionIsPlaying = true;
#if UNITY_6000_0_OR_NEWER
                                frictionSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                                frictionSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.Play(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        tempSoundTag,
                                        frictionSoundParameter
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionStay2D(Collision2D collision) {
            if (frictionPlay && physicsDimension == PhysicsDimension._2D) {
                for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                        if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                            // Play friction
                            if (!part.frictionIsPlaying) {
                                part.frictionIsPlaying = true;
#if UNITY_6000_0_OR_NEWER
                                frictionSoundParameter.Intensity = cachedRigidbody2D.linearVelocity.magnitude;
#else
                                frictionSoundParameter.Intensity = cachedRigidbody2D.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.Play(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        tempSoundTag,
                                        frictionSoundParameter
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionExit(Collision collision) {
            if (physicsDimension == PhysicsDimension._3D) {
                if (frictionPlay) {
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                            if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                                // Stop Friction
                                if (part.frictionIsPlaying) {
                                    part.frictionIsPlaying = false;
                                    for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                        SoundManagerBase.Instance.Internals.Stop(part.soundEvents[ii], cachedTransform);
                                    }
                                }
                            }
                        }
                    }
                }
                if (exitPlay) {
                    for (int i = 0; i < exitSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = exitSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                            if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                                // Play Exit
                                exitSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.PlayAtPosition(
                                            part.soundEvents[ii],
                                            cachedTransform,
                                            cachedTransform.position, // Exit collision has no contacts
                                            tempSoundTag,
                                            exitSoundParameter
                                            );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnCollisionExit2D(Collision2D collision) {
            if (physicsDimension == PhysicsDimension._2D) {
                if (frictionPlay) {
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                            if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                                // Stop Friction
                                if (part.frictionIsPlaying) {
                                    part.frictionIsPlaying = false;
                                    for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                        SoundManagerBase.Instance.Internals.Stop(part.soundEvents[ii], cachedTransform);
                                    }
                                }
                            }
                        }
                    }
                }
                if (exitPlay) {
                    for (int i = 0; i < exitSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = exitSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnTrigger) {
                            if (ShouldPlayConditionMain(part, collision.gameObject, true)) {
                                // Play Exit
                                exitSoundParameter.Intensity = collision.relativeVelocity.magnitude;
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.PlayAtPosition(
                                            part.soundEvents[ii],
                                            cachedTransform,
                                            cachedTransform.position, // Exit collision has no contacts
                                            tempSoundTag,
                                            exitSoundParameter
                                            );
                                }
                            }
                        }
                    }
                }
            }
        }

        // Trigger
        public void OnTriggerEnter(Collider collider) {
            if (impactPlay && physicsDimension == PhysicsDimension._3D) {
                for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = impactSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                        if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
#if UNITY_6000_0_OR_NEWER
                            impactSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                            impactSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                            // Play Impact
                            for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        cachedTransform.transform.position,
                                        tempSoundTag,
                                        impactSoundParameter
                                        );
                            }
                        }
                    }
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collider) {
            if (impactPlay && physicsDimension == PhysicsDimension._2D) {
                for (int i = 0; i < impactSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = impactSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                        if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
#if UNITY_6000_0_OR_NEWER
                            impactSoundParameter.Intensity = cachedRigidbody2D.linearVelocity.magnitude;
#else
                            impactSoundParameter.Intensity = cachedRigidbody2D.velocity.magnitude;
#endif
                            // Play Impact
                            for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                SoundManagerBase.Instance.Internals.PlayAtPosition(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        cachedTransform.transform.position,
                                        tempSoundTag,
                                        impactSoundParameter
                                        );
                            }
                        }
                    }
                }
            }
        }

        public void OnTriggerStay(Collider collider) {
            if (frictionPlay && physicsDimension == PhysicsDimension._3D) {
                for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                        if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                            // Play friction
                            if (!part.frictionIsPlaying) {
                                part.frictionIsPlaying = true;
#if UNITY_6000_0_OR_NEWER
                                frictionSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                                frictionSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.Play(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        tempSoundTag,
                                        frictionSoundParameter
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnTriggerStay2D(Collider2D collider) {
            if (frictionPlay && physicsDimension == PhysicsDimension._2D) {
                for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                    SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                    if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                        if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                            // Play friction
                            if (!part.frictionIsPlaying) {
                                part.frictionIsPlaying = true;
#if UNITY_6000_0_OR_NEWER
                                frictionSoundParameter.Intensity = cachedRigidbody2D.linearVelocity.magnitude;
#else
                                frictionSoundParameter.Intensity = cachedRigidbody2D.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.Play(
                                        part.soundEvents[ii],
                                        cachedTransform,
                                        tempSoundTag,
                                        frictionSoundParameter
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnTriggerExit(Collider collider) {
            if (physicsDimension == PhysicsDimension._3D) {
                if (frictionPlay) {
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                            if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                                // Stop Friction
                                if (part.frictionIsPlaying) {
                                    part.frictionIsPlaying = false;
                                    for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                        SoundManagerBase.Instance.Internals.Stop(part.soundEvents[ii], cachedTransform);
                                    }
                                }
                            }
                        }
                    }
                }
                if (exitPlay) {
                    for (int i = 0; i < exitSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = exitSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                            if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                                // Play Exit
#if UNITY_6000_0_OR_NEWER
                                exitSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                                exitSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.PlayAtPosition(
                                            part.soundEvents[ii],
                                            cachedTransform,
                                            cachedTransform.position, // Exit collision has no contacts
                                            tempSoundTag,
                                            exitSoundParameter
                                            );
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collider) {
            if (physicsDimension == PhysicsDimension._2D) {
                if (frictionPlay) {
                    for (int i = 0; i < frictionSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = frictionSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                            if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                                // Stop Friction
                                if (part.frictionIsPlaying) {
                                    part.frictionIsPlaying = false;
                                    for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                        SoundManagerBase.Instance.Internals.Stop(part.soundEvents[ii], cachedTransform);
                                    }
                                }
                            }
                        }
                    }
                }
                if (exitPlay) {
                    for (int i = 0; i < exitSoundPhysicsParts.Length; i++) {
                        SoundPhysicsPart part = exitSoundPhysicsParts[i];
                        if (part.physicsPlayOn != PhysicsPlayOn.OnCollision) {
                            if (ShouldPlayConditionMain(part, collider.gameObject, false)) {
                                // Play Exit
#if UNITY_6000_0_OR_NEWER
                                exitSoundParameter.Intensity = cachedRigidbody.linearVelocity.magnitude;
#else
                                exitSoundParameter.Intensity = cachedRigidbody.velocity.magnitude;
#endif
                                for (int ii = 0; ii < part.soundEvents.Length; ii++) {
                                    SoundManagerBase.Instance.Internals.PlayAtPosition(
                                            part.soundEvents[ii],
                                            cachedTransform,
                                            cachedTransform.position, // Exit collision has no contacts
                                            tempSoundTag,
                                            exitSoundParameter
                                            );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}