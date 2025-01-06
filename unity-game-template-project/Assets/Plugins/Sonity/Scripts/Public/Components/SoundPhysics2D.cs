// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;
using System;
using Sonity.Internal;

namespace Sonity {

    /// <summary>
    /// <see cref="SoundPhysicsBase">SoundPhysics</see> is a component used for easily playing <see cref="SoundEventBase">SoundEvents</see> on physics interactions.
    /// <see cref="SoundPhysicsBase">SoundPhysics</see> is split up into 3D/2D versions with and without friction because of performance reasons.
    /// If friction sounds aren’t needed and performance is a priority then use the <see cref="SoundPhysicsBase">SoundPhysics</see> with no friction.
    /// A <see cref="Rigidbody"></see>/<see cref="Rigidbody2D"></see> is required on this object.
    /// One or several <see cref="Collider"></see>/<see cref="Collider2D"></see> should be placed on this object or its children.
    /// Use intensity record in the <see cref="SoundContainerBase">SoundContainer</see> for easy scaling of the velocity into a 0 to 1 range.
    /// All <see cref="SoundPhysicsBase">SoundPhysics</see> components are multi-object editable.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Sonity 🔊/Sonity - Sound Physics 2D")]
    public class SoundPhysics2D : SoundPhysicsBase {

        public SoundPhysics2D() {
            internals.physicsDimension = PhysicsDimension._2D;
            internals.soundPhysicsComponentType = SoundPhysicsComponentType.SoundPhysics;
        }

        // Collision & Trigger 2D
        private void OnCollisionEnter2D(Collision2D collision) {
            internals.OnCollisionEnter2D(collision);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            internals.OnCollisionExit2D(collision);
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            internals.OnTriggerEnter2D(collider);
        }

        private void OnTriggerExit2D(Collider2D collider) {
            internals.OnTriggerExit2D(collider);
        }

        // Friction 2D
        private void FixedUpdate() {
            internals.FixedUpdate();
        }

        private void OnCollisionStay2D(Collision2D collision) {
            internals.OnCollisionStay2D(collision);
        }

        private void OnTriggerStay2D(Collider2D collider) {
            internals.OnTriggerStay2D(collider);
        }
    }
}