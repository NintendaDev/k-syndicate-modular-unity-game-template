// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    [Serializable]
    public class SoundTriggerInternals {

        public bool soundEventsExpand = true;

        // Start with one length so it is properly initialized to default values
        public SoundTriggerPart[] soundTriggerPart = new SoundTriggerPart[1];

        /// <summary>
        /// Used to pass the distance scale of the radius handle
        /// </summary>
        public SoundParameterInternals soundParameterDistanceScale = new SoundParameterDistanceScale(1f);

        [NonSerialized]
        public bool initialized = false;
        [NonSerialized]
        public Transform cachedTransform;
        [NonSerialized]
        public Rigidbody cachedRigidbody;
        [NonSerialized]
        public Rigidbody2D cachedRigidbody2D;

        public void Initialize(GameObject gameObject) {
            if (!initialized) {
                initialized = true;
                cachedTransform = gameObject.GetComponent<Transform>();
                cachedRigidbody = gameObject.GetComponent<Rigidbody>();
                cachedRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            }
        }

        private SoundParameterIntensity[] soundParameterIntensityOnce = new SoundParameterIntensity[] {
            new SoundParameterIntensity(1f, UpdateMode.Once)
        };

        public void DoTriggerAction(
            SoundTriggerOnType onType, int i, SoundTriggerAction action, bool tagMatch = false, string otherTag = null,
            Collision collision = null, Collision2D collision2D = null
            ) {
            if (soundTriggerPart[i].soundEvent != null) {

                // Trigger Tag
                if (onType == SoundTriggerOnType.OnTrigger && tagMatch && soundTriggerPart[i].triggerTagUse
                    && !TagMatches(soundTriggerPart[i].triggerTags, otherTag)) {
                    return;
                }

                // Collision Tag
                if (onType == SoundTriggerOnType.OnCollision && tagMatch && soundTriggerPart[i].collisionTagUse
                    && !TagMatches(soundTriggerPart[i].collisionTags, otherTag)) {
                    return;
                }

                if (action == SoundTriggerAction.Play) {
                    // Play
                    if (collision != null) {
                        for (int ii = 0; ii < collision.contacts.Length; ii++) {
                            if (soundTriggerPart[i].collisionVelocityToIntensity) {
                                // Collision Play At Contacts Point With Velocity
                                soundParameterIntensityOnce[0].Intensity = collision.relativeVelocity.magnitude;
                                soundTriggerPart[i].PlayAtPosition(cachedTransform, collision.contacts[ii].point, soundParameterIntensityOnce, soundParameterDistanceScale, null);
                            } else {
                                // Collision Play At Contacts Point
                                soundTriggerPart[i].PlayAtPosition(cachedTransform, collision.contacts[ii].point, null, soundParameterDistanceScale, null);
                            }
                        }
                    } else if (collision2D != null) {
                        for (int ii = 0; ii < collision2D.contacts.Length; ii++) {
                            if (soundTriggerPart[i].collisionVelocityToIntensity) {
                                // Collision2D Play At Contacts Point With Velocity
                                soundParameterIntensityOnce[0].Intensity = collision2D.relativeVelocity.magnitude;
                                soundTriggerPart[i].PlayAtPosition(cachedTransform, collision2D.contacts[ii].point, soundParameterIntensityOnce, soundParameterDistanceScale, null);
                            } else {
                                // Collision2D Play At Contacts Point
                                soundTriggerPart[i].PlayAtPosition(cachedTransform, collision2D.contacts[ii].point, null, soundParameterDistanceScale, null);
                            }
                        }
                    } else {
                        // Play Normal
                        soundTriggerPart[i].Play(cachedTransform, null, soundParameterDistanceScale, null);
                    }
                } else if (action == SoundTriggerAction.Stop) {
                    // Stop
                    soundTriggerPart[i].Stop(cachedTransform, true);
                } else if (action == SoundTriggerAction.StopImmediate) {
                    // Stop Immediate
                    soundTriggerPart[i].Stop(cachedTransform, false);
                } else if (action == SoundTriggerAction.Pause) {
                    // Pause
                    soundTriggerPart[i].Pause(cachedTransform, false);
                } else if (action == SoundTriggerAction.PauseForced) {
                    // Pause Forced
                    soundTriggerPart[i].Pause(cachedTransform, true);
                } else if (action == SoundTriggerAction.Unpause) {
                    // Unpause
                    soundTriggerPart[i].Unpause(cachedTransform);
                }
            }
        }

        // Checks the Tag
        private bool TagMatches(string[] tags, string tag) {
            for (int i = 0; i < tags.Length; i++) {
                if (tag == tags[i]) {
                    return true;
                }
            }
            return false;
        }
    }
}