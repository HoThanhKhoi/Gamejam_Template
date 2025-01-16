using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class StateManagementWindow : EditorWindow
    {
        private string _stateName = "NewState";
        private List<ScriptableState> _states = new List<ScriptableState>();
        private Vector2 _scrollPos;

        private readonly string _folderPath = "Assets/StateMachine/States";
        private const int ItemsPerRow = 5;

        private ScriptableState _selectedState;

        public static void ShowWindow()
        {
            var window = GetWindow<StateManagementWindow>("State Management");
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
            GUILayout.Label("State Management", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "Create and manage ScriptableState assets.\n" +
                "Click an item to select it.\n" +
                "Use 'Delete Selected' to remove it.",
                MessageType.Info
            );

            GUILayout.Space(10);

            // CREATE NEW
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Create New State", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _stateName = EditorGUILayout.TextField(_stateName);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Create", GUILayout.Height(25)))
                {
                    CreateState(_stateName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // LIST
            GUILayout.Label("Existing States:", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                if (_states.Count == 0)
                {
                    EditorGUILayout.HelpBox("No ScriptableState assets found!", MessageType.Info);
                }
                else
                {
                    float buttonWidth = (EditorGUIUtility.currentViewWidth - 40f) / ItemsPerRow;
                    float buttonHeight = 50f;

                    for (int i = 0; i < _states.Count; i++)
                    {
                        if (i % ItemsPerRow == 0)
                            EditorGUILayout.BeginHorizontal();

                        var st = _states[i];
                        if (st == null) continue;

                        if (GUILayout.Button(st.name, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            _selectedState = st;
                            Selection.activeObject = st;
                        }

                        if (i % ItemsPerRow == ItemsPerRow - 1)
                            EditorGUILayout.EndHorizontal();
                    }
                    if (_states.Count % ItemsPerRow != 0)
                        EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            DrawDeleteSection();
        }

        private void DrawDeleteSection()
        {
            if (_selectedState == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label($"Selected State: {_selectedState.name}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Selected", GUILayout.Height(25)))
                {
                    string path = AssetDatabase.GetAssetPath(_selectedState);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();

                    _selectedState = null;
                    LoadAll();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void CreateState(string name)
        {
            var instance = ScriptableObject.CreateInstance<ScriptableState>();
            instance.name = name;
            string assetPath = $"{_folderPath}/{name}.asset";

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            LoadAll();
            _selectedState = instance;
            Selection.activeObject = instance;
        }

        private void LoadAll()
        {
            _states = StateMachineAssetUtility.LoadAllAssetsOfType<ScriptableState>(_folderPath);
        }
    }
}
