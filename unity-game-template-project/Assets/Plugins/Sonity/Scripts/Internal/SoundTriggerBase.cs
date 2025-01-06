// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundTriggerBase : MonoBehaviour {

        public SoundTriggerInternals internals = new SoundTriggerInternals();

        private void Awake() {
            internals.Initialize(gameObject);
        }

        // Basic

        // OnEnable
        protected virtual void OnEnable() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onEnableUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnBasic, i, internals.soundTriggerPart[i].soundTriggerTodo.onEnableAction);
                }
            }
        }

        // OnDisable
        protected virtual void OnDisable() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onDisableUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnBasic, i, internals.soundTriggerPart[i].soundTriggerTodo.onDisableAction);
                }
            }
        }

        // OnStart
        protected virtual void Start() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onStartUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnBasic, i, internals.soundTriggerPart[i].soundTriggerTodo.onStartAction);
                }
            }
        }

        // OnDestroy
        protected virtual void OnDestroy() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onDestroyUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnBasic, i, internals.soundTriggerPart[i].soundTriggerTodo.onDestroyAction);
                }
            }
        }

        // Trigger

        // OnTriggerEnter
        protected virtual void OnTriggerEnter(Collider other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnterUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnTrigger, i, internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnterAction, true, other.tag);
                }
            }
        }

        // OnTriggerExit
        protected virtual void OnTriggerExit(Collider other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExitUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnTrigger, i, internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExitAction, true, other.tag);
                }
            }
        }

        // OnTriggerEnter2D
        protected virtual void OnTriggerEnter2D(Collider2D other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnter2DUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnTrigger, i, internals.soundTriggerPart[i].soundTriggerTodo.onTriggerEnter2DAction, true, other.tag);
                }
            }
        }

        // OnTriggerExit2D
        protected virtual void OnTriggerExit2D(Collider2D other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExit2DUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnTrigger, i, internals.soundTriggerPart[i].soundTriggerTodo.onTriggerExit2DAction, true, other.tag);
                }
            }
        }

        // Collision

        // OnCollisionEnter
        protected virtual void OnCollisionEnter(Collision other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnterUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnCollision, i, internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnterAction, true, other.transform.tag, other, null);
                }
            }
        }

        // OnCollisionExit
        protected virtual void OnCollisionExit(Collision other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExitUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnCollision, i, internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExitAction, true, other.transform.tag, other, null);
                }
            }
        }

        // OnCollisionEnter2D
        protected virtual void OnCollisionEnter2D(Collision2D other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnter2DUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnCollision, i, internals.soundTriggerPart[i].soundTriggerTodo.onCollisionEnter2DAction, true, other.transform.tag, null, other);
                }
            }
        }

        // OnCollisionExit2D
        protected virtual void OnCollisionExit2D(Collision2D other) {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExit2DUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnCollision, i, internals.soundTriggerPart[i].soundTriggerTodo.onCollisionExit2DAction, true, other.transform.tag, null, other);
                }
            }
        }

        // Mouse

        // OnMouseEnter
        protected virtual void OnMouseEnter() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onMouseEnterUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnMouse, i, internals.soundTriggerPart[i].soundTriggerTodo.onMouseEnterAction);
                }
            }
        }

        // OnMouseExit
        protected virtual void OnMouseExit() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onMouseExitUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnMouse, i, internals.soundTriggerPart[i].soundTriggerTodo.onMouseExitAction);
                }
            }
        }

        // OnMouseDown
        protected virtual void OnMouseDown() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onMouseDownUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnMouse, i, internals.soundTriggerPart[i].soundTriggerTodo.onMouseDownAction);
                }
            }
        }

        // OnMouseUp
        protected virtual void OnMouseUp() {
            for (int i = 0; i < internals.soundTriggerPart.Length; i++) {
                if (internals.soundTriggerPart[i].soundTriggerTodo.onMouseUpUse) {
                    internals.DoTriggerAction(SoundTriggerOnType.OnMouse, i, internals.soundTriggerPart[i].soundTriggerTodo.onMouseUpAction);
                }
            }
        }
    }
}