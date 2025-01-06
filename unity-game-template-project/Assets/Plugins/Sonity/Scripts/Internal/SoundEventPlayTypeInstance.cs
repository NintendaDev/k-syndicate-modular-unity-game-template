// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public class SoundEventPlayTypeInstance {

        public SoundEventPlayType playType;
        public Transform instanceIDTransform;
        private Vector3 positionVector;
        public Transform positionTransform;

        private Vector3 cachedPosition;
        private float cachedUnscaledDistance = 0f;
        private float cachedScaledDistance = 0f;
        private float cachedSpeedOfSoundDistance = 0f;
        private bool distanceEnabled = false;

        private float cachedAngle = 0f;
        private bool angleEnabled = false;

        private float cachedScaledIntensityCurrent = 1f;
        private float cachedScaledIntensityTarget = 1f;
        private float intensityUpdateTimeLast;

        private SoundContainerBase soundContainer;
        private SoundEventBase soundEvent;

        public void SetIntensity(float unscaledIntensity, bool isPrepare) {
            cachedScaledIntensityTarget = soundEvent.internals.data.GetScaledIntensity(unscaledIntensity);
            if (isPrepare) {
                cachedScaledIntensityCurrent = cachedScaledIntensityTarget;
            } else if (soundEvent.internals.data.intensitySeekTime > 0f) {
                // Smoothing
                if (cachedScaledIntensityCurrent > cachedScaledIntensityTarget) {
                    cachedScaledIntensityCurrent = Mathf.Clamp(cachedScaledIntensityCurrent - (SoundTimeScale.GetTimeRuntime() - intensityUpdateTimeLast) / soundEvent.internals.data.intensitySeekTime, cachedScaledIntensityTarget, 1f);
                } else if (cachedScaledIntensityCurrent < cachedScaledIntensityTarget) {
                    cachedScaledIntensityCurrent = Mathf.Clamp(cachedScaledIntensityCurrent + (SoundTimeScale.GetTimeRuntime() - intensityUpdateTimeLast) / soundEvent.internals.data.intensitySeekTime, 0f, cachedScaledIntensityTarget);
                }
            } else {
                // No smooting
                cachedScaledIntensityCurrent = cachedScaledIntensityTarget;
            }
            intensityUpdateTimeLast = SoundTimeScale.GetTimeRuntime();
        }

        public float GetScaledIntensity() {
            return cachedScaledIntensityCurrent;
        }

        public void ResetScaledIntensity() {
            cachedScaledIntensityCurrent = 1f;
            cachedScaledIntensityTarget = 1f;
        }

        public float GetAngle() {
            return cachedAngle;
        }

        private void SetAngle(Vector3 position) {
            if (angleEnabled) {
                cachedAngle = SoundManagerBase.Instance.Internals.GetAngleToAudioListener(position);
            } else {
                cachedAngle = 0f;
            }
        }

        public Vector3 GetPosition() {
            return cachedPosition;
        }

        public void CopyTo(SoundEventPlayTypeInstance copyFrom) {
            playType = copyFrom.playType;
            instanceIDTransform = copyFrom.instanceIDTransform;
            positionVector = copyFrom.positionVector;
            positionTransform = copyFrom.positionTransform;
            angleEnabled = copyFrom.angleEnabled;
            distanceEnabled = copyFrom.distanceEnabled;
            cachedPosition = copyFrom.cachedPosition;
            soundContainer = copyFrom.soundContainer;
            soundEvent = copyFrom.soundEvent;
        }

        public void SetValues(SoundEventPlayType playType, Transform instanceIDTransform, Vector3? positionVector, Transform positionTransform, SoundContainerBase soundContainer, SoundEventBase soundEvent) {
            this.playType = playType;
            this.instanceIDTransform = instanceIDTransform;
            if (positionVector == null) {
                this.positionVector = new Vector3();
            } else {
                this.positionVector = positionVector.Value;
            }
            this.positionTransform = positionTransform;

            angleEnabled = soundContainer.internals.data.stereoPanAngleUse;
            distanceEnabled = soundContainer.internals.data.distanceEnabled;

            this.soundContainer = soundContainer;
            this.soundEvent = soundEvent;
        }

        private void SetDistance(Vector3 position, float maxRange) {
            // Avoid divide by zero
            if (distanceEnabled && maxRange > 0f) {
                cachedUnscaledDistance = SoundManagerBase.Instance.Internals.GetDistanceToAudioListener(position);
                cachedScaledDistance = cachedUnscaledDistance / maxRange;
            } else {
                cachedUnscaledDistance = 0f;
                cachedScaledDistance = 0f;
            }
        }

        private bool ShouldFollowPosition(VoiceParameterInstance voiceParameter) {
            if (voiceParameter == null) {
                return soundContainer.internals.data.followPosition;
            } else {
                return (!voiceParameter.currentModifier.followPositionUse && soundContainer.internals.data.followPosition)
                        || (voiceParameter.currentModifier.followPositionUse && voiceParameter.currentModifier.followPosition);
            }
        }

        public void SetCachedDistancesAndAngle(float maxRange, VoiceParameterInstance voiceParameter, bool forceUpdatePosition) {
            if (forceUpdatePosition || ShouldFollowPosition(voiceParameter)) {
                if (playType == SoundEventPlayType.Play) {
                    if (instanceIDTransform != null) {
                        cachedPosition = instanceIDTransform.position;
                    }
                } else if (playType == SoundEventPlayType.PlayAtVector) {
                    cachedPosition = positionVector;
                } else if (playType == SoundEventPlayType.PlayAtTransform) {
                    if (positionTransform != null) {
                        cachedPosition = positionTransform.position;
                    }
                }
            }
            SetDistance(cachedPosition, maxRange);
            SetAngle(cachedPosition);
            cachedSpeedOfSoundDistance = SoundManagerBase.Instance.Internals.settings.GetSpeedOfSoundDelay(cachedUnscaledDistance);
        }

        public float GetScaledDistance() {
            return cachedScaledDistance;
        }

        public float GetSpeedOfSoundDistance() {
            return cachedSpeedOfSoundDistance;
        }
    }
}