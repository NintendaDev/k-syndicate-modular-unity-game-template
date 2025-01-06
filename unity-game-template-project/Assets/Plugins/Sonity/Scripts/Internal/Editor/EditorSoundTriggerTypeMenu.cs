// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Sonity.Internal {

    public static class EditorSoundTriggerTypeMenu {

        private class MenuSetting {
            public UnityEngine.Object[] mTargets;
            public SoundTriggerType triggerType;
            public SoundTriggerTodo[] soundTriggerTypeEnabled;

            public MenuSetting(
                SoundTriggerType triggerType, SoundTriggerTodo[] soundTriggerTypeEnabled, UnityEngine.Object[] mTargets
                ) {
                this.triggerType = triggerType;
                this.soundTriggerTypeEnabled = soundTriggerTypeEnabled;
                this.mTargets = mTargets;
            }
        }

        public static bool PlayerTriggerTypeAnyEnabled(SerializedProperty triggerTypeProperty) {
            for (int i = 0; i < System.Enum.GetValues(typeof(SoundTriggerType)).Length; i++) {
                string propertyName = ((SoundTriggerType)i).ToString() + "Use";
                // Make enum first character lowercase to match variable name
                propertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                SerializedProperty enabledProperty = triggerTypeProperty.FindPropertyRelative(propertyName);
                if (enabledProperty.boolValue) {
                    return true;
                }
            }
            return false;
        }

        public static void TriggerTypeDisableAll(SerializedProperty triggerTypeProperty) {
            for (int i = 0; i < System.Enum.GetValues(typeof(SoundTriggerType)).Length; i++) {
                TriggerTypeEnable(triggerTypeProperty, (SoundTriggerType)i, false);
            }
        }

        private static void TriggerTypeEnable(SerializedProperty triggerTypeProperty, SoundTriggerType triggerType, bool enabled) {
            string propertyName = triggerType.ToString() + "Use";
            // Make enum first character lowercase to match variable name
            propertyName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            SerializedProperty enabledProperty = triggerTypeProperty.FindPropertyRelative(propertyName);
            enabledProperty.boolValue = enabled;
        }

        public static void TriggerTypeReset(SoundTriggerTodo[] playerTriggers) {
            for (int i = 0; i < playerTriggers.Length; i++) {
                playerTriggers[i].onEnableAction = SoundTriggerAction.Play;
                playerTriggers[i].onDisableAction = SoundTriggerAction.Stop;
                playerTriggers[i].onStartAction = SoundTriggerAction.Play;
                playerTriggers[i].onDestroyAction = SoundTriggerAction.Stop;

                playerTriggers[i].onTriggerEnterAction = SoundTriggerAction.Play;
                playerTriggers[i].onTriggerExitAction = SoundTriggerAction.Stop;
                playerTriggers[i].onTriggerEnter2DAction = SoundTriggerAction.Play;
                playerTriggers[i].onTriggerExit2DAction = SoundTriggerAction.Stop;

                playerTriggers[i].onCollisionEnterAction = SoundTriggerAction.Play;
                playerTriggers[i].onCollisionExitAction = SoundTriggerAction.Stop;
                playerTriggers[i].onCollisionEnter2DAction = SoundTriggerAction.Play;
                playerTriggers[i].onCollisionExit2DAction = SoundTriggerAction.Stop;

                playerTriggers[i].onMouseEnterAction = SoundTriggerAction.Play;
                playerTriggers[i].onMouseExitAction = SoundTriggerAction.Stop;
                playerTriggers[i].onMouseDownAction = SoundTriggerAction.Play;
                playerTriggers[i].onMouseUpAction = SoundTriggerAction.Stop;
            }
        }

        public static void TriggerTypeMenuShow(SoundTriggerTodo[] playerTriggers, UnityEngine.Object[] mTargets) {

            if (playerTriggers[0] == null) {
                return;
            }

            GenericMenu menu = new GenericMenu();
            
            // Tooltips dont work for menu
            menu.AddItem(new GUIContent("On Enable"), playerTriggers[0].onEnableUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnEnable, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Disable"), playerTriggers[0].onDisableUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnDisable, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Start"), playerTriggers[0].onStartUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnStart, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Destroy"), playerTriggers[0].onDestroyUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnDestroy, playerTriggers, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("On Trigger Enter"), playerTriggers[0].onTriggerEnterUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnTriggerEnter, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Trigger Exit"), playerTriggers[0].onTriggerExitUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnTriggerExit, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Trigger Enter 2D"), playerTriggers[0].onTriggerEnter2DUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnTriggerEnter2D, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Trigger Exit 2D"), playerTriggers[0].onTriggerExit2DUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnTriggerExit2D, playerTriggers, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("On Collision Enter"), playerTriggers[0].onCollisionEnterUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnCollisionEnter, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Collision Exit"), playerTriggers[0].onCollisionExitUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnCollisionExit, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Collision Enter 2D"), playerTriggers[0].onCollisionEnter2DUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnCollisionEnter2D, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Collision Exit 2D"), playerTriggers[0].onCollisionExit2DUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnCollisionExit2D, playerTriggers, mTargets));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("On Mouse Enter"), playerTriggers[0].onMouseEnterUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnMouseEnter, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Mouse Exit"), playerTriggers[0].onMouseExitUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnMouseExit, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Mouse Down"), playerTriggers[0].onMouseDownUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnMouseDown, playerTriggers, mTargets));
            menu.AddItem(new GUIContent("On Mouse Up"), playerTriggers[0].onMouseUpUse, TrigggerMenuCallback, new MenuSetting(SoundTriggerType.OnMouseUp, playerTriggers, mTargets));

            menu.ShowAsContext();
        }

        private static void TrigggerMenuCallback(object obj) {

            MenuSetting menuSetting;

            try {
                menuSetting = (MenuSetting)obj;
            } catch {
                return;
            }

            bool tempEnable = false;
            for (int i = 0; i < menuSetting.soundTriggerTypeEnabled.Length; i++) {
                if (menuSetting.soundTriggerTypeEnabled[i] != null) {
                    Undo.RecordObject(menuSetting.mTargets[i], "Setting Toggle");
                    // OnEnable
                    if (menuSetting.triggerType == SoundTriggerType.OnEnable) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onEnableUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onEnableUse = tempEnable;
                    }
                    // OnDisable
                    else if (menuSetting.triggerType == SoundTriggerType.OnDisable) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onDisableUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onDisableUse = tempEnable;
                    }
                    // OnStart
                    if (menuSetting.triggerType == SoundTriggerType.OnStart) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onStartUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onStartUse = tempEnable;
                    }
                    // OnDestroy
                    else if (menuSetting.triggerType == SoundTriggerType.OnDestroy) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onDestroyUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onDestroyUse = tempEnable;
                    }
                    // OnTriggerEnter
                    else if (menuSetting.triggerType == SoundTriggerType.OnTriggerEnter) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onTriggerEnterUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onTriggerEnterUse = tempEnable;
                    }
                    // OnTriggerExit
                    else if (menuSetting.triggerType == SoundTriggerType.OnTriggerExit) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onTriggerExitUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onTriggerExitUse = tempEnable;
                    }
                    // OnTriggerEnter2D
                    else if (menuSetting.triggerType == SoundTriggerType.OnTriggerEnter2D) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onTriggerEnter2DUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onTriggerEnter2DUse = tempEnable;
                    }
                    // OnTriggerExit2D
                    else if (menuSetting.triggerType == SoundTriggerType.OnTriggerExit2D) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onTriggerExit2DUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onTriggerExit2DUse = tempEnable;
                    }
                    // OnCollisiOnEnter
                    else if (menuSetting.triggerType == SoundTriggerType.OnCollisionEnter) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onCollisionEnterUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onCollisionEnterUse = tempEnable;
                    }
                    // OnCollisionExit
                    else if (menuSetting.triggerType == SoundTriggerType.OnCollisionExit) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onCollisionExitUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onCollisionExitUse = tempEnable;
                    }
                    // OnCollisionEnter2D
                    else if (menuSetting.triggerType == SoundTriggerType.OnCollisionEnter2D) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onCollisionEnter2DUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onCollisionEnter2DUse = tempEnable;
                    }
                    // OnCollisionExit2D
                    else if (menuSetting.triggerType == SoundTriggerType.OnCollisionExit2D) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onCollisionExit2DUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onCollisionExit2DUse = tempEnable;
                    }
                    // OnMouseEnter
                    else if (menuSetting.triggerType == SoundTriggerType.OnMouseEnter) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onMouseEnterUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onMouseEnterUse = tempEnable;
                    }
                    // OnMouseExit
                    else if (menuSetting.triggerType == SoundTriggerType.OnMouseExit) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onMouseExitUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onMouseExitUse = tempEnable;
                    }
                    // OnMouseDown
                    else if (menuSetting.triggerType == SoundTriggerType.OnMouseDown) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onMouseDownUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onMouseDownUse = tempEnable;
                    }
                    // OnMouseUp
                    else if (menuSetting.triggerType == SoundTriggerType.OnMouseUp) {
                        if (i == 0) {
                            tempEnable = !menuSetting.soundTriggerTypeEnabled[0].onMouseUpUse;
                        }
                        menuSetting.soundTriggerTypeEnabled[i].onMouseUpUse = tempEnable;
                    }
                    EditorUtility.SetDirty(menuSetting.mTargets[i]);
                }
            }
        }
    }
}
#endif