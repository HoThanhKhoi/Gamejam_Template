using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class ConditionManagementWindow : EditorWindow
    {
        private string _conditionName = "NewCondition";

        private readonly string _conditionAssetsPath = "Assets/StateMachine/Conditions/Assets";
        private readonly string _conditionScriptsPath = "Assets/StateMachine/Conditions/Scripts";

        private List<ScriptableCondition> _conditions = new List<ScriptableCondition>();
        private Vector2 _scrollPos;
        private ScriptableCondition _selectedCondition;

        private const int ItemsPerRow = 5;

        public static void ShowWindow()
        {
            var window = GetWindow<ConditionManagementWindow>("Condition Management");
            window.minSize = new Vector2(600, 350);
            window.Show();
        }

        private void OnEnable()
        {
            StateMachineAssetUtility.EnsureFolderExists(_conditionAssetsPath);
            StateMachineAssetUtility.EnsureFolderExists(_conditionScriptsPath);

            LoadAllConditions();
        }

        private void OnGUI()
        {
            GUILayout.Label("Condition Management", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "1) Create Condition Script\n" +
                "2) Wait for compilation\n" +
                "3) Create Condition Asset\n\n" +
                "Click an item to select it and see it in the Inspector,\n" +
                "Then you can delete it if needed.",
                MessageType.Info
            );

            GUILayout.Space(10);

            // CREATE SCRIPTS
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Step 1: Create Condition Script", EditorStyles.boldLabel);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name:", GUILayout.Width(50));
                _conditionName = EditorGUILayout.TextField(_conditionName);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(5);

                if (GUILayout.Button("Create Script", GUILayout.Height(25)))
                {
                    CreateConditionScript(_conditionName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // CREATE ASSET
            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label("Step 2: Create Condition Asset", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(
                    "Make sure the script name and above name match.\n" +
                    "If Unity hasn't finished compiling, this may fail. Try again after compile.",
                    MessageType.None
                );

                if (GUILayout.Button("Create Asset from Script", GUILayout.Height(25)))
                {
                    CreateConditionAsset(_conditionName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // LIST
            GUILayout.Label("Existing Conditions:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                if (_conditions.Count == 0)
                {
                    EditorGUILayout.HelpBox("No ScriptableCondition assets found!", MessageType.Info);
                }
                else
                {
                    float buttonWidth = (EditorGUIUtility.currentViewWidth - 40f) / ItemsPerRow;
                    float buttonHeight = 50f;

                    for (int i = 0; i < _conditions.Count; i++)
                    {
                        if (i % ItemsPerRow == 0)
                            EditorGUILayout.BeginHorizontal();

                        var condition = _conditions[i];
                        if (condition == null) continue;

                        if (GUILayout.Button(condition.name, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                        {
                            _selectedCondition = condition;
                            Selection.activeObject = condition;
                        }

                        if (i % ItemsPerRow == ItemsPerRow - 1)
                            EditorGUILayout.EndHorizontal();
                    }
                    if (_conditions.Count % ItemsPerRow != 0)
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
            if (_selectedCondition == null) return;

            EditorGUILayout.BeginVertical("box");
            {
                GUILayout.Label($"Selected Condition: {_selectedCondition.name}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete Selected Condition", GUILayout.Height(25)))
                {
                    string assetPath = AssetDatabase.GetAssetPath(_selectedCondition);
                    AssetDatabase.DeleteAsset(assetPath);
                    AssetDatabase.Refresh();

                    _selectedCondition = null;
                    LoadAllConditions();
                }
            }
            EditorGUILayout.EndVertical();
        }

        // === TWO-STEP CREATION ===
        private void CreateConditionScript(string conditionName)
        {
            string scriptPath = Path.Combine(_conditionScriptsPath, conditionName + ".cs");
            if (File.Exists(scriptPath))
            {
                Debug.LogError($"Script '{conditionName}.cs' already exists!");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine("namespace StateMachine");
            sb.AppendLine("{");
            sb.AppendLine($"    [CreateAssetMenu(menuName = \"Scriptable State Machine/Conditions/{conditionName}\", fileName = \"{conditionName}\")]");
            sb.AppendLine($"    public class {conditionName} : ScriptableCondition");
            sb.AppendLine("    {");
            sb.AppendLine("        public override bool Verify(StateComponent statesComponent)");
            sb.AppendLine("        {");
            sb.AppendLine("            // TODO: Implement your condition logic here");
            sb.AppendLine("            return false;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(scriptPath, sb.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh();

            Debug.Log($"Created script at: {scriptPath}\nWait for Unity to finish compiling, then use 'Create Asset from Script'.");
        }

        private void CreateConditionAsset(string conditionName)
        {
            string fullTypeName = $"StateMachine.{conditionName}";
            var conditionType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == fullTypeName);

            if (conditionType == null)
            {
                Debug.LogWarning($"Type '{fullTypeName}' not found. Script may still be compiling.");
                return;
            }

            var instance = ScriptableObject.CreateInstance(conditionType) as ScriptableCondition;
            if (instance == null)
            {
                Debug.LogError($"Failed to create instance of '{fullTypeName}'.");
                return;
            }

            string assetPath = Path.Combine(_conditionAssetsPath, conditionName + ".asset");
            if (File.Exists(assetPath))
            {
                Debug.LogError($"Asset '{conditionName}.asset' already exists!");
                return;
            }

            AssetDatabase.CreateAsset(instance, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            LoadAllConditions();
            _selectedCondition = instance;
            Selection.activeObject = instance;
        }

        private void LoadAllConditions()
        {
            _conditions = StateMachineAssetUtility.LoadAllAssetsOfType<ScriptableCondition>(_conditionAssetsPath);
        }
    }
}
