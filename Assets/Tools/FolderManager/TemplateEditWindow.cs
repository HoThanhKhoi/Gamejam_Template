using UnityEditor;
using UnityEngine;
using FolderManager;
using System.Collections.Generic;

public class TemplateEditWindow : EditorWindow
{
    private Template template;
    private TemplateFolder rootFolder; // Store the reconstructed hierarchy
    private Vector2 scrollPos;

    public static void ShowWindow(Template template)
    {
        var window = GetWindow<TemplateEditWindow>("Edit Template");
        window.template = template;
        window.rootFolder = template.ReconstructHierarchy(); // Reconstruct hierarchy on open
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField($"Editing Template: {template.Name}", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (rootFolder != null)
        {
            DrawFolder(rootFolder, 0);
        }
        else
        {
            EditorGUILayout.LabelField("No root folder found.");
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // Save changes button
        if (GUILayout.Button("Save Changes", GUILayout.Height(30)))
        {
            template.FlatFolders.Clear();
            template.Flatten(rootFolder); // Flatten and save the updated hierarchy
            Close();
        }
    }

    private void DrawFolder(TemplateFolder folder, int indentationLevel)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(indentationLevel * 20); // Indentation for hierarchy

        // Editable folder name
        folder.Name = EditorGUILayout.TextField(folder.Name);

        // Add subfolder button
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            if (folder.SubFolders == null)
            {
                folder.SubFolders = new List<TemplateFolder>();
            }

            folder.SubFolders.Add(new TemplateFolder { Name = "New Folder" });
        }

        // Remove folder button (except root)
        if (GUILayout.Button("-", GUILayout.Width(30)) && folder.Name != "Root")
        {
            folder.Name = null; // Mark this folder for deletion
        }

        GUILayout.EndHorizontal();

        // Recursively draw subfolders
        if (folder.SubFolders != null)
        {
            for (int i = folder.SubFolders.Count - 1; i >= 0; i--) // Iterate in reverse
            {
                if (folder.SubFolders[i].Name == null)
                {
                    folder.SubFolders.RemoveAt(i); // Remove marked folders
                }
                else
                {
                    DrawFolder(folder.SubFolders[i], indentationLevel + 1);
                }
            }
        }
    }
}
