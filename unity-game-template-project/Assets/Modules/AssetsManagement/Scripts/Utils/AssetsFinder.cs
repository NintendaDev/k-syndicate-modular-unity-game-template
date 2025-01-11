using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Modules.AssetsManagement.Utils
{
    public sealed class AssetsFinder
    {
        public static List<T> GetAssetsFromFolder<T>(string skinsFolder) where T : Object
        {
            List<T> assetsArray = new();
#if UNITY_EDITOR
            string[] assetsGuids = AssetDatabase.FindAssets($"t:{typeof(T)}", new[] { skinsFolder });

            if (assetsGuids.Length == 0)
                return assetsArray;

            foreach (string configurationGuid in assetsGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(configurationGuid);
                T configuration = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                assetsArray.Add(configuration);
            }
#endif
            return assetsArray;
        }
    }
}