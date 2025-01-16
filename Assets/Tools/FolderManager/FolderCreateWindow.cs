using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderCreateWindow : EditorWindow
{
    private string parentPath;
    private string newFolderName = "";

    public static void ShowWindow(string parentPath)
    {
        var window = GetWindow<FolderCreateWindow>("Create Folder");
        window.parentPath = parentPath;
        window.newFolderName = ""; // Reset the folder name for a fresh input
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create New Folder", EditorStyles.boldLabel);
        GUILayout.Label($"Parent Path: {parentPath}");

        newFolderName = EditorGUILayout.TextField("Folder Name:", newFolderName);

        if (GUILayout.Button("Add"))
        {
            if (!string.IsNullOrEmpty(newFolderName))
            {
                CreateFolder(parentPath, newFolderName);
                Close(); // Close the pop-up window after creation
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Folder name cannot be empty!", "OK");
            }
        }
    }

    private void CreateFolder(string parentPath, string folderName)
    {
        // Correctly resolve the full path from the parent path
        string fullPath = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), parentPath, folderName);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", $"Folder '{folderName}' already exists!", "OK");
        }
    }
}
