// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundPickerInternals {

        // Start with one length so it is properly initialized to default values (Index 0 is used)
        public SoundPickerPart[] soundPickerPart = new SoundPickerPart[1];

        public bool isEnabled = true;
        public bool expand = true;
    }
}