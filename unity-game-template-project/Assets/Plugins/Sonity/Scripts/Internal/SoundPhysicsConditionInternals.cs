// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPhysicsConditionInternals {

#if UNITY_EDITOR
        public string notes = "Notes";
#endif
        public SoundPhysicsConditionBase[] childConditions = new SoundPhysicsConditionBase[0];

        public SoundTagBase soundTag;
        public PhysicsPlayOn physicsPlayOn = PhysicsPlayOn.OnCollisionAndTrigger;
        public bool playDisregardingConditions = false;

        public bool tagExpand = true;
        public bool layerExpand = true;
        public bool terrainIndexExpand = true;
        public bool terrainNameExpand = true;
        public bool componentExpand = true;

        public bool isTagAbortAllOnNoMatch = false;
        public bool isLayerAbortAllOnNoMatch = false;
        public bool isTerrainIndexAbortAllOnNoMatch = false;
        public bool isTerrainNameAbortAllOnNoMatch = false;
        public bool hasComponentAbortAllOnNoMatch = false;

        public bool isNotTagAbortAllOnMatch = false;
        public bool isNotLayerAbortAllOnMatch = false;
        public bool isNotTerrainIndexAbortAllOnMatch = false;
        public bool isNotTerrainNameAbortAllOnMatch = false;
        public bool hasNotComponentAbortAllOnMatch = false;

        public bool isTagUse = false;
        public bool isNotTagUse = false;
        public bool isLayerUse = false;
        public bool isNotLayerUse = false;
        public bool isTerrainIndexUse = false;
        public bool isNotTerrainIndexUse = false;
        public bool isTerrainNameUse = false;
        public bool isNotTerrainNameUse = false;
        public bool hasComponentUse = false;
        public bool hasNotComponentUse = false;

        public string[] isTagArray = new string[] { "Untagged" };
        public string[] isNotTagArray = new string[] { "Untagged" };
        public int[] isLayerArray = new int[1];
        public int[] isNotLayerArray = new int[1];
        public int[] isTerrainIndexArray = new int[1];
        public int[] isNotTerrainIndexArray = new int[1];
        public string[] isTerrainNameArray = new string[] { "Grass" };
        public string[] isNotTerrainNameArray = new string[] { "Grass" };
        public string[] hasComponentArray = new string[] { "Terrain" };
        public string[] hasNotComponentArray = new string[] { "Terrain" };

        public bool CheckIsInfiniteLoop(SoundPhysicsConditionBase condition, bool isEditor) {
            if (isEditor) {
                bool isInfiniteLoop = GetIfInfiniteLoop(condition, out SoundPhysicsConditionBase infiniteInstigator, out SoundPhysicsConditionBase infinitePrevious);
                if (isInfiniteLoop) {
                    if (ShouldDebug.Warnings()) {
                        Debug.LogWarning($"Sonity.{nameof(NameOf.SoundPhysicsCondition)}: "
                            + "\"" + infiniteInstigator.name + "\" in \"" + infinitePrevious.name + "\" creates an infinite loop", infiniteInstigator);
                    }
                }
                return isInfiniteLoop;
            } else {
                if (SoundManagerBase.Instance == null) {
                    Debug.LogWarning($"Sonity.{nameof(NameOf.SoundManager)} is null. Add one to the scene.");
                    return true;
                } else {
                    return SoundManagerBase.Instance.Internals.InternalCheckSoundPhysicsConditionIsInfiniteLoop(condition);
                }
            }
        }

        private bool isInfiniteLoop;
        private SoundPhysicsConditionBase tempInfiniteInstigator;
        private SoundPhysicsConditionBase tempInfinitePrevious;

        // Checks if any object in the hierarchy contains itself
        public bool GetIfInfiniteLoop(SoundPhysicsConditionBase condition, out SoundPhysicsConditionBase infiniteInstigator, out SoundPhysicsConditionBase infinitePrevious) {
            isInfiniteLoop = false;
            infiniteInstigator = null;
            infinitePrevious = null;
            if (condition != null) {
                GetIfInfiniteLoopSub(new IsInfiniteLoopClass(condition, null));
                if (isInfiniteLoop) {
                    infiniteInstigator = tempInfiniteInstigator;
                    infinitePrevious = tempInfinitePrevious;
                    return true;
                }
            }
            return false;
        }

        private void GetIfInfiniteLoopSub(IsInfiniteLoopClass currentObject) {
            for (int i = 0; i < currentObject.self.internals.childConditions.Length; i++) {
                SoundPhysicsConditionBase child = currentObject.self.internals.childConditions[i];
                if (child != null) {
                    if (!currentObject.allParentsList.Contains(child)) {
                        GetIfInfiniteLoopSub(new IsInfiniteLoopClass(child, currentObject.allParentsList));
                    } else {
                        tempInfiniteInstigator = child;
                        tempInfinitePrevious = currentObject.self;
                        isInfiniteLoop = true;
                        return;
                    }
                }
            }
            return;
        }

        private class IsInfiniteLoopClass {
            public SoundPhysicsConditionBase self;
            public List<SoundPhysicsConditionBase> allParentsList = new List<SoundPhysicsConditionBase>();
            public IsInfiniteLoopClass(SoundPhysicsConditionBase soundPhysicsConditionBase, List<SoundPhysicsConditionBase> parentsToAdd) {
                self = soundPhysicsConditionBase;
                allParentsList.Add(self);
                if (parentsToAdd != null) {
                    allParentsList.AddRange(parentsToAdd);
                }
            }
        }
    }
}