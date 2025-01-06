// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundManagerInternalsEditorTools {

        // Editor Tools
        public bool editorToolExpand = true;
        public bool editorToolSelectionHistoryEnable = false;
        public bool editorToolReferenceFinderEnable = false;
        public bool editorToolSelectSameTypeEnable = false;
    }
}
#endif