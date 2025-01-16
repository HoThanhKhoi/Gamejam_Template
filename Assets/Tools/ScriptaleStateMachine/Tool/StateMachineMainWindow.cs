using UnityEditor;
using UnityEngine;

namespace StateMachine.EditorTools
{
    public class StateMachineMainWindow : EditorWindow
    {
        [MenuItem("Tools/State Machine Manager")]
        public static void ShowWindow()
        {
            var window = GetWindow<StateMachineMainWindow>("State Machine Manager");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("State Machine Main Window", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox(
                "Use the buttons below to open dedicated managers for each type of ScriptableObject.\n" +
                "You can create, inspect, and delete items from there.",
                MessageType.Info
            );

            GUILayout.Space(10);

            if (GUILayout.Button("State Machines"))
            {
                StateMachineManagerWindow.ShowWindow();
            }

            if (GUILayout.Button("States"))
            {
                StateManagementWindow.ShowWindow();
            }

            if (GUILayout.Button("Conditions"))
            {
                ConditionManagementWindow.ShowWindow();
            }

            if (GUILayout.Button("Actions"))
            {
                ActionManagementWindow.ShowWindow();
            }

            if (GUILayout.Button("Transitions"))
            {
                TransitionManagementWindow.ShowWindow();
            }
        }
    }
}
