// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Sonity.Internal {

    public enum DebugSoundEventsToLogLogType {
        DebugLog,
        DebugWarning,
        DebugError,
    }

    public enum DebugSoundEventsToLogSelectObject {
        Owner,
        Position,
        SoundEvent,
    }

    public enum DebugSoundEventLogShow {
        Show,
        Hide,
        ForceShow,
    }

    public enum DebugSoundEventDrawShow {
        Show,
        Hide,
    }

    [Serializable]
    public class SoundManagerInternalsDebug {

        public bool debugExpand = true;

        // Log SoundEvents
        public bool logSoundEventsEnabled = false;
        public DebugSoundEventsToLogLogType logSoundEventsLogType = DebugSoundEventsToLogLogType.DebugLog;
        public DebugSoundEventsToLogSelectObject logSoundEventsSelectObject = DebugSoundEventsToLogSelectObject.Owner;
        public bool logSoundEventsPlay = true;
        public bool logSoundEventsStop = true;
        public bool logSoundEventsPool = false;
        public bool logSoundEventsPause = true;
        public bool logSoundEventsUnpause = true;
        public bool logSoundEventsGlobalPause = false;
        public bool logSoundEventsGlobalUnpause = false;
        public bool logSoundEventsSoundParametersOnce = true;
        public bool logSoundEventsSoundParametersContinious = false;

        // Draw SoundEvents
        public bool drawSoundEventsInSceneViewEnabled = false;
        public bool drawSoundEventsInGameViewEnabled = false;
        public float drawSoundEventsHideIfCloserThan = 0.01f;
        public int drawSoundEventsFontSize = 16;
        public float drawSoundEventsVolumeToOpacity = 0.5f;
        public float drawSoundEventsLifetimeToOpacity = 0.75f;
        public float drawSoundEventsLifetimeFadeLength = 1f;
        public Color drawSoundEventsColorStart = EditorColor.GetVolumeMax(1f);
        public Color drawSoundEventsColorEnd = EditorColor.GetVolumeMin(1f);
        public Color drawSoundEventsColorOutline = Color.black;

        public void DebugLogPrint(string toPrint, UnityEngine.Object unityEngineObject) {
            if (logSoundEventsLogType == DebugSoundEventsToLogLogType.DebugLog) {
                Debug.Log(toPrint, unityEngineObject);
            } else if (logSoundEventsLogType == DebugSoundEventsToLogLogType.DebugWarning) {
                Debug.LogWarning(toPrint, unityEngineObject);
            } else if (logSoundEventsLogType == DebugSoundEventsToLogLogType.DebugError) {
                Debug.LogError(toPrint, unityEngineObject);
            }
        }

        public bool LogSoundEventsPlayEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && logSoundEventsPlay;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsPlay;
            }
            return false;
        }

        public void LogSoundEventsPlay(SoundEventBase soundEvent, SoundEventPlayType soundEventPlayType, Transform transformOwner, Transform transformPosition, Vector3? vector3Position) {
            UnityEngine.Object selectedObject = null;
            if (logSoundEventsSelectObject == DebugSoundEventsToLogSelectObject.Owner) {
                selectedObject = transformOwner;
            } else if (logSoundEventsSelectObject == DebugSoundEventsToLogSelectObject.Position) {
                if (soundEventPlayType == SoundEventPlayType.PlayAtTransform) {
                    selectedObject = transformPosition;
                } else {
                    // For Play & PlayAtPosition
                    selectedObject = transformOwner;
                }
            } else if (logSoundEventsSelectObject == DebugSoundEventsToLogSelectObject.SoundEvent) {
                selectedObject = soundEvent;
            }
            if (soundEventPlayType == SoundEventPlayType.Play) {
                DebugLogPrint($"Sonity Log - <color=#62ff00>Play</color>: {soundEvent.name} at Owner \"{transformOwner.name}\"", selectedObject);
            } else if (soundEventPlayType == SoundEventPlayType.PlayAtVector) {
                DebugLogPrint($"Sonity Log - <color=#62ff00>Play</color>: {soundEvent.name} at Owner \"{transformOwner.name}\" and at Vector3 position {vector3Position.Value}", selectedObject);
            } else if (soundEventPlayType == SoundEventPlayType.PlayAtTransform) {
                DebugLogPrint($"Sonity Log - <color=#62ff00>Play</color>: {soundEvent.name} at Owner \"{transformOwner.name}\" and at Transform position \"{transformPosition.name}\"", selectedObject);
            }
        }

        public bool LogSoundEventsStopEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && logSoundEventsStop;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsStop;
            }
            return false;
        }

        public void LogSoundEventsStop(SoundEventBase soundEvent, bool allowFadeOut) {
            if (allowFadeOut) {
                DebugLogPrint($"Sonity Log - <color=red>Stop Allow Fadeout</color>: {soundEvent.name}", soundEvent);
            } else {
                DebugLogPrint($"Sonity Log - <color=red>Stop Immediate</color>: {soundEvent.name}", soundEvent);
            }
        }

        public bool LogSoundEventsPoolEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && logSoundEventsPool;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsPool;
            }
            return false;
        }

        public void LogSoundEventsPool(SoundEventBase soundEvent) {
            DebugLogPrint($"Sonity Log - <color=#bf00ff>Pool</color>: {soundEvent.name}", soundEvent);
        }

        public bool LogSoundEventsPauseUnpauseEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && (logSoundEventsPause || logSoundEventsUnpause);
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsPause || logSoundEventsUnpause;
            }
            return false;
        }
        
        public void LogSoundEventsPauseUnpause(SoundEventBase soundEvent, bool pause) {
            if (pause) {
                DebugLogPrint($"Sonity Log - <color=orange>Pause</color>: {soundEvent.name}", soundEvent);
            } else {
                DebugLogPrint($"Sonity Log - <color=orange>Unpause</color>: {soundEvent.name}", soundEvent);
            }
        }

        public bool LogSoundEventsSoundParametersOnceEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && logSoundEventsSoundParametersOnce;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsSoundParametersOnce;
            }
            return false;
        }

        public bool LogSoundEventsSoundParametersContiniousEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && logSoundEventsSoundParametersContinious;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsSoundParametersContinious;
            }
            return false;
        }

        public void LogSoundEventsSoundParametersText(SoundEventBase soundEvent, SoundParameterType soundParameterType, UpdateMode updateMode, string value) {
            if ((logSoundEventsSoundParametersOnce && updateMode == UpdateMode.Once) || (logSoundEventsSoundParametersContinious && updateMode == UpdateMode.Continuous)) {
                DebugLogPrint($"Sonity Log - <color=yellow>Set SoundParameter{soundParameterType} {updateMode}</color>: {soundEvent.name} to: {value}", soundEvent);
            }
        }

        public bool LogSoundEventsGlobalPauseUnpauseEnabled(SoundEventBase soundEvent) {
            if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Show) {
                return logSoundEventsEnabled && (logSoundEventsGlobalPause || logSoundEventsGlobalUnpause);
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.Hide) {
                return false;
            } else if (soundEvent.internals.data.debugLogShow == DebugSoundEventLogShow.ForceShow) {
                return logSoundEventsGlobalPause || logSoundEventsGlobalUnpause;
            }
            return false;
        }

        public void LogSoundEventsGlobalPauseUnpause(SoundEventBase soundEvent, bool pause) {
            if (pause) {
                DebugLogPrint($"Sonity Log - <color=orange>Global Pause</color>: {soundEvent.name}", soundEvent);
            } else {
                DebugLogPrint($"Sonity Log - <color=orange>Global Unpause</color>: {soundEvent.name}", soundEvent);
            }
        }

        public void LogSoundEventsSoundParameters(SoundEventBase soundEvent, SoundParameterInternals[] soundParameters) {
            if (soundParameters == null) {
                return;
            }
            for (int i = 0; i < soundParameters.Length; i++) {
                if (soundParameters[i] != null) {
                    UpdateMode updateMode = soundParameters[i].internals.updateMode;
                    SoundParameterType soundParameterType = soundParameters[i].internals.type;
                    // Volume
                    if (soundParameterType == SoundParameterType.Volume) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Pitch
                    else if (soundParameterType == SoundParameterType.Pitch) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Delay
                    else if (soundParameterType == SoundParameterType.Delay && updateMode == UpdateMode.Once) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Increase 2D
                    else if (soundParameterType == SoundParameterType.Increase2D) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Intensity
                    else if (soundParameterType == SoundParameterType.Intensity) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Reverb Zone Mix Ratio
                    else if (soundParameterType == SoundParameterType.ReverbZoneMix) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Start Position
                    else if (soundParameterType == SoundParameterType.StartPosition && updateMode == UpdateMode.Once) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Reverse Enabled
                    else if (soundParameterType == SoundParameterType.Reverse) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueBool.ToString());
                    }
                    // Stereo Pan
                    else if (soundParameterType == SoundParameterType.StereoPan) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Polyphony
                    else if (soundParameterType == SoundParameterType.Polyphony && updateMode == UpdateMode.Once) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueInt.ToString());
                    }
                    // Distance Scale
                    else if (soundParameterType == SoundParameterType.DistanceScale && updateMode == UpdateMode.Once) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Distortion Increase
                    else if (soundParameterType == SoundParameterType.DistortionIncrease) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Fade In Length
                    else if (soundParameterType == SoundParameterType.FadeInLength) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Fade In Shape
                    else if (soundParameterType == SoundParameterType.FadeInShape) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Fade Out Length
                    else if (soundParameterType == SoundParameterType.FadeOutLength) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Fade Out Shape
                    else if (soundParameterType == SoundParameterType.FadeOutShape) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueFloat.ToString());
                    }
                    // Follow Position
                    else if (soundParameterType == SoundParameterType.FollowPosition) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueBool.ToString());
                    }
                    // Bypass Reverb Zones
                    else if (soundParameterType == SoundParameterType.BypassReverbZones) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueBool.ToString());
                    }
                    // Bypass Voice Effects
                    else if (soundParameterType == SoundParameterType.BypassVoiceEffects) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueBool.ToString());
                    }
                    // Bypass Listener Effects
                    else if (soundParameterType == SoundParameterType.BypassListenerEffects) {
                        LogSoundEventsSoundParametersText(soundEvent, soundParameterType, updateMode, soundParameters[i].internals.valueBool.ToString());
                    }
                }
            }
        }
    }
}
#endif