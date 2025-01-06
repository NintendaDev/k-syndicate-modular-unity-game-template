// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class SoundEventEditorTimelineData {

        // Alpha
        public float layerBackgroundAlpha = 0.1f;
        public float layerLoopingGhostAlpha = 0.15f;
        public float layerVolumeAlpha = 0.4f;
        public float layerVolumeHandleAlpha = 0.5f;
        public float timelinePlaybackPostionAlpha = 0.7f;

        // Volume Control
        public float volumeSpeedBase = 0.25f;
        public float volumeSpeedSlowerMult = 0.125f;
        public float volumeSpeedCurrent = 0f;

        // Timeline
        public float gridTopSpace = 22f;
        public float gridBottomSpace = 0f;
        public float itemHeight = 70f;
        public float itemHeightSpace = 2f;
        public float nameTopOffset = -2f;
        public float itemTopSpace = 15f;
        public float gridWidth = 2f;

        public int secFloored;
        public int gridIndexOffset;
        public float secScaled;
        public float secScaleMult = 1f;

        public Rect[] layerBaseRectLayout;
        public Rect[] layerBaseRectMesh;
        public Rect[] layerBackgroundRectMesh;
        public Rect[] layerVolumeRectLayout;
        public Rect[] layerVolumeRectMesh;
        public Rect[] layerLoopRectMesh;
        public Rect[] layerVolumeHandleRectLayout;
        public Rect[] layerVolumeHandleRectMesh;

        public void InitializeRect(int length) {
            layerBaseRectLayout = new Rect[length];
            layerBaseRectMesh = new Rect[length];
            layerBackgroundRectMesh = new Rect[length];
            layerVolumeRectLayout = new Rect[length];
            layerVolumeRectMesh = new Rect[length];
            layerLoopRectMesh = new Rect[length];
            layerVolumeHandleRectLayout = new Rect[length];
            layerVolumeHandleRectMesh = new Rect[length];
        }

        public bool[] layerSelectedDelay;
        public bool[] layerSelectedVolume;

        public void InitializeLayerSelected(int length) {
            layerSelectedDelay = new bool[length];
            layerSelectedVolume = new bool[length];
        }

        public float[] soundContainerDelayAndWidth;
        public float[] gridPos;
        public Rect playbackPositionRect;

        public float widthZoomScale = 1f;
        public float horizontalOffset = 0f;

        public float widthMax = 0f;

        // Volume Handle Width
        public float handleWidthVolumeMesh = 5f;

        public float textMinimumSpaceToLeft = 22f;

        public Rect layoutRectangle;

        public float layerColorHueOffset = -0.05f;
    }
}
#endif