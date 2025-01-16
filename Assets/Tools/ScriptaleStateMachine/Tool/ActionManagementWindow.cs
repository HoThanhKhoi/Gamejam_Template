using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class ActionManagementWindow : EditorWindow
    {
        private string _actionName = "NewAction";

        private readonly string _actionAssetsPath = "Assets/StateMachine/Actions/Assets";
        private readonly string _actionScriptsPath = "Assets/StateMachine/Actions/Scripts";

        private List<ScriptableAction> _actions = new List<ScriptableAction>();
        private Vector2 _scrollPos;
        private ScriptableAction _selectedAction;

        private const int ItemsPerRow = 5;

        public static void ShowWindow()
        {
            var window = GetWindow<ActionManagementWindow>("Action Management");
            window.minSize = new Vector2(600, 350);
            window.Show();
        }

        private void OnEnable()
        {
            StateMachineAssetUtility.EnsureFolderExists(_actionAssetsPath);
            StateMachineAssetUtility.EnsureFolderExists(_actionScriptsPath);

            LoadAllActions();
        }

        private void OnGUI()
        {
            GUILayout.Label("Action Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "1) Create Action Script\n" +
                "2) Wait for compilation\n" +
                "3) Create Action Asset\n\n" +
                "Click an item to select it (Inspector), then delete if needed.",
                MessageType.Info
            );

            GUILayout.Space(10);

            // CREATE SCRIPT
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Step 1: Create Action Script", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _actionName = EditorGUILayout.TextField(_actionName);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Create Script", GUILayout.Height(25)))
                {
                    CreateActionScript(_actionName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // CREATE ASSET
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Step 2: Create Action Asset", EditorStyles.boldLabel);

                EditorGUILayout.HelpBox(
                    "Make sure the script name and above name match.\n" +
                    "If Unity hasn't finished compiling, this may fail.\n" +
                    "Try again after compilation finishes.",
                    MessageType.None
                );

                if (GUILayout.Button("Create Asset from Script", GUILayout.Height(25)))
                {
                    CreateActionAsset(_actionName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // LIST
            GUILayout.Label("Existing Actions:", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                if (_actions.Count == 0)
                {
                    EditorGUILayout.HelpBox("No ScriptableAction assets found!", MessageType.Info);
                }
                else
                {
                    float buttonWidth = (EditorGUIUtility.currentViewWidth - 40f) / ItemsPerRow;
                    float buttonHeight = 50f;

                    for (int i = 0; i < _actions.Count; i++)
                    {
                        if (i % ItemsPerRow == 0)
                            EditorGUILayout.BeginHorizontal();

                        var action = _actions[i];
                        if (action == null) continue;

                        if (GUILayout.Button(action.name, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            _selectedAction = action;
                            Selection.activeObject = action;
                        }

                        if (i % ItemsPerRow == ItemsPerRow - 1)
                            EditorGUILayout.EndHorizontal();
                    }
                    if (_actions.Count % ItemsPerRow != 0)
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
            if (_selectedAction == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label($"Selected Action: {_selectedAction.name}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Selected Action", GUILayout.Height(25)))
                {
                    string path = AssetDatabase.GetAssetPath(_selectedAction);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.Refresh();

                    _selectedAction = null;
                    LoadAllActions();
                }
            }
            EditorGUILayout.EndVertical();
        }

        // === TWO-STEP CREATION ===
        private void CreateActionScript(string actionName)
        {
            string scriptPath = Path.Combine(_actionScriptsPath, actionName + ".cs");
            if (File.Exists(scriptPath))
            {
                Debug.LogError($"Script '{actionName}.cs' already exists!");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine("namespace StateMachine");
            sb.AppendLine("{");
            sb.AppendLine($"    [CreateAssetMenu(menuName = \"Scriptable State Machine/Actions/{actionName}\", fileName = \"{actionName}\")]");
            sb.AppendLine($"    public class {actionName} : ScriptableAction");
            sb.AppendLine("    {");
            sb.AppendLine("        public override void Act(StateComponent statesComponent)");
            sb.AppendLine("        {");
            sb.AppendLine("            // TODO: Implement your action logic here");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(scriptPath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh();

            Debug.Log($"Created script at: {scriptPath}\nWait for Unity to finish compiling, then use 'Create Asset from Script'.");
        }

        private void CreateActionAsset(string actionName)
        {
            string fullTypeName = $"StateMachine.{actionName}";
            var actionType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == fullTypeName);

            if (actionType == null)
            {
                Debug.LogWarning($"Type '{fullTypeName}' not found. Script may still be compiling.");
                return;
            }

            var instance = ScriptableObject.CreateInstance(actionType) as ScriptableAction;
            if (instance == null)
            {
                Debug.LogError($"Failed to create instance of '{fullTypeName}'.");
                return;
            }

            string assetPath = Path.Combine(_actionAssetsPath, actionName + ".asset");
            if (File.Exists(assetPath))
            {
                Debug.LogError($"Asset '{actionName}.asset' already exists!");
                return;
            }

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            LoadAllActions();
            _selectedAction = instance;
            Selection.activeObject = instance;
        }

        private void LoadAllActions()
        {
            _actions = StateMachineAssetUtility.LoadAllAssetsOfType<ScriptableAction>(_actionAssetsPath);
        }
    }
}
