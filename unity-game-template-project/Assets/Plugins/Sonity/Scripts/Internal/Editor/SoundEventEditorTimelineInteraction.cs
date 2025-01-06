// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundEventEditorTimelineInteraction : Editor {

        public SoundEventEditorTimeline parent;

        public void Initialize(SoundEventEditorTimeline soundEventEditorTimeline) {
            parent = soundEventEditorTimeline;
        }

        public void TimelineDeselectAll() {
            for (int n = 0; n < parent.parent.mTarget.internals.data.timelineSoundContainerData.Length; n++) {
                parent.data.layerSelectedDelay[n] = false;
                parent.data.layerSelectedVolume[n] = false;
            }
        }

        public bool mouseCursorDraw;
        public MouseCursor mouseCursorCurrent;

        public void SetCursorRect(MouseCursor mouseCursor) {
            mouseCursorCurrent = mouseCursor;
            mouseCursorDraw = true;
        }

        public void DrawCursorRect() {
            if (mouseCursorDraw) {
                EditorGUIUtility.AddCursorRect(parent.data.layoutRectangle, mouseCursorCurrent);
            }
        }

        public bool mouseHold = false;
        public bool mouseInside = false;
        public bool anythingSelected = false;
        public bool backgroundSelected = false;
        public bool volumeOrDelaySelected = false;

        private bool repaintDone = false;

        private void DoRepaint() {
            if (!repaintDone) {
                repaintDone = true;
                Repaint();
            }
        }

        public void CheckInteract() {

            repaintDone = false;

            // If not hold then hover over cursor
            if (!mouseHold) {
                bool mouseHover = false;
                // Hover over mouse cursor
                for (int i = 0; i < parent.parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                    // Volume Priority
                    if (parent.data.layerVolumeHandleRectLayout[i].Contains(Event.current.mousePosition)) {
                        SetCursorRect(MouseCursor.ResizeVertical);
                        mouseHover = true;
                        DoRepaint();
                    }
                }
                if (!mouseHover) {
                    mouseCursorDraw = false;
                }
            }

            DrawCursorRect();

            // Restore Zoom And Horizontal
            if (parent.data.layoutRectangle.Contains(Event.current.mousePosition)) {
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F) {
                    parent.ResetZoomAndHorizontal();
                    // Override Inspector
                    Event.current.keyCode = KeyCode.None;
                    DoRepaint();
                }
            }

            // Check if OnGUI is mouse event
            if (Event.current.isMouse) {

                // Is mouse inside
                mouseInside = parent.data.layoutRectangle.Contains(Event.current.mousePosition);
            }

            // Mouse interaction outside of editor view
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Event eventCurrent = Event.current;
            // Mouse Down
            if (mouseInside && eventCurrent.GetTypeForControl(controlId) == EventType.MouseDown) {
                //Must set hotControl to receive MouseUp outside window
                GUIUtility.hotControl = controlId;
                mouseHold = true;
                anythingSelected = true;
                // Left mouse button
                if (Event.current.button == 0) {
                    for (int i = 0; i < parent.parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                        // Volume Priority
                        if (parent.data.layerVolumeHandleRectLayout[i].Contains(Event.current.mousePosition)) {
                            TimelineDeselectAll();
                            parent.data.layerSelectedVolume[i] = true;
                            backgroundSelected = false;
                            anythingSelected = true;
                            volumeOrDelaySelected = true;
                            DoRepaint();
                        }
                        // Delay
                        else if (parent.data.layerBaseRectLayout[i].Contains(Event.current.mousePosition)) {
                            TimelineDeselectAll();
                            parent.data.layerSelectedDelay[i] = true;
                            backgroundSelected = false;
                            anythingSelected = true;
                            volumeOrDelaySelected = true;
                            DoRepaint();
                        }
                    }
                    // Pan
                    if (!volumeOrDelaySelected) {
                        TimelineDeselectAll();
                        anythingSelected = true;
                        backgroundSelected = true;
                        volumeOrDelaySelected = false;
                        DoRepaint();
                    }
                } 
                // Middle mouse button
                else if (Event.current.button == 2) {
                    // Pan
                    TimelineDeselectAll();
                    anythingSelected = true;
                    backgroundSelected = true;
                    volumeOrDelaySelected = false;
                    DoRepaint();
                }
            } 
            // Mouse Up
            else if (anythingSelected && eventCurrent.GetTypeForControl(controlId) == EventType.MouseUp) {
                TimelineDeselectAll();
                mouseHold = false;
                mouseCursorDraw = false;
                anythingSelected = false;
                backgroundSelected = false;
                volumeOrDelaySelected = false;
                DoRepaint();
            } 
            // Mouse Drag
            else if (anythingSelected && mouseHold && eventCurrent.GetTypeForControl(controlId) == EventType.MouseDrag) {
                if (volumeOrDelaySelected) {
                    // Volume slower if holding alt
                    if (Event.current.alt) {
                        parent.data.volumeSpeedCurrent = parent.data.volumeSpeedBase * parent.data.volumeSpeedSlowerMult;
                    } else {
                        parent.data.volumeSpeedCurrent = parent.data.volumeSpeedBase;
                    }
                    for (int i = 0; i < parent.parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                        // Volume
                        if (parent.data.layerSelectedVolume[i]) {
                            // Deselect Input Fields
                            GUI.FocusControl("");
                            SetCursorRect(MouseCursor.ResizeVertical);
                            float volumeDecibel = parent.parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel;
                            EditorGUI.BeginChangeCheck();
                            volumeDecibel = Mathf.Clamp(volumeDecibel - Event.current.delta.y * parent.data.volumeSpeedCurrent, VolumeScale.lowestVolumeDecibel, 0f);
                            volumeDecibel = Mathf.Round(volumeDecibel * 10f) * 0.1f;
                            // Clamp low volume to -infinity if mouse if moving down
                            if (volumeDecibel <= VolumeScale.lowestVolumeDecibel && Event.current.delta.y > 0f) {
                                volumeDecibel = Mathf.NegativeInfinity;
                            }
                            if (parent.parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel != volumeDecibel) {
                                Undo.RecordObject(parent.parent.mTarget, "Changed Timeline Volume");
                                parent.parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel = volumeDecibel;
                                parent.parent.mTarget.internals.data.timelineSoundContainerData[i].volumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
                            }
                            DoRepaint();
                        }
                        // Delay
                        else if (parent.data.layerSelectedDelay[i]) {
                            // Deselect Input Fields
                            GUI.FocusControl("");
                            SetCursorRect(MouseCursor.ResizeHorizontal);
                            float delayValue = parent.parent.mTarget.internals.data.timelineSoundContainerData[i].delay;
                            EditorGUI.BeginChangeCheck();
                            delayValue = Mathf.Clamp(delayValue + Event.current.delta.x * parent.data.widthZoomScale, 0f, Mathf.Infinity);
                            if (delayValue != parent.parent.mTarget.internals.data.timelineSoundContainerData[i].delay) {
                                Undo.RecordObject(parent.parent.mTarget, "Changed Timeline Delay");
                                parent.parent.mTarget.internals.data.timelineSoundContainerData[i].delay = delayValue;
                            }
                            DoRepaint();
                        }
                    }
                }
                // Pan
                if (backgroundSelected) {
                    // Deselect Input Fields
                    GUI.FocusControl("");
                    SetCursorRect(MouseCursor.Pan);
                    parent.data.horizontalOffset = parent.data.horizontalOffset + Event.current.delta.x * parent.data.widthZoomScale;
                    DoRepaint();
                }
            }
        }
    }
}
#endif