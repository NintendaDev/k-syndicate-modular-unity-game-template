// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundTagInternals {

#if UNITY_EDITOR
        public string notes = "Notes";
#endif
    }
}