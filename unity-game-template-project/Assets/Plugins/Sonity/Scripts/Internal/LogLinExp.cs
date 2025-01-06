// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class LogLinExp {

        public static float bipolarRange = 16f;

        /// <summary>
        /// Makes Linear range (0 to 1) into exponential or logarithmic
        /// The curve which should be applied (less than 0 is logarithmic, 0 is linear, more than 0 is exponential)
        /// </summary>
        public static float Get(float linear, float curve) {
            // Exponential
            if (curve > 0f) {
                return Mathf.Pow(linear, curve + 1f);
            }
            // Logarithmic
            else if (curve < 0f) {
                return -Mathf.Pow(-linear + 1, -curve + 1f) + 1;
            }
            // Linear
            return linear;
        }
    }
}