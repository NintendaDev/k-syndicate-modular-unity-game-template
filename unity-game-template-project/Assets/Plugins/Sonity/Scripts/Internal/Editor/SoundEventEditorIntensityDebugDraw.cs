// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundEventEditorIntensityDebugDraw : Editor {

        private SoundEventEditorBase parent;

        public void Initialize(SoundEventEditorBase parent) {
            this.parent = parent;
        }

        public void Draw() {

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            Rect layoutRectangle = GUILayoutUtility.GetRect(10, 10000, 100, 100);

            GUIStyle guiStyleText = new GUIStyle();
            guiStyleText.alignment = TextAnchor.MiddleLeft;
            if (EditorGUIUtility.isProSkin) {
                guiStyleText.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }
            GUIContent guiContentText = new GUIContent();


            if (Event.current.type == EventType.Repaint) {
                GUI.BeginClip(layoutRectangle);
                GL.PushMatrix();

                Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
                parent.cachedMaterial.SetPass(0);
                
                // Background
                GL.Begin(GL.QUADS);
                GL.Color(backgroundColor);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(layoutRectangle.width, 0, 0);
                GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
                GL.Vertex3(0, layoutRectangle.height, 0);
                GL.End();

                float[] points = new float[parent.mTarget.internals.data.intensityDebugResolution];

                // Making the logged intensity values to points
                for (int i = 0; i < parent.mTarget.internals.data.intensityDebugValueList.Count; i++) {
                    float tempValue = parent.mTarget.internals.data.GetScaledIntensity(parent.mTarget.internals.data.intensityDebugValueList[i]);
                    int tempIndex = Mathf.RoundToInt(tempValue * parent.mTarget.internals.data.intensityDebugResolution);
                    points[(int)Mathf.Clamp(tempIndex, 0f, points.Length - 1f)] += 1f;
                }

                // Find highest
                float highest = Mathf.NegativeInfinity;
                float highestLeftRight = Mathf.NegativeInfinity;
                for (int i = 0; i < points.Length; i++) {
                    // Skip points to left and right
                    if (i > 0 && i < points.Length - 1) {
                        if (highest < points[i]) {
                            highest = points[i];
                        }
                    } else {
                        // Left and right points
                        if (highestLeftRight < points[i]) {
                            highestLeftRight = points[i];
                        }
                    }
                }

                // Scale to highest
                float multiplier = 0;
                if (highestLeftRight > highest) {
                    multiplier = Mathf.Lerp(highestLeftRight, highest, parent.mTarget.internals.data.intensityDebugZoom);
                } else {
                    multiplier = highest;
                }
                if (multiplier != 0) {
                    for (int i = 0; i < points.Length; i++) {
                        points[i] = Mathf.Clamp(points[i] / multiplier, 0f, 1f);
                    }
                } else {
                    for (int i = 0; i < points.Length; i++) {
                        points[i] = Mathf.Clamp(points[i], 0f, 1f);
                    }
                }

                GL.Begin(GL.TRIANGLES);

                Color softColor = EditorColor.GetIntensityMax(0.75f);
                Color hardColor = EditorColor.GetIntensityMin(0.75f);
                Color greyColor = EditorColor.GetDisabled(0.75f);

                // Draw Curve
                for (int i = 0; i < points.Length; i++) {
                    float intensity1 = (float)i / points.Length;
                    float intensity2 = ((float)i + 1) / points.Length;

                    float amount1;
                    float amount2;

                    amount1 = points[i];
                    if (i + 1 < points.Length) {
                        amount2 = points[i + 1];
                    } else {
                        amount2 = points[i];
                    }

                    // Vertex List (Right Side Up)
                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity1 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity1));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity1 * layoutRectangle.width, -amount1 * layoutRectangle.height + layoutRectangle.height, 0f));

                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity2 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity2));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity2 * layoutRectangle.width, -amount2 * layoutRectangle.height + layoutRectangle.height, 0f));

                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity1 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity1));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity1 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));

                    // Vertex List (Right Side Down)
                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity2 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity2));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity2 * layoutRectangle.width, -amount2 * layoutRectangle.height + layoutRectangle.height, 0f));

                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity2 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity2));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity2 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));

                    if (!parent.mTarget.internals.data.intensityThresholdEnable || intensity2 > parent.mTarget.internals.data.intensityThreshold) {
                        GL.Color(Color.Lerp(softColor, hardColor, intensity2));
                    } else {
                        GL.Color(greyColor);
                    }
                    GL.Vertex(new Vector3(intensity1 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));
                }

                GL.End();

                GL.PopMatrix();
                GUI.EndClip();

                string labelTopLeft = "No Recorded Values";
                string labelTopRight = "";
                string labelBottomLeft = "Lowest Intensity";
                string labelBottomRight = "Higest Intensity";

                if (parent.mTarget.internals.data.intensityDebugValueList.Count > 0) {
                    // Top Left
                    float minValue = Mathf.Infinity;
                    for (int i = 0; i < parent.mTarget.internals.data.intensityDebugValueList.Count; i++) {
                        if (parent.mTarget.internals.data.intensityDebugValueList[i] < minValue) {
                            minValue = parent.mTarget.internals.data.intensityDebugValueList[i];
                        }
                    }
                    labelTopLeft = minValue.ToString("0.0") + " Min";

                    // Top Right
                    float maxValue = Mathf.NegativeInfinity;
                    for (int i = 0; i < parent.mTarget.internals.data.intensityDebugValueList.Count; i++) {
                        if (parent.mTarget.internals.data.intensityDebugValueList[i] > maxValue) {
                            maxValue = parent.mTarget.internals.data.intensityDebugValueList[i];
                        }
                    }
                    labelTopRight = maxValue.ToString("0.0") + " Max";

                    // Bottom Left
                    labelBottomLeft = parent.mTarget.internals.data.GetUnscaledIntensity(0f).ToString("0.0") + " Unscaled";

                    // Bottom Right
                    labelBottomRight = parent.mTarget.internals.data.GetUnscaledIntensity(1f).ToString("0.0") + " Unscaled";
                } 

                // Draw Label Top Left
                guiContentText.text = labelTopLeft;
                GUI.Label(new Rect(layoutRectangle.x + 1f, layoutRectangle.y - layoutRectangle.height * 0.5f + 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                // Draw Label Top Right
                guiContentText.text = labelTopRight;
                float labelRightTopOffset = guiStyleText.CalcSize(guiContentText).x;
                GUI.Label(new Rect(layoutRectangle.xMax - labelRightTopOffset - 1f, layoutRectangle.y - layoutRectangle.height * 0.5f + 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                // Draw Label Bottom Left
                guiContentText.text = labelBottomLeft;
                GUI.Label(new Rect(layoutRectangle.x + 1f, layoutRectangle.y + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);

                // Draw Label Bottom Right
                guiContentText.text = labelBottomRight;
                float labelRightBottomOffset = guiStyleText.CalcSize(guiContentText).x;
                GUI.Label(new Rect(layoutRectangle.xMax - labelRightBottomOffset - 1f, layoutRectangle.y + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), guiContentText, guiStyleText);
            }

            GUILayout.EndHorizontal();

            // Text below
            string topLeftString = "0 Intensity";
            string topMiddleString = "";
            string topRightString = "1 Intensity";

            float spaceForTextBelow = 14f;
            Rect topTextLayoutRect = GUILayoutUtility.GetRect(spaceForTextBelow, spaceForTextBelow, spaceForTextBelow, spaceForTextBelow);
            float spaceToCurvePreview = 6f;

            // Draw Top Left String
            guiContentText.text = topLeftString;
            float topLeftStringMaxOffset = guiStyleText.CalcSize(guiContentText).x;
            GUI.Label(new Rect(topTextLayoutRect.xMin + 6f, topTextLayoutRect.y - topTextLayoutRect.height * 0.5f + spaceToCurvePreview, topTextLayoutRect.width, topTextLayoutRect.height), guiContentText, guiStyleText);

            // Draw Top Middle String
            guiContentText.text = topMiddleString;
            float topMiddleStringMaxOffset = guiStyleText.CalcSize(guiContentText).x;
            GUI.Label(new Rect(topTextLayoutRect.xMax - topMiddleStringMaxOffset * 0.5f - topTextLayoutRect.width * 0.5f, topTextLayoutRect.y - topTextLayoutRect.height * 0.5f + spaceToCurvePreview, topTextLayoutRect.width, topTextLayoutRect.height), guiContentText, guiStyleText);

            // Draw Top Right String
            guiContentText.text = topRightString;
            float topRightStringMaxOffset = guiStyleText.CalcSize(guiContentText).x;
            GUI.Label(new Rect(topTextLayoutRect.xMax - topRightStringMaxOffset - 1f, topTextLayoutRect.y - topTextLayoutRect.height * 0.5f + spaceToCurvePreview, topTextLayoutRect.width, topTextLayoutRect.height), guiContentText, guiStyleText);

        }
    }
}
#endif