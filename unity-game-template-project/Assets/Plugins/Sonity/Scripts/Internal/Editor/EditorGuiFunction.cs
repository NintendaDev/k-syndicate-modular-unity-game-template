// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {
    public static class EditorGuiFunction {

        private static GUIStyle guiStyleBoldCenter;

        public static GUIStyle GetGuiStyleBoldCenter() {
            if (guiStyleBoldCenter == null) {
                guiStyleBoldCenter = new GUIStyle();
                guiStyleBoldCenter.fontSize = 16;
                guiStyleBoldCenter.fontStyle = FontStyle.Bold;
                guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
                if (EditorGUIUtility.isProSkin) {
                    guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
                }
            }
            return guiStyleBoldCenter;
        }

        public static void DrawObjectNameBox(UnityEngine.Object target, string typeName, string tooltip, bool showObjectName) {
            StartBackgroundColor(Color.white);
            string titleString = $"Sonity - " + typeName;
            float height = 25;
            if (showObjectName) {
                titleString = titleString + "\n" + target.name;
                height = 40;
            }
            if (GUILayout.Button(new GUIContent(
                 titleString, tooltip), GetGuiStyleBoldCenter(), GUILayout.ExpandWidth(true), GUILayout.Height(height)
#if !UNITY_2019_1_OR_NEWER
                // Unity 2018 or older has wrong offset
                , GUILayout.Width(EditorGUIUtility.currentViewWidth - 55)
#endif
                )) {
                EditorGUIUtility.PingObject(target);
            }
            StopBackgroundColor();
        }

        public enum EditorGuiTypeIs {
            Property,
            Tag,
            Layer,
        }

        public static void BeginChange(SerializedObject serializedObject) {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        public static void EndChange(SerializedObject serializedObject) {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void DrawReordableArray(
            SerializedProperty arrayProperty, SerializedObject serializedObject, int lowestArrayLength, bool fieldOffsetRight,
            bool setNull = true, bool lengthCanBeZero = true, EditorGuiTypeIs typeIs = EditorGuiTypeIs.Property, int offsetSize = 0, SerializedProperty arrayPropertySecond = null,
            bool showSize = true, string stringLabelOnFirstIndexWhenOffset = "", bool replaceSizeTextWithLabel = false
            ) {

            int oldIntentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUILayout.BeginHorizontal();
            if (fieldOffsetRight) {
                EditorGUI.indentLevel = oldIntentLevel;
                if (showSize) {
                    // For offsetting the buttons to the right
                    if (replaceSizeTextWithLabel) {
                        EditorGUILayout.LabelField(new GUIContent(stringLabelOnFirstIndexWhenOffset), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                    } else {
                        EditorGUILayout.LabelField(new GUIContent("Size"), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                    }
                    EditorGUI.indentLevel = 0;
                    BeginChange(serializedObject);
                    if (lengthCanBeZero) {
                        arrayProperty.arraySize = EditorGUILayout.IntField(arrayProperty.arraySize);
                    } else {
                        arrayProperty.arraySize = Mathf.Clamp(EditorGUILayout.IntField(arrayProperty.arraySize), 1, int.MaxValue);
                    }
                    EndChange(serializedObject);
                }
            } else {
                EditorGUI.indentLevel = oldIntentLevel;
                if (showSize) {
                    BeginChange(serializedObject);

                    if (lengthCanBeZero) {
                        arrayProperty.arraySize = EditorGUILayout.IntField(arrayProperty.arraySize
#if UNITY_2019_1_OR_NEWER
                            , GUILayout.Width(EditorGUIUtility.currentViewWidth - 136)
#endif
                            );
                    } else {
                        arrayProperty.arraySize = Mathf.Clamp(EditorGUILayout.IntField(arrayProperty.arraySize
#if UNITY_2019_1_OR_NEWER
                            , GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
#endif
                            ), 1, int.MaxValue);
                    }

                    EndChange(serializedObject);
                    EditorGUI.indentLevel = 0;
                    // For offsetting the field to the left
                    EditorGUILayout.LabelField(new GUIContent("Size"), GUILayout.Width(EditorGUIUtility.labelWidth));
                }
            }
            EditorGUILayout.EndHorizontal();

            int buttonWidth = 19;

            for (int i = 0; i < arrayProperty.arraySize; i++) {
                // Out of bounds check
                if (i >= lowestArrayLength) {
                    break;
                }

                SerializedProperty arrayElement = arrayProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                if (fieldOffsetRight) {
                    // For offsetting the buttons to the right
                    if (i == 0) {
                        if (!replaceSizeTextWithLabel) {
                            EditorGUILayout.LabelField(new GUIContent(stringLabelOnFirstIndexWhenOffset), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                        } else {
                            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                        }
                    } else {
                        EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth - offsetSize));
                    }
                }

                BeginChange(serializedObject);

                if (typeIs == EditorGuiTypeIs.Tag) {
                    if (fieldOffsetRight) {
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue);
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        arrayElement.stringValue = EditorGUILayout.TagField(arrayElement.stringValue
#if UNITY_2019_1_OR_NEWER
                            , GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
#endif
                            );
                        EditorGUI.indentLevel = 0;
                    }
                } else if (typeIs == EditorGuiTypeIs.Layer) {
                    if (fieldOffsetRight) {
                        arrayElement.intValue = EditorGUILayout.LayerField(arrayElement.intValue);
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        arrayElement.intValue = EditorGUILayout.LayerField(arrayElement.intValue
#if UNITY_2019_1_OR_NEWER
                            , GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
#endif
                            );
                        EditorGUI.indentLevel = 0;
                    }
                } else if (typeIs == EditorGuiTypeIs.Property) {
                    if (fieldOffsetRight) {
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", ""));
                    } else {
                        EditorGUI.indentLevel = oldIntentLevel;
                        EditorGUILayout.PropertyField(arrayElement, new GUIContent($"", "")
#if UNITY_2019_1_OR_NEWER
                            , GUILayout.Width(EditorGUIUtility.currentViewWidth - 136)
#endif
                            );
                        EditorGUI.indentLevel = 0;
                    }
                }

                EndChange(serializedObject);

                BeginChange(serializedObject);
                if (GUILayout.Button("+", GUILayout.Width(buttonWidth))) {
                    arrayProperty.InsertArrayElementAtIndex(i);
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.InsertArrayElementAtIndex(i);
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("-", GUILayout.Width(buttonWidth))) {
                    // Set to null so it will delete instantly
                    if (setNull) {
                        arrayElement.objectReferenceValue = null;
                    }
                    if (lengthCanBeZero) {
                        if (arrayProperty.arraySize > 0) {
                            arrayProperty.DeleteArrayElementAtIndex(i);
                            // Used by timeline data
                            if (arrayPropertySecond != null) {
                                arrayPropertySecond.DeleteArrayElementAtIndex(i);
                            }
                        }
                    } else {
                        if (arrayProperty.arraySize > 1) {
                            arrayProperty.DeleteArrayElementAtIndex(i);
                            // Used by timeline data
                            if (arrayPropertySecond != null) {
                                arrayPropertySecond.DeleteArrayElementAtIndex(i);
                            }
                        }
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("↑", GUILayout.Width(buttonWidth))) {
                    arrayProperty.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, arrayProperty.arraySize));
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.MoveArrayElement(i, Mathf.Clamp(i - 1, 0, arrayProperty.arraySize));
                    }
                }
                EndChange(serializedObject);
                BeginChange(serializedObject);
                if (GUILayout.Button("↓", GUILayout.Width(buttonWidth))) {
                    arrayProperty.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, arrayProperty.arraySize));
                    // Used by timeline data
                    if (arrayPropertySecond != null) {
                        arrayPropertySecond.MoveArrayElement(i, Mathf.Clamp(i + 1, 0, arrayProperty.arraySize));
                    }
                }
                EndChange(serializedObject);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel = oldIntentLevel;
        }

        public static void DrawFoldout(SerializedProperty expanded, string label, string tooltip = "", int indentLevel = 0, bool expandedBool = false, bool smallFont = false, bool mediumFont = false) {

            GUIStyle guiStyle = new GUIStyle(EditorStyles.foldout);

            if (mediumFont) {
                guiStyle.fontSize = 14;
            } else {
                if (!smallFont) {
                    guiStyle.fontSize = 16;
                }
            }
            guiStyle.fontStyle = FontStyle.Bold;

            if (smallFont) {
                guiStyle.margin = new RectOffset(indentLevel * 22, 0, 3, 5);
            } else {
                guiStyle.margin = new RectOffset(indentLevel * 20, 0, 0, 3);
            }

            // Draw Toggle
            if (expandedBool) {
                expanded.isExpanded = GUILayout.Toggle(expanded.isExpanded, new GUIContent(label, tooltip), guiStyle);
            } else {
                expanded.boolValue = GUILayout.Toggle(expanded.boolValue, new GUIContent(label, tooltip), guiStyle);
            }
        }

        public static void DrawFoldoutTitle(string label, string tooltip = "", int indentLevel = 0, bool smallFont = false) {

            GUIStyle guiStyle = new GUIStyle();

            if (EditorGUIUtility.isProSkin) {
                guiStyle.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            if (!smallFont) {
                guiStyle.fontSize = 16;
            }
            guiStyle.fontStyle = FontStyle.Bold;

            if (smallFont) {
                guiStyle.margin = new RectOffset(indentLevel * 22, 0, 3, 5);
            } else {
                guiStyle.margin = new RectOffset(indentLevel * 20, 0, 0, 3);
            }

            GUILayout.Label(new GUIContent(label, tooltip), guiStyle);
        }

        public static void DrawScriptableObjectTitle(string label) {

            GUIStyle guiStyle = new GUIStyle();

            if (EditorGUIUtility.isProSkin) {
                guiStyle.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            guiStyle.fontSize = 16;
            guiStyle.fontStyle = FontStyle.Bold;

            guiStyle.margin = new RectOffset(0, 0, 0, 3);

            GUILayout.Label(new GUIContent(label), guiStyle);
        }

        private static void StartBackgroundColor(Color color) {
            Color defaultGuiColor = GUI.color;
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private static void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public static void DrawLabel(
            string title, 
            string tooltip = "", 
            bool drawBackground = true, 
            bool center = true, bool 
            smallFont = false, 
            bool useSmallerLabel = false, 
            bool skipGuiStyle = false, 
            bool useIndentLevel = false,
            float smallOffsetToAdd = 0f
            ) {

            GUIStyle guiStyle = new GUIStyle();

            guiStyle.fontStyle = FontStyle.Bold;
            if (center) {
                guiStyle.alignment = TextAnchor.MiddleCenter;
            }

            if (!smallFont) {
                guiStyle.fontSize = 16;
            }
            
            if (EditorGUIUtility.isProSkin) {
                guiStyle.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            if (drawBackground) {
                StartBackgroundColor(Color.white);
            }
            if (useIndentLevel || smallOffsetToAdd > 0) {
                GUILayout.BeginHorizontal();
                float offset = smallOffsetToAdd;
                if (useIndentLevel) {
                    offset += EditorGUI.indentLevel * 15f;
                }
                GUILayout.Space(offset);
            }
            if (useSmallerLabel) {
                if (skipGuiStyle) {
                    GUILayout.Label(new GUIContent(title, tooltip));
                } else {
                    GUILayout.Label(new GUIContent(title, tooltip), guiStyle);
                }
            } else {
                GUILayout.Box(new GUIContent(title, tooltip), guiStyle, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            }
            if (useIndentLevel || smallOffsetToAdd > 0) {
                GUILayout.EndHorizontal();
            }
            if (drawBackground) {
                StopBackgroundColor();
            }

        }
    }
}
#endif