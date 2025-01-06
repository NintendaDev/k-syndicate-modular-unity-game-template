// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;

namespace Sonity.Internal {

    [Serializable]
    public abstract class SoundPhysicsBase : MonoBehaviour {

        public SoundPhysicsInternals internals = new SoundPhysicsInternals();

        private void Start() {
            internals.FindComponents(gameObject);
        }

        private void Awake() {
            internals.FindComponents(gameObject);
        }
    }
}