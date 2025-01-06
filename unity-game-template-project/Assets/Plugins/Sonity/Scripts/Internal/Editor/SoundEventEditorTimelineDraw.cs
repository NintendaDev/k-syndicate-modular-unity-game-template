// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundEventEditorTimelineDraw : Editor {

        public SoundEventEditorTimelineData data;
        public SoundEventEditorBase parent;

        public void Initialize(SoundEventEditorBase soundEventEditor, SoundEventEditorTimelineData soundEventEditorTimelineData) {
            parent = soundEventEditor;
            data = soundEventEditorTimelineData;
        }

        public bool zoomAndHorizontalInitialized = false;

        public void Draw() {

            if (Event.current.type == EventType.Repaint) {

                if (!zoomAndHorizontalInitialized) {
                    zoomAndHorizontalInitialized = true;
                    parent.soundEventEditorTimeline.ResetZoomAndHorizontal();
                }

                GUI.BeginClip(data.layoutRectangle);
                GL.PushMatrix();

                Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
                
                parent.cachedMaterial.SetPass(0);

                // Background
                GL.Begin(GL.QUADS);
                GL.Color(backgroundColor);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(data.layoutRectangle.width, 0, 0);
                GL.Vertex3(data.layoutRectangle.width, data.layoutRectangle.height, 0);
                GL.Vertex3(0, data.layoutRectangle.height, 0);
                GL.End();

                // Track background
                GL.Begin(GL.QUADS);
                GL.Color(new Color(1f, 1f, 1f, 0.05f));

                Rect rectGridBackground = new Rect();

                rectGridBackground.xMin = 0f;
                rectGridBackground.xMax = data.layoutRectangle.width;
                rectGridBackground.yMin = 0f;
                rectGridBackground.yMax = data.gridTopSpace;

                GL.Vertex3(rectGridBackground.xMin, rectGridBackground.yMin, 0f);
                GL.Vertex3(rectGridBackground.xMax, rectGridBackground.yMin, 0f);
                GL.Vertex3(rectGridBackground.xMax, rectGridBackground.yMax, 0f);
                GL.Vertex3(rectGridBackground.xMin, rectGridBackground.yMax, 0f);

                for (int i = 0; i < parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                    // Evey even track
                    if (i % 2 == 1) {
                        Rect rectTemp = new Rect();

                        rectTemp.xMin = 0f;
                        rectTemp.xMax = data.layoutRectangle.width;

                        rectTemp.yMin = i * data.itemHeight + data.gridTopSpace;
                        rectTemp.yMax = (i + 1) * data.itemHeight + data.gridTopSpace;

                        GL.Vertex3(rectTemp.xMin, rectTemp.yMin, 0f);
                        GL.Vertex3(rectTemp.xMax, rectTemp.yMin, 0f);
                        GL.Vertex3(rectTemp.xMax, rectTemp.yMax, 0f);
                        GL.Vertex3(rectTemp.xMin, rectTemp.yMax, 0f);
                    }
                }
                GL.End();

                // Drawing Grid
                GL.Begin(GL.QUADS);
                int tempCount = 0;
                for (int i = 0; i < data.gridPos.Length; i++) {
                    if (data.gridPos[i] + data.gridWidth > 0f && data.gridPos[i] < data.layoutRectangle.width) {
                        tempCount++;
                        int highlight = i;
                        if (data.gridIndexOffset % 4 != 0) {
                            highlight -= (4 - data.gridIndexOffset % 4);
                        }

                        if (highlight >= 0 && highlight % 4 == 0) {
                            GL.Color(new Color(1f, 1f, 1f, 0.2f));
                        } else {
                            GL.Color(new Color(1f, 1f, 1f, 0.1f));
                        }

                        Rect rectTemp = new Rect();

                        rectTemp.xMin = Mathf.Clamp(data.gridPos[i], 0f, data.layoutRectangle.width);
                        rectTemp.xMax = Mathf.Clamp(data.gridPos[i] + data.gridWidth, 0f, data.layoutRectangle.width);
                        rectTemp.yMin = 0f;
                        rectTemp.yMax = data.layoutRectangle.height;

                        GL.Vertex3(rectTemp.xMin, rectTemp.yMin, 0f);
                        GL.Vertex3(rectTemp.xMax, rectTemp.yMin, 0f);
                        GL.Vertex3(rectTemp.xMax, rectTemp.yMax, 0f);
                        GL.Vertex3(rectTemp.xMin, rectTemp.yMax, 0f);
                    }
                }
                GL.End();

                // Drawing Layer
                GL.Begin(GL.QUADS);
                for (int i = 0; i < parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {
                    // Layer Background Rect Mesh
                    Color layerColor = EditorColor.ChangeHue(EditorColor.GetVolumeMax(1f), data.layerColorHueOffset * i);
                    GL.Color(EditorColor.ChangeAlpha(layerColor, data.layerBackgroundAlpha));
                    // Vertex Top Left
                    GL.Vertex(new Vector3(data.layerBackgroundRectMesh[i].xMin, data.layerBackgroundRectMesh[i].yMin, 0f));
                    // Vertex Top Right
                    GL.Vertex(new Vector3(data.layerBackgroundRectMesh[i].xMax, data.layerBackgroundRectMesh[i].yMin, 0f));
                    // Vertex Bottom Right
                    GL.Vertex(new Vector3(data.layerBackgroundRectMesh[i].xMax, data.layerBackgroundRectMesh[i].yMax, 0f));
                    // Vertex Bottom Left
                    GL.Vertex(new Vector3(data.layerBackgroundRectMesh[i].xMin, data.layerBackgroundRectMesh[i].yMax, 0f));

                    if (parent.mTarget.internals.soundContainers[i] != null) {
                        // SoundContainer Looping Ghost
                        if (parent.mTarget.internals.soundContainers[i].internals.data.loopEnabled) {
                            // Color
                            GL.Color(EditorColor.ChangeAlpha(layerColor, data.layerLoopingGhostAlpha));
                            // Vertex Top Left
                            GL.Vertex(new Vector3(data.layerLoopRectMesh[i].xMin, data.layerLoopRectMesh[i].yMin, 0f));
                            // Vertex Top Right
                            GL.Vertex(new Vector3(data.layerLoopRectMesh[i].xMax, data.layerLoopRectMesh[i].yMin, 0f));
                            // Vertex Bottom Right
                            GL.Vertex(new Vector3(data.layerLoopRectMesh[i].xMax, data.layerLoopRectMesh[i].yMax, 0f));
                            // Vertex Bottom Left
                            GL.Vertex(new Vector3(data.layerLoopRectMesh[i].xMin, data.layerLoopRectMesh[i].yMax, 0f));
                        }
                    }

                    // SoundContainer Volume Rect
                    // Overlay Volume Layer
                    GL.Color(EditorColor.ChangeAlpha(layerColor, data.layerVolumeAlpha));
                    // Vertex Top Left
                    GL.Vertex(new Vector3(data.layerVolumeRectMesh[i].xMin, data.layerVolumeRectMesh[i].yMin, 0f));
                    // Vertex Top Right
                    GL.Vertex(new Vector3(data.layerVolumeRectMesh[i].xMax, data.layerVolumeRectMesh[i].yMin, 0f));
                    // Vertex Bottom Right
                    GL.Vertex(new Vector3(data.layerVolumeRectMesh[i].xMax, data.layerVolumeRectMesh[i].yMax, 0f));
                    // Vertex Bottom Left
                    GL.Vertex(new Vector3(data.layerVolumeRectMesh[i].xMin, data.layerVolumeRectMesh[i].yMax, 0f));

                    // SoundContainer Rect Volume Handle Mesh
                    GL.Color(EditorColor.ChangeAlpha(layerColor, data.layerVolumeHandleAlpha));
                    // Vertex Top Left
                    GL.Vertex(new Vector3(data.layerVolumeHandleRectMesh[i].xMin, data.layerVolumeHandleRectMesh[i].yMin, 0f));
                    // Vertex Top Right
                    GL.Vertex(new Vector3(data.layerVolumeHandleRectMesh[i].xMax, data.layerVolumeHandleRectMesh[i].yMin, 0f));
                    // Vertex Bottom Right
                    GL.Vertex(new Vector3(data.layerVolumeHandleRectMesh[i].xMax, data.layerVolumeHandleRectMesh[i].yMax, 0f));
                    // Vertex Bottom Left
                    GL.Vertex(new Vector3(data.layerVolumeHandleRectMesh[i].xMin, data.layerVolumeHandleRectMesh[i].yMax, 0f));
                }
                GL.End();

                // Drawing Timeline Playback Position
                if (EditorPreviewSound.GetTimeSinceStart() > 0f) {
                    GL.Begin(GL.QUADS);
                    GL.Color(EditorColor.GetTimelinePlaybackPosition(data.timelinePlaybackPostionAlpha));
                    // Vertex Top Left
                    GL.Vertex(new Vector3(data.playbackPositionRect.xMin, data.playbackPositionRect.yMin, 0f));
                    // Vertex Top Right
                    GL.Vertex(new Vector3(data.playbackPositionRect.xMax, data.playbackPositionRect.yMin, 0f));
                    // Vertex Bottom Right
                    GL.Vertex(new Vector3(data.playbackPositionRect.xMax, data.playbackPositionRect.yMax, 0f));
                    // Vertex Bottom Left
                    GL.Vertex(new Vector3(data.playbackPositionRect.xMin, data.playbackPositionRect.yMax, 0f));
                    // End drawing
                    GL.End();
                }

                GL.PopMatrix();
                GUI.EndClip();

                // Styling for the text
                GUIStyle guiStyleText = new GUIStyle();
                guiStyleText.alignment = TextAnchor.MiddleLeft;
                if (EditorGUIUtility.isProSkin) {
                    guiStyleText.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
                }
                GUIContent guiContentText = new GUIContent();

                // Grid Time Text
                float gridTimeTextOffset = 70f;
                float gridTimeLast = -1f;
                for (int i = 0; i < data.gridPos.Length; i++) {
                    Rect timeRect = new Rect();
                    timeRect.xMin = data.gridPos[i] + gridTimeTextOffset - data.layoutRectangle.xMin * 2f;
                    timeRect.xMax = data.layoutRectangle.xMax;
                    timeRect.y = data.layoutRectangle.y - data.layoutRectangle.height + 14f;
                    timeRect.yMax = data.layoutRectangle.yMax;

                    float tempTime = (data.gridIndexOffset + i) * data.secScaleMult;
                    float tempTimeRounded;
                    float tempTimeMult = 100f;

                    // Rounding time
                    tempTimeRounded = Mathf.Floor(tempTime * tempTimeMult) / tempTimeMult;

                    // If the last grid time is the same as the current
                    while (tempTimeRounded <= gridTimeLast) {
                        tempTimeRounded = Mathf.Floor(tempTime * tempTimeMult) / tempTimeMult;
                        // Increasing the resolution
                        tempTimeMult *= 10f;
                    }

                    gridTimeLast = tempTimeRounded;
                    guiContentText.text = tempTimeRounded.ToString() + "s";
                    GUI.Label(timeRect, guiContentText, guiStyleText);
                }
            }

            // Drawing names, float fields & dB
            for (int i = 0; i < parent.mTarget.internals.data.timelineSoundContainerData.Length; i++) {

                // If out of bounds when resizing
                if (i >= parent.mTarget.internals.soundContainers.Length) {
                    return;
                }
                
                // Styling for the text
                GUIStyle guiStyleText = new GUIStyle();
                guiStyleText.alignment = TextAnchor.MiddleLeft;
                if (EditorGUIUtility.isProSkin) {
                    guiStyleText.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
                }
                
                string nameTextString = "";
                if (parent.mTarget.internals.soundContainers[i] == null) {
                    nameTextString = "None";
                } else {
                    nameTextString = parent.mTarget.internals.soundContainers[i] == null ? "null" : parent.mTarget.internals.soundContainers[i].name;
                    if (parent.mTarget.internals.soundContainers[i].internals.data.loopEnabled) {
                        nameTextString += " Loop";
                    }
                }

                Rect nameRect = new Rect();

                GUIContent guiContentName = new GUIContent();
                guiContentName.text = nameTextString;

                // Offset Name
                float nameTextOffset = guiStyleText.CalcSize(guiContentName).x;
                float nameTextOffsetLeft = 13f;

                // Offset Volume
                float volumeDecibel = parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel;
                float volumeTextOffset = guiStyleText.CalcSize(new GUIContent(volumeDecibel.ToString())).x;

                // Offset Floatfield
                float floatFieldWidthX = 52f;
                float floatFieldWidthY = 14f;
                float floatFieldOffsetX = -12f;
                float floatFieldOffsetY = 53f;

                // Offset Db
                float dbTextOffset = 6f;

                // Offset Edge Right
                float offsetEdgeRight = 4f;

                // Name Rect
                nameRect.xMin = Mathf.Clamp(
                    data.layerBaseRectLayout[i].xMin - data.layoutRectangle.xMin + nameTextOffsetLeft * 2f,
                    data.textMinimumSpaceToLeft,
                    data.layoutRectangle.xMax - (nameTextOffset + volumeTextOffset + dbTextOffset + -floatFieldOffsetX + offsetEdgeRight)
                    );
                nameRect.xMax = data.layoutRectangle.xMax;
                nameRect.yMin = data.layerBaseRectLayout[i].yMin - data.layerBaseRectLayout[i].height - data.itemTopSpace + data.nameTopOffset;
                nameRect.yMax = data.layerBaseRectLayout[i].yMax;

                // Name Label
                GUI.Label(nameRect, guiContentName, guiStyleText);

                Rect floatRect = new Rect();
                floatRect.xMin = nameRect.xMin + floatFieldOffsetX + nameTextOffset;
                floatRect.xMax = nameRect.xMin + floatFieldOffsetX + nameTextOffset + floatFieldWidthX;
                floatRect.yMin = nameRect.yMin + floatFieldOffsetY;
                floatRect.yMax = nameRect.yMin + floatFieldOffsetY + floatFieldWidthY;

                EditorGUI.BeginChangeCheck();

                // Volume Decibel Floatfield
                volumeDecibel = EditorGUI.FloatField(floatRect, "", volumeDecibel, guiStyleText);

                // If positive then turn it negative
                if (volumeDecibel > 0f) {
                    volumeDecibel = -volumeDecibel;
                }
                // Clamp low volume to -infinity if mouse if moving down
                if (volumeDecibel <= VolumeScale.lowestVolumeDecibel && Event.current.delta.y > 0f) {
                    volumeDecibel = Mathf.NegativeInfinity;
                }
                if (parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel != volumeDecibel) {
                    Undo.RecordObject(parent.mTarget, "Changed Timeline Volume");
                    parent.mTarget.internals.data.timelineSoundContainerData[i].volumeDecibel = volumeDecibel;
                    parent.mTarget.internals.data.timelineSoundContainerData[i].volumeRatio = VolumeScale.ConvertDecibelToRatio(volumeDecibel);
                }

                Rect dbRect = new Rect();
                dbRect.xMin = nameRect.xMin + nameTextOffset + volumeTextOffset + dbTextOffset;
                dbRect.xMax = nameRect.xMax;
                dbRect.yMin = nameRect.yMin;
                dbRect.yMax = nameRect.yMax;

                GUIContent guiContentDb = new GUIContent();
                guiContentDb.text = "dB";

                // dB Label
                GUI.Label(dbRect, guiContentDb, guiStyleText);
            }

            GUILayout.EndHorizontal();
        }
    }
}
#endif