// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundContainerEditorPreview : Editor {

        private SoundContainerEditorBase parent;

        public void Initialize(SoundContainerEditorBase parent) {
            this.parent = parent;
        }

        public bool mouseCursorDraw;
        public MouseCursor mouseCursorCurrent;

        public void SetCursorRect(MouseCursor mouseCursor) {
            mouseCursorCurrent = mouseCursor;
            mouseCursorDraw = true;
        }

        public void DrawCursorRect() {
            if (mouseCursorDraw) {
                EditorGUIUtility.AddCursorRect(layoutSelectionRect, mouseCursorCurrent);
            }
        }

        private Vector3[] polygon = new Vector3[4];
        private bool previewSelected = false;
        private Rect layoutSelectionRect = new Rect();

        public void Preview() {

            if (parent.expandPreview == null) {
                return;
            }

            if (parent.expandPreview.boolValue) {

                parent.BeginChange();
                EditorGuiFunction.DrawFoldout(parent.expandPreview, "Preview");
                parent.EndChange();

                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                float height = 125f;
                float heightHalf = height * 0.5f;
                float alpha = 1f;
                float size = 20f;
                float marginSize = 15f;
                float marginMult = (height - marginSize * 2f) / height;

                Rect layoutRectangle = GUILayoutUtility.GetRect(height, height, height, height);

                // Repaint if mouse is inside or is moving preview
                if (layoutRectangle.Contains(Event.current.mousePosition) || previewSelected) {
                    Repaint();
                }

                if (Event.current.type == EventType.Repaint) {
                    polygon[0] = new Vector3(parent.previewEditorSetting.position.x * heightHalf + size + heightHalf, parent.previewEditorSetting.position.z * heightHalf + heightHalf, 0f);
                    polygon[1] = new Vector3(parent.previewEditorSetting.position.x * heightHalf + heightHalf, parent.previewEditorSetting.position.z * heightHalf + size + heightHalf, 0f);
                    polygon[2] = new Vector3(parent.previewEditorSetting.position.x * heightHalf - size + heightHalf, parent.previewEditorSetting.position.z * heightHalf + heightHalf, 0f);
                    polygon[3] = new Vector3(parent.previewEditorSetting.position.x * heightHalf + heightHalf, parent.previewEditorSetting.position.z * heightHalf - size + heightHalf, 0f);

                    polygon[0].x = polygon[0].x * marginMult + marginSize;
                    polygon[0].y = polygon[0].y * marginMult + marginSize;
                    polygon[1].x = polygon[1].x * marginMult + marginSize;
                    polygon[1].y = polygon[1].y * marginMult + marginSize;
                    polygon[2].x = polygon[2].x * marginMult + marginSize;
                    polygon[2].y = polygon[2].y * marginMult + marginSize;
                    polygon[3].x = polygon[3].x * marginMult + marginSize;
                    polygon[3].y = polygon[3].y * marginMult + marginSize;

                    layoutSelectionRect.xMin = parent.previewEditorSetting.position.x * heightHalf - size + heightHalf;
                    layoutSelectionRect.xMax = parent.previewEditorSetting.position.x * heightHalf + size + heightHalf;
                    layoutSelectionRect.yMin = parent.previewEditorSetting.position.z * heightHalf - size + heightHalf;
                    layoutSelectionRect.yMax = parent.previewEditorSetting.position.z * heightHalf + size + heightHalf;

                    layoutSelectionRect.xMin = layoutSelectionRect.xMin * marginMult + marginSize;
                    layoutSelectionRect.xMax = layoutSelectionRect.xMax * marginMult + marginSize;
                    layoutSelectionRect.yMin = layoutSelectionRect.yMin * marginMult + marginSize;
                    layoutSelectionRect.yMax = layoutSelectionRect.yMax * marginMult + marginSize;

                    layoutSelectionRect.xMin += layoutRectangle.xMin;
                    layoutSelectionRect.xMax += layoutRectangle.xMin;
                    layoutSelectionRect.yMin += layoutRectangle.yMin;
                    layoutSelectionRect.yMax += layoutRectangle.yMin;

                    GUI.BeginClip(layoutRectangle);
                    GL.PushMatrix();

                    Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
                    parent.cachedMaterial.SetPass(0);
                    
                    GL.Begin(GL.QUADS);

                    // Background
                    GL.Color(backgroundColor);
                    GL.Vertex3(0f, 0f, 0f);
                    GL.Vertex3(layoutRectangle.width, 0f, 0f);
                    GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0f);
                    GL.Vertex3(0f, layoutRectangle.height, 0f);

                    float lineWidth = 1f;
                    Color lineColor = new Color();
                    if (EditorGUIUtility.isProSkin) {
                        lineColor = EditorColorProSkin.GetDarkSkinTextColor();
                    }

                    // Line left top to right bottom
                    GL.Color(lineColor);
                    GL.Vertex3(0f, 0f, 0f);
                    GL.Vertex3(0f + lineWidth, 0f, 0f);
                    GL.Vertex3(height, height, 0f);
                    GL.Vertex3(height - lineWidth, height, 0f);

                    // Line right top to left bottom
                    GL.Color(lineColor);
                    GL.Vertex3(height - lineWidth, 0f, 0f);
                    GL.Vertex3(height, 0f, 0f);
                    GL.Vertex3(0f, height + lineWidth, 0f);
                    GL.Vertex3(0f, height, 0f);

                    // Vertex List
                    GL.Color(EditorColor.GetVolumeMax(alpha));
                    GL.Vertex(polygon[0]);
                    GL.Vertex(polygon[1]);
                    GL.Vertex(polygon[2]);
                    GL.Vertex(polygon[3]);

                    GL.End();
                    GL.PopMatrix();
                    GUI.EndClip();

                    GUIStyle guiStyleText = new GUIStyle();
                    guiStyleText.alignment = TextAnchor.MiddleLeft;
                    if (EditorGUIUtility.isProSkin) {
                        guiStyleText.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
                    }
                    GUIContent guiContentText = new GUIContent();

                    // Draw Label Front
                    guiContentText.text = EditorTextPreview.boxFront;
                    float labelFrontOffset = guiStyleText.CalcSize(guiContentText).x * 0.5f;
                    GUI.Label(new Rect(layoutRectangle.x + heightHalf - labelFrontOffset, layoutRectangle.y - layoutRectangle.height * 0.5f + 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                    // Draw Label Back
                    guiContentText.text = EditorTextPreview.boxBack;
                    float labelBackOffset = guiStyleText.CalcSize(guiContentText).x * 0.5f;
                    GUI.Label(new Rect(layoutRectangle.x + heightHalf - labelBackOffset, layoutRectangle.y + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                    // Draw Label Left
                    guiContentText.text = EditorTextPreview.boxLeft;
                    float labelLeftOffset = 1f;
                    GUI.Label(new Rect(layoutRectangle.x + labelLeftOffset, layoutRectangle.y, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                    // Draw Label Right
                    guiContentText.text = EditorTextPreview.boxRight;
                    float labelRightOffset = guiStyleText.CalcSize(guiContentText).x + 1f;
                    GUI.Label(new Rect(layoutRectangle.x + height - labelRightOffset, layoutRectangle.y, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);
                }

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                // Play
                parent.BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextPreview.soundContainerPlayLabel, EditorTextPreview.soundContainerPlayTooltip)) || EditorShortcutsPreview.GetPlayIsPressed()) {
                    EditorPreviewSound.Stop(false, false);
                    EditorPreviewSoundData newPreviewSoundContainerSetting = new EditorPreviewSoundData();
                    newPreviewSoundContainerSetting.soundContainer = parent.mTarget;
                    EditorPreviewSound.SetEditorSetting(parent.previewEditorSetting);
                    EditorPreviewSound.AddEditorPreviewSoundData(newPreviewSoundContainerSetting);
                    // Used to draw the preview dots on the SoundContainer Curves
                    EditorPreviewSound.UseCurvePreviewValues();
                    EditorPreviewSound.PlaySoundEvent();
                }
                parent.EndChange();
                // Stop
                parent.BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextPreview.stopLabel, EditorTextPreview.stopTooltip)) || EditorShortcutsPreview.GetStopIsPressed()) {
                    EditorPreviewSound.Stop(true, true);
                }
                parent.EndChange();
                // Reset
                parent.BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextPreview.resetLabel, EditorTextPreview.resetTooltip))) {
                    parent.previewEditorSetting.position = Vector3.zero;
                    parent.previewEditorSetting.intensityTarget = 1f;
                }
                parent.EndChange();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                parent.BeginChange();
                // Make intensity slider show better
                float rightWidth = EditorGUIUtility.currentViewWidth - height;
                GUIContent textIntensity = new GUIContent(EditorTextPreview.intensityLabel, EditorTextPreview.intensityTooltip);
                float textWidth = textIntensity.text.Length * 6f;
                float sliderWidth = rightWidth - textWidth - EditorGUIUtility.fieldWidth;
                EditorGUILayout.LabelField(textIntensity, GUILayout.Width(textWidth));
                parent.previewEditorSetting.intensityTarget = EditorGUILayout.Slider(
                    parent.previewEditorSetting.intensityTarget, 0f, 1f, GUILayout.Width(sliderWidth));
                parent.EndChange();
                EditorGUILayout.EndHorizontal();

                // Preview AudioMixerGroup
                parent.BeginChange();
                EditorGUILayout.ObjectField(parent.previewAudioMixerGroup, new GUIContent(EditorTextPreview.audioMixerGroupLabel, EditorTextPreview.audioMixerGroupTooltip));
                parent.EndChange();

                EditorGUILayout.EndVertical();

                GUILayout.EndHorizontal();

                // Hover over cursor
                if (layoutSelectionRect.Contains(Event.current.mousePosition)) {
                    SetCursorRect(MouseCursor.MoveArrow);
                }

                // Mouse interaction outside of editor view
                int controlId = GUIUtility.GetControlID(FocusType.Passive);
                Event eventCurrent = Event.current;

                // Mouse Down
                if (layoutRectangle.Contains(Event.current.mousePosition) && eventCurrent.GetTypeForControl(controlId) == EventType.MouseDown) {
                    //Must set hotControl to receive MouseUp outside window
                    GUIUtility.hotControl = controlId;
                    if (layoutSelectionRect.Contains(Event.current.mousePosition)) {
                        previewSelected = true;
                        SetCursorRect(MouseCursor.MoveArrow);
                        // Deselect Input Fields
                        GUI.FocusControl("");
                    }
                }
                // Mouse Up
                else if (previewSelected && eventCurrent.GetTypeForControl(controlId) == EventType.MouseUp) {
                    previewSelected = false;
                    mouseCursorDraw = false;
                }
                // Mouse Drag
                else if (previewSelected && eventCurrent.GetTypeForControl(controlId) == EventType.MouseDrag) {
                    // Deselect Input Fields
                    GUI.FocusControl("");
                    SetCursorRect(MouseCursor.MoveArrow);
                    float moveSpeed = 1.25f;
                    Vector3 positionVector = new Vector3(
                        parent.previewEditorSetting.position.x,
                        0f,
                        parent.previewEditorSetting.position.z);
                    EditorGUI.BeginChangeCheck();
                    positionVector.x = Mathf.Clamp(positionVector.x + Event.current.delta.x * moveSpeed / heightHalf, -1f, 1f);
                    positionVector.z = Mathf.Clamp(positionVector.z + Event.current.delta.y * moveSpeed / heightHalf, -1f, 1f);
                    if (positionVector != parent.previewEditorSetting.position) {
                        Undo.RecordObject(parent.mTarget, "Moved Audio Preview Position");
                        parent.previewEditorSetting.position = positionVector;
                    }
                }

                // Draw cursor icon
                DrawCursorRect();
            } else {

                // Transparent background so the offset will be right
                parent.StartBackgroundColor(new Color(0f, 0f, 0f, 0f));

                EditorGUILayout.BeginHorizontal();

                // Extra horizontal for labelWidth
                EditorGUILayout.BeginHorizontal(GUILayout.Width(EditorGUIUtility.labelWidth));
                GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
                parent.BeginChange();
                EditorGuiFunction.DrawFoldout(parent.expandPreview, "Preview");
                parent.EndChange();
                EditorGUILayout.EndHorizontal();

                // Play
                parent.BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextPreview.soundContainerPlayLabel, EditorTextPreview.soundContainerPlayTooltip)) || EditorShortcutsPreview.GetPlayIsPressed()) {
                    EditorPreviewSound.Stop(false, false);
                    EditorPreviewSoundData newPreviewSoundContainerSetting = new EditorPreviewSoundData();
                    newPreviewSoundContainerSetting.soundContainer = parent.mTarget;
                    EditorPreviewSound.SetEditorSetting(new EditorPreviewControls());
                    EditorPreviewSound.AddEditorPreviewSoundData(newPreviewSoundContainerSetting);
                    // Used to draw the preview dots on the SoundContainer Curves
                    EditorPreviewSound.UseCurvePreviewValues();
                    EditorPreviewSound.PlaySoundEvent();
                }
                parent.EndChange();
                // Stop
                parent.BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextPreview.stopLabel, EditorTextPreview.stopTooltip)) || EditorShortcutsPreview.GetStopIsPressed()) {
                    EditorPreviewSound.Stop(true, true);
                }
                parent.EndChange();
                EditorGUILayout.EndHorizontal();
                parent.StopBackgroundColor();
            }
        }
    }
}
#endif