// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public class SoundContainerEditorCurveDraw : Editor {

        private SoundContainerEditorBase parent;

        public void Initialize(SoundContainerEditorBase parent) {
            this.parent = parent;
        }

        public void Draw(EditorSoundContainerCurveType curveType, EditorSoundContainerCurveValue curveValue) {

            if (!parent.previewCurves.boolValue) {
                return;
            }

            bool isDistance = false;
            bool isIntensity = false;
            bool isAngle = false;
            bool isBasic = false;

            if (curveValue == EditorSoundContainerCurveValue.Distance) {
                isDistance = true;
            } else if (curveValue == EditorSoundContainerCurveValue.Intensity) {
                isIntensity = true;
            } else if (curveValue == EditorSoundContainerCurveValue.Angle) {
                isAngle = true;
            } else if (curveValue == EditorSoundContainerCurveValue.Basic) {
                isBasic = true;
            }

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            Rect layoutRectangle;
            if (isBasic) {
                // Basic smaller rect
                layoutRectangle = GUILayoutUtility.GetRect(10, 10000, 50, 50);
            } else {
                layoutRectangle = GUILayoutUtility.GetRect(10, 10000, 100, 100);
            }

            if (Event.current.type == EventType.Repaint) {
                GUI.BeginClip(layoutRectangle);
                GL.PushMatrix();

                Color backgroundColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);

                parent.cachedMaterial.SetPass(0);

                GL.Begin(GL.QUADS);
                GL.Color(backgroundColor);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(layoutRectangle.width, 0, 0);
                GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
                GL.Vertex3(0, layoutRectangle.height, 0);
                GL.End();

                Color maxColor = new Color();
                Color minColor = new Color();
                float alpha = 0.5f;
                if (curveType == EditorSoundContainerCurveType.Volume) {
                    maxColor = EditorColor.GetVolumeMax(alpha);
                    minColor = EditorColor.GetVolumeMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.Pitch) {
                    maxColor = EditorColor.GetPitchMax(alpha);
                    minColor = EditorColor.GetPitchMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.StereoPan) {
                    if (EditorGUIUtility.isProSkin) {
                        alpha = 0.75f;
                        maxColor = EditorColor.GetStereoPanMax(alpha);
                        minColor = EditorColor.GetStereoPanMin(alpha);
                    } else {
                        alpha = 1f;
                        maxColor = EditorColor.ChangeValue(EditorColor.GetStereoPanMax(alpha), -0.2f);
                        minColor = EditorColor.ChangeValue(EditorColor.GetStereoPanMin(alpha), -0.2f);
                    }
                } else if (curveType == EditorSoundContainerCurveType.ReverbZoneMix) {
                    maxColor = EditorColor.GetReverbZoneMixColorMax(alpha);
                    minColor = EditorColor.GetReverbZoneMixColorMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.SpatialBlend) {
                    maxColor = EditorColor.GetSpatialBlendMax(alpha);
                    minColor = EditorColor.GetSpatialBlendMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.SpatialSpread) {
                    maxColor = EditorColor.GetSpatialSpreadMax(alpha);
                    minColor = EditorColor.GetSpatialSpreadMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.Distortion) {
                    maxColor = EditorColor.GetDistortionMax(alpha);
                    minColor = EditorColor.GetDistortionMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.LowpassFrequency) {
                    maxColor = EditorColor.GetLowpassFrequencyMax(alpha);
                    minColor = EditorColor.GetLowpassFrequencyMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.LowpassAmount) {
                    maxColor = EditorColor.GetLowpassAmountMax(alpha);
                    minColor = EditorColor.GetLowpassAmountMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.HighpassFrequency) {
                    maxColor = EditorColor.GetHighpassFrequencyMax(alpha);
                    minColor = EditorColor.GetHighpassFrequencyMin(alpha);
                } else if (curveType == EditorSoundContainerCurveType.HighpassAmount) {
                    maxColor = EditorColor.GetHighpassAmountMax(alpha);
                    minColor = EditorColor.GetHighpassAmountMin(alpha);
                }

                int curveResolution = 100;

                // Avoid divide by zero
                if (curveResolution <= 0) {
                    return;
                }

                SoundEventModifier tempSoundEventModifier = new SoundEventModifier();

                for (int i = 0; i < curveResolution; i++) {
                    float amount1 = (float)i / curveResolution;
                    float amount2 = ((float)i + 1) / curveResolution;
                    float value1 = 0f;
                    float value2 = 0f;

                    if (curveType == EditorSoundContainerCurveType.Volume) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, 0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, 0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetVolume(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                        }

                        // Volume increase scale back so max ratio is 0
                        value1 *= VolumeScale.volumeIncrease24dbMaxToOneRatio;
                        value2 *= VolumeScale.volumeIncrease24dbMaxToOneRatio;
                        // Make so volume is full scale all the time
                        if (parent.mTarget.internals.data.GetVolumeRatioClamped() != 0f) {
                            value1 = value1 / (parent.mTarget.internals.data.GetVolumeRatioClamped() / VolumeScale.volumeIncrease24dbMaxRatio);
                            value2 = value2 / (parent.mTarget.internals.data.GetVolumeRatioClamped() / VolumeScale.volumeIncrease24dbMaxRatio);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.Pitch) {
                        if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetPitchRatioIntensity(amount1);
                            value2 = parent.mTarget.internals.data.GetPitchRatioIntensity(amount2);
                            value1 = PitchScale.RatioToSemitones(value1);
                            value2 = PitchScale.RatioToSemitones(value2);
                            value1 = value1 - parent.mTarget.internals.data.pitchIntensityBaseSemitone;
                            value2 = value2 - parent.mTarget.internals.data.pitchIntensityBaseSemitone;
                            if (parent.mTarget.internals.data.pitchIntensityRangeSemitone != 0f) {
                                value1 = value1 / parent.mTarget.internals.data.pitchIntensityRangeSemitone;
                                value2 = value2 / parent.mTarget.internals.data.pitchIntensityRangeSemitone;
                            }
                        }
                    } else if (curveType == EditorSoundContainerCurveType.StereoPan) {
                        // Scaling the 0 to 1 input to -1 to 1
                        value1 = parent.mTarget.internals.data.GetStereoPan(tempSoundEventModifier, (amount1 - 0.5f) * 2f);
                        value2 = parent.mTarget.internals.data.GetStereoPan(tempSoundEventModifier, (amount2 - 0.5f) * 2f);
                        // Scaling the -1 to 1 output to 0 to 1
                        value1 = (value1 + 1f) * 0.5f;
                        value2 = (value2 + 1f) * 0.5f;
                    } else if (curveType == EditorSoundContainerCurveType.SpatialBlend) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, 0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, 0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialBlend(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.SpatialSpread) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetSpatialSpread(amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialSpread(amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetSpatialSpread(0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialSpread(0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetSpatialSpread(0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetSpatialSpread(0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.ReverbZoneMix) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, 0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, 0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetReverbZoneMixRatio(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                        }
                        // Scaling from 3.1622776601683795 to 1
                        value1 = Mathf.Clamp(value1 / 3.1622776601683795f, 0f, 1f);
                        value2 = Mathf.Clamp(value2 / 3.1622776601683795f, 0f, 1f);
                    } else if (curveType == EditorSoundContainerCurveType.Distortion) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, 0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, 0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetDistortion(tempSoundEventModifier, 0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.LowpassFrequency) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetLowpassFrequency(amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassFrequency(amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetLowpassFrequency(0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassFrequency(0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetLowpassFrequency(0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassFrequency(0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.LowpassAmount) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetLowpassAmount(amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassAmount(amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetLowpassAmount(0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassAmount(0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetLowpassAmount(0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetLowpassAmount(0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.HighpassFrequency) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetHighpassFrequency(amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassFrequency(amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetHighpassFrequency(0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassFrequency(0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetHighpassFrequency(0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassFrequency(0f, 1f, isDistance, isIntensity);
                        }
                    } else if (curveType == EditorSoundContainerCurveType.HighpassAmount) {
                        if (isDistance) {
                            value1 = parent.mTarget.internals.data.GetHighpassAmount(amount1, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassAmount(amount2, 1f, isDistance, isIntensity);
                        } else if (isIntensity) {
                            value1 = parent.mTarget.internals.data.GetHighpassAmount(0f, amount1, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassAmount(0f, amount2, isDistance, isIntensity);
                        } else if (isBasic) {
                            value1 = parent.mTarget.internals.data.GetHighpassAmount(0f, 1f, isDistance, isIntensity);
                            value2 = parent.mTarget.internals.data.GetHighpassAmount(0f, 1f, isDistance, isIntensity);
                        }
                    }

                    value1 = Mathf.Clamp(value1, 0f, 1f);
                    value2 = Mathf.Clamp(value2, 0f, 1f);

                    // Stereo Pan is rotated
                    if (curveType == EditorSoundContainerCurveType.StereoPan) {

                        // StereoPan Middle
                        GL.Begin(GL.LINES);
                        // Left up
                        GL.Color(Color.Lerp(minColor, maxColor, value2));
                        GL.Vertex(new Vector3(value2 * layoutRectangle.width, amount2 * layoutRectangle.height, 0f));
                        // Left down
                        GL.Color(Color.Lerp(minColor, maxColor, value1));
                        GL.Vertex(new Vector3(value1 * layoutRectangle.width, amount1 * layoutRectangle.height, 0f));
                        GL.End();
                    } else {

                        GL.Begin(GL.TRIANGLES);

                        // Vertex List (Right Side Up)
                        GL.Color(Color.Lerp(minColor, maxColor, value1));
                        GL.Vertex(new Vector3(amount1 * layoutRectangle.width, -value1 * layoutRectangle.height + layoutRectangle.height, 0f));

                        GL.Color(Color.Lerp(minColor, maxColor, value2));
                        GL.Vertex(new Vector3(amount2 * layoutRectangle.width, -value2 * layoutRectangle.height + layoutRectangle.height, 0f));

                        GL.Color(Color.Lerp(minColor, maxColor, value1));
                        GL.Vertex(new Vector3(amount1 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));

                        // Vertex List (Right Side Down)
                        GL.Color(Color.Lerp(minColor, maxColor, value2));
                        GL.Vertex(new Vector3(amount2 * layoutRectangle.width, -value2 * layoutRectangle.height + layoutRectangle.height, 0f));

                        GL.Color(Color.Lerp(minColor, maxColor, value2));
                        GL.Vertex(new Vector3(amount2 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));

                        GL.Color(Color.Lerp(minColor, maxColor, value1));
                        GL.Vertex(new Vector3(amount1 * layoutRectangle.width, 0f + layoutRectangle.height, 0f));

                        GL.End();
                    }
                }

                // Used to draw the preview dots on the SoundContainer Curves
                if (EditorPreviewSound.GetCurvePreviewValues().use) {

                    GL.Begin(GL.TRIANGLES);

                    float tempValue = 0f;
                    if (isDistance) {
                        // Range 0 to 1
                        tempValue = EditorPreviewSound.GetCurvePreviewValues().distance;
                    } else if (isIntensity) {
                        // Range 0 to 1
                        tempValue = EditorPreviewSound.GetCurvePreviewValues().intensity;
                    } else if (isAngle) {
                        // Range -1 to +1
                        // Re-scaled to 0 to 1
                        tempValue = (parent.mTarget.internals.data.GetStereoPan(tempSoundEventModifier, EditorPreviewSound.GetCurvePreviewValues().angleToCenter) + 1f) * 0.5f;
                        tempValue = -tempValue + 1f;
                    }

                    tempValue = Mathf.Clamp(tempValue, 0f, 1f);

                    float dotWidth = 5f;

                    // Vertex List (Clockwise)
                    GL.Color(EditorColor.GetVolumeMax(1f));

                    // Left up
                    GL.Vertex(new Vector3(Mathf.Clamp(tempValue * layoutRectangle.width - dotWidth, 0f, layoutRectangle.width), tempValue, 0f));
                    // Right up
                    GL.Vertex(new Vector3(Mathf.Clamp(tempValue * layoutRectangle.width + dotWidth, 0f, layoutRectangle.width), tempValue, 0f));
                    // Down
                    GL.Vertex(new Vector3(tempValue * layoutRectangle.width, tempValue + dotWidth, 0f));

                    GL.End();
                }

                GL.PopMatrix();
                GUI.EndClip();

                string labelLeftTop = "";
                string labelLeftMiddle = "";
                string labelLeftBottom = "";

                if (curveType == EditorSoundContainerCurveType.Volume) {
                    if (parent.mTarget.internals.data.GetVolumeRatioClamped() <= 0f) {
                        labelLeftTop = "-inf dB";
                    } else {
                        labelLeftTop = Mathf.Round(parent.mTarget.internals.data.volumeDecibel) + " dB";
                    }
                    labelLeftBottom = "-inf dB";
                } else if (curveType == EditorSoundContainerCurveType.Pitch) {
                    float tempPitchTop = Mathf.Round((parent.mTarget.internals.data.pitchIntensityBaseSemitone + parent.mTarget.internals.data.pitchIntensityRangeSemitone) * 10f) * 0.1f;
                    if (tempPitchTop > 0f) {
                        labelLeftTop = "+" + tempPitchTop + " Semitones";
                    } else {
                        labelLeftTop = tempPitchTop + " Semitones";
                    }
                    float tempPitchBottom = Mathf.Round(parent.mTarget.internals.data.pitchIntensityBaseSemitone * 10f) * 0.1f;
                    if (tempPitchBottom > 0f) {
                        labelLeftBottom = "+" + tempPitchBottom + " Semitones";
                    } else {
                        labelLeftBottom = tempPitchBottom + " Semitones";
                    }
                } else if (curveType == EditorSoundContainerCurveType.StereoPan) {
                    labelLeftTop = "-180°";
                    labelLeftMiddle = "0°";
                    labelLeftBottom = "180°";
                } else if (curveType == EditorSoundContainerCurveType.ReverbZoneMix) {
                    if (isDistance) {
                        labelLeftTop = "+10 dB";
                        labelLeftBottom = "-inf dB";
                    } else if (isIntensity) {
                        labelLeftTop = "+10 dB";
                        labelLeftBottom = "-inf dB";
                    }
                } else if (curveType == EditorSoundContainerCurveType.SpatialBlend) {
                    labelLeftTop = "3D";
                    labelLeftBottom = "2D";
                } else if (curveType == EditorSoundContainerCurveType.SpatialSpread) {
                    if (isDistance) {
                        labelLeftTop = "360°";
                        labelLeftMiddle = "180°";
                        labelLeftBottom = "0°";
                    } else if (isIntensity) {
                        labelLeftTop = "360°";
                        labelLeftMiddle = "180°";
                        labelLeftBottom = "0°";
                    }
                } else if (curveType == EditorSoundContainerCurveType.Distortion) {
                    if (isDistance) {
                        labelLeftTop = "1 Distorted";
                        labelLeftBottom = "0 Clean";
                    } else if (isIntensity) {
                        labelLeftTop = "1 Distorted";
                        labelLeftBottom = "0 Clean";
                    }
                } else if (curveType == EditorSoundContainerCurveType.LowpassFrequency) {
                    if (isDistance) {
                        labelLeftTop = "20,000Hz";
                        labelLeftBottom = "20Hz";
                    } else if (isIntensity) {
                        labelLeftTop = "20,000Hz";
                        labelLeftBottom = "20Hz";
                    }
                } else if (curveType == EditorSoundContainerCurveType.LowpassAmount) {
                    labelLeftTop = "6dB/oct";
                    labelLeftBottom = "0dB/oct";
                } else if (curveType == EditorSoundContainerCurveType.HighpassFrequency) {
                    if (isDistance) {
                        labelLeftTop = "20,000Hz";
                        labelLeftBottom = "20Hz";
                    } else if (isIntensity) {
                        labelLeftTop = "20,000Hz";
                        labelLeftBottom = "20Hz";
                    }
                } else if (curveType == EditorSoundContainerCurveType.HighpassAmount) {
                    labelLeftTop = "6dB/oct";
                    labelLeftBottom = "0dB/oct";
                }

#if UNITY_2019_1_OR_NEWER
                // Draw Label Left Top
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y - layoutRectangle.height * 0.5f + 7f, layoutRectangle.width, layoutRectangle.height), labelLeftTop);

                // Draw Label Left Middle
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y, layoutRectangle.width, layoutRectangle.height), labelLeftMiddle);

                // Draw Label Left Bottom
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), labelLeftBottom);
#else
                // Code for older because text offset is wrong
                // Draw Label Left Top
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y - layoutRectangle.height * 0.5f + 7f + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), labelLeftTop);

                // Draw Label Left Middle
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y, layoutRectangle.width, layoutRectangle.height), labelLeftMiddle);

                // Draw Label Left Bottom
                GUI.Label(new Rect(layoutRectangle.x - 1f, layoutRectangle.y + layoutRectangle.height * 0.5f - 7f + layoutRectangle.height * 0.5f - 7f, layoutRectangle.width, layoutRectangle.height), labelLeftBottom);
#endif
            }

            GUILayout.EndHorizontal();

            string topLeftString = "";
            string topMiddleString = "";
            string topRightString = "";
            if (isDistance) {
                topLeftString = "0 Units Away";
                if (SoundManagerBase.Instance == null) {
                    topRightString = (parent.distanceScale.floatValue + " Units Away");
                } else {
                    topRightString = (parent.distanceScale.floatValue * SoundManagerBase.Instance.Internals.settings.distanceScale) + " Units Away";
                }
            } else if (isIntensity) {
                topLeftString = "0 Intensity";
                topRightString = "1 Intensity";
            } else if (isAngle) {
                topLeftString = "Left";
                topMiddleString = "Center";
                topRightString = "Right";
            }

            // Text below
            GUIStyle guiStyleText = new GUIStyle();
            guiStyleText.alignment = TextAnchor.MiddleLeft;
            if (EditorGUIUtility.isProSkin) {
                guiStyleText.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }
            GUIContent guiContentText = new GUIContent();

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