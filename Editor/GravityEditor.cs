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
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            if(Target.target == null)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetVector"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("force"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("updateMethod"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}