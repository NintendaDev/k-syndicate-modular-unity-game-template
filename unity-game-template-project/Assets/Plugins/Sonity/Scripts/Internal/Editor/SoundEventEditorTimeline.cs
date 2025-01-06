// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundEventEditorTimeline : Editor {

        public SoundEventEditorBase parent;
        public SoundEventEditorTimelineData data;
        public SoundEventEditorTimelineRects timelineRects;
        public SoundEventEditorTimelineInteraction timelineInteraction;

        public void Initialize(SoundEventEditorBase soundEventEditor, SoundEventEditorTimelineData soundEventEditorTimelineData) {
            parent = soundEventEditor;
            data = soundEventEditorTimelineData;
            timelineRects = new SoundEventEditorTimelineRects();
            timelineRects.Initialize(this);
            timelineInteraction = CreateInstance<SoundEventEditorTimelineInteraction>();
            timelineInteraction.Initialize(this);
        }

        public float GetItemLength(int layerIndex) {
            if (layerIndex >= parent.mTarget.internals.soundContainers.Length) {
                return 0f;
            }
            if (parent.mTarget.internals.soundContainers[layerIndex] == null) {
                return 0f;
            }
            float tempLength = parent.mTarget.internals.soundContainers[layerIndex].internals.GetLongestAudioClipLength();
            // SoundContainer pitch
            if (parent.mTarget.internals.soundContainers[layerIndex].internals.data.pitchRatio > 0f) {
                tempLength /= parent.mTarget.internals.soundContainers[layerIndex].internals.data.pitchRatio;
            }
            // SoundEventModifier pitch
            if (parent.mTarget.internals.data.soundEventModifier.pitchUse) {
                tempLength /= parent.mTarget.internals.data.soundEventModifier.pitchRatio;
            }
            return tempLength;
        }

        public void SetZoomScaleToFit() {
            // Get Max Width
            data.widthMax = 0f;
            data.soundContainerDelayAndWidth = new float[parent.mTarget.internals.data.timelineSoundContainerData.Length];
            for (int i = 0; i < data.soundContainerDelayAndWidth.Length; i++) {
                data.soundContainerDelayAndWidth[i] = GetItemLength(i) + parent.mTarget.internals.data.timelineSoundContainerData[i].delay;
                if (data.widthMax < data.soundContainerDelayAndWidth[i]) {
                    data.widthMax = data.soundContainerDelayAndWidth[i];
                }
            }
            // Set max width & Avoid Divide by 0
            if (data.widthMax > 0f) {
                data.widthZoomScale = data.widthMax / data.layoutRectangle.width;
            } else {
                data.widthZoomScale = 4f / data.layoutRectangle.width;
            }
        }

        // Timeline Reset Zoom and Horizontal
        public void ResetZoomAndHorizontal() {
            SetZoomScaleToFit();
            data.horizontalOffset = 0f;
            Repaint();
        }

        public void TimelineInteraction() {

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            data.layoutRectangle = GUILayoutUtility.GetRect(10f, data.itemHeight * parent.mTarget.internals.data.timelineSoundContainerData.Length + data.gridTopSpace + data.gridBottomSpace);

            if (data.layerSelectedDelay == null || data.layerSelectedDelay.Length != parent.mTarget.internals.data.timelineSoundContainerData.Length) {
                data.InitializeLayerSelected(parent.mTarget.internals.data.timelineSoundContainerData.Length);
            }

            // If Mouse is within editor view
            if (data.layoutRectangle.Contains(Event.current.mousePosition)) {

                // Repaint when inside layout rectangle so that hover over works
                Repaint();

                parent.soundEventEditorPreview.previewSelected = false;

                // Zoom
                if (Event.current.type == EventType.ScrollWheel && Event.current.control) {
                    float zoomDirection;

                    if (Event.current.delta.y > 0f) {
                        zoomDirection = -1f;
                    } else {
                        zoomDirection = 1f;
                    }

                    float zoomSpeed = 0.2f;
                    float mousePosition = (Event.current.mousePosition.x - data.layoutRectangle.xMin) / data.layoutRectangle.width;

                    data.horizontalOffset -= zoomDirection * (mousePosition * data.widthZoomScale * data.layoutRectangle.width * zoomSpeed);

                    data.widthZoomScale = Mathf.Clamp(data.widthZoomScale - zoomDirection * (data.widthZoomScale * zoomSpeed), 0f, Mathf.Infinity - 1f);

                    // Override Inspector Scroll
                    GUIUtility.ExitGUI();
                }
            }

            // Grid Time
            data.secScaled = data.widthZoomScale * data.layoutRectangle.width;
            data.secScaleMult = 1f;

            // Removing grid resolution
            while (data.secScaled < Mathf.Infinity && data.secScaled > 8f) {
                data.secScaled *= 0.5f;
                data.secScaleMult *= 2f;
            }

            // Adding grid resolution
            while (data.secScaled > 0f && data.secScaled < 4f) {
                data.secScaled *= 2f;
                data.secScaleMult *= 0.5f;
            }

            // If inspector is width is very small
            if (data.layoutRectangle.width / data.secScaled < 100f) {
                data.secScaled *= 0.5f;
                data.secScaleMult *= 2f;
            }

            data.secFloored = Mathf.CeilToInt(data.secScaled);
            data.gridIndexOffset = Mathf.Max(Mathf.CeilToInt(Mathf.Max(-data.horizontalOffset, 0) / data.secScaleMult), 0);
            data.secFloored = Mathf.Max(data.secFloored, 0);
            data.gridPos = new float[data.secFloored];
            for (int i = 0; i < data.gridPos.Length; i++) {
                data.gridPos[i] = ((data.gridIndexOffset + i) * data.secScaleMult + data.horizontalOffset) / data.widthZoomScale;
            }

            // Playback Positon
            data.playbackPositionRect = new Rect();
            data.playbackPositionRect.xMin = (EditorPreviewSound.GetTimeSinceStart() + data.horizontalOffset) / data.widthZoomScale;
            data.playbackPositionRect.xMax = (EditorPreviewSound.GetTimeSinceStart() + data.horizontalOffset) / data.widthZoomScale + 2f;
            data.playbackPositionRect.yMin = 0f;
            data.playbackPositionRect.yMax = data.layoutRectangle.height;

            // Initialize Rect
            if (data.layerBaseRectLayout == null || data.layerBaseRectLayout.Length != parent.mTarget.internals.data.timelineSoundContainerData.Length) {
                data.InitializeRect(parent.mTarget.internals.data.timelineSoundContainerData.Length);
            }
            for (int i = 0; i < parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                timelineRects.CalculateRects(i);
            }

            timelineInteraction.CheckInteract();
        }
    }
}
#endif