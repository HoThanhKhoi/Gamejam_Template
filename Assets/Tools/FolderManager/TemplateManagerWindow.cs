using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FolderManager;

public class TemplateManagerWindow : EditorWindow
{
    private const string TemplatesKey = "FolderTemplates";
    private List<Template> templates = new List<Template>();
    private Vector2 scrollPos;
    private string newTemplateName = "";

    [MenuItem("Tools/Template Manager")]
    public static void ShowWindow()
    {
        GetWindow<TemplateManagerWindow>("Template Manager");
    }

    private void OnEnable()
    {
        LoadTemplates();
    }

    public static List<Template> LoadTemplates()
    {
        if (EditorPrefs.HasKey(TemplatesKey))
        {
            string json = EditorPrefs.GetString(TemplatesKey);
            var templateList = JsonUtility.FromJson<TemplateList>(json);
            return templateList?.Templates ?? new List<Template>();
        }

        return new List<Template>();
    }

    private void SaveTemplates()
    {
        var templateList = new TemplateList { Templates = templates };

        // Flatten each template's hierarchy before saving
        foreach (var template in templateList.Templates)
        {
            template.FlatFolders.Clear();
            var root = template.ReconstructHierarchy();
            template.Flatten(root);
        }

        string json = JsonUtility.ToJson(templateList);
        EditorPrefs.SetString(TemplatesKey, json);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Manage Folder Templates", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < templates.Count; i++)
        {
            GUILayout.BeginHorizontal();

            templates[i].Name = EditorGUILayout.TextField(templates[i].Name);

            if (GUILayout.Button("Edit", GUILayout.Width(50)))
            {
                TemplateEditWindow.ShowWindow(templates[i]);
            }

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete", $"Delete template '{templates[i].Name}'?", "Yes", "No"))
                {
                    templates.RemoveAt(i);
                    SaveTemplates();
                }
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Create New Template", EditorStyles.boldLabel);
        newTemplateName = EditorGUILayout.TextField("Template Name:", newTemplateName);

        if (GUILayout.Button("Add Template", GUILayout.Height(30)))
        {
            if (!string.IsNullOrEmpty(newTemplateName))
            {
                templates.Add(new Template { Name = newTemplateName });
                SaveTemplates();
                newTemplateName = "";
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Template name cannot be empty!", "OK");
            }
        }
    }
}
