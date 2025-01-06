// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace Sonity.Internal {

    public static class ApplicationQuitting {

        private static bool applicationQuitting = false;

        public static bool GetApplicationIsQuitting() {
            return applicationQuitting;
        }

        static void SetQuitting() {
            applicationQuitting = true;
        }

        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad() {
            Application.quitting += SetQuitting;
        }
    }
}