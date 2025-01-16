using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderManagerWindow : EditorWindow
{
    private Vector2 scrollPos;
    private string projectPath;
    private Dictionary<string, bool> expandedFolders = new Dictionary<string, bool>();

    [MenuItem("Tools/Folder Manager")]
    public static void ShowWindow()
    {
        GetWindow<FolderManagerWindow>("Folder Manager");
    }

    private void OnEnable()
    {
        projectPath = Application.dataPath;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Project Hierarchy", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        try
        {
            DrawHierarchy(projectPath, 0);
        }
        finally
        {
            EditorGUILayout.EndScrollView();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Manage Templates", GUILayout.Height(30)))
        {
            TemplateManagerWindow.ShowWindow();
        }
    }

    private void DrawHierarchy(string path, int indentationLevel)
    {
        if (Directory.Exists(path))
        {
            var subdirectories = Directory.GetDirectories(path);

            foreach (var dir in subdirectories)
            {
                string relativePath = dir.Replace(projectPath, "Assets");

                if (!expandedFolders.ContainsKey(relativePath))
                {
                    expandedFolders[relativePath] = false;
                }

                GUILayout.BeginHorizontal();
                GUILayout.Space(indentationLevel * 20); // Add padding based on indentation level

                expandedFolders[relativePath] = EditorGUILayout.Foldout(expandedFolders[relativePath], Path.GetFileName(dir), true);

                if (GUILayout.Button("Create", GUILayout.Width(60)))
                {
                    FolderCreateWindow.ShowWindow(relativePath);
                }

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    if (EditorUtility.DisplayDialog("Confirm Delete", $"Delete folder '{Path.GetFileName(dir)}'?", "Yes", "No"))
                    {
                        DeleteFolder(dir);
                        expandedFolders.Remove(relativePath);
                        AssetDatabase.Refresh();
                        return; // Prevent further GUI processing after deletion
                    }
                }
                GUILayout.EndHorizontal();

                if (expandedFolders[relativePath])
                {
                    DrawHierarchy(dir, indentationLevel + 1);
                }
            }
        }
    }

    private void DeleteFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
            File.Delete(folderPath + ".meta");
            AssetDatabase.Refresh();
        }
    }
}
