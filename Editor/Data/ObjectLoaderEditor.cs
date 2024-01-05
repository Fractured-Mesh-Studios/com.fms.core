using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CoreEngine.Data;
using System.Linq;
using System.Reflection;
using System;

namespace CoreEditor.Data
{
    [CustomEditor(typeof(ObjectLoader))]
    public class ObjectLoaderEditor : Editor
    {
        private ObjectLoader m_target;

        private void OnEnable()
        {
            m_target = (ObjectLoader)target;
        }

        public override void OnInspectorGUI()
        {
            KeyInspector();
            DrawDefaultInspector();

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
            {
                m_target.Load();
            }
            if (GUILayout.Button("Save"))
            {
                m_target.Save();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Path"))
            {
                System.Diagnostics.Process.Start(Application.persistentDataPath);
            }
            if (GUILayout.Button("Setup"))
            {
                var components = m_target.GetComponents<Component>();
                m_target.components = components.Where(x => x != m_target).ToList();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void KeyInspector()
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var keyField = m_target.GetType().GetField("m_key", flags);
            EditorGUILayout.TextField((string)keyField.GetValue(m_target));
        }

    }
}
