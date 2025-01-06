// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundParameterInternals {

        public SoundParameterInternalsData internals = new SoundParameterInternalsData();

        [Serializable]
        public class SoundParameterInternalsData {

            public SoundParameterType type;
            public UpdateMode updateMode = UpdateMode.Once;

            public float valueFloat;
            public int valueInt;
            public bool valueBool;
        }
    }
}