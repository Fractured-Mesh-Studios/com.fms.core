using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameEngine.Data;
using System.Diagnostics;
using System.Linq;

namespace GameEditor.Data
{
    [CustomEditor(typeof(DataLoader))]
    public class DataLoaderEditor : Editor
    {
        private DataLoader m_target;

        private void OnEnable()
        {
            m_target = (DataLoader)target; 
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load"))
            {
                m_target.Load();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Save"))
            {
                m_target.Save();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Path"))
            {
                Process.Start(Application.persistentDataPath);
            }
            if (GUILayout.Button("Setup"))
            {
                var components = m_target.GetComponents<Component>();

                m_target.components = components.Where(x => x != m_target).ToList();
            }
        }

    }
}
