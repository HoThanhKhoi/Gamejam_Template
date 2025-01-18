using UnityEditor;
using UnityEngine;

namespace StateMachine
{
    [CustomEditor(typeof(StateComponent))]
    public class ScriptableStatesComponentEditor : Editor
    {
        private GUIStyle _richTextStyle;

        private void OnEnable()
        {
            _richTextStyle = new GUIStyle
            {
                richText = true,
                wordWrap = true
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw the Animator field
            var animatorProp = serializedObject.FindProperty("animator");
            EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));

            if (!animatorProp.objectReferenceValue)
            {
                EditorGUILayout.HelpBox("Animator is missing. Assign an Animator to handle animations.", MessageType.Warning);
            }

            // Draw the State Machine field
            var stateMachineProp = serializedObject.FindProperty("_stateMachine");
            EditorGUILayout.PropertyField(stateMachineProp, new GUIContent("State Machine"));

            if (!stateMachineProp.objectReferenceValue)
            {
                EditorGUILayout.HelpBox("State Machine is missing. Assign a ScriptableStateMachine to define state behavior.", MessageType.Warning);
            }

            // Display runtime information if in play mode
            if (Application.isPlaying)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Runtime Information:</b>", _richTextStyle);

                StateComponent component = (StateComponent)target;

                // Show current state
                if (component.CurrentState)
                {
                    EditorGUILayout.LabelField($"<b>Current State:</b> <color=green>{component.CurrentState.name}</color>", _richTextStyle);
                }
                else
                {
                    EditorGUILayout.LabelField("<b>Current State:</b> <color=red>None</color>", _richTextStyle);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
