using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public static class StateMachineAssetUtility
    {
        /// <summary>
        /// Ensures that a folder at 'fullPath' (e.g. "Assets/StateMachine/States") exists.
        /// Creates it recursively if needed.
        /// </summary>
        public static void EnsureFolderExists(string fullPath)
        {
            if (AssetDatabase.IsValidFolder(fullPath))
                return;

            string[] parts = fullPath.Split('/');
            string currentPath = parts[0]; // "Assets"

            for (int i = 1; i < parts.Length; i++)
            {
                string nextPath = currentPath + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, parts[i]);
                }
                currentPath = nextPath;
            }
        }

        /// <summary>
        /// Load all assets of type T from the specified folder path.
        /// </summary>
        public static List<T> LoadAllAssetsOfType<T>(string folderPath) where T : ScriptableObject
        {
            var results = new List<T>();

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                // If folder doesn't exist, just return empty
                return results;
            }

            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    results.Add(asset);
                }
            }
            return results;
        }
    }
}
