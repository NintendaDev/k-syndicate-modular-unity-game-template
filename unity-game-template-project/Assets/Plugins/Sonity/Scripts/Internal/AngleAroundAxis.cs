// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class AngleAroundAxis {

        public static float oneDividedByPi = 1f / Mathf.PI;

        /// <summary>
        /// Find the projected angle off a forward around an axis.
        /// Use like this:
        /// float angle = AngleAroundAxis.Get(targetTransform.position - cameraTransform.position, cameraTransform.forward, Vector3.up);
        /// </summary>
        /// <returns> Angle in -1 to 1 </returns>
        public static float Get(Vector3 vectorTarget, Vector3 vectorForward, Vector3 vectorAxis, bool clockwise = false) {
            Vector3 right;
            if (clockwise) {
                right = Vector3.Cross(vectorForward, vectorAxis);
                vectorForward = Vector3.Cross(vectorAxis, right);
            } else {
                right = Vector3.Cross(vectorAxis, vectorForward);
                vectorForward = Vector3.Cross(right, vectorAxis);
            }
            return Mathf.Atan2(Vector3.Dot(vectorTarget, right), Vector3.Dot(vectorTarget, vectorForward)) * oneDividedByPi;
        }
    }
}