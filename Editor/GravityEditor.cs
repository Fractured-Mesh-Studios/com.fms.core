using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameEngine;

namespace GameEditor
{
    [CustomEditor(typeof(Gravity))]
    public class GravityEditor : Editor
    {
        Gravity Target;

        private void OnEnable()
        {
            Target = (Gravity)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUILayout.Label("Properties", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Target"));
            if(Target.Target == null)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetVector"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Force"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateMethod"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}