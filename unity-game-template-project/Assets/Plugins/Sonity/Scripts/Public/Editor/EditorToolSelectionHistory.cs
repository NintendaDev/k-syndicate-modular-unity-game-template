// MIT License for Selection History Navigator
// From https://github.com/mminer/selection-history-navigator
// Copyright (c) 2018 Matthew Miner
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including withoutlimitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions: The above copyright notice and this permission notice shall
// be included in all copies or substantial portions of the Software. THE SOFTWARE IS
// PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
// OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.

// Toolbar Actions
// Tools/Sonity Tools 🛠/Selection History 📜/Back
// Tools/Sonity Tools 🛠/Selection History 📜/Forward
// Assets/Sonity Tools 🛠/Selection History 📜/Back
// Assets/Sonity Tools 🛠/Selection History 📜/Forward

// Default Shortcuts
// U - Selection History 📜/Back
// Shift+U - Selection History 📜/Forward

// Version 1.1
// Added shortcuts
// Moved toolbar from "Edit/Selection"

#if UNITY_EDITOR
#if SONITY_ENABLE_EDITOR_TOOL_SELECTION_HISTORY

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sonity.Internal.EditorToolSelectionHistory {

    /// <summary>
    /// Adds back and forward selection history actions
    /// This works for both Hierarchy and Project selections
    /// </summary>
    [InitializeOnLoad]
    static class EditorToolSelectionHistory {

        // Shortcut key modifiers
        // % -> Ctrl on Windows, Linux, CMD on MacOS
        // ^ -> Ctrl on Windows, Linux, MacOS
        // # -> Shift
        // & -> Alt

        const string toolsMenuPathBack = "Tools/Sonity Tools 🛠/Selection History 📜/Back";
        const string toolsMenuPathForward = "Tools/Sonity Tools 🛠/Selection History 📜/Forward";
        const string assetsMenuPathBack = "Assets/Sonity Tools 🛠/Selection History 📜/Back _U";
        const string assetsMenuPathForward = "Assets/Sonity Tools 🛠/Selection History 📜/Forward #U";

        const int toolsMenuPriority = 102;
        const int assetsMenuPriority = 102;

        static Object activeSelection;
        static bool ignoreNextSelectionChangedEvent;
        static readonly Stack<Object> nextSelections = new Stack<Object>();
        static readonly Stack<Object> previousSelections = new Stack<Object>();

        static EditorToolSelectionHistory() {
            Selection.selectionChanged += SelectionChangedHandler;
        }

        static void SelectionChangedHandler() {
            if (ignoreNextSelectionChangedEvent) {
                ignoreNextSelectionChangedEvent = false;
                return;
            }

            if (activeSelection != null) {
                previousSelections.Push(activeSelection);
            }

            activeSelection = Selection.activeObject;
            nextSelections.Clear();
        }

        [MenuItem(toolsMenuPathBack, false, toolsMenuPriority)]
        static void ToolsBack() {
            if (activeSelection != null) {
                nextSelections.Push(activeSelection);
            }

            Selection.activeObject = previousSelections.Pop();
            activeSelection = Selection.activeObject;
            ignoreNextSelectionChangedEvent = true;
        }

        [MenuItem(toolsMenuPathBack, true, toolsMenuPriority)]
        static bool ToolsValidateBack() {
            return previousSelections.Count > 0;
        }

        [MenuItem(toolsMenuPathForward, false, toolsMenuPriority)]
        static void ToolsForward() {
            if (activeSelection != null) {
                previousSelections.Push(activeSelection);
            }

            Selection.activeObject = nextSelections.Pop();
            activeSelection = Selection.activeObject;
            ignoreNextSelectionChangedEvent = true;
        }

        [MenuItem(toolsMenuPathForward, true, toolsMenuPriority)]
        static bool ToolsValidateForward() {
            return nextSelections.Count > 0;
        }

        // Assets menu

        [MenuItem(assetsMenuPathBack, false, assetsMenuPriority)]
        static void AssetsBack() {
            if (activeSelection != null) {
                nextSelections.Push(activeSelection);
            }

            Selection.activeObject = previousSelections.Pop();
            activeSelection = Selection.activeObject;
            ignoreNextSelectionChangedEvent = true;
        }

        [MenuItem(assetsMenuPathBack, true, assetsMenuPriority)]
        static bool AssetsValidateBack() {
            return previousSelections.Count > 0;
        }

        [MenuItem(assetsMenuPathForward, false, assetsMenuPriority)]
        static void AssetsForward() {
            if (activeSelection != null) {
                previousSelections.Push(activeSelection);
            }

            Selection.activeObject = nextSelections.Pop();
            activeSelection = Selection.activeObject;
            ignoreNextSelectionChangedEvent = true;
        }

        [MenuItem(assetsMenuPathForward, true, assetsMenuPriority)]
        static bool AssetsValidateForward() {
            return nextSelections.Count > 0;
        }
    }
}
#endif
#endif