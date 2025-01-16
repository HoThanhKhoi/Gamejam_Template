using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class StateMachineManagerWindow : EditorWindow
    {
        private string _stateMachineName = "NewStateMachine";
        private List<ScriptableStateMachine> _stateMachines = new List<ScriptableStateMachine>();
        private Vector2 _scrollPos;

        private readonly string _folderPath = "Assets/StateMachine/StateMachines";
        private const int ItemsPerRow = 5;

        private ScriptableStateMachine _selectedStateMachine;

        public static void ShowWindow()
        {
            var window = GetWindow<StateMachineManagerWindow>("State Machine Manager");
            window.minSize = new Vector2(600, 350);
            window.Show();
        }

        private void OnEnable()
        {
            StateMachineAssetUtility.EnsureFolderExists(_folderPath);
            LoadAll();
        }

        private void OnGUI()
        {
            GUILayout.Label("State Machine Manager", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "Create and manage ScriptableStateMachine assets.\n" +
                "Click an item to select it and see it in the Inspector.\n" +
                "Then you can delete it if you wish.",
                MessageType.Info
            );

            GUILayout.Space(10);

            // CREATE NEW
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Create New State Machine", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _stateMachineName = EditorGUILayout.TextField(_stateMachineName);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Create", GUILayout.Height(25)))
                {
                    CreateStateMachine(_stateMachineName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // EXISTING ITEMS
            GUILayout.Label("Existing State Machines:", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                if (_stateMachines.Count == 0)
                {
                    EditorGUILayout.HelpBox("No ScriptableStateMachine assets found!", MessageType.Info);
                }
                else
                {
                    // Calculate button width so all 5 items share the available space
                    float buttonWidth = (EditorGUIUtility.currentViewWidth - 40f) / ItemsPerRow;
                    float buttonHeight = 50f;

                    for (int i = 0; i < _stateMachines.Count; i++)
                    {
                        if (i % ItemsPerRow == 0)
                            EditorGUILayout.BeginHorizontal();

                        var sm = _stateMachines[i];
                        if (sm == null) continue;

                        if (GUILayout.Button(sm.name, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            _selectedStateMachine = sm;
                            Selection.activeObject = sm; // Show in Inspector
                        }

                        if (i % ItemsPerRow == ItemsPerRow - 1)
                            EditorGUILayout.EndHorizontal();
                    }

                    if (_stateMachines.Count % ItemsPerRow != 0)
                        EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // DELETE SELECTED
            DrawDeleteSection();
        }

        private void DrawDeleteSection()
        {
            if (_selectedStateMachine == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label($"Selected State Machine: {_selectedStateMachine.name}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Selected", GUILayout.Height(25)))
                {
                    string assetPath = AssetDatabase.GetAssetPath(_selectedStateMachine);
                    AssetDatabase.DeleteAsset(assetPath);
                    AssetDatabase.Refresh();

                    _selectedStateMachine = null;
                    LoadAll();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void CreateStateMachine(string name)
        {
            var instance = ScriptableObject.CreateInstance<ScriptableStateMachine>();
            instance.name = name;
            string assetPath = $"{_folderPath}/{name}.asset";

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            LoadAll();
            _selectedStateMachine = instance;
            Selection.activeObject = instance;
        }

        private void LoadAll()
        {
            _stateMachines = StateMachineAssetUtility.LoadAllAssetsOfType<ScriptableStateMachine>(_folderPath);
        }
    }
}
