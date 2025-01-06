// Created by Victor Engström
// Copyright 2024 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;
using System.Security.Cryptography;

namespace Sonity.Internal {

    public static class EditorAssetGuid {

        public static string GetAssetGuid(UnityEngine.Object scriptableObject) {
#if !SONITY_DLL_RUNTIME
            return UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(scriptableObject));
#else
            return "";
#endif
        }

        public static long GetInt64HashFromString(string text) {
            if (String.IsNullOrEmpty(text)) {
                return 0;
            }

            // Uses SHA256 to create the hash
            using (SHA256Managed sha = new SHA256Managed()) {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha.ComputeHash(textBytes);
                long hash = BitConverter.ToInt64(hashBytes);
                return hash;
            }
        }
    }
}

#endif