// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Sonity.Internal {

    public static class EditorDebugDrawInGameView {

        public static void UpdateDraw() {
            if (SoundManagerBase.Instance != null && SoundManagerBase.Instance.Internals.debug.drawSoundEventsInGameViewEnabled) {
                if (SoundManagerBase.Instance.Internals.voicePool.voicePool == null) {
                    return;
                }
                for (int i = 0; i < SoundManagerBase.Instance.Internals.voicePool.voicePool.Length; i++) {
                    Voice voice = SoundManagerBase.Instance.Internals.voicePool.voicePool[i];
                    if (voice != null
                        && voice.isAssigned
                        && voice.soundEvent != null
                        && voice.cachedGameObject != null
                        && voice.GetState() == VoiceState.Play
                        && voice.soundEvent.internals.data.debugDrawShow == DebugSoundEventDrawShow.Show
                        ) {
                        Draw(
                            voice.soundEventName,
                            voice.cachedGameObject.transform.position,
                            GetColorStart(voice.soundEvent),
                            GetColorEnd(voice.soundEvent),
                            GetColorOutline(voice.soundEvent),
                            voice.GetTimePlayed(),
                            SoundManagerBase.Instance.Internals.debug.drawSoundEventsVolumeToOpacity,
                            SoundManagerBase.Instance.Internals.debug.drawSoundEventsLifetimeToOpacity,
                            SoundManagerBase.Instance.Internals.debug.drawSoundEventsLifetimeFadeLength,
                            voice.GetVolumeRatioWithFade(),
                            GetFontSize(voice.soundEvent),
                            GetOpacityMultiplier(voice.soundEvent)
                        );
                    }
                }
            }
        }

        private static int GetFontSize(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugDrawSoundEventStyleOverride) {
                return Mathf.RoundToInt(SoundManagerBase.Instance.Internals.debug.drawSoundEventsFontSize * soundEvent.internals.data.debugDrawSoundEventFontSizeMultiplier);
            } else {
                return SoundManagerBase.Instance.Internals.debug.drawSoundEventsFontSize;
            }
        }

        private static float GetOpacityMultiplier(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugDrawSoundEventStyleOverride) {
                return soundEvent.internals.data.debugDrawSoundEventOpacityMultiplier;
            } else {
                return 1f;
            }
        }

        private static Color GetColorStart(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugDrawSoundEventStyleOverride) {
                return soundEvent.internals.data.debugDrawSoundEventColorStart;
            } else {
                return SoundManagerBase.Instance.Internals.debug.drawSoundEventsColorStart;
            }
        }

        private static Color GetColorEnd(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugDrawSoundEventStyleOverride) {
                return soundEvent.internals.data.debugDrawSoundEventColorEnd;
            } else {
                return SoundManagerBase.Instance.Internals.debug.drawSoundEventsColorEnd;
            }
        }

        private static Color GetColorOutline(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugDrawSoundEventStyleOverride) {
                return soundEvent.internals.data.debugDrawSoundEventColorOutline;
            } else {
                return SoundManagerBase.Instance.Internals.debug.drawSoundEventsColorOutline;
            }
        }

        private static GUIStyle guiStyle = new GUIStyle();
        private static GUIContent guiContent = new GUIContent();
        private static Vector2 guiContentSize;
        private static Vector3 screenPos;
        private static Vector2 textSize;
        private static bool hasWarnedNullCamera = false;

        // Older version doesn't have the SubsystemRegistration load type
#if UNITY_2019_2_OR_NEWER
        // Needed for disabling domain reloading
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RuntimeInitializeOnLoad() {
            hasWarnedNullCamera = false;
        }
#endif

        private static void Draw(
            string text, Vector3 worldPos, Color lifetimeStartColor, Color lifetimeFadeColor, Color outlineColor, 
            float timePlayed, float volumeToAlpha, float lifetimeToAlpha, float lifetimeFadeLength, 
            float voiceVolume, int fontSize, float opacityMultiplier, bool outline = true
            ) {

            if (Camera.main == null) {
                if (!hasWarnedNullCamera && ShouldDebug.Warnings()) {
                    hasWarnedNullCamera = true;
                    Debug.LogWarning("Sonity.SoundManager \"Draw SoundEvents in Game View\": Cant render because Camera.main is null");
                }
                return;
            }

            if (Vector3.Distance(Camera.main.transform.position, worldPos) < SoundManagerBase.Instance.Internals.debug.drawSoundEventsHideIfCloserThan){
                return;
            }

            if (lifetimeFadeLength == 0f) {
                lifetimeFadeLength = 0.00001f;
            }
            float timeAlpha = 1f - Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f);
            float textAlpha = (timeAlpha -1f) * lifetimeToAlpha + 1f;
            textAlpha *= (LogLinExp.Get(voiceVolume, -2f) - 1f) * volumeToAlpha + 1f;
            textAlpha *= opacityMultiplier;

            guiStyle.fontSize = Mathf.RoundToInt(fontSize * 1.8f);
            guiStyle.alignment = TextAnchor.MiddleCenter;

            guiContent.text = text;
            guiContentSize = guiStyle.CalcSize(guiContent);

            screenPos = Camera.main.WorldToScreenPoint(worldPos);
            textSize = GUI.skin.label.CalcSize(guiContent);

            // If outside of screen
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x + guiContentSize.x < 0 || screenPos.x - guiContentSize.x > Screen.width || screenPos.z < 0) {
                return;
            }

            if (outline) {
                // Alpha divided by 4
                outlineColor.a *= textAlpha * 0.25f;
                guiStyle.normal.textColor = outlineColor;
                float shadowOffset = 1f;
                // Below
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f + shadowOffset, textSize.x, textSize.y), guiContent, guiStyle);
                // Above
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f - shadowOffset, textSize.x, textSize.y), guiContent, guiStyle);
                // Left
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f + shadowOffset, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
                // Right
                GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f - shadowOffset, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
            }

            // Colored text
            lifetimeStartColor = Color.Lerp(lifetimeStartColor, lifetimeFadeColor, Mathf.Clamp(timePlayed / lifetimeFadeLength, 0f, 1f));
            lifetimeStartColor.a *= textAlpha;
            guiStyle.normal.textColor = lifetimeStartColor;
            GUI.Label(new Rect(screenPos.x - textSize.x * 0.5f, Screen.height - screenPos.y - textSize.y * 0.5f, textSize.x, textSize.y), guiContent, guiStyle);
        }
    }
}
#endif