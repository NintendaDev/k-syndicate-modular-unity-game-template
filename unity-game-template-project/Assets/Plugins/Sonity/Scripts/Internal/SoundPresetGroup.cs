// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

#if UNITY_EDITOR

namespace Sonity.Internal {

    [Serializable]
    public class SoundPresetGroup {

        public bool disable = false;

        public SoundEventBase soundEventPreset;
        public SoundContainerBase soundContainerPreset;

        public bool matchExpand = true;

        public bool matchUse = true;
        public bool applyOnAll = false;
        public bool isPrefixUse = true;
        public string[] isPrefixString = new string[] { "SFX" };
        public bool isNotPrefixUse = false;
        public string[] isNotPrefixString = new string[] { "" };
        public bool containsUse = false;
        public string[] containsString = new string[] { "" };
        public bool notContainsUse = false;
        public string[] notContainsString = new string[] { "" };

        public bool ShouldUseMatch(bool isSoundContainer) {
            if (disable || !matchUse) {
                return false;
            }

            if (isSoundContainer && soundContainerPreset == null) {
                return false;
            } else if (!isSoundContainer && soundEventPreset == null) {
                return false;
            }
            return true;
        }

        public bool GetNameMatches(string nameOriginal, bool isSoundContainer) {

            if (applyOnAll) {
                return true;
            }

            nameOriginal = nameOriginal.ToLower();

            string nameCleaned = nameOriginal;
            if (isSoundContainer) {
                nameCleaned = nameCleaned.Remove(nameCleaned.Length - GetNameToRemoveWithRightSeparatorChar(nameCleaned, "SC").Length);
            } else {
                nameCleaned = nameCleaned.Remove(nameCleaned.Length - GetNameToRemoveWithRightSeparatorChar(nameCleaned, "SE").Length);
            }
            RemoveTrailingJunk(nameCleaned, true);

            if (isNotPrefixUse) {
                for (int i = 0; i < isNotPrefixString.Length; i++) {
                    if (nameCleaned.StartsWith(isNotPrefixString[i].ToLower())) {
                        return false;
                    }
                }
                return true;
            }
            if (notContainsUse) {
                for (int i = 0; i < notContainsString.Length; i++) {
                    if (nameOriginal.Contains(notContainsString[i].ToLower())) {
                        return false;
                    }
                }
                return true;
            }
            if (isPrefixUse) {
                for (int i = 0; i < isPrefixString.Length; i++) {
                    if (nameCleaned.StartsWith(isPrefixString[i].ToLower())) {
                        return true;
                    }
                }
            }
            if (containsUse) {
                for (int i = 0; i < containsString.Length; i++) {
                    if (nameOriginal.Contains(containsString[i].ToLower())) {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string GetNameToRemoveWithRightSeparatorChar(string fileName, string nameToRemove) {
            if (fileName.EndsWith("_" + nameToRemove)) {
                return "_" + nameToRemove;
            } else if (fileName.EndsWith(" " + nameToRemove)) {
                return " " + nameToRemove;
            } else if (fileName.EndsWith("-" + nameToRemove)) {
                return "-" + nameToRemove;
            }
            return "_" + nameToRemove;
        }

        private static string RemoveTrailingJunk(string input, bool removeNumbers = true) {
            char[] charsToRemove;
            if (removeNumbers) {
                charsToRemove = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', '_', '-' };
            } else {
                charsToRemove = new char[] { ' ', '_', '-' };
            }
            return input.TrimEnd(charsToRemove);
        }
    }
}
#endif