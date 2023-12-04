using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameEngine;
using System.Diagnostics;

namespace GameEditor
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : Editor
    {
        private DataManager m_target;

        private void OnEnable()
        {
            m_target = (DataManager)target; 
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
        }

    }
}
