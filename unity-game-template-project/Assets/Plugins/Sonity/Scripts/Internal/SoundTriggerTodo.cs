// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundTriggerTodo {

        public bool onEnableUse;
        public bool onDisableUse;
        public bool onStartUse;
        public bool onDestroyUse;

        public bool onTriggerEnterUse;
        public bool onTriggerExitUse;
        public bool onTriggerEnter2DUse;
        public bool onTriggerExit2DUse;

        public bool onCollisionEnterUse;
        public bool onCollisionExitUse;
        public bool onCollisionEnter2DUse;
        public bool onCollisionExit2DUse;

        public bool onMouseEnterUse;
        public bool onMouseExitUse;
        public bool onMouseDownUse;
        public bool onMouseUpUse;

        public SoundTriggerAction onEnableAction = SoundTriggerAction.Play;
        public SoundTriggerAction onDisableAction = SoundTriggerAction.Stop;
        public SoundTriggerAction onStartAction = SoundTriggerAction.Play;
        public SoundTriggerAction onDestroyAction = SoundTriggerAction.Stop;

        public SoundTriggerAction onTriggerEnterAction = SoundTriggerAction.Play;
        public SoundTriggerAction onTriggerExitAction = SoundTriggerAction.Stop;
        public SoundTriggerAction onTriggerEnter2DAction = SoundTriggerAction.Play;
        public SoundTriggerAction onTriggerExit2DAction = SoundTriggerAction.Stop;

        public SoundTriggerAction onCollisionEnterAction = SoundTriggerAction.Play;
        public SoundTriggerAction onCollisionExitAction = SoundTriggerAction.Stop;
        public SoundTriggerAction onCollisionEnter2DAction = SoundTriggerAction.Play;
        public SoundTriggerAction onCollisionExit2DAction = SoundTriggerAction.Stop;

        public SoundTriggerAction onMouseEnterAction = SoundTriggerAction.Play;
        public SoundTriggerAction onMouseExitAction = SoundTriggerAction.Stop;
        public SoundTriggerAction onMouseDownAction = SoundTriggerAction.Play;
        public SoundTriggerAction onMouseUpAction = SoundTriggerAction.Stop;
    }
}