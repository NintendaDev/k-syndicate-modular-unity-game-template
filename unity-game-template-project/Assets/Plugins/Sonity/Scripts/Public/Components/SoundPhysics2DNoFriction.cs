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
    [AddComponentMenu("Sonity 🔊/Sonity - Sound Physics 2D No Friction")]
    public class SoundPhysics2DNoFriction : SoundPhysicsBase {

        public SoundPhysics2DNoFriction() {
            internals.physicsDimension = PhysicsDimension._2D;
            internals.soundPhysicsComponentType = SoundPhysicsComponentType.SoundPhysicsNoFriction;
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
    }
}