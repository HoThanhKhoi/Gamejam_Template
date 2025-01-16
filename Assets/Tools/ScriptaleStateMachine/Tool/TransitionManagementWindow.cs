using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class TransitionManagementWindow : EditorWindow
    {
        private string _transitionName = "NewTransition";
        private List<ScriptableObject> _transitions = new List<ScriptableObject>();
        private Vector2 _scrollPos;
        private ScriptableObject _selectedTransition;

        private readonly string _folderPath = "Assets/StateMachine/Transitions";
        private const int ItemsPerRow = 5;

        public static void ShowWindow()
        {
            var window = GetWindow<TransitionManagementWindow>("Transition Management");
            window.minSize = new Vector2(600, 350);
            window.Show();
        }

        private void OnEnable()
        {
            StateMachineAssetUtility.EnsureFolderExists(_folderPath);
            LoadAllTransitions();
        }

        private void OnGUI()
        {
            GUILayout.Label("Transition Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Create and manage Transition assets (if you use them as ScriptableObjects).\n" +
                "Click an item to select it. Delete if needed.",
                MessageType.Info
            );

            GUILayout.Space(10);

            // CREATE
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Create New Transition", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _transitionName = EditorGUILayout.TextField(_transitionName);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Create Transition", GUILayout.Height(25)))
                {
                    CreateTransition(_transitionName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // LIST
            GUILayout.Label("Existing Transitions:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                if (_transitions.Count == 0)
                {
                    EditorGUILayout.HelpBox("No transition assets found!", MessageType.Info);
                }
                else
                {
                    float buttonWidth = (EditorGUIUtility.currentViewWidth - 40f) / ItemsPerRow;
                    float buttonHeight = 50f;

                    for (int i = 0; i < _transitions.Count; i++)
                    {
                        if (i % ItemsPerRow == 0)
                            EditorGUILayout.BeginHorizontal();

                        var transition = _transitions[i];
                        if (transition == null) continue;

                        if (GUILayout.Button(transition.name, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            _selectedTransition = transition;
                            Selection.activeObject = transition;
                        }

                        if (i % ItemsPerRow == ItemsPerRow - 1)
                            EditorGUILayout.EndHorizontal();
                    }
                    if (_transitions.Count % ItemsPerRow != 0)
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
            if (_selectedTransition == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label($"Selected Transition: {_selectedTransition.name}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Selected Transition", GUILayout.Height(25)))
                {
                    string path = AssetDatabase.GetAssetPath(_selectedTransition);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();

                    _selectedTransition = null;
                    LoadAllTransitions();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void CreateTransition(string name)
        {
            // If you have a specific type (e.g. ScriptableTransition), create that instead:
            // var instance = ScriptableObject.CreateInstance<ScriptableTransition>();

            // Demo: Just create a ScriptableStateMachine as a placeholder
            var instance = ScriptableObject.CreateInstance<ScriptableStateMachine>();
            instance.name = name;

            string assetPath = $"{_folderPath}/{name}.asset";
            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            LoadAllTransitions();
            _selectedTransition = instance;
            Selection.activeObject = instance;
        }

        private void LoadAllTransitions()
        {
            // If you have a real type, e.g. ScriptableTransition, replace it
            var loaded = StateMachineAssetUtility.LoadAllAssetsOfType<ScriptableStateMachine>(_folderPath);
            _transitions.Clear();
            _transitions.AddRange(loaded);
        }
    }
}
