// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

using System;

namespace Sonity.Internal {

    public static class StringOperation {

        /// <summary>
        /// Returns if stringA is the same as stringA
        /// </summary>
        public static bool EqualsCaseInsensitive(string stringA, string stringB) {
            return string.Compare(stringA, stringB, true, System.Globalization.CultureInfo.CurrentCulture) == 0;
        }

        /// <summary>
        /// Returns if stringA contains stringB
        /// </summary>
        public static bool ContainsCaseInsensitive(string stringA, string stringB) {
            return stringA.IndexOf(stringB, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}