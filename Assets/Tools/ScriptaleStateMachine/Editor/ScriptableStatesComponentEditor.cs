using UnityEditor;
using UnityEngine;

namespace StateMachine
{
    [CustomEditor(typeof(StateComponent))]
    public class ScriptableStatesComponentEditor : Editor
    {
        private Color _rectColor;
        private GUIStyle _richTextStyle;

        private void OnEnable()
        {
            _rectColor = new Color(0, 0, 0, 0.2f);
            _richTextStyle = new GUIStyle { richText = true };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var stateMachineProp = serializedObject.FindProperty("_stateMachine");
            EditorGUILayout.PropertyField(stateMachineProp);
            if (!stateMachineProp.objectReferenceValue)
            {
                EditorGUILayout.HelpBox("State Machine missing, select a state machine to run.", MessageType.Warning, true);
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Runtime Information:</b>", _richTextStyle);
                StateComponent component = (StateComponent)target;
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

