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
        private Gravity m_target;

        private void OnEnable()
        {
            m_target = (Gravity)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUILayout.Label("Properties", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            if(m_target.target == null)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("targetVector"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("force"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("updateMethod"));

            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Enable"))
            {
                Physics.gravity = Vector3.up * 9.81f;
            }
            EditorGUILayout.LabelField("[" + Physics.gravity + "]", GUILayout.MaxWidth(120));
            if(GUILayout.Button("Disable"))
            {
                Physics.gravity = Vector3.zero;
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            if (!m_target.target)
            {
                m_target.targetVector = Handles.PositionHandle(
                    m_target.targetVector, Quaternion.identity
                );
            }
        }
    }
}